using pvfUtility.Dialog;

namespace pvfUtility.Shell.Dialogs.Search
{
    partial class SearchDialog
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
            this.textSearchKeyword = new System.Windows.Forms.TextBox();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.checkBoxUseLikePath = new System.Windows.Forms.CheckBox();
            this.buttonPattern = new System.Windows.Forms.Button();
            this.buttonHistory = new System.Windows.Forms.Button();
            this.radioButtonMethodNone = new System.Windows.Forms.RadioButton();
            this.radioButtonMethodRemove = new System.Windows.Forms.RadioButton();
            this.radioButtonMethodMatch = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this._Name = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this._Commet = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textSearchScriptContent = new System.Windows.Forms.TextBox();
            this.contextMenuStripPattern = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripHis = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButtonRegex = new System.Windows.Forms.RadioButton();
            this.radioButtonBinary = new System.Windows.Forms.RadioButton();
            this.panelNormal = new System.Windows.Forms.Panel();
            this.groupBoxMode = new System.Windows.Forms.GroupBox();
            this.radioButtonUseRegex = new System.Windows.Forms.RadioButton();
            this.radioButtonUseLike = new System.Windows.Forms.RadioButton();
            this.radioButtonUseNormal = new System.Windows.Forms.RadioButton();
            this.checkBoxStartMatch = new System.Windows.Forms.CheckBox();
            this.panelScriptContent = new System.Windows.Forms.Panel();
            this.linkLabelLearningRegex = new System.Windows.Forms.LinkLabel();
            this.labelText = new System.Windows.Forms.Label();
            this.checkBoxNewCollection = new System.Windows.Forms.CheckBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.contextMenuStripPattern.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panelNormal.SuspendLayout();
            this.groupBoxMode.SuspendLayout();
            this.panelScriptContent.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textSearchKeyword
            // 
            this.textSearchKeyword.Location = new System.Drawing.Point(75, 5);
            this.textSearchKeyword.Margin = new System.Windows.Forms.Padding(2);
            this.textSearchKeyword.Name = "textSearchKeyword";
            this.textSearchKeyword.Size = new System.Drawing.Size(450, 27);
            this.textSearchKeyword.TabIndex = 52;
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(42, 49);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(410, 27);
            this.textBoxPath.TabIndex = 62;
            this.textBoxPath.Enter += new System.EventHandler(this.textBox_Enter);
            this.textBoxPath.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // checkBoxUseLikePath
            // 
            this.checkBoxUseLikePath.AutoSize = true;
            this.checkBoxUseLikePath.Location = new System.Drawing.Point(365, 78);
            this.checkBoxUseLikePath.Name = "checkBoxUseLikePath";
            this.checkBoxUseLikePath.Size = new System.Drawing.Size(106, 24);
            this.checkBoxUseLikePath.TabIndex = 61;
            this.checkBoxUseLikePath.Text = "使用通配符";
            this.checkBoxUseLikePath.UseVisualStyleBackColor = true;
            // 
            // buttonPattern
            // 
            this.buttonPattern.Location = new System.Drawing.Point(479, 48);
            this.buttonPattern.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPattern.Name = "buttonPattern";
            this.buttonPattern.Size = new System.Drawing.Size(32, 24);
            this.buttonPattern.TabIndex = 60;
            this.buttonPattern.Text = "...";
            this.buttonPattern.UseVisualStyleBackColor = true;
            this.buttonPattern.Click += new System.EventHandler(this.buttonPattern_Click);
            // 
            // buttonHistory
            // 
            this.buttonHistory.Image = global::pvfUtility.Properties.Resources.SearchContract_16x;
            this.buttonHistory.Location = new System.Drawing.Point(455, 48);
            this.buttonHistory.Margin = new System.Windows.Forms.Padding(0);
            this.buttonHistory.Name = "buttonHistory";
            this.buttonHistory.Size = new System.Drawing.Size(24, 24);
            this.buttonHistory.TabIndex = 59;
            this.buttonHistory.UseVisualStyleBackColor = true;
            this.buttonHistory.Click += new System.EventHandler(this.buttonHistory_Click);
            // 
            // radioButtonMethodNone
            // 
            this.radioButtonMethodNone.AutoSize = true;
            this.radioButtonMethodNone.Checked = true;
            this.radioButtonMethodNone.Location = new System.Drawing.Point(3, 17);
            this.radioButtonMethodNone.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonMethodNone.Name = "radioButtonMethodNone";
            this.radioButtonMethodNone.Size = new System.Drawing.Size(325, 24);
            this.radioButtonMethodNone.TabIndex = 58;
            this.radioButtonMethodNone.TabStop = true;
            this.radioButtonMethodNone.Text = "在所有文件中搜索(目录留空为搜索全部文件)";
            this.radioButtonMethodNone.UseVisualStyleBackColor = true;
            this.radioButtonMethodNone.CheckedChanged += new System.EventHandler(this.radioButtonMethod0_CheckedChanged);
            // 
            // radioButtonMethodRemove
            // 
            this.radioButtonMethodRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonMethodRemove.AutoSize = true;
            this.radioButtonMethodRemove.Location = new System.Drawing.Point(5, 129);
            this.radioButtonMethodRemove.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonMethodRemove.Name = "radioButtonMethodRemove";
            this.radioButtonMethodRemove.Size = new System.Drawing.Size(195, 24);
            this.radioButtonMethodRemove.TabIndex = 4;
            this.radioButtonMethodRemove.Text = "从搜索结果中排除符合项";
            this.radioButtonMethodRemove.UseVisualStyleBackColor = true;
            this.radioButtonMethodRemove.CheckedChanged += new System.EventHandler(this.radioButtonMethod1_CheckedChanged);
            // 
            // radioButtonMethodMatch
            // 
            this.radioButtonMethodMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonMethodMatch.AutoSize = true;
            this.radioButtonMethodMatch.Location = new System.Drawing.Point(5, 108);
            this.radioButtonMethodMatch.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonMethodMatch.Name = "radioButtonMethodMatch";
            this.radioButtonMethodMatch.Size = new System.Drawing.Size(195, 24);
            this.radioButtonMethodMatch.TabIndex = 3;
            this.radioButtonMethodMatch.Text = "从搜索结果中筛选符合项";
            this.radioButtonMethodMatch.UseVisualStyleBackColor = true;
            this.radioButtonMethodMatch.CheckedChanged += new System.EventHandler(this.radioButtonMethod1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 20);
            this.label1.TabIndex = 48;
            this.label1.Text = "目录：";
            // 
            // _Name
            // 
            this._Name.DataPropertyName = "Name";
            this._Name.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._Name.IncrementalSearchEnabled = true;
            this._Name.LeftMargin = 3;
            this._Name.ParentColumn = null;
            this._Name.UseCompatibleTextRendering = true;
            // 
            // _Commet
            // 
            this._Commet.DataPropertyName = "Commet";
            this._Commet.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._Commet.IncrementalSearchEnabled = true;
            this._Commet.LeftMargin = 3;
            this._Commet.ParentColumn = null;
            this._Commet.UseCompatibleTextRendering = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 20);
            this.label2.TabIndex = 49;
            this.label2.Text = "关键词：";
            // 
            // textSearchScriptContent
            // 
            this.textSearchScriptContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.textSearchScriptContent.Location = new System.Drawing.Point(0, 0);
            this.textSearchScriptContent.Margin = new System.Windows.Forms.Padding(4);
            this.textSearchScriptContent.Multiline = true;
            this.textSearchScriptContent.Name = "textSearchScriptContent";
            this.textSearchScriptContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textSearchScriptContent.Size = new System.Drawing.Size(537, 114);
            this.textSearchScriptContent.TabIndex = 54;
            this.textSearchScriptContent.Enter += new System.EventHandler(this.textBox_Enter);
            this.textSearchScriptContent.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // contextMenuStripPattern
            // 
            this.contextMenuStripPattern.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripPattern.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.帮助ToolStripMenuItem});
            this.contextMenuStripPattern.Name = "contextMenuStrip1";
            this.contextMenuStripPattern.Size = new System.Drawing.Size(243, 154);
            this.contextMenuStripPattern.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStripPattern_ItemClicked);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(242, 24);
            this.toolStripMenuItem1.Tag = "?";
            this.toolStripMenuItem1.Text = "任何单个字符(?)";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(242, 24);
            this.toolStripMenuItem2.Tag = "*";
            this.toolStripMenuItem2.Text = "一个或多个字符(*)";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(242, 24);
            this.toolStripMenuItem3.Tag = "#";
            this.toolStripMenuItem3.Text = "任何一个数字(#)";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(242, 24);
            this.toolStripMenuItem4.Tag = "[! ]";
            this.toolStripMenuItem4.Text = "不在字符集中的字符([! ])";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(242, 24);
            this.toolStripMenuItem5.Tag = "[ ]";
            this.toolStripMenuItem5.Text = "字符集([ ])";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(239, 6);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(242, 24);
            this.帮助ToolStripMenuItem.Text = "帮助";
            this.帮助ToolStripMenuItem.Click += new System.EventHandler(this.帮助ToolStripMenuItem_Click);
            // 
            // contextMenuStripHis
            // 
            this.contextMenuStripHis.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripHis.Name = "contextMenuStripHis";
            this.contextMenuStripHis.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStripHis.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStripHis_ItemClicked);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButtonRegex);
            this.groupBox3.Controls.Add(this.radioButtonBinary);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox3.Location = new System.Drawing.Point(0, 124);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(537, 49);
            this.groupBox3.TabIndex = 58;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "脚本文件内容搜索模式";
            // 
            // radioButtonRegex
            // 
            this.radioButtonRegex.AutoSize = true;
            this.radioButtonRegex.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonRegex.Location = new System.Drawing.Point(213, 23);
            this.radioButtonRegex.Name = "radioButtonRegex";
            this.radioButtonRegex.Size = new System.Drawing.Size(240, 23);
            this.radioButtonRegex.TabIndex = 2;
            this.radioButtonRegex.Text = "正则表达式（基于文本，慢速）";
            this.radioButtonRegex.UseVisualStyleBackColor = true;
            // 
            // radioButtonBinary
            // 
            this.radioButtonBinary.AutoSize = true;
            this.radioButtonBinary.Checked = true;
            this.radioButtonBinary.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonBinary.Location = new System.Drawing.Point(3, 23);
            this.radioButtonBinary.Name = "radioButtonBinary";
            this.radioButtonBinary.Size = new System.Drawing.Size(210, 23);
            this.radioButtonBinary.TabIndex = 3;
            this.radioButtonBinary.TabStop = true;
            this.radioButtonBinary.Text = "普通（基于二进制，快速）";
            this.radioButtonBinary.UseVisualStyleBackColor = true;
            // 
            // panelNormal
            // 
            this.panelNormal.Controls.Add(this.groupBoxMode);
            this.panelNormal.Controls.Add(this.textSearchKeyword);
            this.panelNormal.Controls.Add(this.checkBoxStartMatch);
            this.panelNormal.Controls.Add(this.label2);
            this.panelNormal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNormal.Location = new System.Drawing.Point(0, 0);
            this.panelNormal.Name = "panelNormal";
            this.panelNormal.Size = new System.Drawing.Size(537, 173);
            this.panelNormal.TabIndex = 59;
            // 
            // groupBoxMode
            // 
            this.groupBoxMode.AutoSize = true;
            this.groupBoxMode.Controls.Add(this.radioButtonUseRegex);
            this.groupBoxMode.Controls.Add(this.radioButtonUseLike);
            this.groupBoxMode.Controls.Add(this.radioButtonUseNormal);
            this.groupBoxMode.Location = new System.Drawing.Point(7, 33);
            this.groupBoxMode.Name = "groupBoxMode";
            this.groupBoxMode.Size = new System.Drawing.Size(246, 51);
            this.groupBoxMode.TabIndex = 56;
            this.groupBoxMode.TabStop = false;
            this.groupBoxMode.Text = "搜索模式";
            // 
            // radioButtonUseRegex
            // 
            this.radioButtonUseRegex.AutoSize = true;
            this.radioButtonUseRegex.Dock = System.Windows.Forms.DockStyle.Right;
            this.radioButtonUseRegex.Location = new System.Drawing.Point(63, 23);
            this.radioButtonUseRegex.Name = "radioButtonUseRegex";
            this.radioButtonUseRegex.Size = new System.Drawing.Size(105, 25);
            this.radioButtonUseRegex.TabIndex = 57;
            this.radioButtonUseRegex.Text = "正则表达式";
            this.radioButtonUseRegex.UseVisualStyleBackColor = true;
            // 
            // radioButtonUseLike
            // 
            this.radioButtonUseLike.AutoSize = true;
            this.radioButtonUseLike.Dock = System.Windows.Forms.DockStyle.Right;
            this.radioButtonUseLike.Location = new System.Drawing.Point(168, 23);
            this.radioButtonUseLike.Name = "radioButtonUseLike";
            this.radioButtonUseLike.Size = new System.Drawing.Size(75, 25);
            this.radioButtonUseLike.TabIndex = 56;
            this.radioButtonUseLike.Text = "通配符";
            this.radioButtonUseLike.UseVisualStyleBackColor = true;
            // 
            // radioButtonUseNormal
            // 
            this.radioButtonUseNormal.AutoSize = true;
            this.radioButtonUseNormal.Checked = true;
            this.radioButtonUseNormal.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonUseNormal.Location = new System.Drawing.Point(3, 23);
            this.radioButtonUseNormal.Name = "radioButtonUseNormal";
            this.radioButtonUseNormal.Size = new System.Drawing.Size(60, 25);
            this.radioButtonUseNormal.TabIndex = 55;
            this.radioButtonUseNormal.TabStop = true;
            this.radioButtonUseNormal.Text = "普通";
            this.radioButtonUseNormal.UseVisualStyleBackColor = true;
            this.radioButtonUseNormal.CheckedChanged += new System.EventHandler(this.radioButtonUseNormal_CheckedChanged);
            // 
            // checkBoxStartMatch
            // 
            this.checkBoxStartMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxStartMatch.AutoSize = true;
            this.checkBoxStartMatch.Location = new System.Drawing.Point(7, 88);
            this.checkBoxStartMatch.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxStartMatch.Name = "checkBoxStartMatch";
            this.checkBoxStartMatch.Size = new System.Drawing.Size(91, 24);
            this.checkBoxStartMatch.TabIndex = 53;
            this.checkBoxStartMatch.Text = "开头匹配";
            this.checkBoxStartMatch.UseVisualStyleBackColor = true;
            // 
            // panelScriptContent
            // 
            this.panelScriptContent.Controls.Add(this.groupBox3);
            this.panelScriptContent.Controls.Add(this.textSearchScriptContent);
            this.panelScriptContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScriptContent.Location = new System.Drawing.Point(0, 0);
            this.panelScriptContent.Name = "panelScriptContent";
            this.panelScriptContent.Size = new System.Drawing.Size(537, 173);
            this.panelScriptContent.TabIndex = 60;
            this.panelScriptContent.Visible = false;
            // 
            // linkLabelLearningRegex
            // 
            this.linkLabelLearningRegex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelLearningRegex.AutoSize = true;
            this.linkLabelLearningRegex.BackColor = System.Drawing.SystemColors.Control;
            this.linkLabelLearningRegex.Location = new System.Drawing.Point(4, 386);
            this.linkLabelLearningRegex.Name = "linkLabelLearningRegex";
            this.linkLabelLearningRegex.Size = new System.Drawing.Size(188, 20);
            this.linkLabelLearningRegex.TabIndex = 59;
            this.linkLabelLearningRegex.TabStop = true;
            this.linkLabelLearningRegex.Text = "正则表达式语言 - 快速参考";
            this.linkLabelLearningRegex.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLearningRegex_LinkClicked);
            // 
            // labelText
            // 
            this.labelText.AutoSize = true;
            this.labelText.Location = new System.Drawing.Point(39, 79);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(21, 20);
            this.labelText.TabIndex = 64;
            this.labelText.Text = "...";
            // 
            // checkBoxNewCollection
            // 
            this.checkBoxNewCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxNewCollection.AutoSize = true;
            this.checkBoxNewCollection.Checked = true;
            this.checkBoxNewCollection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxNewCollection.Location = new System.Drawing.Point(486, 14);
            this.checkBoxNewCollection.Name = "checkBoxNewCollection";
            this.checkBoxNewCollection.Size = new System.Drawing.Size(106, 24);
            this.checkBoxNewCollection.TabIndex = 63;
            this.checkBoxNewCollection.Text = "新建文件集";
            this.checkBoxNewCollection.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(10, 373);
            this.panel7.TabIndex = 64;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelText);
            this.groupBox1.Controls.Add(this.radioButtonMethodNone);
            this.groupBox1.Controls.Add(this.buttonPattern);
            this.groupBox1.Controls.Add(this.checkBoxUseLikePath);
            this.groupBox1.Controls.Add(this.checkBoxNewCollection);
            this.groupBox1.Controls.Add(this.radioButtonMethodMatch);
            this.groupBox1.Controls.Add(this.buttonHistory);
            this.groupBox1.Controls.Add(this.textBoxPath);
            this.groupBox1.Controls.Add(this.radioButtonMethodRemove);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(599, 158);
            this.groupBox1.TabIndex = 67;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "搜索源";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 20);
            this.label3.TabIndex = 68;
            this.label3.Text = "搜索类型:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelNormal);
            this.panel1.Controls.Add(this.panelScriptContent);
            this.panel1.Location = new System.Drawing.Point(12, 197);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(537, 173);
            this.panel1.TabIndex = 69;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(87, 167);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(111, 24);
            this.radioButton1.TabIndex = 70;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "整数/浮点数";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(184, 167);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(177, 24);
            this.radioButton2.TabIndex = 71;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "字符串链接/字符串/节";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(334, 167);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(75, 24);
            this.radioButton3.TabIndex = 72;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "文件名";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(498, 167);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(120, 24);
            this.radioButton4.TabIndex = 73;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "脚本文件内容";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(403, 167);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(90, 24);
            this.radioButton5.TabIndex = 74;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "名称搜索";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // SearchDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(620, 415);
            this.Controls.Add(this.radioButton5);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.linkLabelLearningRegex);
            this.DialogButtons = pvfUtility.Dialog.DialogButton.OkCancel;
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "搜索";
            this.TopMost = true;
            this.Controls.SetChildIndex(this.linkLabelLearningRegex, 0);
            this.Controls.SetChildIndex(this.panel7, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.radioButton1, 0);
            this.Controls.SetChildIndex(this.radioButton2, 0);
            this.Controls.SetChildIndex(this.radioButton3, 0);
            this.Controls.SetChildIndex(this.radioButton4, 0);
            this.Controls.SetChildIndex(this.radioButton5, 0);
            this.contextMenuStripPattern.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panelNormal.ResumeLayout(false);
            this.panelNormal.PerformLayout();
            this.groupBoxMode.ResumeLayout(false);
            this.groupBoxMode.PerformLayout();
            this.panelScriptContent.ResumeLayout(false);
            this.panelScriptContent.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textSearchKeyword;
        private System.Windows.Forms.RadioButton radioButtonMethodRemove;
        private System.Windows.Forms.RadioButton radioButtonMethodMatch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox textSearchScriptContent;
        private Aga.Controls.Tree.NodeControls.NodeTextBox _Name;
        private Aga.Controls.Tree.NodeControls.NodeTextBox _Commet;
        private System.Windows.Forms.RadioButton radioButtonMethodNone;
        private System.Windows.Forms.Button buttonHistory;
        private System.Windows.Forms.Button buttonPattern;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripPattern;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripHis;
        private System.Windows.Forms.CheckBox checkBoxUseLikePath;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButtonBinary;
        private System.Windows.Forms.RadioButton radioButtonRegex;
        private System.Windows.Forms.Panel panelNormal;
        private System.Windows.Forms.CheckBox checkBoxStartMatch;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Panel panelScriptContent;
        private System.Windows.Forms.LinkLabel linkLabelLearningRegex;
        private System.Windows.Forms.RadioButton radioButtonUseNormal;
        private System.Windows.Forms.GroupBox groupBoxMode;
        private System.Windows.Forms.RadioButton radioButtonUseLike;
        private System.Windows.Forms.RadioButton radioButtonUseRegex;
        private System.Windows.Forms.CheckBox checkBoxNewCollection;
        private System.Windows.Forms.Label labelText;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
    }
}