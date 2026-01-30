using pvfUtility.Dialog;

namespace pvfUtility.Model.PvfOperation
{
    partial class PvfHeaderEditorDialog
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
            this.textBoxGuid = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxFileVersion = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Guid:";
            // 
            // textBoxGuid
            // 
            this.textBoxGuid.Location = new System.Drawing.Point(73, 22);
            this.textBoxGuid.Name = "textBoxGuid";
            this.textBoxGuid.Size = new System.Drawing.Size(271, 23);
            this.textBoxGuid.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "版本:";
            // 
            // textBoxFileVersion
            // 
            this.textBoxFileVersion.Location = new System.Drawing.Point(73, 51);
            this.textBoxFileVersion.Name = "textBoxFileVersion";
            this.textBoxFileVersion.Size = new System.Drawing.Size(271, 23);
            this.textBoxFileVersion.TabIndex = 3;
            // 
            // PvfHeaderEditorDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(373, 125);
            this.Controls.Add(this.textBoxFileVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxGuid);
            this.Controls.Add(this.label1);
            this.DialogButtons = DialogButton.OkCancel;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PvfHeaderEditorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "pvf 头修改器";
            this.Load += new System.EventHandler(this.PvfHeaderEditor_Load);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.textBoxGuid, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.textBoxFileVersion, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxGuid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxFileVersion;
    }
}