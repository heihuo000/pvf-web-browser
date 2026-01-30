using System.ComponentModel;
using System.Windows.Forms;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace pvfUtility.Shell.Docks
{
    partial class FileExplorerDock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStripListFileExplorer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExtractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExtractTo7ZToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.ImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.addToFileCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.CutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.EditCommentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.getSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lst实用工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.检查lst错误ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出为代码名称表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.检查未注册项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeNotExistCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeviewFile = new Aga.Controls.Tree.TreeViewAdv();
            this._Icon = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this._Name = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this._Comment = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this._ExtraText = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripTextBoxPath = new System.Windows.Forms.ToolStripTextBox();
            this.contextMenuStripListFileExplorer.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripListFileExplorer
            // 
            this.contextMenuStripListFileExplorer.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripListFileExplorer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.hexOpenToolStripMenuItem,
            this.toolStripMenuItem1,
            this.ExtractToolStripMenuItem,
            this.extractToToolStripMenuItem,
            this.ExtractTo7ZToolStripMenuItem,
            this.toolStripMenuItem5,
            this.ImportToolStripMenuItem,
            this.ImportToToolStripMenuItem,
            this.toolStripMenuItem6,
            this.addToFileCollectionToolStripMenuItem,
            this.toolStripSeparator3,
            this.CutToolStripMenuItem,
            this.CopyToolStripMenuItem,
            this.PasteToolStripMenuItem,
            this.DeleteToolStripMenuItem,
            this.toolStripMenuItem2,
            this.RefreshToolStripMenuItem,
            this.toolStripSeparator1,
            this.RenameToolStripMenuItem,
            this.NewFileToolStripMenuItem,
            this.toolStripMenuItem3,
            this.EditCommentToolStripMenuItem,
            this.NewBookmarkToolStripMenuItem,
            this.toolStripMenuItem4,
            this.getSelectedCodeToolStripMenuItem,
            this.lst实用工具ToolStripMenuItem});
            this.contextMenuStripListFileExplorer.Name = "contextMenuStrip1";
            this.contextMenuStripListFileExplorer.Size = new System.Drawing.Size(209, 492);
            this.contextMenuStripListFileExplorer.Opened += new System.EventHandler(this.contextMenuStripList_Opened);
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.OpenFolder_16x;
            this.OpenToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.OpenToolStripMenuItem.Text = "打开";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // hexOpenToolStripMenuItem
            // 
            this.hexOpenToolStripMenuItem.Name = "hexOpenToolStripMenuItem";
            this.hexOpenToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.hexOpenToolStripMenuItem.Text = "使用十六进制编辑器打开";
            this.hexOpenToolStripMenuItem.Click += new System.EventHandler(this.hexOpenToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(205, 6);
            // 
            // ExtractToolStripMenuItem
            // 
            this.ExtractToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.DownloadFile_16x;
            this.ExtractToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ExtractToolStripMenuItem.Name = "ExtractToolStripMenuItem";
            this.ExtractToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.ExtractToolStripMenuItem.Text = "提取...";
            this.ExtractToolStripMenuItem.Click += new System.EventHandler(this.ExtractToolStripMenuItem_Click);
            // 
            // extractToToolStripMenuItem
            // 
            this.extractToToolStripMenuItem.Name = "extractToToolStripMenuItem";
            this.extractToToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.extractToToolStripMenuItem.Text = "提取到";
            this.extractToToolStripMenuItem.Click += new System.EventHandler(this.ExtractToToolStripMenuItem_ClickAsync);
            // 
            // ExtractTo7ZToolStripMenuItem
            // 
            this.ExtractTo7ZToolStripMenuItem.Name = "ExtractTo7ZToolStripMenuItem";
            this.ExtractTo7ZToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.ExtractTo7ZToolStripMenuItem.Text = "提取到7z压缩包...";
            this.ExtractTo7ZToolStripMenuItem.Click += new System.EventHandler(this.ExtractTo7ZToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(205, 6);
            // 
            // ImportToolStripMenuItem
            // 
            this.ImportToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.ImportFile_16x;
            this.ImportToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ImportToolStripMenuItem.Name = "ImportToolStripMenuItem";
            this.ImportToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.ImportToolStripMenuItem.Text = "导入...";
            this.ImportToolStripMenuItem.Click += new System.EventHandler(this.ImportToolStripMenuItem_Click);
            // 
            // ImportToToolStripMenuItem
            // 
            this.ImportToToolStripMenuItem.Name = "ImportToToolStripMenuItem";
            this.ImportToToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.ImportToToolStripMenuItem.Text = "导入文件到当前文件夹";
            this.ImportToToolStripMenuItem.Click += new System.EventHandler(this.ImportToToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(205, 6);
            // 
            // addToFileCollectionToolStripMenuItem
            // 
            this.addToFileCollectionToolStripMenuItem.Name = "addToFileCollectionToolStripMenuItem";
            this.addToFileCollectionToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.addToFileCollectionToolStripMenuItem.Text = "添加到文件集";
            this.addToFileCollectionToolStripMenuItem.Click += new System.EventHandler(this.AddToFileCollectionToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(205, 6);
            // 
            // CutToolStripMenuItem
            // 
            this.CutToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.Cut_16x;
            this.CutToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CutToolStripMenuItem.Name = "CutToolStripMenuItem";
            this.CutToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.CutToolStripMenuItem.Text = "剪切";
            this.CutToolStripMenuItem.Click += new System.EventHandler(this.CutToolStripMenuItem_Click);
            // 
            // CopyToolStripMenuItem
            // 
            this.CopyToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.Copy_16x;
            this.CopyToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem";
            this.CopyToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.CopyToolStripMenuItem.Text = "复制";
            this.CopyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            // 
            // PasteToolStripMenuItem
            // 
            this.PasteToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.Paste_16x;
            this.PasteToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem";
            this.PasteToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.PasteToolStripMenuItem.Text = "粘贴";
            this.PasteToolStripMenuItem.Click += new System.EventHandler(this.PasteToolStripMenuItem_Click);
            // 
            // DeleteToolStripMenuItem
            // 
            this.DeleteToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.DeleteFolder_16x;
            this.DeleteToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            this.DeleteToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.DeleteToolStripMenuItem.Text = "删除";
            this.DeleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(205, 6);
            // 
            // RefreshToolStripMenuItem
            // 
            this.RefreshToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.Refresh_16x;
            this.RefreshToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            this.RefreshToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.RefreshToolStripMenuItem.Text = "刷新";
            this.RefreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(205, 6);
            // 
            // RenameToolStripMenuItem
            // 
            this.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem";
            this.RenameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.RenameToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.RenameToolStripMenuItem.Text = "重命名...";
            this.RenameToolStripMenuItem.Click += new System.EventHandler(this.RenameToolStripMenuItem_Click);
            // 
            // NewFileToolStripMenuItem
            // 
            this.NewFileToolStripMenuItem.Name = "NewFileToolStripMenuItem";
            this.NewFileToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.NewFileToolStripMenuItem.Text = "新建文件...";
            this.NewFileToolStripMenuItem.Click += new System.EventHandler(this.新建文件ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(205, 6);
            // 
            // EditCommentToolStripMenuItem
            // 
            this.EditCommentToolStripMenuItem.Name = "EditCommentToolStripMenuItem";
            this.EditCommentToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.EditCommentToolStripMenuItem.Text = "编辑注释";
            this.EditCommentToolStripMenuItem.Click += new System.EventHandler(this.EditCommentToolStripMenuItem_Click);
            // 
            // NewBookmarkToolStripMenuItem
            // 
            this.NewBookmarkToolStripMenuItem.Name = "NewBookmarkToolStripMenuItem";
            this.NewBookmarkToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.NewBookmarkToolStripMenuItem.Text = "添加到书签...";
            this.NewBookmarkToolStripMenuItem.Click += new System.EventHandler(this.NewBookmarkToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(205, 6);
            // 
            // getSelectedCodeToolStripMenuItem
            // 
            this.getSelectedCodeToolStripMenuItem.Name = "getSelectedCodeToolStripMenuItem";
            this.getSelectedCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.getSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.getSelectedCodeToolStripMenuItem.Text = "获取选中项代码";
            this.getSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.getSelectedCodeToolStripMenuItem_Click);
            // 
            // lst实用工具ToolStripMenuItem
            // 
            this.lst实用工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.检查lst错误ToolStripMenuItem,
            this.导出为代码名称表ToolStripMenuItem,
            this.检查未注册项ToolStripMenuItem,
            this.removeNotExistCodeToolStripMenuItem});
            this.lst实用工具ToolStripMenuItem.Name = "lst实用工具ToolStripMenuItem";
            this.lst实用工具ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.lst实用工具ToolStripMenuItem.Text = "lst实用工具";
            // 
            // 检查lst错误ToolStripMenuItem
            // 
            this.检查lst错误ToolStripMenuItem.Name = "检查lst错误ToolStripMenuItem";
            this.检查lst错误ToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.检查lst错误ToolStripMenuItem.Text = "检查lst错误(代码重复和文件不存在)";
            this.检查lst错误ToolStripMenuItem.Click += new System.EventHandler(this.检查lst错误ToolStripMenuItem_Click);
            // 
            // 导出为代码名称表ToolStripMenuItem
            // 
            this.导出为代码名称表ToolStripMenuItem.Name = "导出为代码名称表ToolStripMenuItem";
            this.导出为代码名称表ToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.导出为代码名称表ToolStripMenuItem.Text = "导出为\"代码(tab)名称\"表";
            this.导出为代码名称表ToolStripMenuItem.Click += new System.EventHandler(this.导出为代码名称表ToolStripMenuItem_Click);
            // 
            // 检查未注册项ToolStripMenuItem
            // 
            this.检查未注册项ToolStripMenuItem.Name = "检查未注册项ToolStripMenuItem";
            this.检查未注册项ToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.检查未注册项ToolStripMenuItem.Text = "检查未注册进该lst的文件";
            this.检查未注册项ToolStripMenuItem.Click += new System.EventHandler(this.检查未注册项ToolStripMenuItem_Click);
            // 
            // removeNotExistCodeToolStripMenuItem
            // 
            this.removeNotExistCodeToolStripMenuItem.Name = "removeNotExistCodeToolStripMenuItem";
            this.removeNotExistCodeToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.removeNotExistCodeToolStripMenuItem.Text = "删除不存在的代码";
            this.removeNotExistCodeToolStripMenuItem.Click += new System.EventHandler(this.removeNotExistCodeToolStripMenuItem_Click);
            // 
            // treeviewFile
            // 
            this.treeviewFile.BackColor = System.Drawing.SystemColors.Window;
            this.treeviewFile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeviewFile.ColumnHeaderHeight = 0;
            this.treeviewFile.DefaultToolTipProvider = null;
            this.treeviewFile.DisplayDraggingNodes = true;
            this.treeviewFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeviewFile.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeviewFile.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeviewFile.FullRowSelect = true;
            this.treeviewFile.FullRowSelectActiveColor = System.Drawing.SystemColors.Highlight;
            this.treeviewFile.FullRowSelectInactiveColor = System.Drawing.SystemColors.ControlDark;
            this.treeviewFile.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeviewFile.LoadOnDemand = true;
            this.treeviewFile.Location = new System.Drawing.Point(0, 25);
            this.treeviewFile.Margin = new System.Windows.Forms.Padding(4);
            this.treeviewFile.Model = null;
            this.treeviewFile.Name = "treeviewFile";
            this.treeviewFile.NodeControls.Add(this._Icon);
            this.treeviewFile.NodeControls.Add(this._Name);
            this.treeviewFile.NodeControls.Add(this._Comment);
            this.treeviewFile.NodeControls.Add(this._ExtraText);
            this.treeviewFile.NodeFilter = null;
            this.treeviewFile.RowHeight = 20;
            this.treeviewFile.SelectedNode = null;
            this.treeviewFile.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
            this.treeviewFile.ShowLines = false;
            this.treeviewFile.ShowNodeToolTips = true;
            this.treeviewFile.Size = new System.Drawing.Size(256, 311);
            this.treeviewFile.TabIndex = 27;
            this.treeviewFile.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeViewFile_ItemDrag);
            this.treeviewFile.NodeMouseClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this.TreeviewFile_NodeMouseClick);
            this.treeviewFile.NodeMouseDoubleClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this.FileListView_NodeMouseDoubleClick);
            this.treeviewFile.Collapsing += new System.EventHandler<Aga.Controls.Tree.TreeViewAdvEventArgs>(this.treeviewFile_Collapsing);
            this.treeviewFile.Expanding += new System.EventHandler<Aga.Controls.Tree.TreeViewAdvEventArgs>(this.treeviewFile_Expanding);
            this.treeviewFile.Expanded += new System.EventHandler<Aga.Controls.Tree.TreeViewAdvEventArgs>(this.TreeviewFile_Expanded);
            this.treeviewFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.TreeViewFile_DragDrop);
            this.treeviewFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.TreeViewFile_DragEnter);
            this.treeviewFile.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeViewFile_KeyDown);
            this.treeviewFile.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TreeViewFile_KeyUp);
            // 
            // _Icon
            // 
            this._Icon.DataPropertyName = "Icon";
            this._Icon.LeftMargin = 1;
            this._Icon.ParentColumn = null;
            this._Icon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // _Name
            // 
            this._Name.DataPropertyName = "Name";
            this._Name.EditEnabled = true;
            this._Name.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._Name.IncrementalSearchEnabled = true;
            this._Name.LeftMargin = 3;
            this._Name.ParentColumn = null;
            this._Name.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            // 
            // _Comment
            // 
            this._Comment.DataPropertyName = "Comment";
            this._Comment.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this._Comment.IncrementalSearchEnabled = true;
            this._Comment.LeftMargin = 3;
            this._Comment.ParentColumn = null;
            this._Comment.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this._Comment.UseCompatibleTextRendering = true;
            // 
            // _ExtraText
            // 
            this._ExtraText.DataPropertyName = "ExtraText";
            this._ExtraText.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this._ExtraText.IncrementalSearchEnabled = true;
            this._ExtraText.LeftMargin = 3;
            this._ExtraText.ParentColumn = null;
            this._ExtraText.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBoxPath});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(256, 25);
            this.toolStrip.TabIndex = 28;
            this.toolStrip.SizeChanged += new System.EventHandler(this.toolStrip_SizeChanged);
            // 
            // toolStripTextBoxPath
            // 
            this.toolStripTextBoxPath.AutoSize = false;
            this.toolStripTextBoxPath.AutoToolTip = true;
            this.toolStripTextBoxPath.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.toolStripTextBoxPath.Name = "toolStripTextBoxPath";
            this.toolStripTextBoxPath.Size = new System.Drawing.Size(200, 25);
            // 
            // FileExplorerDock
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(256, 336);
            this.Controls.Add(this.treeviewFile);
            this.Controls.Add(this.toolStrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FileExplorerDock";
            this.Text = "文件资源管理器";
            this.contextMenuStripListFileExplorer.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ContextMenuStrip contextMenuStripListFileExplorer;
        private ToolStripMenuItem OpenToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem ExtractToolStripMenuItem;
        private ToolStripMenuItem DeleteToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem NewFileToolStripMenuItem;
        private NodeTextBox _Name;
        private ToolStripMenuItem CutToolStripMenuItem;
        private ToolStripMenuItem CopyToolStripMenuItem;
        private ToolStripMenuItem PasteToolStripMenuItem;
        private ToolStripMenuItem RefreshToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private NodeTextBox _Comment;
        private NodeTextBox _ExtraText;
        private ToolStripMenuItem extractToToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripMenuItem ImportToolStripMenuItem;
        private ToolStripMenuItem ImportToToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripMenuItem RenameToolStripMenuItem;
        private ToolStrip toolStrip;
        private ToolStripTextBox toolStripTextBoxPath;
        private NodeIcon _Icon;
        private ToolStripMenuItem EditCommentToolStripMenuItem;
        private ToolStripMenuItem lst实用工具ToolStripMenuItem;
        private ToolStripMenuItem 检查lst错误ToolStripMenuItem;
        private ToolStripMenuItem 检查未注册项ToolStripMenuItem;
        private ToolStripMenuItem 导出为代码名称表ToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem NewBookmarkToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem ExtractTo7ZToolStripMenuItem;
        private ToolStripMenuItem addToFileCollectionToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem hexOpenToolStripMenuItem;
        private TreeViewAdv treeviewFile;
        private ToolStripMenuItem getSelectedCodeToolStripMenuItem;
        private ToolStripMenuItem removeNotExistCodeToolStripMenuItem;
    }
}