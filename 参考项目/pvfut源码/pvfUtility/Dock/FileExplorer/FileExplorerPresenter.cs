using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using pvfUtility.Action.Import;
using pvfUtility.Action.Search;
using pvfUtility.Actions;
using pvfUtility.Document.TextEditor;
using pvfUtility.Helper;
using pvfUtility.Model;
using pvfUtility.Model.PvfOperation;
using pvfUtility.Model.TreeModel;
using pvfUtility.Properties;
using pvfUtility.Service;
using pvfUtility.Shell.Dialogs;
using pvfUtility.Shell.Docks;
using WeifenLuo.WinFormsUI.Docking;
using static pvfUtility.Service.PackService;
using static pvfUtility.Helper.PathsHelper;

namespace pvfUtility.Dock.FileExplorer
{
    internal class FileExplorerPresenter
    {
        public FileExplorerPresenter()
        {
            var x = this;
            View = new FileExplorerDock(this);
        }

        public static FileExplorerPresenter Instance { get; } = new FileExplorerPresenter();

        public FileExplorerDock View { get; }

        public Dictionary<string, TreeFileModel> FileTree { get; set; }

        public string[] CopyFiles { get; set; }

        public bool IsCut { get; set; }

        public string Name => "封包资源管理器";

        public DockContent DockView => View;

        public bool SaveLocation => true;
        public DockState DefaultDockState => DockState.DockLeft;

        /// <summary>
        ///     delete a file or folder
        /// </summary>
        /// <param name="paths"></param>
        public void DeleteFiles(string[] paths)
        {
            var tmp = new StringBuilder();
            foreach (var s in paths)
                tmp.AppendLine(s);
            if (!DialogService.AskYesNo(lang.DeleteConfirm, tmp.ToString(), View.Handle))
                return;
            lock (CurrentPack)
            {
                foreach (var s in paths)
                    if (CurrentPack.GetFile(s) != null)
                    {
                        CurrentPack.FileList.Remove(s);
                        TreeFileHelper.RemoveString2TreeList(s, FileTree);
                    }
                    else
                    {
                        foreach (var x in CurrentPack.GetFiles(s))
                            CurrentPack.FileList.Remove(x);
                        TreeFileHelper.RemoveString2TreeList(s, FileTree);
                    }

                MainPresenter.Instance.View.UpdateFileCount();
            }
        }

        /// <summary>
        ///     删除文件 (无确认)
        /// </summary>
        /// <param name="fileName"></param>
        public string[] DeleteFile(string fileName)
        {
            var list = new List<string>();
            lock (CurrentPack)
            {
                if (CurrentPack.GetFile(fileName) != null)
                {
                    CurrentPack.FileList.Remove(fileName);
                    TreeFileHelper.RemoveString2TreeList(fileName, FileTree);
                    list.Add(fileName);
                }
                else
                {
                    foreach (var x in CurrentPack.GetFiles(fileName))
                    {
                        CurrentPack.FileList.Remove(x);
                        list.Add(x);
                    }

                    TreeFileHelper.RemoveString2TreeList(fileName, FileTree);
                }

                MainPresenter.Instance.View.UpdateFileCount();
            }

            return list.ToArray();
        }

        /// <summary>
        ///     move/copy a file or folder to
        /// </summary>
        /// <param name="files"></param>
        /// <param name="targetPath"></param>
        /// <param name="isMove"></param>
        public void MoveItem(IEnumerable<string> files, string targetPath, bool isMove)
        {
            //var text = cut ? lang.Move : lang.Copy;
            lock (CurrentPack)
            {
                foreach (var s in files)
                {
                    if (isMove)
                        TreeFileHelper.RemoveString2TreeList(s, FileTree);
                    foreach (var item in CurrentPack.MoveFile(s, targetPath, isMove))
                        TreeFileHelper.AddString2TreeList(item, FileTree);
                }

                MainPresenter.Instance.View.UpdateFileCount();
                CopyFiles = null;
            }
        }

        /// <summary>
        ///     rename a file or a folder
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="newName"></param>
        /// <returns>full path name after rename</returns>
        public void RenameItem(string filepath, string newName)
        {
            lock (CurrentPack)
            {
                var file = CurrentPack.GetFile(filepath);
                if (file != null) //改文件名
                {
                    var path = file.PathName;
                    TreeFileHelper.RemoveString2TreeList(file.FileName, FileTree);
                    CurrentPack.FileList.Remove(file.FileName);
                    file.Rename(string.Concat(PathFix(path), newName.ToLower()));
                    CurrentPack.FileList.Add(file.FileName, file);
                    TreeFileHelper.AddString2TreeList(file.FileName, FileTree);
                }
                else
                {
                    var x = filepath.LastIndexOf('/');
                    var path = PathFix(newName);
                    if (x > 0)
                        path = string.Concat(PathFix(filepath.Remove(x)), newName, "/");
                    var old = PathFix(filepath);
                    TreeFileHelper.RemoveString2TreeList(filepath, FileTree);
                    foreach (var y in CurrentPack.GetFileObjs(old))
                    {
                        CurrentPack.FileList.Remove(y.FileName);
                        var nN = string.Concat(path, y.FileName.Remove(0, old.Length));
                        y.Rename(nN.ToLower());
                        CurrentPack.FileList.Add(y.FileName, y);
                        TreeFileHelper.AddString2TreeList(y.FileName, FileTree);
                    }
                }
            }
        }

        public void AddFile(string path)
        {
            var ui = new InputBox("新建文件", $"当前文件夹：{path}\r\n输入新文件名称，可以带“/”组织新的目录。", "文件名：");
            if (ui.ShowDialog(View) != DialogResult.OK) return;
            var text = ui.InputtedText;
            if (text.IndexOf('.') > 0 && !CurrentPack.FileList.ContainsKey(text))
                lock (CurrentPack)
                {
                    var filename = string.Concat(PathFix(path), text);
                    var item = new PvfFile(filename);
                    CurrentPack.FileList.Add(item.FileName, item);
                    TreeFileHelper.AddString2TreeList(filename, FileTree);
                }
            else
                DialogService.Error("请正确输入文件名称，不要与已存在的文件发生冲突。", "", View.Handle);
        }

        /// <summary>
        ///     处理拖曳导入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="item"></param>
        /// <param name="targetPath"></param>
        public void DropImport(string path, string[] item, string targetPath)
        {
            using (var info = new TaskDialog
            {
                OwnerWindowHandle = View.Handle,
                Caption = "导入",
                InstructionText = "",
                Icon = TaskDialogStandardIcon.Information,
                Text = $"即将从\r\n{path}\r\n导入{item.Length}个项目到：\r\n{targetPath}/\r\n若需要修改配置，选择【更改选项...】。"
            })
            {
                var btnOk = new TaskDialogButton("1", "导入");
                btnOk.Click += async (o, args) =>
                {
                    info.Close();
                    await new Importer(CurrentPack, Importer.GetSetting()).ImportFileFast(item, path, targetPath);
                };
                var btnCancel = new TaskDialogButton("2", "取消");
                btnCancel.Click += (o, args) => { info.Close(); };
                var btnMore = new TaskDialogButton("3", "更改选项...");
                btnMore.Click += (o, args) =>
                {
                    new Importer(CurrentPack).ShowDialog(item, path, targetPath);
                    info.Close();
                };
                info.Controls.Add(btnOk);
                info.Controls.Add(btnCancel);
                info.Controls.Add(btnMore);
                info.Show();
            }
        }

        internal void GetSelectedCode(IEnumerable<string> files)
        {
            var sb = new StringBuilder();
            foreach (var item in files)
            {
                var file = CurrentPack.GetFile(item);
                if (file == null)
                {
                    var fileInFolder = CurrentPack.GetFileObjs(item);
                    foreach (var itemInFolder in fileInFolder)
                    {
                        var code = CurrentPack.ListFileTable.GetCode(itemInFolder);
                        if (code > 0) sb.Append(code).Append("\t");
                    }
                }
                else
                {
                    var code = CurrentPack.ListFileTable.GetCode(file);
                    if (code > 0) sb.Append(code).Append("\t");
                }
            }

            var text = sb.ToString();
            if (text.Length > 0)
            {
                text = text.Substring(0, text.Length - 1);
                Clipboard.SetText(text); //修复空文本的时候会复制到剪贴板的问题
            }


            new DetailInfoDialog("以下内容是选中文件(文件夹则是里面的文件)的代码,以Tab分割\r\n已经复制到剪贴板中,按Esc关闭窗口\r\n" + text
            ).Show();
        }

        public async void OnPackOpened()
        {
            await Task.Run(() =>
            {
                FileTree = TreeFileHelper.CreateTreeList(CurrentPack.FileList.Keys.ToList());
                SearchPresenter.Pathmodel = new FolderTreeModel(FileTree);
            });
            View.LoadPack();
        }

        public void OnPackClosed()
        {
            FileTree = null;
            SearchPresenter.Pathmodel = null;
            View.ClosePack();
        }

        #region lst实用工具

        /// <summary>
        ///     检查lst常见错误
        /// </summary>
        /// <param name="file"></param>
        public async void CheckLstError(PvfFile file)
        {
            if (!file.IsListFile)
                return;
            var dat = file.Data;
            var len = file.DataLen;
            var path = PathFix(file.PathName);
            var codelst = new HashSet<int>();
            var sb = new StringBuilder($"在lst文件{file.FileName}中：\r\n");
            await Task.Run(() => //检查错误
            {
                for (var i = 2; i < len; i += 10)
                {
                    var code = BitConverter.ToInt32(dat, i + 1);
                    if (codelst.Contains(code))
                        sb.AppendLine($"代码重复：{code}");
                    else
                        codelst.Add(code);
                    var fileName = string.Concat(path,
                        CurrentPack.Strtable.GetStringItem(BitConverter.ToInt32(dat, i + 1 + 5)).ToLower());
                    var fileItem = CurrentPack.GetFile(fileName);
                    if (fileItem == null) sb.AppendLine($"文件不存在：{fileName}");
                }
            });
            new DetailInfoDialog(sb.ToString()).Show();
        }

        /// <summary>
        ///     导出为代码 名称表格
        /// </summary>
        /// <param name="file"></param>
        public async void ExportLstData(PvfFile file)
        {
            if (!file.IsListFile)
                return;
            var path = PathFix(file.PathName);
            var dat = file.Data;
            var len = file.DataLen;
            var filelst = new List<(int, string)>();
            var sb = new StringBuilder($"在lst文件{file.FileName}中：\r\n");
            await Task.Run(() => //生成表格
            {
                for (var i = 2; i < len; i += 10)
                    filelst.Add((BitConverter.ToInt32(dat, i + 1),
                        string.Concat(path, CurrentPack.Strtable.GetStringItem(
                            BitConverter.ToInt32(dat, i + 1 + 5)))));
                foreach ((var code, var pfile) in filelst)
                {
                    var fileItem = CurrentPack.GetFile(pfile);
                    if (fileItem == null)
                    {
                        Logger.Error($"代码{code}的文件 file://{pfile} 不存在");
                        continue;
                    }

                    sb.AppendLine($"{code}\t{CurrentPack.GetName(fileItem)}");
                }
            });
            new DetailInfoDialog(sb.ToString()).Show();
        }

        /// <summary>
        ///     检查未注册的项目
        /// </summary>
        /// <param name="file"></param>
        public async void CheckUnregItem(PvfFile file)
        {
            if (!file.IsListFile)
                return;
            var path = PathFix(file.PathName);
            var lst = CurrentPack.GetFiles(path);
            var dat = file.Data;
            var len = file.DataLen;
            var filelst = new List<string>();
            var sb = new StringBuilder($"在lst文件{file.FileName}中未注册项有：\r\n");
            var last = BitConverter.ToInt32(dat, len - 9);
            var tmp = CurrentPack.Strtable.GetStringItem(BitConverter.ToInt32(dat, 2 + 1 + 5)).ToLower();
            var ext = tmp.Substring(tmp.LastIndexOf('.')); //拓展名

            await Task.Run(() =>
            {
                for (var i = 2; i < len; i += 10)
                    filelst.Add(string.Concat(path,
                        CurrentPack.Strtable.GetStringItem(BitConverter.ToInt32(dat, i + 1 + 5)).ToLower())); //写出已有的表
                foreach (var item in lst.Except(filelst).ToArray())
                {
                    if (!CurrentPack.FileList[item].IsScriptFile || CurrentPack.FileList[item]
                        .FileName.Substring(CurrentPack.FileList[item].FileName.LastIndexOf('.')) != ext)
                        continue;
                    last++;
                    sb.AppendLine($"{last}\t`{item}`");
                }
            });
            new DetailInfoDialog(sb.ToString()).Show();
        }

        internal void RemoveNotExistFile(string basePath)
        {
            if (!CurrentPack.ListFileTable.FileCodeList.ContainsKey(basePath))
            {
                Logger.Info("非常规的lst文件.");
                return;
            }

            var codeDic = CurrentPack.ListFileTable.FileCodeList[basePath];
            var filesNotExist = codeDic.Where(x => CurrentPack.GetFile(x.Item1) == null)
                .Select(x => x).ToArray();
            foreach (var item in filesNotExist)
            {
                codeDic.Remove(item);
                Logger.Error($"删除了不存在于{basePath}的文件及其代码: {item}");
            }
            // Saved

            SaveList(basePath);
        }

        private void SaveList(string basePath)
        {
            var codeDic = CurrentPack.ListFileTable.FileCodeList[basePath];
            var stringBuilder = new StringBuilder("#PVF_File\r\n");
            foreach (var (fileName, code) in codeDic)
                stringBuilder.AppendLine($"{code}\t`{fileName.Substring(PathFix(basePath).Length)}`");

            var file = CurrentPack.GetFile($"{basePath}/{basePath}.lst");

            TextEditorPresenter.Instance.SaveFileAsScript(file, stringBuilder.ToString(), CurrentEncodingType);
        }

        #endregion
    }
}