
using pvfUtility.Dialog;

namespace pvfUtility.Actions.Batch
{
    partial class BatchTaskDialog
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
            this.radioButtonModeDelete = new System.Windows.Forms.RadioButton();
            this.radioButtonModeReplace = new System.Windows.Forms.RadioButton();
            this.radioButtonModeAdd = new System.Windows.Forms.RadioButton();
            this.labelText = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxFindContent = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxReplaceContent = new System.Windows.Forms.TextBox();
            this.panelModeDelete = new System.Windows.Forms.Panel();
            this.textBoxDeleteSectionName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonDeleteSection = new System.Windows.Forms.RadioButton();
            this.radioButtonDeleteContent = new System.Windows.Forms.RadioButton();
            this.textBoxDeleteContent = new System.Windows.Forms.TextBox();
            this.panelModeReplace = new System.Windows.Forms.Panel();
            this.panelModeAdd = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxAddContent = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panelModeDelete.SuspendLayout();
            this.panelModeReplace.SuspendLayout();
            this.panelModeAdd.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonModeDelete
            // 
            this.radioButtonModeDelete.AutoSize = true;
            this.radioButtonModeDelete.Location = new System.Drawing.Point(117, 19);
            this.radioButtonModeDelete.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonModeDelete.Name = "radioButtonModeDelete";
            this.radioButtonModeDelete.Size = new System.Drawing.Size(50, 21);
            this.radioButtonModeDelete.TabIndex = 2;
            this.radioButtonModeDelete.Text = "删除";
            this.radioButtonModeDelete.UseVisualStyleBackColor = true;
            this.radioButtonModeDelete.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // radioButtonModeReplace
            // 
            this.radioButtonModeReplace.AutoSize = true;
            this.radioButtonModeReplace.Location = new System.Drawing.Point(67, 19);
            this.radioButtonModeReplace.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonModeReplace.Name = "radioButtonModeReplace";
            this.radioButtonModeReplace.Size = new System.Drawing.Size(50, 21);
            this.radioButtonModeReplace.TabIndex = 1;
            this.radioButtonModeReplace.Text = "替换";
            this.radioButtonModeReplace.UseVisualStyleBackColor = true;
            this.radioButtonModeReplace.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // radioButtonModeAdd
            // 
            this.radioButtonModeAdd.AutoSize = true;
            this.radioButtonModeAdd.Checked = true;
            this.radioButtonModeAdd.Location = new System.Drawing.Point(17, 19);
            this.radioButtonModeAdd.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonModeAdd.Name = "radioButtonModeAdd";
            this.radioButtonModeAdd.Size = new System.Drawing.Size(50, 21);
            this.radioButtonModeAdd.TabIndex = 0;
            this.radioButtonModeAdd.TabStop = true;
            this.radioButtonModeAdd.Text = "追加";
            this.radioButtonModeAdd.UseVisualStyleBackColor = true;
            this.radioButtonModeAdd.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // labelText
            // 
            this.labelText.AutoSize = true;
            this.labelText.Location = new System.Drawing.Point(14, 45);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(17, 17);
            this.labelText.TabIndex = 3;
            this.labelText.Text = "...";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxFindContent);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(425, 87);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "搜索内容";
            // 
            // textBoxFindContent
            // 
            this.textBoxFindContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFindContent.Location = new System.Drawing.Point(3, 19);
            this.textBoxFindContent.Multiline = true;
            this.textBoxFindContent.Name = "textBoxFindContent";
            this.textBoxFindContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFindContent.Size = new System.Drawing.Size(419, 65);
            this.textBoxFindContent.TabIndex = 12;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxReplaceContent);
            this.groupBox3.Location = new System.Drawing.Point(6, 94);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(425, 91);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "替换为";
            // 
            // textBoxReplaceContent
            // 
            this.textBoxReplaceContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReplaceContent.Location = new System.Drawing.Point(3, 19);
            this.textBoxReplaceContent.Multiline = true;
            this.textBoxReplaceContent.Name = "textBoxReplaceContent";
            this.textBoxReplaceContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxReplaceContent.Size = new System.Drawing.Size(419, 69);
            this.textBoxReplaceContent.TabIndex = 2;
            // 
            // panelModeDelete
            // 
            this.panelModeDelete.Controls.Add(this.textBoxDeleteSectionName);
            this.panelModeDelete.Controls.Add(this.label1);
            this.panelModeDelete.Controls.Add(this.radioButtonDeleteSection);
            this.panelModeDelete.Controls.Add(this.radioButtonDeleteContent);
            this.panelModeDelete.Controls.Add(this.textBoxDeleteContent);
            this.panelModeDelete.Location = new System.Drawing.Point(10, 87);
            this.panelModeDelete.Name = "panelModeDelete";
            this.panelModeDelete.Size = new System.Drawing.Size(447, 186);
            this.panelModeDelete.TabIndex = 17;
            // 
            // textBoxDeleteSectionName
            // 
            this.textBoxDeleteSectionName.Location = new System.Drawing.Point(89, 3);
            this.textBoxDeleteSectionName.Name = "textBoxDeleteSectionName";
            this.textBoxDeleteSectionName.Size = new System.Drawing.Size(349, 23);
            this.textBoxDeleteSectionName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(432, 35);
            this.label1.TabIndex = 4;
            this.label1.Text = "如果该节有结束标识（如[Section]-[\\Section]），那么该节至结束标识内的全部内容都会被删除。";
            // 
            // radioButtonDeleteSection
            // 
            this.radioButtonDeleteSection.AutoSize = true;
            this.radioButtonDeleteSection.Checked = true;
            this.radioButtonDeleteSection.Location = new System.Drawing.Point(3, 2);
            this.radioButtonDeleteSection.Name = "radioButtonDeleteSection";
            this.radioButtonDeleteSection.Size = new System.Drawing.Size(98, 21);
            this.radioButtonDeleteSection.TabIndex = 0;
            this.radioButtonDeleteSection.TabStop = true;
            this.radioButtonDeleteSection.Text = "删除指定节：";
            this.radioButtonDeleteSection.UseVisualStyleBackColor = true;
            // 
            // radioButtonDeleteContent
            // 
            this.radioButtonDeleteContent.AutoSize = true;
            this.radioButtonDeleteContent.Location = new System.Drawing.Point(3, 66);
            this.radioButtonDeleteContent.Name = "radioButtonDeleteContent";
            this.radioButtonDeleteContent.Size = new System.Drawing.Size(110, 21);
            this.radioButtonDeleteContent.TabIndex = 2;
            this.radioButtonDeleteContent.Text = "删除指定内容：";
            this.radioButtonDeleteContent.UseVisualStyleBackColor = true;
            // 
            // textBoxDeleteContent
            // 
            this.textBoxDeleteContent.Location = new System.Drawing.Point(20, 93);
            this.textBoxDeleteContent.Multiline = true;
            this.textBoxDeleteContent.Name = "textBoxDeleteContent";
            this.textBoxDeleteContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDeleteContent.Size = new System.Drawing.Size(418, 83);
            this.textBoxDeleteContent.TabIndex = 3;
            // 
            // panelModeReplace
            // 
            this.panelModeReplace.Controls.Add(this.groupBox2);
            this.panelModeReplace.Controls.Add(this.groupBox3);
            this.panelModeReplace.Location = new System.Drawing.Point(4, 91);
            this.panelModeReplace.Name = "panelModeReplace";
            this.panelModeReplace.Size = new System.Drawing.Size(447, 185);
            this.panelModeReplace.TabIndex = 19;
            // 
            // panelModeAdd
            // 
            this.panelModeAdd.Controls.Add(this.label2);
            this.panelModeAdd.Controls.Add(this.textBoxAddContent);
            this.panelModeAdd.Location = new System.Drawing.Point(10, 91);
            this.panelModeAdd.Name = "panelModeAdd";
            this.panelModeAdd.Size = new System.Drawing.Size(441, 186);
            this.panelModeAdd.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "追加的内容";
            // 
            // textBoxAddContent
            // 
            this.textBoxAddContent.Location = new System.Drawing.Point(9, 23);
            this.textBoxAddContent.Multiline = true;
            this.textBoxAddContent.Name = "textBoxAddContent";
            this.textBoxAddContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxAddContent.Size = new System.Drawing.Size(422, 83);
            this.textBoxAddContent.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.labelText);
            this.groupBox4.Controls.Add(this.radioButtonModeDelete);
            this.groupBox4.Controls.Add(this.radioButtonModeAdd);
            this.groupBox4.Controls.Add(this.radioButtonModeReplace);
            this.groupBox4.Location = new System.Drawing.Point(12, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(445, 79);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "操作模式";
            // 
            // BatchTaskDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(468, 329);
            this.Controls.Add(this.panelModeDelete);
            this.Controls.Add(this.panelModeReplace);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.panelModeAdd);
            this.DialogButtons = pvfUtility.Dialog.DialogButton.OkCancel;
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BatchTaskDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批处理";
            this.Controls.SetChildIndex(this.panelModeAdd, 0);
            this.Controls.SetChildIndex(this.groupBox4, 0);
            this.Controls.SetChildIndex(this.panelModeReplace, 0);
            this.Controls.SetChildIndex(this.panelModeDelete, 0);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panelModeDelete.ResumeLayout(false);
            this.panelModeDelete.PerformLayout();
            this.panelModeReplace.ResumeLayout(false);
            this.panelModeAdd.ResumeLayout(false);
            this.panelModeAdd.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton radioButtonModeDelete;
        private System.Windows.Forms.RadioButton radioButtonModeReplace;
        private System.Windows.Forms.RadioButton radioButtonModeAdd;
        private System.Windows.Forms.Label labelText;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxFindContent;
        private System.Windows.Forms.TextBox textBoxReplaceContent;
        private System.Windows.Forms.Panel panelModeDelete;
        private System.Windows.Forms.TextBox textBoxDeleteSectionName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonDeleteSection;
        private System.Windows.Forms.RadioButton radioButtonDeleteContent;
        private System.Windows.Forms.TextBox textBoxDeleteContent;
        private System.Windows.Forms.Panel panelModeAdd;
        private System.Windows.Forms.TextBox textBoxAddContent;
        private System.Windows.Forms.Panel panelModeReplace;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label2;
    }
}