using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using pvfUtility.Helper;
using pvfUtility.Model;
using pvfUtility.Model.PvfOperation;
using pvfUtility.Service;
using pvfUtility.Shell.Dialogs;
using SevenZipSharp;
using static pvfUtility.Helper.PathsHelper;
using static pvfUtility.Service.PackService;

namespace pvfUtility.Action.Extract
{
    /// <summary>
    ///     提取
    /// </summary>
    internal class Extractor
    {
        private readonly PvfPack _pvf;

        public Extractor(PvfPack pvf, ExtractSetting setting = null, IEnumerable<string> files = null)
        {
            _pvf = pvf;
            CurSetting = setting ?? GetSetting();
            CurSetting.Items = files?.Select(x => (x, false)).ToList();
        }

        private ExtractDialog ExtractDialog { get; set; }

        /// <summary>
        ///     本次提取配置
        /// </summary>
        public ExtractSetting CurSetting { get; }

        /// <summary>
        ///     获取默认设置
        /// </summary>
        /// <returns></returns>
        public static ExtractSetting GetSetting()
        {
            return new ExtractSetting
            {
                DecompileBinaryAni = Config.Instance.ExtractDecompileBinaryAni,
                DecompileScript = Config.Instance.ExtractDecompileScript,
                ConvertConvertSimplifiedChinese = Config.Instance.ExtractConvertSimplifiedChinese,
                OpenFolder = Config.Instance.ExtractOpenFolder,
                TargetPath = CurrentPackPathFolder
            };
        }

        /// <summary>
        ///     显示对话框
        /// </summary>
        /// <param name="files"></param>
        public void ShowDialog(IEnumerable<string> files = null)
        {
            ExtractDialog = new ExtractDialog(files, this);
            ExtractDialog.Show();
        }

        /// <summary>
        ///     处理按Ctrl拖曳的事件
        /// </summary>
        /// <returns></returns>
        public async Task<string[]> ExtractFileToTemp(int tag)
        {
            CurSetting.TargetPath = PathFixWin(Environment.GetEnvironmentVariable("TEMP")) + "pvfUtility\\" + tag;
            return await ExtractFile();
        }

        public void ExtractFileFast()
        {
            CurSetting.TargetPath = CurrentPackPathFolder;
//            CurSetting = new ExtractSetting
//            {
//                
//                DecompileBinaryAni = Setting.ExtractDecompileBinaryAni,
//                DecompileScript = Setting.ExtractDecompileScript,
//                OpenFolder = Setting.ExtractOpenFolder,
//                Items = items,
//                
//            };
            Task.Run(ExtractFile);
        }

        public void ExtractFileTo7ZFast()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "选择7zip文件保存位置",
                ValidateNames = true,
                CheckPathExists = true,
                DefaultExt = ".7z",
                Filter = "7z|*.7z"
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
//            var items = path.Select(x => (x, false)).ToList();
//            CurSetting = new ExtractSetting
//            {
//                DecompileBinaryAni = Setting.ExtractDecompileBinaryAni,
//                DecompileScript = Setting.ExtractDecompileScript,
//                OpenFolder = Setting.ExtractOpenFolder,
//                Items = items,
//                TargetPath = CurrentPackPathFolder
//            };
            Task.Run(() => ExtractFileTo7Z(saveFileDialog.FileName));
        }

        private HashSet<PvfFile> GetExtractFiles()
        {
            var list = new HashSet<PvfFile>();
            foreach (var (source, useLike) in CurSetting.Items)
            {
                var file = _pvf.GetFile(source);
                if (file != null)
                {
                    list.Add(file);
                }
                else
                {
                    var path = source;
                    if (!useLike)
                        path = PathFix(source);
                    foreach (var item in _pvf.FileList)
                        if (!useLike && IsPathMatch(item.Key, path))
                            list.Add(item.Value);
                        else if (useLike && IsPathMatchEx(item.Key, path))
                            list.Add(item.Value);
                }
            }

            return list;
        }

        public Task<string[]> ExtractFile()
        {
            var results = new ConcurrentBag<string>();
            MainPresenter.Instance.DoAction("提取文件中...", progress =>
            {
                try
                {
                    var workCount = 0;
                    var list = GetExtractFiles();
                    var progressNum = DataHelper.GetResolve(list.Count);
                    var len = list.Count;

                    Parallel.ForEach(list, item =>
                    {
                        var resultFileName = ExtractFileObj(item);
                        results.Add(resultFileName);
                        workCount++;
                        if (workCount % progressNum == 0)
                            progress.Report(workCount * 100 / len);
                    });

                    if (CurSetting.OpenFolder)
                        Process.Start("Explorer.exe", CurSetting.TargetPath);
                }
                catch (IOException e)
                {
                    DialogService.Error(e.Message, "", MainPresenter.Instance.View.Handle);
                    return Task.FromResult(false);
                }

                return Task.FromResult(true);
            });

            return Task.FromResult(results.ToArray());
        }

        public void ExtractFileTo7Z(string fileName)
        {
            MainPresenter.Instance.DoAction("提取文件到7z中...", async progress =>
            {
                var cmp = new SevenZipCompressor
                {
                    ArchiveFormat = OutArchiveFormat.SevenZip,
                    CompressionMethod = CompressionMethod.BZip2,
                    VolumeSize = 0,
                    CompressionLevel = CompressionLevel.Fast
                };

                var directory = new Dictionary<string, Stream>();

                foreach (var item in GetExtractFiles())
                {
                    var stream = new MemoryStream();
                    if (!_pvf.ExtractFile(stream, item, CurSetting.DecompileBinaryAni, CurSetting.DecompileScript,
                        CurSetting.ConvertConvertSimplifiedChinese))
                        Logger.Error($"提取文件到7z中时发生错误: file://{item.FileName} ");
                    else
                        directory.Add(item.FileName.Replace("/", "\\"), stream);

                    /*                var obj = _pvf.GetFile(source);
                                    if (obj != null)
                                    {
                                    }
                                    else
                                    {
                                        var pathFixed = PathFix(source);
                                        foreach (var item in _pvf.FileList)
                                        {
                                            // ReSharper disable once InvertIf
                                            if (!useLike & IsPathMatch(item.Value.FileName, pathFixed) ||
                                                useLike & IsPathMatchEx(item.Value.FileName, source))
                                            {
                                                var stream = new MemoryStream();
                                                if (!_pvf.ExtractFile(stream, item.Value, CurSetting.DecompileBinaryAni, CurSetting.DecompileScript))
                                                    AppLogger.AddConsoleErrorInfo(string.Format(lang.ExtractFileTo7zError, item.Key));
                                                else
                                                    directory.Add(item.Value.FileName.Replace("/", "\\"), stream);
                                            }
                                        }
                                    }*/
                }

                var progressNum = 0;
                cmp.Compressing += CmpCompressing;
                cmp.CompressionFinished += CmpCompressionFinished;
                await Task.Run(() => cmp.CompressStreamDictionary(directory, fileName));

                void CmpCompressing(object sender, ProgressEventArgs e)
                {
                    progressNum += e.PercentDelta;
                    progress.Report(progressNum);
//                    if (MainPresenter.Instance.View.InvokeRequired)
//                        MainPresenter.Instance.View.BeginInvoke(new MethodInvoker(() =>
//                            MainPresenter.Instance.View.toolStripProgressBar.Increment(e.PercentDelta)));
//                    else
//                        MainPresenter.Instance.View.toolStripProgressBar.Increment();
                }

                void CmpCompressionFinished(object sender, EventArgs e)
                {
                    MainPresenter.Instance.SetStatusText("提取文件到7z完成。");
                    GC.Collect();
                }
            });
        }

        private string ExtractFileObj(PvfFile obj)
        {
            var str = PathFixWin(CurSetting.TargetPath) + obj.FileName.Replace("/", "\\");
            DirectoryExistCheck(str);

            // if (File.Exists(str))
            // {
            //     switch (CurSetting.CurrentFileOperation)
            //     {
            //         // TODO 询问用户如何处理
            //         case FileOperation.Cancel:
            //             throw new Exception("Operation Cancelled");
            //         case FileOperation.OverWrite:
            //             break;
            //         case FileOperation.Rename:
            //             while (File.Exists(str))
            //             {
            //                 str += "(copy)";
            //             }
            //             break;
            //         case FileOperation.Skip:
            //             return;
            //         default:
            //             throw new ArgumentOutOfRangeException();
            //     }
            // }

            using (var streamFile = File.Create(str))
            {
                _pvf.ExtractFile(streamFile, obj, CurSetting.DecompileBinaryAni, CurSetting.DecompileScript,
                    CurSetting.ConvertConvertSimplifiedChinese);
            }

            return str;
        }
    }
}