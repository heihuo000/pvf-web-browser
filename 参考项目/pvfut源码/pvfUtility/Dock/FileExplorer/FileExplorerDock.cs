using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aga.Controls.Tree;
using pvfUtility.Action;
using pvfUtility.Action.Extract;
using pvfUtility.Action.Import;
using pvfUtility.Dock.CollectionExplorer;
using pvfUtility.Dock.FileExplorer;
using pvfUtility.Document.TextEditor;
using pvfUtility.FileExplorer.Bookmark;
using pvfUtility.Helper;
using pvfUtility.Model.TreeModel;
using pvfUtility.Service;
using pvfUtility.Shell.Dialogs;
using WeifenLuo.WinFormsUI.Docking;

namespace pvfUtility.Shell.Docks
{
    /// <summary>
    /// </summary>
    internal partial class FileExplorerDock : DockContent
    {
        private readonly FileExplorerPresenter _presenter;

        /// <summary>
        ///     文件资源模型
        /// </summary>
        private FileTreeViewModel _treeModel;

        public FileExplorerDock(FileExplorerPresenter fileExplorerPresenter)
        {
            _presenter = fileExplorerPresenter;
            InitializeComponent();
            MainPresenter.Instance.View.vs.SetStyle(toolStrip, VisualStudioToolStripExtender.VsVersion.Vs2015,
                MainPresenter.Instance.View.Theme);
            MainPresenter.Instance.View.vs.SetStyle(contextMenuStripListFileExplorer,
                VisualStudioToolStripExtender.VsVersion.Vs2015, MainPresenter.Instance.View.Theme);

            treeviewFile.BackColor = Color.FromArgb(245, 245, 245);
            treeviewFile.LineColor = Color.FromArgb(37, 37, 37);
            treeviewFile.FullRowSelectActiveColor = Color.FromArgb(0, 122, 204);
            treeviewFile.FullRowSelectInactiveColor = Color.FromArgb(204, 206, 219);
            if (MainPresenter.Instance.View.IsDark)
            {
                treeviewFile.BackColor = Color.FromArgb(37, 37, 37);
                treeviewFile.ForeColor = Color.FromArgb(241, 241, 241);
                treeviewFile.FullRowSelectInactiveColor = Color.FromArgb(63, 63, 70);
                toolStripTextBoxPath.ForeColor = Color.FromArgb(241, 241, 241);
                toolStripTextBoxPath.BackColor = Color.FromArgb(63, 63, 70);
            }
        }

        public void LoadPack()
        {
            _treeModel = new FileTreeViewModel(FileExplorerPresenter.Instance.FileTree);
            this.BeginDo(() =>
            {
                treeviewFile.AllowDrop = true;
                treeviewFile.Model = _treeModel;
                TabPageContextMenuStrip = contextMenuStripListFileExplorer;
            });
        }

        public void ClosePack()
        {
            this.BeginDo(() =>
            {
                treeviewFile.Model = null;
                treeviewFile.AllowDrop = false;
                TabPageContextMenuStrip = null;
            });
            _treeModel = null;
        }

        /// <summary>
        ///     取选择项
        /// </summary>
        public string[] GetSelectedNode()
        {
            var selectedNode = treeviewFile.SelectedNodes;
            if (selectedNode == null)
                return null;
            var x = new string[selectedNode.Count];
            for (var i = 0; i < x.Length; i++)
                x[i] = selectedNode[i].ToString();
            return x;
        }

        /// <summary>
        ///     取最近的文件夹名称。选择文件夹则返回它，选择文件则返回父对象。
        /// </summary>
        public string GetFolderNode(TreeNodeAdv node)
        {
            if (node == null)
                return "";
            if (treeviewFile.SelectedNode?.Tag is FileTreeViewNode obj && !obj.IsFolder)
                return node.Parent.ToString() == typeof(TreeNodeAdv).ToString() ? "" : node.Parent.ToString();
            return node.ToString() == typeof(TreeNodeAdv).ToString() ? "" : node.ToString();
        }

        /// <summary>
        ///     Update Node if it was opened
        /// </summary>
        /// <param name="path"></param>
        public void UpdateNode(string path)
        {
            if (path.IndexOf('/') < 0)
            {
                FileExplorerPresenter.Instance.View._treeModel.Refresh();
            }
            else
            {
                char[] chrArray = {'\\', '/'};
                var strArrays = path.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
                var root = treeviewFile.FindNodeByTag(new FileTreeViewNode(strArrays[0]));
                if (!root.IsExpanded)
                    return;
                var str = strArrays[0];

                for (var i = 1; i < strArrays.Length; i++)
                {
                    str = string.Concat(str, '/', strArrays[i]);
                    var item = treeviewFile.FindNodeByTag(root, new FileTreeViewNode(str));
                    if (item == null || !item.IsExpandedOnce) return;
                    root = item;
                }

                this.BeginDo(() =>
                {
                    FileExplorerPresenter.Instance.View._treeModel.RefreshTree(treeviewFile.GetPath(root));
                });
            }
        }

        /// <summary>
        ///     set selected note to...
        /// </summary>
        /// <param name="path"></param>
        public async void SetNodeTo(string path)
        {
            try
            {
                if (path.IndexOf('/') < 0)
                {
                    var root = treeviewFile.FindNodeByTag(new FileTreeViewNode(path));
                    treeviewFile.SelectedNode = root;
                    await root.Expand();
                }
                else
                {
                    char[] chrArray = {'\\', '/'};
                    var strArrays = path.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
                    var root = treeviewFile.FindNodeByTag(new FileTreeViewNode(strArrays[0]));
                    await root.Expand();
                    var str = strArrays[0];
                    for (var i = 1; i < strArrays.Length; i++)
                    {
                        str = string.Concat(str, '/', strArrays[i]);
                        var item = treeviewFile.FindNodeByTag(root, new FileTreeViewNode(str));
                        if (item == null) continue;
                        await item.Expand();
                        root = item;
                        treeviewFile.SelectedNode = item;
                    }
                    //root.Expand();
                }
            }
            catch
            {
                // ignored
            }
        }

        private void RefreshTree()
        {
            try
            {
                _treeModel.RefreshTree(treeviewFile.GetPath(treeviewFile.SelectedNode.Parent));
            }
            catch
            {
                // ignored
            }
        }

        private void toolStrip_SizeChanged(object sender, EventArgs e)
        {
            toolStripTextBoxPath.Width = Width - 20;
        }

        private void getSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(treeviewFile.SelectedNode?.Tag is FileTreeViewNode obj))
                return;
            FileExplorerPresenter.Instance.GetSelectedCode(GetSelectedNode());
        }

        private void removeNotExistCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(treeviewFile.SelectedNode?.Tag is FileTreeViewNode obj))
                return;
            FileExplorerPresenter.Instance.RemoveNotExistFile(obj.ItemPath.Substring(0, obj.ItemPath.IndexOf('/')));
        }

        #region 菜单

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(treeviewFile.SelectedNode?.Tag is FileTreeViewNode obj))
                return;
            if (obj.IsFolder)
                treeviewFile.SelectedNode?.Expand();
            else
                TextEditorPresenter.Instance.CreateInternal(obj.ItemPath);
        }

        private void hexOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(treeviewFile.SelectedNode?.Tag is FileTreeViewNode obj))
                return;
            if (obj.IsFolder)
                treeviewFile.SelectedNode?.Expand();
            else
                TextEditorPresenter.Instance.CreateInternal(obj.ItemPath, false, true);
        }

        private void FileListView_NodeMouseDoubleClick(object sender, TreeNodeAdvMouseEventArgs e)
        {
            OpenToolStripMenuItem_Click(sender, e);
        }

        private void contextMenuStripList_Opened(object sender, EventArgs e)
        {
            extractToToolStripMenuItem.Text = $"提取到{AppCore.GetPvfPath()}";
            ImportToToolStripMenuItem.Text = $"导入到{toolStripTextBoxPath.Text}...";
            PasteToolStripMenuItem.Enabled = FileExplorerPresenter.Instance.CopyFiles != null;
        }

        private void ExtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Extractor(PackService.CurrentPack, Extractor.GetSetting()).ShowDialog(GetSelectedNode());
        }

        private void ExtractToToolStripMenuItem_ClickAsync(object sender, EventArgs e)
        {
            new Extractor(PackService.CurrentPack, Extractor.GetSetting(), GetSelectedNode()).ExtractFileFast();
        }

        private void ExtractTo7ZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Extractor(PackService.CurrentPack, Extractor.GetSetting(), GetSelectedNode()).ExtractFileTo7ZFast();
        }

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Importer(PackService.CurrentPack).ShowDialog(null, null, toolStripTextBoxPath.Text);
        }

        private void ImportToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog
            {
                Title = "导入文件到当前文件夹",
                ValidateNames = true,
                CheckPathExists = true,
                Multiselect = true,
                CheckFileExists = true,
                Filter = "All Files|*.*"
            })
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                var lst = openFileDialog.FileNames.ToArray();
                var path = lst[0].Remove(lst[0].LastIndexOf('\\'));
                new Importer(PackService.CurrentPack, Importer.GetSetting())
                    .ShowDialog(lst, path, toolStripTextBoxPath.Text);
            }
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!treeviewFile.Focused) return;
            FileExplorerPresenter.Instance.IsCut = true;
            FileExplorerPresenter.Instance.CopyFiles = GetSelectedNode();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!treeviewFile.Focused) return;
            FileExplorerPresenter.Instance.CopyFiles = GetSelectedNode();
            FileExplorerPresenter.Instance.IsCut = false;
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!treeviewFile.Focused) return;
            FileExplorerPresenter.Instance.MoveItem(FileExplorerPresenter.Instance.CopyFiles,
                GetFolderNode(treeviewFile.SelectedNode), FileExplorerPresenter.Instance.IsCut);
            RefreshTree();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!treeviewFile.Focused) return;
            FileExplorerPresenter.Instance.DeleteFiles(GetSelectedNode());
            RefreshTree();
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!treeviewFile.Focused) return;
            RefreshTree();
        }

        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = treeviewFile.SelectedNode;
            if (node.Tag is FileTreeViewNode obj)
            {
                var ui = new InputBox("重命名", $"输入`{node}`的新名称。", "文件名：", obj.File?.ShortName);
                if (ui.ShowDialog(this) != DialogResult.OK)
                    return;
                FileExplorerPresenter.Instance.RenameItem(obj.ItemPath, ui.InputtedText);
                RefreshTree();
            }
        }

        private void EditCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!treeviewFile.Focused)
                return;
            FileNameCommentService.Edit(treeviewFile.SelectedNode?.ToString());
        }

        private void NewBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!treeviewFile.Focused) return;
            new NewBookmarkDialog(GetSelectedNode()[0]).ShowDialog(this);
        }

        private void 新建文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileExplorerPresenter.Instance.AddFile(GetFolderNode(treeviewFile.SelectedNode));
            RefreshTree();
        }

        private void AddToFileCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in GetSelectedNode())
                if (PackService.CurrentPack.GetFile(item) != null)
                    CollectionExplorerPresenter.Instance.AddFileToCurrentCollection(item);
                else
                    foreach (var i in PackService.CurrentPack.GetFiles(item))
                        CollectionExplorerPresenter.Instance.AddFileToCurrentCollection(i);
            CollectionExplorerPresenter.Instance.View.ShowSearchResult();
        }

        #endregion

        #region TreeView

        private void TreeViewFile_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
        }

        /// <summary>
        ///     拖曳导入文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewFile_DragDrop(object sender, DragEventArgs e)
        {
            var item = (string[]) e.Data.GetData(DataFormats.FileDrop);
            var path = item[0].Remove(item[0].LastIndexOf('\\'));
            _presenter.DropImport(path, item, toolStripTextBoxPath.Text);
        }

        /// <summary>
        ///     处理按Ctrl拖曳提取文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TreeViewFile_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            if (!_canDoDrop)
                return;
            var items = GetSelectedNode();
            var tag = new Random().Next();
            await Task.Run(() => new Extractor(PackService.CurrentPack, Extractor.GetSetting(),
                items).ExtractFileToTemp(tag)); //先解压到临时目录下

            var tempPath = PathsHelper.PathFixWin(Environment.GetEnvironmentVariable("TEMP")) + "pvfUtility\\" + tag +
                           "\\";
            for (var i = 0; i < items.Length; i++)
                items[i] = string.Concat(tempPath, items[i].Replace('/', '\\'));

            var data = new DataObject(DataFormats.FileDrop);
            var files = new StringCollection();
            files.AddRange(items);
            data.SetFileDropList(files);
            DoDragDrop(data, DragDropEffects.Copy); // Do DragDrop
        }

        private bool _canDoDrop;

        /// <summary>
        ///     快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
                _canDoDrop = true;
            if (e.Control & (e.KeyCode == Keys.X))
                CutToolStripMenuItem_Click(this, e);
            else if (e.Control & (e.KeyCode == Keys.C))
                CopyToolStripMenuItem_Click(this, e);
            else if (e.Control & (e.KeyCode == Keys.V))
                PasteToolStripMenuItem_Click(this, e);
            else if (e.Control & (e.KeyCode == Keys.E))
                getSelectedCodeToolStripMenuItem_Click(this, e);
            else
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        DeleteToolStripMenuItem_Click(this, e);
                        break;
                    case Keys.F5:
                        RefreshToolStripMenuItem_Click(this, e);
                        break;
                }
        }

        /// <summary>
        ///     Ctrl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewFile_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
                _canDoDrop = false;
        }

        private void TreeviewFile_NodeMouseClick(object sender, TreeNodeAdvMouseEventArgs e)
        {
            toolStripTextBoxPath.Text = GetFolderNode(e.Node);
            if (e.Button == MouseButtons.Right && treeviewFile.SelectedNode != null)
                contextMenuStripListFileExplorer.Show(treeviewFile, e.X, e.Y);
        }

        private void TreeviewFile_Expanded(object sender, TreeViewAdvEventArgs e)
        {
            this.Do(() => toolStripTextBoxPath.Text = GetFolderNode(e.Node));
        }

        private void treeviewFile_Collapsing(object sender, TreeViewAdvEventArgs e)
        {
            if (e.Node.Tag is FileTreeViewNode node)
                node.IsExpanded = false;
        }

        private void treeviewFile_Expanding(object sender, TreeViewAdvEventArgs e)
        {
            if (e.Node.Tag is FileTreeViewNode node)
                node.IsExpanded = true;
        }

        #endregion

        #region Lst实用工具

        private void 检查lst错误ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(treeviewFile.SelectedNode?.Tag is FileTreeViewNode obj))
                return;
            FileExplorerPresenter.Instance.CheckLstError(obj.File);
        }

        private void 检查未注册项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(treeviewFile.SelectedNode?.Tag is FileTreeViewNode obj))
                return;
            FileExplorerPresenter.Instance.CheckUnregItem(obj.File);
        }

        private void 导出为代码名称表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(treeviewFile.SelectedNode?.Tag is FileTreeViewNode obj)) return;
            FileExplorerPresenter.Instance.ExportLstData(obj.File);
        }

        #endregion
    }
}