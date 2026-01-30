namespace pvfUtility.Shell.Docks
{
    partial class CollectionExplorerDock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.treeViewCollection = new Aga.Controls.Tree.TreeViewAdv();
            this._Icon = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this._ExtraText = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this._Name = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.contextMenuStripList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExtractToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExtractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.CutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.RemoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExtractAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MaintoolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripComboBoxResultList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonUnzipAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonImport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonBatch = new System.Windows.Forms.ToolStripButton();
            this.NameText = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.contextMenuStripList.SuspendLayout();
            this.MaintoolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewCollection
            // 
            this.treeViewCollection.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewCollection.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewCollection.ColumnHeaderHeight = 0;
            this.treeViewCollection.DefaultToolTipProvider = null;
            this.treeViewCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewCollection.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewCollection.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeViewCollection.FullRowSelect = true;
            this.treeViewCollection.FullRowSelectActiveColor = System.Drawing.SystemColors.Highlight;
            this.treeViewCollection.FullRowSelectInactiveColor = System.Drawing.Color.LightGray;
            this.treeViewCollection.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeViewCollection.LoadOnDemand = true;
            this.treeViewCollection.Location = new System.Drawing.Point(0, 25);
            this.treeViewCollection.Margin = new System.Windows.Forms.Padding(4);
            this.treeViewCollection.Model = null;
            this.treeViewCollection.Name = "treeViewCollection";
            this.treeViewCollection.NodeControls.Add(this._Icon);
            this.treeViewCollection.NodeControls.Add(this._ExtraText);
            this.treeViewCollection.NodeControls.Add(this._Name);
            this.treeViewCollection.NodeFilter = null;
            this.treeViewCollection.RowHeight = 20;
            this.treeViewCollection.SelectedNode = null;
            this.treeViewCollection.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
            this.treeViewCollection.ShowLines = false;
            this.treeViewCollection.ShowNodeToolTips = true;
            this.treeViewCollection.Size = new System.Drawing.Size(297, 316);
            this.treeViewCollection.TabIndex = 28;
            this.treeViewCollection.Text = "  ";
            this.treeViewCollection.NodeMouseClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this.ResultListView_NodeMouseClick);
            this.treeViewCollection.NodeMouseDoubleClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this.TreeViewCollection_NodeMouseDoubleClick);
            // 
            // _Icon
            // 
            this._Icon.DataPropertyName = "Icon";
            this._Icon.LeftMargin = 1;
            this._Icon.ParentColumn = null;
            this._Icon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // _ExtraText
            // 
            this._ExtraText.DataPropertyName = "ExtraText";
            this._ExtraText.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._ExtraText.IncrementalSearchEnabled = true;
            this._ExtraText.LeftMargin = 3;
            this._ExtraText.ParentColumn = null;
            this._ExtraText.UseCompatibleTextRendering = true;
            // 
            // _Name
            // 
            this._Name.DataPropertyName = "Name";
            this._Name.Font = new System.Drawing.Font("HoloLens MDL2 Assets", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._Name.IncrementalSearchEnabled = true;
            this._Name.LeftMargin = 3;
            this._Name.ParentColumn = null;
            this._Name.UseCompatibleTextRendering = true;
            // 
            // contextMenuStripList
            // 
            this.contextMenuStripList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewToolStripMenuItem,
            this.toolStripSeparator3,
            this.ExtractToToolStripMenuItem,
            this.ExtractToolStripMenuItem,
            this.RefreshToolStripMenuItem,
            this.toolStripMenuItem1,
            this.CutToolStripMenuItem,
            this.CopyToolStripMenuItem,
            this.toolStripSeparator5,
            this.RemoveToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripMenuItem3,
            this.ExtractAllToolStripMenuItem});
            this.contextMenuStripList.Name = "contextMenuStrip1";
            this.contextMenuStripList.Size = new System.Drawing.Size(146, 226);
            this.contextMenuStripList.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripList_Opening);
            // 
            // ViewToolStripMenuItem
            // 
            this.ViewToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.OpenFolder_16x;
            this.ViewToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem";
            this.ViewToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.ViewToolStripMenuItem.Text = "查看文件";
            this.ViewToolStripMenuItem.Click += new System.EventHandler(this.ViewToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(142, 6);
            // 
            // ExtractToToolStripMenuItem
            // 
            this.ExtractToToolStripMenuItem.Name = "ExtractToToolStripMenuItem";
            this.ExtractToToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.ExtractToToolStripMenuItem.Text = "提取到";
            this.ExtractToToolStripMenuItem.Click += new System.EventHandler(this.ExtractToToolStripMenuItem_Click);
            // 
            // ExtractToolStripMenuItem
            // 
            this.ExtractToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.DownloadFile_16x;
            this.ExtractToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ExtractToolStripMenuItem.Name = "ExtractToolStripMenuItem";
            this.ExtractToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.ExtractToolStripMenuItem.Text = "提取...";
            this.ExtractToolStripMenuItem.Click += new System.EventHandler(this.ExtractToolStripMenuItem_Click);
            // 
            // RefreshToolStripMenuItem
            // 
            this.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            this.RefreshToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.RefreshToolStripMenuItem.Text = "刷新";
            this.RefreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(142, 6);
            // 
            // CutToolStripMenuItem
            // 
            this.CutToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.Cut_16x;
            this.CutToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.CutToolStripMenuItem.Name = "CutToolStripMenuItem";
            this.CutToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.CutToolStripMenuItem.Text = "剪切";
            this.CutToolStripMenuItem.Click += new System.EventHandler(this.CutToolStripMenuItem_Click);
            // 
            // CopyToolStripMenuItem
            // 
            this.CopyToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.Copy_16x;
            this.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem";
            this.CopyToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.CopyToolStripMenuItem.Text = "复制";
            this.CopyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(142, 6);
            // 
            // RemoveToolStripMenuItem
            // 
            this.RemoveToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.DeleteKPI_16x;
            this.RemoveToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.RemoveToolStripMenuItem.Name = "RemoveToolStripMenuItem";
            this.RemoveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.RemoveToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.RemoveToolStripMenuItem.Text = "移除";
            this.RemoveToolStripMenuItem.Click += new System.EventHandler(this.RemoveToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.deleteToolStripMenuItem.Text = "删除文件";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(142, 6);
            // 
            // ExtractAllToolStripMenuItem
            // 
            this.ExtractAllToolStripMenuItem.Name = "ExtractAllToolStripMenuItem";
            this.ExtractAllToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.ExtractAllToolStripMenuItem.Text = "全部提取";
            this.ExtractAllToolStripMenuItem.Click += new System.EventHandler(this.ExtractAllToolStripMenuItem_Click);
            // 
            // MaintoolStrip
            // 
            this.MaintoolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBoxResultList,
            this.toolStripSeparator1,
            this.toolStripButtonUnzipAll,
            this.toolStripSeparator2,
            this.toolStripButtonImport,
            this.toolStripButtonExport,
            this.toolStripSeparator4,
            this.toolStripButtonSearch,
            this.toolStripButtonBatch});
            this.MaintoolStrip.Location = new System.Drawing.Point(0, 0);
            this.MaintoolStrip.Name = "MaintoolStrip";
            this.MaintoolStrip.Size = new System.Drawing.Size(297, 25);
            this.MaintoolStrip.TabIndex = 30;
            // 
            // toolStripComboBoxResultList
            // 
            this.toolStripComboBoxResultList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxResultList.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripComboBoxResultList.Name = "toolStripComboBoxResultList";
            this.toolStripComboBoxResultList.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxResultList.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxResultList_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonUnzipAll
            // 
            this.toolStripButtonUnzipAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUnzipAll.Image = global::pvfUtility.Properties.Resources.DownloadFile_16x;
            this.toolStripButtonUnzipAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonUnzipAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUnzipAll.Name = "toolStripButtonUnzipAll";
            this.toolStripButtonUnzipAll.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonUnzipAll.Text = "全部解压";
            this.toolStripButtonUnzipAll.Click += new System.EventHandler(this.toolStripButtonUnzipAll_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonImport
            // 
            this.toolStripButtonImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImport.Image = global::pvfUtility.Properties.Resources.ImportFilter_16x;
            this.toolStripButtonImport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImport.Name = "toolStripButtonImport";
            this.toolStripButtonImport.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonImport.Text = "从txt导入";
            this.toolStripButtonImport.Click += new System.EventHandler(this.toolStripButtonImport_Click);
            // 
            // toolStripButtonExport
            // 
            this.toolStripButtonExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExport.Image = global::pvfUtility.Properties.Resources.ExportData_16x;
            this.toolStripButtonExport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExport.Name = "toolStripButtonExport";
            this.toolStripButtonExport.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonExport.Text = "导出到txt";
            this.toolStripButtonExport.Click += new System.EventHandler(this.toolStripButtonExport_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSearch
            // 
            this.toolStripButtonSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSearch.Image = global::pvfUtility.Properties.Resources.FindinFiles_16x;
            this.toolStripButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSearch.Name = "toolStripButtonSearch";
            this.toolStripButtonSearch.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSearch.Text = "搜索";
            this.toolStripButtonSearch.Click += new System.EventHandler(this.toolStripButtonSearch_Click_1);
            // 
            // toolStripButtonBatch
            // 
            this.toolStripButtonBatch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBatch.Image = global::pvfUtility.Properties.Resources.CollapseGroup_16x;
            this.toolStripButtonBatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBatch.Name = "toolStripButtonBatch";
            this.toolStripButtonBatch.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonBatch.Text = "批处理";
            this.toolStripButtonBatch.Click += new System.EventHandler(this.toolStripButtonBatch_Click);
            // 
            // NameText
            // 
            this.NameText.DataPropertyName = "Name";
            this.NameText.IncrementalSearchEnabled = true;
            this.NameText.LeftMargin = 3;
            this.NameText.ParentColumn = null;
            this.NameText.UseCompatibleTextRendering = true;
            // 
            // CollectionExplorerDock
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(297, 341);
            this.Controls.Add(this.treeViewCollection);
            this.Controls.Add(this.MaintoolStrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CollectionExplorerDock";
            this.Text = "文件集浏览器";
            this.contextMenuStripList.ResumeLayout(false);
            this.MaintoolStrip.ResumeLayout(false);
            this.MaintoolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Aga.Controls.Tree.NodeControls.NodeTextBox _Name;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripList;
        private System.Windows.Forms.ToolStripMenuItem ViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStrip MaintoolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonUnzipAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonImport;
        private System.Windows.Forms.ToolStripButton toolStripButtonExport;
        private System.Windows.Forms.ToolStripMenuItem RemoveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem ExtractAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
        private Aga.Controls.Tree.NodeControls.NodeTextBox NameText;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxResultList;
        private System.Windows.Forms.ToolStripMenuItem ExtractToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExtractToolStripMenuItem;
        private Aga.Controls.Tree.NodeControls.NodeTextBox _ExtraText;
        private System.Windows.Forms.ToolStripMenuItem CutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyToolStripMenuItem;
        private Aga.Controls.Tree.NodeControls.NodeIcon _Icon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButtonSearch;
        private System.Windows.Forms.ToolStripButton toolStripButtonBatch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private Aga.Controls.Tree.TreeViewAdv treeViewCollection;
    }
}