namespace pvfUtility.Shell.Tools
{
    partial class NameSearcher
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
            this.textBoxKeyword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.listViewCodeSearcher = new System.Windows.Forms.ListView();
            this.columnHeaderFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderScriptName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxKeyword
            // 
            this.textBoxKeyword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxKeyword.Location = new System.Drawing.Point(65, 6);
            this.textBoxKeyword.Name = "textBoxKeyword";
            this.textBoxKeyword.Size = new System.Drawing.Size(325, 23);
            this.textBoxKeyword.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "关键词:";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(396, 6);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(78, 23);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "搜索";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // listViewCodeSearcher
            // 
            this.listViewCodeSearcher.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCodeSearcher.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFileName,
            this.columnHeaderScriptName});
            this.listViewCodeSearcher.HideSelection = false;
            this.listViewCodeSearcher.Location = new System.Drawing.Point(12, 35);
            this.listViewCodeSearcher.Name = "listViewCodeSearcher";
            this.listViewCodeSearcher.Size = new System.Drawing.Size(459, 279);
            this.listViewCodeSearcher.TabIndex = 7;
            this.listViewCodeSearcher.UseCompatibleStateImageBehavior = false;
            this.listViewCodeSearcher.View = System.Windows.Forms.View.Details;
            this.listViewCodeSearcher.DoubleClick += new System.EventHandler(this.listViewCodeSearcher_DoubleClick);
            // 
            // columnHeaderFileName
            // 
            this.columnHeaderFileName.Text = "文件名";
            this.columnHeaderFileName.Width = 305;
            // 
            // columnHeaderScriptName
            // 
            this.columnHeaderScriptName.Text = "名称";
            this.columnHeaderScriptName.Width = 150;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 317);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "双击打开文件";
            // 
            // NameSearcher
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(480, 338);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listViewCodeSearcher);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxKeyword);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NameSearcher";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "名称搜索器";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxKeyword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.ListView listViewCodeSearcher;
        private System.Windows.Forms.ColumnHeader columnHeaderFileName;
        private System.Windows.Forms.ColumnHeader columnHeaderScriptName;
        private System.Windows.Forms.Label label2;
    }
}