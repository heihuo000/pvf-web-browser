namespace pvfUtility.Shell
{
    partial class ErrorListDockView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorListDockView));
            this.listViewError = new System.Windows.Forms.ListView();
            this.columnHeaderAction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderErrorDetail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLine = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonClearAll = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewError
            // 
            this.listViewError.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewError.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAction,
            this.columnHeaderFileName,
            this.columnHeaderErrorDetail,
            this.columnHeaderLine});
            this.listViewError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewError.FullRowSelect = true;
            this.listViewError.HideSelection = false;
            this.listViewError.Location = new System.Drawing.Point(0, 25);
            this.listViewError.Name = "listViewError";
            this.listViewError.Size = new System.Drawing.Size(908, 101);
            this.listViewError.TabIndex = 0;
            this.listViewError.UseCompatibleStateImageBehavior = false;
            this.listViewError.View = System.Windows.Forms.View.Details;
            this.listViewError.DoubleClick += new System.EventHandler(this.listViewError_DoubleClick);
            // 
            // columnHeaderAction
            // 
            this.columnHeaderAction.Text = "操作";
            this.columnHeaderAction.Width = 100;
            // 
            // columnHeaderFileName
            // 
            this.columnHeaderFileName.Text = "文件";
            this.columnHeaderFileName.Width = 400;
            // 
            // columnHeaderErrorDetail
            // 
            this.columnHeaderErrorDetail.Text = "说明";
            this.columnHeaderErrorDetail.Width = 300;
            // 
            // columnHeaderLine
            // 
            this.columnHeaderLine.Text = "行";
            this.columnHeaderLine.Width = 50;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonClearAll});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(908, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonClearAll
            // 
            this.toolStripButtonClearAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonClearAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClearAll.Image")));
            this.toolStripButtonClearAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClearAll.Name = "toolStripButtonClearAll";
            this.toolStripButtonClearAll.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonClearAll.Text = "清除全部";
            this.toolStripButtonClearAll.Click += new System.EventHandler(this.toolStripButtonClearAll_Click);
            // 
            // ErrorListDockView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 126);
            this.Controls.Add(this.listViewError);
            this.Controls.Add(this.toolStrip1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ErrorListDockView";
            this.Text = "错误列表";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewError;
        private System.Windows.Forms.ColumnHeader columnHeaderFileName;
        private System.Windows.Forms.ColumnHeader columnHeaderErrorDetail;
        private System.Windows.Forms.ColumnHeader columnHeaderLine;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonClearAll;
        private System.Windows.Forms.ColumnHeader columnHeaderAction;
    }
}