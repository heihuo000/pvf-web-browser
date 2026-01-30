namespace pvfUtility.Actions
{
    partial class DialogBase
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
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.flowInner = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnRetry = new System.Windows.Forms.Button();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.pnlFooter.SuspendLayout();
            this.flowInner.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.SystemColors.Control;
            this.pnlFooter.Controls.Add(this.flowInner);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 257);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(731, 42);
            this.pnlFooter.TabIndex = 1;
            // 
            // flowInner
            // 
            this.flowInner.BackColor = System.Drawing.SystemColors.Control;
            this.flowInner.Controls.Add(this.btnOk);
            this.flowInner.Controls.Add(this.btnCancel);
            this.flowInner.Controls.Add(this.btnClose);
            this.flowInner.Controls.Add(this.btnYes);
            this.flowInner.Controls.Add(this.btnNo);
            this.flowInner.Controls.Add(this.btnAbort);
            this.flowInner.Controls.Add(this.btnRetry);
            this.flowInner.Controls.Add(this.btnIgnore);
            this.flowInner.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowInner.Location = new System.Drawing.Point(68, 0);
            this.flowInner.Name = "flowInner";
            this.flowInner.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.flowInner.Size = new System.Drawing.Size(663, 42);
            this.flowInner.TabIndex = 10014;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(10, 9);
            this.btnOk.Margin = new System.Windows.Forms.Padding(0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 24);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "确定";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(85, 9);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(160, 9);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 24);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "关闭";
            // 
            // btnYes
            // 
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYes.Location = new System.Drawing.Point(235, 9);
            this.btnYes.Margin = new System.Windows.Forms.Padding(0);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 24);
            this.btnYes.TabIndex = 6;
            this.btnYes.Text = "是";
            // 
            // btnNo
            // 
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.Location = new System.Drawing.Point(310, 9);
            this.btnNo.Margin = new System.Windows.Forms.Padding(0);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 24);
            this.btnNo.TabIndex = 7;
            this.btnNo.Text = "否";
            // 
            // btnAbort
            // 
            this.btnAbort.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAbort.Location = new System.Drawing.Point(385, 9);
            this.btnAbort.Margin = new System.Windows.Forms.Padding(0);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(75, 24);
            this.btnAbort.TabIndex = 8;
            this.btnAbort.Text = "终止";
            // 
            // btnRetry
            // 
            this.btnRetry.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btnRetry.Location = new System.Drawing.Point(460, 9);
            this.btnRetry.Margin = new System.Windows.Forms.Padding(0);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(75, 24);
            this.btnRetry.TabIndex = 9;
            this.btnRetry.Text = "重试";
            // 
            // btnIgnore
            // 
            this.btnIgnore.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnIgnore.Location = new System.Drawing.Point(535, 9);
            this.btnIgnore.Margin = new System.Windows.Forms.Padding(0);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(75, 24);
            this.btnIgnore.TabIndex = 10;
            this.btnIgnore.Text = "忽略";
            // 
            // DialogBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(731, 299);
            this.Controls.Add(this.pnlFooter);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DialogBase";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.pnlFooter.ResumeLayout(false);
            this.flowInner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.FlowLayoutPanel flowInner;
        protected System.Windows.Forms.Button btnOk;
        protected System.Windows.Forms.Button btnCancel;
        protected System.Windows.Forms.Button btnClose;
        protected System.Windows.Forms.Button btnYes;
        protected System.Windows.Forms.Button btnNo;
        protected System.Windows.Forms.Button btnAbort;
        protected System.Windows.Forms.Button btnRetry;
        protected System.Windows.Forms.Button btnIgnore;
    }
}