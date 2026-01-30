namespace pvfUtility.Shell.Document.TextEditor
{
    partial class TextEditorView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextEditorView));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripSplitButton();
            this.contextMenuStripSave = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SaveTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveAsBinaryAniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveAsUtf8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsTextBig5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsTextGBKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButtonFind = new System.Windows.Forms.ToolStripSplitButton();
            this.FindTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReplaceTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GoToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonAutoFold = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButtonEncoding = new System.Windows.Forms.ToolStripDropDownButton();
            this.tWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBoxPath = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripComboBoxSection = new System.Windows.Forms.ToolStripComboBox();
            this.saveAdvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scintilla = new ScintillaNET.Scintilla();
            this.contextMenuStripEditor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TurningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveTextToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.SelectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.UndoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RedoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.CutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.showBeginningTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip.SuspendLayout();
            this.contextMenuStripSave.SuspendLayout();
            this.contextMenuStripEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripSeparator1,
            this.toolStripSplitButtonFind,
            this.toolStripButtonAutoFold,
            this.toolStripSplitButtonEncoding,
            this.toolStripTextBoxPath,
            this.toolStripComboBoxSection});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(800, 25);
            this.toolStrip.TabIndex = 2;
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DropDown = this.contextMenuStripSave;
            this.toolStripButtonSave.Image = global::pvfUtility.Properties.Resources.SaveFileDialogControl_16x;
            this.toolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(100, 22);
            this.toolStripButtonSave.Text = "保存此文件";
            this.toolStripButtonSave.ButtonClick += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // contextMenuStripSave
            // 
            this.contextMenuStripSave.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveTextToolStripMenuItem,
            this.toolStripSeparator4,
            this.SaveAsBinaryAniToolStripMenuItem,
            this.SaveAsScriptToolStripMenuItem,
            this.toolStripSeparator5,
            this.SaveAsUtf8ToolStripMenuItem,
            this.SaveAsTextBig5ToolStripMenuItem,
            this.SaveAsTextGBKToolStripMenuItem});
            this.contextMenuStripSave.Name = "contextMenuStrip1";
            this.contextMenuStripSave.OwnerItem = this.saveAdvToolStripMenuItem;
            this.contextMenuStripSave.Size = new System.Drawing.Size(243, 148);
            // 
            // SaveTextToolStripMenuItem
            // 
            this.SaveTextToolStripMenuItem.Name = "SaveTextToolStripMenuItem";
            this.SaveTextToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.SaveTextToolStripMenuItem.Text = "保存";
            this.SaveTextToolStripMenuItem.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(239, 6);
            // 
            // SaveAsBinaryAniToolStripMenuItem
            // 
            this.SaveAsBinaryAniToolStripMenuItem.Name = "SaveAsBinaryAniToolStripMenuItem";
            this.SaveAsBinaryAniToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.SaveAsBinaryAniToolStripMenuItem.Text = "保存为二进制ANI文件";
            this.SaveAsBinaryAniToolStripMenuItem.Click += new System.EventHandler(this.SaveAsBinaryAniToolStripMenuItem_Click);
            // 
            // SaveAsScriptToolStripMenuItem
            // 
            this.SaveAsScriptToolStripMenuItem.Name = "SaveAsScriptToolStripMenuItem";
            this.SaveAsScriptToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.SaveAsScriptToolStripMenuItem.Text = "保存为脚本文件";
            this.SaveAsScriptToolStripMenuItem.Click += new System.EventHandler(this.SaveAsScriptToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(239, 6);
            // 
            // SaveAsUtf8ToolStripMenuItem
            // 
            this.SaveAsUtf8ToolStripMenuItem.Name = "SaveAsUtf8ToolStripMenuItem";
            this.SaveAsUtf8ToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.SaveAsUtf8ToolStripMenuItem.Text = "以文本文件保存（UTF-8编码）";
            this.SaveAsUtf8ToolStripMenuItem.Click += new System.EventHandler(this.SaveAsUtf8ToolStripMenuItem_Click);
            // 
            // SaveAsTextBig5ToolStripMenuItem
            // 
            this.SaveAsTextBig5ToolStripMenuItem.Name = "SaveAsTextBig5ToolStripMenuItem";
            this.SaveAsTextBig5ToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.SaveAsTextBig5ToolStripMenuItem.Text = "以文本文件保存（TW编码）";
            this.SaveAsTextBig5ToolStripMenuItem.Click += new System.EventHandler(this.SaveAsTextBig5ToolStripMenuItem_Click);
            // 
            // SaveAsTextGBKToolStripMenuItem
            // 
            this.SaveAsTextGBKToolStripMenuItem.Name = "SaveAsTextGBKToolStripMenuItem";
            this.SaveAsTextGBKToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.SaveAsTextGBKToolStripMenuItem.Text = "以文本文件保存（CN编码）";
            this.SaveAsTextGBKToolStripMenuItem.Click += new System.EventHandler(this.SaveAsTextGBKToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitButtonFind
            // 
            this.toolStripSplitButtonFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonFind.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FindTextToolStripMenuItem,
            this.ReplaceTextToolStripMenuItem,
            this.GoToToolStripMenuItem});
            this.toolStripSplitButtonFind.Image = global::pvfUtility.Properties.Resources.FindSymbol_16x;
            this.toolStripSplitButtonFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonFind.Name = "toolStripSplitButtonFind";
            this.toolStripSplitButtonFind.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButtonFind.Text = "文本查找";
            this.toolStripSplitButtonFind.ButtonClick += new System.EventHandler(this.toolStripSplitButtonFind_ButtonClick);
            // 
            // FindTextToolStripMenuItem
            // 
            this.FindTextToolStripMenuItem.Image = global::pvfUtility.Properties.Resources.Search_16x;
            this.FindTextToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.FindTextToolStripMenuItem.Name = "FindTextToolStripMenuItem";
            this.FindTextToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.FindTextToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.FindTextToolStripMenuItem.Text = "查找";
            this.FindTextToolStripMenuItem.Click += new System.EventHandler(this.FindTextToolStripMenuItem_Click);
            // 
            // ReplaceTextToolStripMenuItem
            // 
            this.ReplaceTextToolStripMenuItem.Name = "ReplaceTextToolStripMenuItem";
            this.ReplaceTextToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.ReplaceTextToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.ReplaceTextToolStripMenuItem.Text = "替换";
            this.ReplaceTextToolStripMenuItem.Click += new System.EventHandler(this.ReplaceTextToolStripMenuItem_Click);
            // 
            // GoToToolStripMenuItem
            // 
            this.GoToToolStripMenuItem.Name = "GoToToolStripMenuItem";
            this.GoToToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.GoToToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.GoToToolStripMenuItem.Text = "跳转";
            this.GoToToolStripMenuItem.Click += new System.EventHandler(this.GoToToolStripMenuItem_Click);
            // 
            // toolStripButtonAutoFold
            // 
            this.toolStripButtonAutoFold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonAutoFold.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAutoFold.Image")));
            this.toolStripButtonAutoFold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAutoFold.Name = "toolStripButtonAutoFold";
            this.toolStripButtonAutoFold.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonAutoFold.Text = "自动折叠";
            this.toolStripButtonAutoFold.Click += new System.EventHandler(this.autoFoldToolStripMenuItem_Click);
            // 
            // toolStripSplitButtonEncoding
            // 
            this.toolStripSplitButtonEncoding.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tWToolStripMenuItem,
            this.cNToolStripMenuItem,
            this.kRToolStripMenuItem,
            this.jPToolStripMenuItem});
            this.toolStripSplitButtonEncoding.Image = global::pvfUtility.Properties.Resources.CodeDefinitionWindow_16x;
            this.toolStripSplitButtonEncoding.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripSplitButtonEncoding.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonEncoding.Name = "toolStripSplitButtonEncoding";
            this.toolStripSplitButtonEncoding.Size = new System.Drawing.Size(61, 22);
            this.toolStripSplitButtonEncoding.Text = "编码";
            // 
            // tWToolStripMenuItem
            // 
            this.tWToolStripMenuItem.Name = "tWToolStripMenuItem";
            this.tWToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.tWToolStripMenuItem.Text = "Big5(TW)";
            this.tWToolStripMenuItem.Click += new System.EventHandler(this.tWToolStripMenuItem_Click);
            // 
            // cNToolStripMenuItem
            // 
            this.cNToolStripMenuItem.Name = "cNToolStripMenuItem";
            this.cNToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.cNToolStripMenuItem.Text = "GBK(CN)";
            this.cNToolStripMenuItem.Click += new System.EventHandler(this.cNToolStripMenuItem_Click);
            // 
            // kRToolStripMenuItem
            // 
            this.kRToolStripMenuItem.Name = "kRToolStripMenuItem";
            this.kRToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.kRToolStripMenuItem.Text = "EUC-KR(KR)";
            this.kRToolStripMenuItem.Click += new System.EventHandler(this.kRToolStripMenuItem_Click);
            // 
            // jPToolStripMenuItem
            // 
            this.jPToolStripMenuItem.Name = "jPToolStripMenuItem";
            this.jPToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.jPToolStripMenuItem.Text = "Shift-JIS(JP)";
            this.jPToolStripMenuItem.Click += new System.EventHandler(this.jPToolStripMenuItem_Click);
            // 
            // toolStripTextBoxPath
            // 
            this.toolStripTextBoxPath.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBoxPath.AutoSize = false;
            this.toolStripTextBoxPath.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.toolStripTextBoxPath.Name = "toolStripTextBoxPath";
            this.toolStripTextBoxPath.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripTextBoxPath.Size = new System.Drawing.Size(200, 25);
            this.toolStripTextBoxPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBoxPath_KeyDown);
            // 
            // toolStripComboBoxSection
            // 
            this.toolStripComboBoxSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxSection.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripComboBoxSection.Name = "toolStripComboBoxSection";
            this.toolStripComboBoxSection.Size = new System.Drawing.Size(150, 25);
            this.toolStripComboBoxSection.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxSection_SelectedIndexChanged);
            // 
            // saveAdvToolStripMenuItem
            // 
            this.saveAdvToolStripMenuItem.DropDown = this.contextMenuStripSave;
            this.saveAdvToolStripMenuItem.Name = "saveAdvToolStripMenuItem";
            this.saveAdvToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.saveAdvToolStripMenuItem.Text = "高级保存";
            // 
            // scintilla
            // 
            this.scintilla.AdditionalCaretForeColor = System.Drawing.Color.AntiqueWhite;
            this.scintilla.AnnotationVisible = ScintillaNET.Annotation.Standard;
            this.scintilla.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.scintilla.CaretLineBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(242)))));
            this.scintilla.CaretLineVisible = true;
            this.scintilla.ContextMenuStrip = this.contextMenuStripEditor;
            this.scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla.EdgeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(232)))));
            this.scintilla.EdgeMode = ScintillaNET.EdgeMode.Line;
            this.scintilla.FontQuality = ScintillaNET.FontQuality.LcdOptimized;
            this.scintilla.Lexer = ScintillaNET.Lexer.Null;
            this.scintilla.Location = new System.Drawing.Point(0, 25);
            this.scintilla.Margin = new System.Windows.Forms.Padding(4);
            this.scintilla.Name = "scintilla";
            this.scintilla.Size = new System.Drawing.Size(800, 455);
            this.scintilla.TabIndex = 3;
            this.scintilla.Visible = false;
            this.scintilla.HotspotClick += new System.EventHandler<ScintillaNET.HotspotClickEventArgs>(this.scintilla_HotspotClick);
            this.scintilla.StyleNeeded += new System.EventHandler<ScintillaNET.StyleNeededEventArgs>(this.scintilla_StyleNeeded);
            this.scintilla.ZoomChanged += new System.EventHandler<System.EventArgs>(this.scintilla_ZoomChanged);
            this.scintilla.KeyDown += new System.Windows.Forms.KeyEventHandler(this.scintilla_KeyDown);
            // 
            // contextMenuStripEditor
            // 
            this.contextMenuStripEditor.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TurningToolStripMenuItem,
            this.toolStripMenuItem1,
            this.saveAdvToolStripMenuItem,
            this.refreshTextToolStripMenuItem,
            this.toolStripSeparator3,
            this.SaveTextToToolStripMenuItem,
            this.ShowInExplorerToolStripMenuItem,
            this.toolStripSeparator2,
            this.SelectAllToolStripMenuItem,
            this.toolStripMenuItem3,
            this.UndoToolStripMenuItem,
            this.RedoToolStripMenuItem,
            this.toolStripMenuItem4,
            this.CutToolStripMenuItem,
            this.CopyToolStripMenuItem,
            this.PasteToolStripMenuItem,
            this.toolStripMenuItem2,
            this.showBeginningTabToolStripMenuItem});
            this.contextMenuStripEditor.Name = "EditorcontextMenuStrip";
            this.contextMenuStripEditor.Size = new System.Drawing.Size(209, 304);
            this.contextMenuStripEditor.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripEditor_Opening);
            // 
            // TurningToolStripMenuItem
            // 
            this.TurningToolStripMenuItem.Name = "TurningToolStripMenuItem";
            this.TurningToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.TurningToolStripMenuItem.Text = "不可用";
            this.TurningToolStripMenuItem.ToolTipText = "转到";
            this.TurningToolStripMenuItem.Click += new System.EventHandler(this.TurningToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(205, 6);
            // 
            // refreshTextToolStripMenuItem
            // 
            this.refreshTextToolStripMenuItem.Name = "refreshTextToolStripMenuItem";
            this.refreshTextToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.refreshTextToolStripMenuItem.Text = "刷新文本";
            this.refreshTextToolStripMenuItem.Click += new System.EventHandler(this.refreshTextToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(205, 6);
            // 
            // SaveTextToToolStripMenuItem
            // 
            this.SaveTextToToolStripMenuItem.Name = "SaveTextToToolStripMenuItem";
            this.SaveTextToToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.SaveTextToToolStripMenuItem.Text = "保存当前文本到...";
            this.SaveTextToToolStripMenuItem.Click += new System.EventHandler(this.SaveTextToToolStripMenuItem_Click);
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
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(205, 6);
            // 
            // UndoToolStripMenuItem
            // 
            this.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem";
            this.UndoToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.UndoToolStripMenuItem.Text = "撤销";
            this.UndoToolStripMenuItem.Click += new System.EventHandler(this.UndoToolStripMenuItem_Click);
            // 
            // RedoToolStripMenuItem
            // 
            this.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem";
            this.RedoToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.RedoToolStripMenuItem.Text = "重做";
            this.RedoToolStripMenuItem.Click += new System.EventHandler(this.RedoToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(205, 6);
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
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(205, 6);
            // 
            // showBeginningTabToolStripMenuItem
            // 
            this.showBeginningTabToolStripMenuItem.Name = "showBeginningTabToolStripMenuItem";
            this.showBeginningTabToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.showBeginningTabToolStripMenuItem.Text = "显示行首的Tab";
            this.showBeginningTabToolStripMenuItem.Click += new System.EventHandler(this.showBeginningTabToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(0, 209);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(800, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "Loading";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextEditorView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scintilla);
            this.Controls.Add(this.toolStrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TextEditorView";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextEditorFrom_FormClosing);
            this.Load += new System.EventHandler(this.TextEditorFrom_Load);
            this.SizeChanged += new System.EventHandler(this.TextEditorFrom_SizeChanged);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.contextMenuStripSave.ResumeLayout(false);
            this.contextMenuStripEditor.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public ScintillaNET.Scintilla scintilla;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxPath;
        private System.Windows.Forms.ToolStripMenuItem TurningToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem SelectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem UndoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RedoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem CutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripDropDownButton toolStripSplitButtonEncoding;
        private System.Windows.Forms.ToolStripMenuItem tWToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveTextToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowInExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonFind;
        private System.Windows.Forms.ToolStripMenuItem FindTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReplaceTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GoToToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAutoFold;
        private System.Windows.Forms.ToolStripMenuItem refreshTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxSection;
        public System.Windows.Forms.ContextMenuStrip contextMenuStripEditor;
        private System.Windows.Forms.ToolStripMenuItem saveAdvToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSave;
        private System.Windows.Forms.ToolStripMenuItem SaveTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem SaveAsBinaryAniToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem SaveAsUtf8ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsTextBig5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsTextGBKToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem showBeginningTabToolStripMenuItem;
    }
}