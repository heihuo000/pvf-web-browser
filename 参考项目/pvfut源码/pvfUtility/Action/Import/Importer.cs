using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using pvfUtility.Dock.ErrorList;
using pvfUtility.Dock.FileExplorer;
using pvfUtility.Helper;
using pvfUtility.Model;
using pvfUtility.Model.PvfOperation;
using pvfUtility.Service;
using pvfUtility.Shell.Dialogs;
using static pvfUtility.Helper.PathsHelper;

namespace pvfUtility.Action.Import
{
    /// <summary>
    ///     导入
    /// </summary>
    internal class Importer
    {
        private readonly PvfPack _pvf;

        public Importer(PvfPack pvf, ImportSetting setting = null)
        {
            _pvf = pvf;
            CurSetting = setting ?? GetSetting();
        }

        /// <summary>
        ///     本次导入配置
        /// </summary>
        public ImportSetting CurSetting { get; }

        private ImportDialog ImportDialog { get; set; }

        public static ImportSetting GetSetting()
        {
            return new ImportSetting
            {
                CompileScript = Config.Instance.ImportCompileScript,
                CompileBinaryAni = Config.Instance.ImportCompileBinaryAni,
                // UseAppendMode = Config.Instance.ImportUseAppendMode,
                ConvertToTraditionalChinese = Config.Instance.ImportConvertTraditionalChinese
            };
        }

        public void ShowDialog(string[] targets, string rootPath, string targetPath)
        {
            ImportDialog = new ImportDialog(this, targets, rootPath, targetPath);
            ImportDialog.Show();
        }

        /// <summary>
        ///     create full file path
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public List<(string Source, string Target)> GetFullPaths()
        {
            var targets = CurSetting.Targets;
            var rootPath = PathFix(CurSetting.SourcePath);
            var targetPath = PathFix(CurSetting.TargetPaths);
            var tmp = new ConcurrentBag<(string Source, string Target)>();
            Parallel.ForEach(targets, item =>
            {
                if (Directory.Exists(item))
                {
                    var directoryInfo = new DirectoryInfo(item);
                    var files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
                    Parallel.ForEach(files, t =>
                    {
                        tmp.Add((t.FullName, string.Concat(targetPath,
                            t.FullName.Substring(rootPath.Length).Replace('\\', '/').ToLower())));
                    });
                }
                else if (File.Exists(item))
                {
                    tmp.Add((item,
                        string.Concat(targetPath, item.Substring(rootPath.Length).Replace('\\', '/').ToLower())));
                }
            });
            //            foreach (var item in tmp)
            //                item.Target = string.Concat(targetPath, item.Source.Substring(rootPath.Length).Replace('\\', '/').ToLower());
            return tmp.ToList();
        }

        public async Task ImportFileFast(string[] targets, string rootPath, string targetPath)
        {
            CurSetting.SourcePath = rootPath;
            CurSetting.TargetPaths = targetPath;
            CurSetting.Targets = targets;
            await Task.Run(ImportFile);
        }

        public Task<(string[], string[])> ImportFile()
        {
            var listAdded = new List<string>();
            var listUpdated = new List<string>();

            MainPresenter.Instance.DoAction("正在导入文件...", progress =>
            {
                lock (_pvf)
                {
                    try
                    {
                        var items = GetFullPaths();
                        var successNum = 0;
                        var max = items.Count;
                        var progressNum = DataHelper.GetResolve(items.Count);
                        for (var i = 0; i < max; i++)
                        {
                            using (var stream = File.OpenRead(items[i].Source))
                            {
                                var (success, added, finalName) = ImportFile(items[i].Target, stream);
                                if (success)
                                    successNum++;
                                if (added)
                                    listAdded.Add(finalName);
                                else
                                    listUpdated.Add(finalName);
                            }

                            if (i % progressNum == 0)
                                progress.Report(i * 100 / max);
                        }

                        FileExplorerPresenter.Instance.View.UpdateNode(PathFix(CurSetting.TargetPaths));
                        Logger.Success(
                            $"导入 :: 成功导入了{successNum}个文件,出现错误的文件不会被导入,可以在错误列表中看到,双击即可打开编辑器修正错误,之后点击保存即可保存数据");
                    }
                    catch (IOException e)
                    {
                        DialogService.Error(e.Message, "", MainPresenter.Instance.View.Handle);
                        return Task.FromResult(false);
                    }
                }

                return Task.FromResult(true);
            });
            return Task.FromResult((listAdded.ToArray(), listUpdated.ToArray()));
        }

        /// <summary>
        ///     导入单个文件
        /// </summary>
        /// <param name="target"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public (bool success, bool isNewFile, string fileName) ImportFile(string target, Stream stream)
        {
            var success = false;
            var added = false;
            var finalName = target;
            var file = _pvf.GetFile(finalName);
            if (file != null)
            {
                success = ImportUpdateFile(file, stream, finalName);
            }
            else if (AddFileFromStream(finalName, stream))
            {
                success = true;
                added = true;
            }

            return (success, added, finalName);
        }

        /// <summary>
        ///     导入更新原有的文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="stream"></param>
        /// <param name="finalName"></param>
        /// <returns></returns>
        private bool ImportUpdateFile(PvfFile file, Stream stream, string finalName)
        {
            if (stream == null || stream.Length == 0L) Logger.Warning($"导入 :: 注意文件内容为空：file://{finalName} ");
            var (result, errors) = _pvf.UpdateFile(file, stream,
                CurSetting.CompileScript, CurSetting.CompileBinaryAni, CurSetting.ConvertToTraditionalChinese);
            if (result) return true;
            Logger.Warning($"导入 :: 导入文件失败：file://{finalName} ");


            if (errors == null)
                return false;

            var id = Guid.NewGuid();
            foreach (var errorListItem in errors)
            {
                errorListItem.Action = "导入文件";
                errorListItem.Guid = id;
                errorListItem.IsExternal = true;
                ErrorListPresenter.Instance.AddError(errorListItem);
            }

            return false;
        }

        /// <summary>
        ///     导入一个新增的文件
        /// </summary>
        /// <param name="target"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        private bool AddFileFromStream(string target, Stream stream)
        {
            var obj = new PvfFile(target);
            var result = ImportUpdateFile(obj, stream, target);
            if (result)
            {
                _pvf.FileList.Add(obj.FileName, obj);
                TreeFileHelper.AddString2TreeList(target, FileExplorerPresenter.Instance.FileTree);
            }

            return result;
        }
    }
}