using pvfUtility.Dialog;

namespace pvfUtility.Shell.Dialogs
{
    partial class ExtractDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxUseLike = new System.Windows.Forms.CheckBox();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.checkBoxDecompileScript = new System.Windows.Forms.CheckBox();
            this.checkBoxDecompileBinaryAni = new System.Windows.Forms.CheckBox();
            this.checkBoxOpenFolder = new System.Windows.Forms.CheckBox();
            this.checkBoxExtractAs7z = new System.Windows.Forms.CheckBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.labelTargetPath = new System.Windows.Forms.Label();
            this.textBoxTargetPath = new System.Windows.Forms.TextBox();
            this.buttonSelectPath = new System.Windows.Forms.Button();
            this.listBoxExtract = new System.Windows.Forms.CheckedListBox();
            this.buttonChangeToFolder = new System.Windows.Forms.Button();
            this.buttonDel = new System.Windows.Forms.Button();
            this.buttonSaveSetting = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonImportFromtxt = new System.Windows.Forms.Button();
            this.buttonSelectAll2 = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.toolTipChangeToFolder = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxConvertChinese = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "目录:";
            // 
            // checkBoxUseLike
            // 
            this.checkBoxUseLike.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxUseLike.AutoSize = true;
            this.checkBoxUseLike.Location = new System.Drawing.Point(391, 149);
            this.checkBoxUseLike.Name = "checkBoxUseLike";
            this.checkBoxUseLike.Size = new System.Drawing.Size(75, 21);
            this.checkBoxUseLike.TabIndex = 1;
            this.checkBoxUseLike.Text = "模糊匹配";
            this.checkBoxUseLike.UseVisualStyleBackColor = true;
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.Location = new System.Drawing.Point(53, 149);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(332, 23);
            this.textBoxPath.TabIndex = 2;
            // 
            // checkBoxDecompileScript
            // 
            this.checkBoxDecompileScript.AutoSize = true;
            this.checkBoxDecompileScript.Checked = true;
            this.checkBoxDecompileScript.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDecompileScript.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxDecompileScript.Location = new System.Drawing.Point(12, 28);
            this.checkBoxDecompileScript.Name = "checkBoxDecompileScript";
            this.checkBoxDecompileScript.Size = new System.Drawing.Size(494, 21);
            this.checkBoxDecompileScript.TabIndex = 18;
            this.checkBoxDecompileScript.Text = "反编译脚本文件";
            this.checkBoxDecompileScript.UseVisualStyleBackColor = true;
            // 
            // checkBoxDecompileBinaryAni
            // 
            this.checkBoxDecompileBinaryAni.AutoSize = true;
            this.checkBoxDecompileBinaryAni.Checked = true;
            this.checkBoxDecompileBinaryAni.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDecompileBinaryAni.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxDecompileBinaryAni.Location = new System.Drawing.Point(12, 49);
            this.checkBoxDecompileBinaryAni.Name = "checkBoxDecompileBinaryAni";
            this.checkBoxDecompileBinaryAni.Size = new System.Drawing.Size(494, 21);
            this.checkBoxDecompileBinaryAni.TabIndex = 19;
            this.checkBoxDecompileBinaryAni.Text = "反编译二进制ani文件";
            this.checkBoxDecompileBinaryAni.UseVisualStyleBackColor = true;
            // 
            // checkBoxOpenFolder
            // 
            this.checkBoxOpenFolder.AutoSize = true;
            this.checkBoxOpenFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxOpenFolder.Location = new System.Drawing.Point(12, 70);
            this.checkBoxOpenFolder.Name = "checkBoxOpenFolder";
            this.checkBoxOpenFolder.Size = new System.Drawing.Size(494, 21);
            this.checkBoxOpenFolder.TabIndex = 20;
            this.checkBoxOpenFolder.Text = "完成后打开目录";
            this.checkBoxOpenFolder.UseVisualStyleBackColor = true;
            // 
            // checkBoxExtractAs7z
            // 
            this.checkBoxExtractAs7z.AutoSize = true;
            this.checkBoxExtractAs7z.Location = new System.Drawing.Point(9, 39);
            this.checkBoxExtractAs7z.Name = "checkBoxExtractAs7z";
            this.checkBoxExtractAs7z.Size = new System.Drawing.Size(292, 21);
            this.checkBoxExtractAs7z.TabIndex = 21;
            this.checkBoxExtractAs7z.Text = "提取到7zip压缩包中（按确定后选择7z保存路径）";
            this.checkBoxExtractAs7z.UseVisualStyleBackColor = true;
            this.checkBoxExtractAs7z.CheckedChanged += new System.EventHandler(this.checkBoxExtractAs7z_CheckedChanged);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.AutoSize = true;
            this.buttonAdd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonAdd.Location = new System.Drawing.Point(479, 145);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(28, 27);
            this.buttonAdd.TabIndex = 23;
            this.buttonAdd.Text = "⇱";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // labelTargetPath
            // 
            this.labelTargetPath.AutoSize = true;
            this.labelTargetPath.Location = new System.Drawing.Point(6, 19);
            this.labelTargetPath.Name = "labelTargetPath";
            this.labelTargetPath.Size = new System.Drawing.Size(56, 17);
            this.labelTargetPath.TabIndex = 26;
            this.labelTargetPath.Text = "提取到：";
            // 
            // textBoxTargetPath
            // 
            this.textBoxTargetPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTargetPath.Location = new System.Drawing.Point(68, 16);
            this.textBoxTargetPath.Name = "textBoxTargetPath";
            this.textBoxTargetPath.Size = new System.Drawing.Size(398, 23);
            this.textBoxTargetPath.TabIndex = 27;
            // 
            // buttonSelectPath
            // 
            this.buttonSelectPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectPath.Location = new System.Drawing.Point(475, 16);
            this.buttonSelectPath.Name = "buttonSelectPath";
            this.buttonSelectPath.Size = new System.Drawing.Size(32, 23);
            this.buttonSelectPath.TabIndex = 28;
            this.buttonSelectPath.Text = "...";
            this.buttonSelectPath.UseVisualStyleBackColor = true;
            this.buttonSelectPath.Click += new System.EventHandler(this.buttonSelectPath_Click);
            // 
            // listBoxExtract
            // 
            this.listBoxExtract.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxExtract.CheckOnClick = true;
            this.listBoxExtract.FormattingEnabled = true;
            this.listBoxExtract.Location = new System.Drawing.Point(7, 22);
            this.listBoxExtract.Name = "listBoxExtract";
            this.listBoxExtract.Size = new System.Drawing.Size(500, 94);
            this.listBoxExtract.TabIndex = 29;
            // 
            // buttonChangeToFolder
            // 
            this.buttonChangeToFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonChangeToFolder.AutoSize = true;
            this.buttonChangeToFolder.Location = new System.Drawing.Point(206, 118);
            this.buttonChangeToFolder.Name = "buttonChangeToFolder";
            this.buttonChangeToFolder.Size = new System.Drawing.Size(186, 27);
            this.buttonChangeToFolder.TabIndex = 30;
            this.buttonChangeToFolder.Text = "从文件切换到文件所在的文件夹";
            this.buttonChangeToFolder.UseVisualStyleBackColor = true;
            this.buttonChangeToFolder.Click += new System.EventHandler(this.buttonChangeToFolder_Click);
            // 
            // buttonDel
            // 
            this.buttonDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDel.AutoSize = true;
            this.buttonDel.Location = new System.Drawing.Point(479, 118);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(28, 27);
            this.buttonDel.TabIndex = 32;
            this.buttonDel.Text = "✗";
            this.buttonDel.UseVisualStyleBackColor = true;
            this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
            // 
            // buttonSaveSetting
            // 
            this.buttonSaveSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSaveSetting.AutoSize = true;
            this.buttonSaveSetting.Location = new System.Drawing.Point(11, 405);
            this.buttonSaveSetting.Name = "buttonSaveSetting";
            this.buttonSaveSetting.Size = new System.Drawing.Size(125, 27);
            this.buttonSaveSetting.TabIndex = 33;
            this.buttonSaveSetting.Text = "保存为默认配置";
            this.buttonSaveSetting.UseVisualStyleBackColor = true;
            this.buttonSaveSetting.Click += new System.EventHandler(this.buttonSaveSetting_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 17);
            this.label2.TabIndex = 30;
            this.label2.Text = "默认的目录为pvf所在目录,可以在选项中固定其他目录";
            // 
            // buttonImportFromtxt
            // 
            this.buttonImportFromtxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonImportFromtxt.AutoSize = true;
            this.buttonImportFromtxt.Location = new System.Drawing.Point(105, 118);
            this.buttonImportFromtxt.Name = "buttonImportFromtxt";
            this.buttonImportFromtxt.Size = new System.Drawing.Size(95, 27);
            this.buttonImportFromtxt.TabIndex = 36;
            this.buttonImportFromtxt.Text = "从txt导入";
            this.buttonImportFromtxt.UseVisualStyleBackColor = true;
            this.buttonImportFromtxt.Click += new System.EventHandler(this.buttonImportFromTxt_Click);
            // 
            // buttonSelectAll2
            // 
            this.buttonSelectAll2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectAll2.AutoSize = true;
            this.buttonSelectAll2.Location = new System.Drawing.Point(55, 118);
            this.buttonSelectAll2.Name = "buttonSelectAll2";
            this.buttonSelectAll2.Size = new System.Drawing.Size(44, 27);
            this.buttonSelectAll2.TabIndex = 36;
            this.buttonSelectAll2.Text = "反选";
            this.buttonSelectAll2.UseVisualStyleBackColor = true;
            this.buttonSelectAll2.Click += new System.EventHandler(this.buttonSelectAll2_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectAll.AutoSize = true;
            this.buttonSelectAll.Location = new System.Drawing.Point(5, 118);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(44, 27);
            this.buttonSelectAll.TabIndex = 35;
            this.buttonSelectAll.Text = "全选";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // toolTipChangeToFolder
            // 
            this.toolTipChangeToFolder.AutomaticDelay = 0;
            this.toolTipChangeToFolder.AutoPopDelay = 500000;
            this.toolTipChangeToFolder.InitialDelay = 0;
            this.toolTipChangeToFolder.IsBalloon = true;
            this.toolTipChangeToFolder.ReshowDelay = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.labelTargetPath);
            this.groupBox1.Controls.Add(this.buttonSelectPath);
            this.groupBox1.Controls.Add(this.textBoxTargetPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.checkBoxExtractAs7z);
            this.groupBox1.Location = new System.Drawing.Point(10, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(518, 93);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "提取位置";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listBoxExtract);
            this.groupBox2.Controls.Add(this.buttonImportFromtxt);
            this.groupBox2.Controls.Add(this.checkBoxUseLike);
            this.groupBox2.Controls.Add(this.buttonSelectAll2);
            this.groupBox2.Controls.Add(this.buttonChangeToFolder);
            this.groupBox2.Controls.Add(this.buttonSelectAll);
            this.groupBox2.Controls.Add(this.buttonDel);
            this.groupBox2.Controls.Add(this.buttonAdd);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBoxPath);
            this.groupBox2.Location = new System.Drawing.Point(10, 101);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(518, 176);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "提取目标";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.checkBoxConvertChinese);
            this.groupBox3.Controls.Add(this.checkBoxOpenFolder);
            this.groupBox3.Controls.Add(this.checkBoxDecompileBinaryAni);
            this.groupBox3.Controls.Add(this.checkBoxDecompileScript);
            this.groupBox3.Location = new System.Drawing.Point(10, 277);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(12);
            this.groupBox3.Size = new System.Drawing.Size(518, 116);
            this.groupBox3.TabIndex = 38;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "提取选项";
            // 
            // checkBoxConvertChinese
            // 
            this.checkBoxConvertChinese.AutoSize = true;
            this.checkBoxConvertChinese.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxConvertChinese.Location = new System.Drawing.Point(12, 91);
            this.checkBoxConvertChinese.Name = "checkBoxConvertChinese";
            this.checkBoxConvertChinese.Size = new System.Drawing.Size(494, 21);
            this.checkBoxConvertChinese.TabIndex = 21;
            this.checkBoxConvertChinese.Text = "转换为简体中文";
            this.checkBoxConvertChinese.UseVisualStyleBackColor = true;
            // 
            // ExtractDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(537, 441);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonSaveSetting);
            this.DialogButtons = pvfUtility.Dialog.DialogButton.OkCancel;
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "ExtractDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提取";
            this.Controls.SetChildIndex(this.buttonSaveSetting, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.groupBox3, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.CheckBox checkBoxUseLike;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxDecompileScript;
        private System.Windows.Forms.CheckBox checkBoxDecompileBinaryAni;
        private System.Windows.Forms.CheckBox checkBoxOpenFolder;
        private System.Windows.Forms.CheckBox checkBoxExtractAs7z;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Label labelTargetPath;
        private System.Windows.Forms.TextBox textBoxTargetPath;
        private System.Windows.Forms.Button buttonSelectPath;
        private System.Windows.Forms.CheckedListBox listBoxExtract;
        private System.Windows.Forms.Button buttonChangeToFolder;
        private System.Windows.Forms.Button buttonDel;
        private System.Windows.Forms.Button buttonSaveSetting;
        private System.Windows.Forms.ToolTip toolTipChangeToFolder;
        private System.Windows.Forms.Button buttonSelectAll2;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonImportFromtxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxConvertChinese;
    }
}