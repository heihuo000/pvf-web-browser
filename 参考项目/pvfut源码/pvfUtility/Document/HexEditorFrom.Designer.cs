namespace pvfUtility.Document.HexEditor
{
    partial class HexEditorFrom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HexEditorFrom));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBoxPath = new System.Windows.Forms.ToolStripTextBox();
            this.contextMenuStripEditor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.SelectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.CopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexBox = new Be.Windows.Forms.HexBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip.SuspendLayout();
            this.contextMenuStripEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripTextBoxPath});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(800, 25);
            this.toolStrip.TabIndex = 2;
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = global::pvfUtility.Properties.Resources.SaveFileDialogControl_16x;
            this.toolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(52, 22);
            this.toolStripButtonSave.Text = "保存";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripTextBoxPath
            // 
            this.toolStripTextBoxPath.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBoxPath.AutoSize = false;
            this.toolStripTextBoxPath.Name = "toolStripTextBoxPath";
            this.toolStripTextBoxPath.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripTextBoxPath.Size = new System.Drawing.Size(200, 25);
            this.toolStripTextBoxPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBoxPath_KeyDown);
            // 
            // contextMenuStripEditor
            // 
            this.contextMenuStripEditor.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowInExplorerToolStripMenuItem,
            this.toolStripSeparator2,
            this.SelectAllToolStripMenuItem,
            this.toolStripMenuItem4,
            this.CopyToolStripMenuItem,
            this.PasteToolStripMenuItem});
            this.contextMenuStripEditor.Name = "EditorcontextMenuStrip";
            this.contextMenuStripEditor.Size = new System.Drawing.Size(209, 126);
            this.contextMenuStripEditor.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripEditor_Opening);
            // 
            // ShowInExplorerToolStripMenuItem
            // 
            this.ShowInExplorerToolStripMenuItem.Name = "ShowInExplorerToolStripMenuItem";
            this.ShowInExplorerToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.ShowInExplorerToolStripMenuItem.Text = "在文件资源管理器中显示";
            this.ShowInExplorerToolStripMenuItem.Click += new System.EventHandler(this.ShowInExplorerToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(205, 6);
            // 
            // SelectAllToolStripMenuItem
            // 
            this.SelectAllToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.SelectAllToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.SelectAll_16x;
            this.SelectAllToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem";
            this.SelectAllToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.SelectAllToolStripMenuItem.Text = "全选";
            this.SelectAllToolStripMenuItem.Click += new System.EventHandler(this.SelectAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(205, 6);
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
            // hexBox
            // 
            this.hexBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            // 
            // 
            // 
            this.hexBox.BuiltInContextMenu.CopyMenuItemImage = global::pvfUtility.Properties.Resources.Copy_16x;
            this.hexBox.BuiltInContextMenu.CopyMenuItemText = "复制";
            this.hexBox.BuiltInContextMenu.CutMenuItemImage = global::pvfUtility.Properties.Resources.Cut_16x;
            this.hexBox.BuiltInContextMenu.CutMenuItemText = "剪切";
            this.hexBox.BuiltInContextMenu.PasteMenuItemImage = global::pvfUtility.Properties.Resources.Paste_16x;
            this.hexBox.BuiltInContextMenu.PasteMenuItemText = "粘贴";
            this.hexBox.BuiltInContextMenu.SelectAllMenuItemImage = global::pvfUtility.Properties.Resources.SelectAll_16x;
            this.hexBox.BuiltInContextMenu.SelectAllMenuItemText = "全选";
            this.hexBox.ColumnInfoVisible = true;
            this.hexBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.hexBox.LineInfoVisible = true;
            this.hexBox.Location = new System.Drawing.Point(0, 25);
            this.hexBox.Name = "hexBox";
            this.hexBox.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.hexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBox.Size = new System.Drawing.Size(800, 455);
            this.hexBox.StringViewVisible = true;
            this.hexBox.TabIndex = 7;
            this.hexBox.UseFixedBytesPerLine = true;
            this.hexBox.VScrollBarVisible = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(0, 231);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(800, 19);
            this.label1.TabIndex = 8;
            this.label1.Text = "Loading";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HexEditorFrom
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hexBox);
            this.Controls.Add(this.toolStrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "HexEditorFrom";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.HexEditorFrom_Load);
            this.SizeChanged += new System.EventHandler(this.HexEditorFrom_SizeChanged);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.contextMenuStripEditor.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxPath;
        private System.Windows.Forms.ToolStripMenuItem SelectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem CopyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowInExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ContextMenuStrip contextMenuStripEditor;
        private Be.Windows.Forms.HexBox hexBox;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.Label label1;
    }
}