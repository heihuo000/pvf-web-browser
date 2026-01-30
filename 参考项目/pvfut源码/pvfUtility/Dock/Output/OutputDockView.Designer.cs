namespace pvfUtility.Shell
{
    partial class OutputDockView
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
            this.textBoxOutput = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOutput.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxOutput.Location = new System.Drawing.Point(0, 0);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.Size = new System.Drawing.Size(352, 131);
            this.textBoxOutput.TabIndex = 2;
            this.textBoxOutput.Text = "";
            this.textBoxOutput.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.textBoxOutput_LinkClicked);
            // 
            // OutputDockView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(352, 131);
            this.Controls.Add(this.textBoxOutput);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OutputDockView";
            this.Text = "输出";
            this.Load += new System.EventHandler(this.DummyOutput_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox textBoxOutput;
    }
}