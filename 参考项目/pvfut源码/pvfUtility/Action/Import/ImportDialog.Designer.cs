using pvfUtility.Dialog;

namespace pvfUtility.Shell.Dialogs
{
    partial class ImportDialog
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
            this.nodeIcon = new Aga.Controls.Tree.NodeControls.NodeIcon();
            this.nodeTextBoxName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.buttonSaveSetting = new System.Windows.Forms.Button();
            this.checkBoxConvertChinese = new System.Windows.Forms.CheckBox();
            this.checkBoxCompileFile = new System.Windows.Forms.CheckBox();
            this.checkBoxCompileBinaryAni = new System.Windows.Forms.CheckBox();
            this.listBoxImport = new System.Windows.Forms.ListBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.buttonDel = new System.Windows.Forms.Button();
            this.buttonSelectpvfPath = new System.Windows.Forms.Button();
            this.textBoxSourcePath = new System.Windows.Forms.TextBox();
            this.textBoxTargetPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSelectPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.treeViewAdvImport = new Aga.Controls.Tree.TreeViewAdv();
            this.buttonRefreshTree = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // nodeIcon
            // 
            this.nodeIcon.DataPropertyName = "Icon";
            this.nodeIcon.LeftMargin = 1;
            this.nodeIcon.ParentColumn = null;
            this.nodeIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // nodeTextBoxName
            // 
            this.nodeTextBoxName.DataPropertyName = "Name";
            this.nodeTextBoxName.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nodeTextBoxName.IncrementalSearchEnabled = true;
            this.nodeTextBoxName.LeftMargin = 3;
            this.nodeTextBoxName.ParentColumn = null;
            // 
            // buttonSaveSetting
            // 
            this.buttonSaveSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSaveSetting.AutoSize = true;
            this.buttonSaveSetting.Location = new System.Drawing.Point(11, 485);
            this.buttonSaveSetting.Name = "buttonSaveSetting";
            this.buttonSaveSetting.Size = new System.Drawing.Size(125, 27);
            this.buttonSaveSetting.TabIndex = 30;
            this.buttonSaveSetting.Text = "保存为默认配置";
            this.buttonSaveSetting.UseVisualStyleBackColor = true;
            this.buttonSaveSetting.Click += new System.EventHandler(this.ButtonSaveSetting_Click);
            // 
            // checkBoxConvertChinese
            // 
            this.checkBoxConvertChinese.AutoSize = true;
            this.checkBoxConvertChinese.Checked = true;
            this.checkBoxConvertChinese.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxConvertChinese.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxConvertChinese.Location = new System.Drawing.Point(12, 70);
            this.checkBoxConvertChinese.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxConvertChinese.Name = "checkBoxConvertChinese";
            this.checkBoxConvertChinese.Size = new System.Drawing.Size(413, 21);
            this.checkBoxConvertChinese.TabIndex = 22;
            this.checkBoxConvertChinese.Text = "自动转换至繁体中文";
            this.checkBoxConvertChinese.UseVisualStyleBackColor = true;
            // 
            // checkBoxCompileFile
            // 
            this.checkBoxCompileFile.AutoSize = true;
            this.checkBoxCompileFile.Checked = true;
            this.checkBoxCompileFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCompileFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxCompileFile.Location = new System.Drawing.Point(12, 28);
            this.checkBoxCompileFile.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxCompileFile.Name = "checkBoxCompileFile";
            this.checkBoxCompileFile.Size = new System.Drawing.Size(413, 21);
            this.checkBoxCompileFile.TabIndex = 3;
            this.checkBoxCompileFile.Text = "编译脚本文件(文本头部为#PVF_File)";
            this.checkBoxCompileFile.UseVisualStyleBackColor = true;
            // 
            // checkBoxCompileBinaryAni
            // 
            this.checkBoxCompileBinaryAni.AutoSize = true;
            this.checkBoxCompileBinaryAni.Checked = true;
            this.checkBoxCompileBinaryAni.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCompileBinaryAni.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxCompileBinaryAni.Location = new System.Drawing.Point(12, 49);
            this.checkBoxCompileBinaryAni.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxCompileBinaryAni.Name = "checkBoxCompileBinaryAni";
            this.checkBoxCompileBinaryAni.Size = new System.Drawing.Size(413, 21);
            this.checkBoxCompileBinaryAni.TabIndex = 4;
            this.checkBoxCompileBinaryAni.Text = "编译ani文本为二进制ani文件";
            this.checkBoxCompileBinaryAni.UseVisualStyleBackColor = true;
            // 
            // listBoxImport
            // 
            this.listBoxImport.AllowDrop = true;
            this.listBoxImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxImport.HorizontalScrollbar = true;
            this.listBoxImport.ItemHeight = 17;
            this.listBoxImport.Location = new System.Drawing.Point(6, 22);
            this.listBoxImport.Name = "listBoxImport";
            this.listBoxImport.Size = new System.Drawing.Size(422, 208);
            this.listBoxImport.TabIndex = 16;
            this.listBoxImport.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListBoxImport_DragDrop);
            this.listBoxImport.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListBoxImport_DragEnter);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.AutoSize = true;
            this.buttonAdd.Location = new System.Drawing.Point(369, 237);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(28, 27);
            this.buttonAdd.TabIndex = 19;
            this.buttonAdd.Text = "⇱";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(6, 242);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(32, 17);
            this.labelInfo.TabIndex = 34;
            this.labelInfo.Text = "就绪";
            // 
            // buttonDel
            // 
            this.buttonDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDel.AutoSize = true;
            this.buttonDel.Location = new System.Drawing.Point(403, 237);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(28, 27);
            this.buttonDel.TabIndex = 18;
            this.buttonDel.Text = "✗";
            this.buttonDel.UseVisualStyleBackColor = true;
            this.buttonDel.Click += new System.EventHandler(this.ButtonDel_Click);
            // 
            // buttonSelectpvfPath
            // 
            this.buttonSelectpvfPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectpvfPath.AutoSize = true;
            this.buttonSelectpvfPath.Location = new System.Drawing.Point(396, 66);
            this.buttonSelectpvfPath.Name = "buttonSelectpvfPath";
            this.buttonSelectpvfPath.Size = new System.Drawing.Size(33, 27);
            this.buttonSelectpvfPath.TabIndex = 36;
            this.buttonSelectpvfPath.Text = "...";
            this.buttonSelectpvfPath.UseVisualStyleBackColor = true;
            this.buttonSelectpvfPath.Click += new System.EventHandler(this.ButtonSelectPvfPath_Click);
            // 
            // textBoxSourcePath
            // 
            this.textBoxSourcePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSourcePath.Location = new System.Drawing.Point(68, 29);
            this.textBoxSourcePath.Name = "textBoxSourcePath";
            this.textBoxSourcePath.Size = new System.Drawing.Size(323, 23);
            this.textBoxSourcePath.TabIndex = 12;
            this.textBoxSourcePath.Leave += new System.EventHandler(this.TextBoxRootPath_Leave);
            // 
            // textBoxTargetPath
            // 
            this.textBoxTargetPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTargetPath.Location = new System.Drawing.Point(68, 66);
            this.textBoxTargetPath.Name = "textBoxTargetPath";
            this.textBoxTargetPath.Size = new System.Drawing.Size(323, 23);
            this.textBoxTargetPath.TabIndex = 15;
            this.textBoxTargetPath.Leave += new System.EventHandler(this.TextBoxTargetPaths_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "源目录：";
            // 
            // buttonSelectPath
            // 
            this.buttonSelectPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectPath.AutoSize = true;
            this.buttonSelectPath.Location = new System.Drawing.Point(396, 29);
            this.buttonSelectPath.Name = "buttonSelectPath";
            this.buttonSelectPath.Size = new System.Drawing.Size(33, 27);
            this.buttonSelectPath.TabIndex = 13;
            this.buttonSelectPath.Text = "...";
            this.buttonSelectPath.UseVisualStyleBackColor = true;
            this.buttonSelectPath.Click += new System.EventHandler(this.ButtonSelectPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "导入到：";
            // 
            // treeViewAdvImport
            // 
            this.treeViewAdvImport.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewAdvImport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewAdvImport.ColumnHeaderHeight = 0;
            this.treeViewAdvImport.DefaultToolTipProvider = null;
            this.treeViewAdvImport.DisplayDraggingNodes = true;
            this.treeViewAdvImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewAdvImport.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewAdvImport.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeViewAdvImport.FullRowSelect = true;
            this.treeViewAdvImport.FullRowSelectActiveColor = System.Drawing.SystemColors.Highlight;
            this.treeViewAdvImport.FullRowSelectInactiveColor = System.Drawing.SystemColors.ControlDark;
            this.treeViewAdvImport.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
            this.treeViewAdvImport.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeViewAdvImport.LoadOnDemand = true;
            this.treeViewAdvImport.Location = new System.Drawing.Point(3, 19);
            this.treeViewAdvImport.Margin = new System.Windows.Forms.Padding(4);
            this.treeViewAdvImport.Model = null;
            this.treeViewAdvImport.Name = "treeViewAdvImport";
            this.treeViewAdvImport.NodeControls.Add(this.nodeIcon);
            this.treeViewAdvImport.NodeControls.Add(this.nodeTextBoxName);
            this.treeViewAdvImport.NodeFilter = null;
            this.treeViewAdvImport.RowHeight = 20;
            this.treeViewAdvImport.SelectedNode = null;
            this.treeViewAdvImport.ShowLines = false;
            this.treeViewAdvImport.ShowNodeToolTips = true;
            this.treeViewAdvImport.Size = new System.Drawing.Size(348, 455);
            this.treeViewAdvImport.TabIndex = 28;
            // 
            // buttonRefreshTree
            // 
            this.buttonRefreshTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefreshTree.AutoSize = true;
            this.buttonRefreshTree.Image = global::pvfUtility.Properties.Resources.Refresh_16x;
            this.buttonRefreshTree.Location = new System.Drawing.Point(319, 3);
            this.buttonRefreshTree.Name = "buttonRefreshTree";
            this.buttonRefreshTree.Size = new System.Drawing.Size(24, 24);
            this.buttonRefreshTree.TabIndex = 35;
            this.buttonRefreshTree.UseVisualStyleBackColor = true;
            this.buttonRefreshTree.Click += new System.EventHandler(this.ButtonRefreshTree_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonRefreshTree);
            this.groupBox1.Controls.Add(this.treeViewAdvImport);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 477);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "预计文件变更";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox4);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(799, 479);
            this.splitContainer1.SplitterDistance = 439;
            this.splitContainer1.TabIndex = 41;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBoxImport);
            this.groupBox3.Controls.Add(this.buttonDel);
            this.groupBox3.Controls.Add(this.buttonAdd);
            this.groupBox3.Controls.Add(this.labelInfo);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 107);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(437, 265);
            this.groupBox3.TabIndex = 36;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "导入内容(支持拖曳添加)";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxConvertChinese);
            this.groupBox4.Controls.Add(this.checkBoxCompileBinaryAni);
            this.groupBox4.Controls.Add(this.checkBoxCompileFile);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox4.Location = new System.Drawing.Point(0, 372);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(12);
            this.groupBox4.Size = new System.Drawing.Size(437, 105);
            this.groupBox4.TabIndex = 43;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "导入选项";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonSelectpvfPath);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBoxSourcePath);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBoxTargetPath);
            this.groupBox2.Controls.Add(this.buttonSelectPath);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(437, 107);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "导入源";
            // 
            // ImportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(799, 521);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.buttonSaveSetting);
            this.DialogButtons = pvfUtility.Dialog.DialogButton.OkCancel;
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "ImportDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导入";
            this.Load += new System.EventHandler(this.ImportDialog_Load);
            this.Controls.SetChildIndex(this.buttonSaveSetting, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectPath;
        private System.Windows.Forms.TextBox textBoxSourcePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxTargetPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxImport;
        private System.Windows.Forms.CheckBox checkBoxCompileBinaryAni;
        private System.Windows.Forms.CheckBox checkBoxCompileFile;
        private System.Windows.Forms.Button buttonDel;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.CheckBox checkBoxConvertChinese;
        public Aga.Controls.Tree.TreeViewAdv treeViewAdvImport;
        private Aga.Controls.Tree.NodeControls.NodeIcon nodeIcon;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBoxName;
        private System.Windows.Forms.Button buttonSaveSetting;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Button buttonRefreshTree;
        private System.Windows.Forms.Button buttonSelectpvfPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}