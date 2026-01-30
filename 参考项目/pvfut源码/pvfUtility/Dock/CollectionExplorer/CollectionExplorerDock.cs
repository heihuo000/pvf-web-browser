using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Aga.Controls.Tree;
using pvfUtility.Action;
using pvfUtility.Action.Batch;
using pvfUtility.Action.Extract;
using pvfUtility.Action.Search;
using pvfUtility.Dock.CollectionExplorer;
using pvfUtility.Dock.FileExplorer;
using pvfUtility.Document.TextEditor;
using pvfUtility.Helper;
using pvfUtility.Model;
using pvfUtility.Model.TreeModel;
using pvfUtility.Service;
using WeifenLuo.WinFormsUI.Docking;

namespace pvfUtility.Shell.Docks
{
    internal partial class CollectionExplorerDock : DockContent
    {
        private readonly CollectionExplorerPresenter _presenter;

        public CollectionExplorerDock(CollectionExplorerPresenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            MainPresenter.Instance.View.vs.SetStyle(contextMenuStripList,
                VisualStudioToolStripExtender.VsVersion.Vs2015, MainPresenter.Instance.View.Theme);
            MainPresenter.Instance.View.vs.SetStyle(MaintoolStrip, VisualStudioToolStripExtender.VsVersion.Vs2015,
                MainPresenter.Instance.View.Theme);
            treeViewCollection.BackColor = Color.FromArgb(245, 245, 245);
            treeViewCollection.FullRowSelectActiveColor = Color.FromArgb(0, 122, 204);
            treeViewCollection.FullRowSelectInactiveColor = Color.FromArgb(204, 206, 219);
            if (MainPresenter.Instance.View.IsDark)
            {
                treeViewCollection.BackColor = Color.FromArgb(37, 37, 37);
                treeViewCollection.ForeColor = Color.FromArgb(241, 241, 241);
                treeViewCollection.FullRowSelectInactiveColor = Color.FromArgb(63, 63, 70);
            }
        }


        public void AddFileCollection(FileCollectionData fileCollection)
        {
            toolStripComboBoxResultList.SelectedIndex = toolStripComboBoxResultList.Items.Add(fileCollection.Name);
        }

        /// <summary>
        ///     从当前文件集中删除文件
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newIndex"></param>
        public void RemoveFileCollection(int index, int newIndex)
        {
            toolStripComboBoxResultList.Items.RemoveAt(index);
            toolStripComboBoxResultList.SelectedIndex = newIndex;
            ShowSearchResult();
        }

        /// <summary>
        ///     /切换文件集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBoxResultList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBoxResultList.SelectedIndex < _presenter.FileCollections.Count)
            {
                _presenter.ChangeCurFileCollection(toolStripComboBoxResultList.SelectedIndex);
                ShowSearchResult();
            }
        }

        /// <summary>
        ///     显示搜索结果
        /// </summary>
        public void ShowSearchResult()
        {
            if (_presenter.FileCollections.Count == 0)
            {
                this.BeginDo(() => treeViewCollection.Model = null);
            }
            else
            {
                _presenter.FileCollectionTreeModel =
                    new FileTreeViewModel(_presenter.CurFileCollection.FileList.ToList());
                this.BeginDo(() => treeViewCollection.Model = _presenter.FileCollectionTreeModel);
                if (_presenter.CurFileCollection.FileList.Count < 300)
                    this.BeginDo(() => treeViewCollection.ExpandAll());
            }
        }

        /// <summary>
        ///     取选择项
        /// </summary>
        public string[] GetSelectedNode()
        {
            var selectedNode = treeViewCollection.SelectedNodes;
            if (selectedNode == null)
                return null;
            var x = new string[selectedNode.Count];
            for (var i = 0; i < x.Length; i++)
                x[i] = selectedNode[i].ToString();
            return x;
        }

        private void contextMenuStripList_Opening(object sender, CancelEventArgs e)
        {
            ExtractToToolStripMenuItem.Text = $"提取到{AppCore.GetPvfPath()}...";
        }

        private void ExtractToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lst = _presenter.GetRealFileList(GetSelectedNode());
            if (lst == null) return;
            new Extractor(PackService.CurrentPack, Extractor.GetSetting(), lst).ExtractFileFast();
        }

        private void ExtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lst = _presenter.GetRealFileList(GetSelectedNode());
            if (lst == null) return;
            new Extractor(PackService.CurrentPack).ShowDialog(lst);
        }

        private void ResultListView_NodeMouseClick(object sender, TreeNodeAdvMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && treeViewCollection.SelectedNode != null)
                contextMenuStripList.Show(treeViewCollection, e.X, e.Y);
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileExplorerPresenter.Instance.CopyFiles = _presenter.GetRealFileList(GetSelectedNode());
            FileExplorerPresenter.Instance.IsCut = true;
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileExplorerPresenter.Instance.CopyFiles = _presenter.GetRealFileList(GetSelectedNode());
            FileExplorerPresenter.Instance.IsCut = false;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileExplorerPresenter.Instance.DeleteFiles(GetSelectedNode());
        }

        private void toolStripButtonBatch_Click(object sender, EventArgs e)
        {
            new BatchPresenter().ShowDialog();
        }

        private void toolStripButtonSearch_Click_1(object sender, EventArgs e)
        {
            new SearchPresenter(PackService.CurrentPack).ShowDialog();
        }

        #region Menu & Toolbar

        private void toolStripButtonImport_Click(object sender, EventArgs e)
        {
            _presenter.ImportAsNewResultData();
        }

        private void toolStripButtonExport_Click(object sender, EventArgs e)
        {
            _presenter.CurFileCollection.ExportToTxt();
        }

        private void toolStripButtonUnzipAll_Click(object sender, EventArgs e)
        {
            _presenter.ExtractAllFiles();
        }

        private void ExtractAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.ExtractAllFiles();
        }

        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lst0 = GetSelectedNode();
            var lst = _presenter.GetRealFileList(GetSelectedNode());
            if (lst == null) return;
            var currentPath = treeViewCollection.GetPath(treeViewCollection.SelectedNode.Parent);
            foreach (var str in lst)
                _presenter.CurFileCollection.FileList.Remove(str);
            foreach (var str in lst0)
                TreeFileHelper.RemoveString2TreeList(str, _presenter.FileCollectionTreeModel.FileListTree);
            _presenter.FileCollectionTreeModel.RefreshTree(currentPath);
        }

        /// <summary>
        ///     查看文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeViewCollection.SelectedNode == null) return;
            if (treeViewCollection.SelectedNode.Tag is FileTreeViewNode obj && obj.IsFolder)
            {
                await treeViewCollection.SelectedNode.Expand();
            }
            else
            {
                if (PackService.CurrentPack == null)
                    DialogService.Error("在访问文件之前,请先打开一个封包", "", Handle);
                else
                    TextEditorPresenter.Instance.CreateInternal(treeViewCollection.SelectedNode.ToString(), true);
            }
        }

        private void TreeViewCollection_NodeMouseDoubleClick(object sender, TreeNodeAdvMouseEventArgs e)
        {
            ViewToolStripMenuItem_Click(this, e);
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeViewCollection.SelectedNode == null) return;
            var nodes = treeViewCollection.GetPath(treeViewCollection.SelectedNode.Parent);
            _presenter.FileCollectionTreeModel.RefreshTree(nodes);
        }

        #endregion Menu & Toolbar
    }
}