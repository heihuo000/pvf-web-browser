namespace pvfUtility.Shell.Dialogs
{
    partial class FileExistDialog
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
            this.buttonRename = new System.Windows.Forms.Button();
            this.buttonOverwrite = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSkip = new System.Windows.Forms.Button();
            this.labelSource = new System.Windows.Forms.Label();
            this.labelDestination = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxApplyToAll = new System.Windows.Forms.CheckBox();
            this.mErrorIconLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonRename
            // 
            this.buttonRename.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonRename.Location = new System.Drawing.Point(137, 177);
            this.buttonRename.Name = "buttonRename";
            this.buttonRename.Size = new System.Drawing.Size(85, 27);
            this.buttonRename.TabIndex = 7;
            this.buttonRename.Text = "重命名";
            this.buttonRename.UseVisualStyleBackColor = true;
            this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
            // 
            // buttonOverwrite
            // 
            this.buttonOverwrite.Location = new System.Drawing.Point(25, 177);
            this.buttonOverwrite.Name = "buttonOverwrite";
            this.buttonOverwrite.Size = new System.Drawing.Size(85, 27);
            this.buttonOverwrite.TabIndex = 6;
            this.buttonOverwrite.Text = "覆盖";
            this.buttonOverwrite.UseVisualStyleBackColor = true;
            this.buttonOverwrite.Click += new System.EventHandler(this.buttonOverwrite_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(358, 177);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(85, 27);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSkip
            // 
            this.buttonSkip.Location = new System.Drawing.Point(246, 177);
            this.buttonSkip.Name = "buttonSkip";
            this.buttonSkip.Size = new System.Drawing.Size(85, 27);
            this.buttonSkip.TabIndex = 8;
            this.buttonSkip.Text = "跳过";
            this.buttonSkip.UseVisualStyleBackColor = true;
            this.buttonSkip.Click += new System.EventHandler(this.buttonSkip_Click);
            // 
            // labelSource
            // 
            this.labelSource.Location = new System.Drawing.Point(76, 41);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(361, 53);
            this.labelSource.TabIndex = 10;
            this.labelSource.Text = "0";
            // 
            // labelDestination
            // 
            this.labelDestination.Location = new System.Drawing.Point(22, 94);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(415, 53);
            this.labelDestination.TabIndex = 11;
            this.labelDestination.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "下列文件已存在:";
            // 
            // checkBoxApplyToAll
            // 
            this.checkBoxApplyToAll.AutoSize = true;
            this.checkBoxApplyToAll.Location = new System.Drawing.Point(25, 150);
            this.checkBoxApplyToAll.Name = "checkBoxApplyToAll";
            this.checkBoxApplyToAll.Size = new System.Drawing.Size(111, 21);
            this.checkBoxApplyToAll.TabIndex = 13;
            this.checkBoxApplyToAll.Text = "应用于所有文件";
            this.checkBoxApplyToAll.UseVisualStyleBackColor = true;
            // 
            // mErrorIconLabel
            // 
            this.mErrorIconLabel.Location = new System.Drawing.Point(22, 41);
            this.mErrorIconLabel.Name = "mErrorIconLabel";
            this.mErrorIconLabel.Size = new System.Drawing.Size(48, 48);
            this.mErrorIconLabel.TabIndex = 14;
            // 
            // FileExistDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 220);
            this.Controls.Add(this.mErrorIconLabel);
            this.Controls.Add(this.checkBoxApplyToAll);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelDestination);
            this.Controls.Add(this.labelSource);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSkip);
            this.Controls.Add(this.buttonRename);
            this.Controls.Add(this.buttonOverwrite);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileExistDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "文件冲突";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRename;
        private System.Windows.Forms.Button buttonOverwrite;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSkip;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Label labelDestination;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxApplyToAll;
        private System.Windows.Forms.Label mErrorIconLabel;
    }
}