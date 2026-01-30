namespace pvfUtility.Shell.Tools
{
    partial class CodeSearcher
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxLstFiles = new System.Windows.Forms.ComboBox();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.buttonExport = new System.Windows.Forms.Button();
            this.listViewCodeSearcher = new System.Windows.Forms.ListView();
            this.columnHeaderFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderScriptName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "lst源：";
            // 
            // comboBoxLstFiles
            // 
            this.comboBoxLstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLstFiles.FormattingEnabled = true;
            this.comboBoxLstFiles.Location = new System.Drawing.Point(60, 8);
            this.comboBoxLstFiles.Name = "comboBoxLstFiles";
            this.comboBoxLstFiles.Size = new System.Drawing.Size(633, 25);
            this.comboBoxLstFiles.TabIndex = 2;
            // 
            // textBoxCode
            // 
            this.textBoxCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCode.Location = new System.Drawing.Point(14, 39);
            this.textBoxCode.Multiline = true;
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(679, 79);
            this.textBoxCode.TabIndex = 3;
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(132, 444);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(167, 27);
            this.buttonExport.TabIndex = 5;
            this.buttonExport.Text = "导出文件至当前文件集";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // listViewCodeSearcher
            // 
            this.listViewCodeSearcher.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCodeSearcher.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFileName,
            this.columnHeaderScriptName,
            this.columnHeaderCode});
            this.listViewCodeSearcher.HideSelection = false;
            this.listViewCodeSearcher.Location = new System.Drawing.Point(12, 124);
            this.listViewCodeSearcher.Name = "listViewCodeSearcher";
            this.listViewCodeSearcher.Size = new System.Drawing.Size(681, 296);
            this.listViewCodeSearcher.TabIndex = 6;
            this.listViewCodeSearcher.UseCompatibleStateImageBehavior = false;
            this.listViewCodeSearcher.View = System.Windows.Forms.View.Details;
            this.listViewCodeSearcher.DoubleClick += new System.EventHandler(this.listViewCodeSearcher_DoubleClick);
            // 
            // columnHeaderFileName
            // 
            this.columnHeaderFileName.Text = "文件名";
            this.columnHeaderFileName.Width = 250;
            // 
            // columnHeaderScriptName
            // 
            this.columnHeaderScriptName.Text = "名称";
            this.columnHeaderScriptName.Width = 150;
            // 
            // columnHeaderCode
            // 
            this.columnHeaderCode.Text = "代码";
            this.columnHeaderCode.Width = 100;
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.Location = new System.Drawing.Point(14, 444);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(112, 27);
            this.buttonClear.TabIndex = 7;
            this.buttonClear.Text = "清除";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(581, 444);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(112, 27);
            this.buttonSearch.TabIndex = 8;
            this.buttonSearch.Text = "搜索";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 423);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "双击打开文件";
            // 
            // CodeSearcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 483);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.listViewCodeSearcher);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.textBoxCode);
            this.Controls.Add(this.comboBoxLstFiles);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CodeSearcher";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "代码搜索器";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.CodeSearcher_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxLstFiles;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.ListView listViewCodeSearcher;
        private System.Windows.Forms.ColumnHeader columnHeaderCode;
        private System.Windows.Forms.ColumnHeader columnHeaderFileName;
        private System.Windows.Forms.ColumnHeader columnHeaderScriptName;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Label label2;
    }
}