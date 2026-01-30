using pvfUtility.Actions;
using pvfUtility.Dialog;

namespace pvfUtility.Model.PvfOperation
{
    partial class FileProperties
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
            this.labelName = new System.Windows.Forms.Label();
            this.listBoxProperties = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(15, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(442, 38);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "文件名";
            // 
            // listBoxProperties
            // 
            this.listBoxProperties.FormattingEnabled = true;
            this.listBoxProperties.ItemHeight = 17;
            this.listBoxProperties.Location = new System.Drawing.Point(15, 59);
            this.listBoxProperties.Name = "listBoxProperties";
            this.listBoxProperties.Size = new System.Drawing.Size(443, 140);
            this.listBoxProperties.TabIndex = 3;
            // 
            // FileProperties
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(469, 279);
            this.Controls.Add(this.listBoxProperties);
            this.Controls.Add(this.labelName);
            this.DialogButtons = DialogButton.Close;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "属性";
            this.Controls.SetChildIndex(this.labelName, 0);
            this.Controls.SetChildIndex(this.listBoxProperties, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ListBox listBoxProperties;
    }
}