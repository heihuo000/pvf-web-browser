namespace pvfUtility.Shell.Document
{
    partial class DummyPackComparer
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
            this.tab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.MainListView = new Aga.Controls.Tree.TreeViewAdv();
            this._name = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.OutListView = new Aga.Controls.Tree.TreeViewAdv();
            this.@__name = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.panel = new System.Windows.Forms.Panel();
            this.buttonUnzipMain = new System.Windows.Forms.Button();
            this.buttonUnzipOut = new System.Windows.Forms.Button();
            this.labelOut = new System.Windows.Forms.Label();
            this.labelMain = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.FileDifferent = new Aga.Controls.Tree.TreeViewAdv();
            this.___name = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonUnzipMainDiff = new System.Windows.Forms.Button();
            this.labeldiff = new System.Windows.Forms.Label();
            this.buttonUnzipOutDiff = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOpenPVF = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonStartCompare = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonReset = new System.Windows.Forms.ToolStripButton();
            this.tab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab
            // 
            this.tab.Controls.Add(this.tabPage1);
            this.tab.Controls.Add(this.tabPage2);
            this.tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab.Location = new System.Drawing.Point(0, 25);
            this.tab.Margin = new System.Windows.Forms.Padding(2);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(551, 289);
            this.tab.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.MainListView);
            this.tabPage1.Controls.Add(this.splitter1);
            this.tabPage1.Controls.Add(this.OutListView);
            this.tabPage1.Controls.Add(this.panel);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(543, 259);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "文件差异";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // MainListView
            // 
            this.MainListView.AsyncExpanding = true;
            this.MainListView.BackColor = System.Drawing.SystemColors.Window;
            this.MainListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainListView.ColumnHeaderHeight = 0;
            this.MainListView.DefaultToolTipProvider = null;
            this.MainListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainListView.DragDropMarkColor = System.Drawing.Color.Black;
            this.MainListView.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainListView.FullRowSelect = true;
            this.MainListView.FullRowSelectActiveColor = System.Drawing.Color.DodgerBlue;
            this.MainListView.FullRowSelectInactiveColor = System.Drawing.Color.Silver;
            this.MainListView.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
            this.MainListView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.MainListView.LoadOnDemand = true;
            this.MainListView.Location = new System.Drawing.Point(278, 2);
            this.MainListView.Model = null;
            this.MainListView.Name = "MainListView";
            this.MainListView.NodeControls.Add(this._name);
            this.MainListView.NodeFilter = null;
            this.MainListView.SelectedNode = null;
            this.MainListView.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
            this.MainListView.ShowNodeToolTips = true;
            this.MainListView.Size = new System.Drawing.Size(263, 222);
            this.MainListView.TabIndex = 28;
            // 
            // _name
            // 
            this._name.DataPropertyName = "Name";
            this._name.IncrementalSearchEnabled = true;
            this._name.LeftMargin = 3;
            this._name.ParentColumn = null;
            this._name.UseCompatibleTextRendering = true;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(268, 2);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 222);
            this.splitter1.TabIndex = 31;
            this.splitter1.TabStop = false;
            // 
            // OutListView
            // 
            this.OutListView.AsyncExpanding = true;
            this.OutListView.BackColor = System.Drawing.SystemColors.Window;
            this.OutListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OutListView.ColumnHeaderHeight = 0;
            this.OutListView.DefaultToolTipProvider = null;
            this.OutListView.Dock = System.Windows.Forms.DockStyle.Left;
            this.OutListView.DragDropMarkColor = System.Drawing.Color.Black;
            this.OutListView.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OutListView.FullRowSelect = true;
            this.OutListView.FullRowSelectActiveColor = System.Drawing.Color.DodgerBlue;
            this.OutListView.FullRowSelectInactiveColor = System.Drawing.Color.Silver;
            this.OutListView.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
            this.OutListView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.OutListView.LoadOnDemand = true;
            this.OutListView.Location = new System.Drawing.Point(2, 2);
            this.OutListView.Model = null;
            this.OutListView.Name = "OutListView";
            this.OutListView.NodeControls.Add(this.@__name);
            this.OutListView.NodeFilter = null;
            this.OutListView.SelectedNode = null;
            this.OutListView.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
            this.OutListView.ShowNodeToolTips = true;
            this.OutListView.Size = new System.Drawing.Size(266, 222);
            this.OutListView.TabIndex = 29;
            // 
            // __name
            // 
            this.@__name.DataPropertyName = "Name";
            this.@__name.IncrementalSearchEnabled = true;
            this.@__name.LeftMargin = 3;
            this.@__name.ParentColumn = null;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.buttonUnzipMain);
            this.panel.Controls.Add(this.buttonUnzipOut);
            this.panel.Controls.Add(this.labelOut);
            this.panel.Controls.Add(this.labelMain);
            this.panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel.Location = new System.Drawing.Point(2, 224);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(539, 33);
            this.panel.TabIndex = 30;
            // 
            // buttonUnzipMain
            // 
            this.buttonUnzipMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUnzipMain.Enabled = false;
            this.buttonUnzipMain.Location = new System.Drawing.Point(451, 6);
            this.buttonUnzipMain.Name = "buttonUnzipMain";
            this.buttonUnzipMain.Size = new System.Drawing.Size(87, 26);
            this.buttonUnzipMain.TabIndex = 30;
            this.buttonUnzipMain.Text = "解压";
            this.buttonUnzipMain.UseVisualStyleBackColor = true;
            this.buttonUnzipMain.Click += new System.EventHandler(this.buttonUnzipMain_Click);
            // 
            // buttonUnzipOut
            // 
            this.buttonUnzipOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUnzipOut.Enabled = false;
            this.buttonUnzipOut.Location = new System.Drawing.Point(179, 6);
            this.buttonUnzipOut.Name = "buttonUnzipOut";
            this.buttonUnzipOut.Size = new System.Drawing.Size(87, 26);
            this.buttonUnzipOut.TabIndex = 31;
            this.buttonUnzipOut.Text = "解压";
            this.buttonUnzipOut.UseVisualStyleBackColor = true;
            this.buttonUnzipOut.Click += new System.EventHandler(this.buttonUnzipOut_Click);
            // 
            // labelOut
            // 
            this.labelOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelOut.Location = new System.Drawing.Point(0, 0);
            this.labelOut.Name = "labelOut";
            this.labelOut.Size = new System.Drawing.Size(176, 33);
            this.labelOut.TabIndex = 33;
            this.labelOut.Text = "等待比较";
            // 
            // labelMain
            // 
            this.labelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMain.Location = new System.Drawing.Point(273, 0);
            this.labelMain.Name = "labelMain";
            this.labelMain.Size = new System.Drawing.Size(179, 35);
            this.labelMain.TabIndex = 32;
            this.labelMain.Text = "等待比较";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.FileDifferent);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(543, 259);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "内容差异";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // FileDifferent
            // 
            this.FileDifferent.AsyncExpanding = true;
            this.FileDifferent.BackColor = System.Drawing.SystemColors.Window;
            this.FileDifferent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FileDifferent.ColumnHeaderHeight = 0;
            this.FileDifferent.DefaultToolTipProvider = null;
            this.FileDifferent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileDifferent.DragDropMarkColor = System.Drawing.Color.Black;
            this.FileDifferent.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FileDifferent.FullRowSelect = true;
            this.FileDifferent.FullRowSelectActiveColor = System.Drawing.Color.DodgerBlue;
            this.FileDifferent.FullRowSelectInactiveColor = System.Drawing.Color.Silver;
            this.FileDifferent.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
            this.FileDifferent.LineColor = System.Drawing.SystemColors.ControlDark;
            this.FileDifferent.LoadOnDemand = true;
            this.FileDifferent.Location = new System.Drawing.Point(2, 2);
            this.FileDifferent.Model = null;
            this.FileDifferent.Name = "FileDifferent";
            this.FileDifferent.NodeControls.Add(this.___name);
            this.FileDifferent.NodeFilter = null;
            this.FileDifferent.SelectedNode = null;
            this.FileDifferent.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
            this.FileDifferent.ShowNodeToolTips = true;
            this.FileDifferent.Size = new System.Drawing.Size(539, 220);
            this.FileDifferent.TabIndex = 30;
            // 
            // ___name
            // 
            this.___name.DataPropertyName = "Name";
            this.___name.IncrementalSearchEnabled = true;
            this.___name.LeftMargin = 3;
            this.___name.ParentColumn = null;
            this.___name.UseCompatibleTextRendering = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonUnzipMainDiff);
            this.panel1.Controls.Add(this.labeldiff);
            this.panel1.Controls.Add(this.buttonUnzipOutDiff);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(2, 222);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(539, 35);
            this.panel1.TabIndex = 37;
            // 
            // buttonUnzipMainDiff
            // 
            this.buttonUnzipMainDiff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUnzipMainDiff.Enabled = false;
            this.buttonUnzipMainDiff.Location = new System.Drawing.Point(353, 6);
            this.buttonUnzipMainDiff.Name = "buttonUnzipMainDiff";
            this.buttonUnzipMainDiff.Size = new System.Drawing.Size(87, 26);
            this.buttonUnzipMainDiff.TabIndex = 34;
            this.buttonUnzipMainDiff.Text = "解压当前";
            this.buttonUnzipMainDiff.UseVisualStyleBackColor = true;
            this.buttonUnzipMainDiff.Click += new System.EventHandler(this.buttonUnzipMainDiff_Click);
            // 
            // labeldiff
            // 
            this.labeldiff.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labeldiff.Location = new System.Drawing.Point(0, 0);
            this.labeldiff.Name = "labeldiff";
            this.labeldiff.Size = new System.Drawing.Size(347, 32);
            this.labeldiff.TabIndex = 36;
            this.labeldiff.Text = "等待比较";
            // 
            // buttonUnzipOutDiff
            // 
            this.buttonUnzipOutDiff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUnzipOutDiff.Enabled = false;
            this.buttonUnzipOutDiff.Location = new System.Drawing.Point(446, 6);
            this.buttonUnzipOutDiff.Name = "buttonUnzipOutDiff";
            this.buttonUnzipOutDiff.Size = new System.Drawing.Size(87, 26);
            this.buttonUnzipOutDiff.TabIndex = 35;
            this.buttonUnzipOutDiff.Text = "解压外部";
            this.buttonUnzipOutDiff.UseVisualStyleBackColor = true;
            this.buttonUnzipOutDiff.Click += new System.EventHandler(this.buttonUnzipOutDiff_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpenPVF,
            this.toolStripSeparator1,
            this.toolStripButtonStartCompare,
            this.toolStripButtonReset});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(551, 25);
            this.toolStrip1.TabIndex = 24;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonOpenPVF
            // 
            this.toolStripButtonOpenPVF.Image = global::pvfUtility.Properties.Resources.DownloadFile_16x;
            this.toolStripButtonOpenPVF.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonOpenPVF.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenPVF.Name = "toolStripButtonOpenPVF";
            this.toolStripButtonOpenPVF.Size = new System.Drawing.Size(76, 22);
            this.toolStripButtonOpenPVF.Text = "打开封包";
            this.toolStripButtonOpenPVF.Click += new System.EventHandler(this.toolStripButtonOpenPVF_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonStartCompare
            // 
            this.toolStripButtonStartCompare.Enabled = false;
            this.toolStripButtonStartCompare.Image = global::pvfUtility.Properties.Resources.Run_16x;
            this.toolStripButtonStartCompare.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonStartCompare.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStartCompare.Name = "toolStripButtonStartCompare";
            this.toolStripButtonStartCompare.Size = new System.Drawing.Size(76, 22);
            this.toolStripButtonStartCompare.Text = "开始比较";
            this.toolStripButtonStartCompare.Click += new System.EventHandler(this.toolStripButtonStartCompare_Click);
            // 
            // toolStripButtonReset
            // 
            this.toolStripButtonReset.Image = global::pvfUtility.Properties.Resources.ClearCollection_16x;
            this.toolStripButtonReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReset.Name = "toolStripButtonReset";
            this.toolStripButtonReset.Size = new System.Drawing.Size(52, 22);
            this.toolStripButtonReset.Text = "重置";
            this.toolStripButtonReset.Click += new System.EventHandler(this.toolStripButtonReset_Click);
            // 
            // DummyPackComparer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(551, 314);
            this.Controls.Add(this.tab);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DummyPackComparer";
            this.Text = "封包文件差异比较";
            this.tab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenPVF;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public Aga.Controls.Tree.TreeViewAdv OutListView;
        public Aga.Controls.Tree.TreeViewAdv MainListView;
        public Aga.Controls.Tree.TreeViewAdv FileDifferent;
        private Aga.Controls.Tree.NodeControls.NodeTextBox __name;
        private Aga.Controls.Tree.NodeControls.NodeTextBox _name;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ___name;
        private System.Windows.Forms.Label labelOut;
        private System.Windows.Forms.Label labelMain;
        private System.Windows.Forms.Button buttonUnzipOut;
        private System.Windows.Forms.Button buttonUnzipMain;
        private System.Windows.Forms.Label labeldiff;
        private System.Windows.Forms.Button buttonUnzipOutDiff;
        private System.Windows.Forms.Button buttonUnzipMainDiff;
        private System.Windows.Forms.ToolStripButton toolStripButtonStartCompare;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton toolStripButtonReset;
    }
}