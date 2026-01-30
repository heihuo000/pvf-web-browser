using pvfUtility.Dialog;
using pvfUtility.Dock.FileExplorer.Bookmark;

namespace pvfUtility.FileExplorer.Bookmark
{
    partial class BookmarkManager
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
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonNewFolder = new System.Windows.Forms.Button();
            this.bookmarkTreeView = new BookmarkTreeView();
            this.buttonAddSeparator = new System.Windows.Forms.Button();
            this.textBoxFolderName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.buttonNewBookmark = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(400, 10);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(64, 32);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "删除";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(400, 48);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(64, 32);
            this.buttonUp.TabIndex = 2;
            this.buttonUp.Text = "上移";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Location = new System.Drawing.Point(400, 86);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(64, 32);
            this.buttonDown.TabIndex = 3;
            this.buttonDown.Text = "下移";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonNewFolder
            // 
            this.buttonNewFolder.Location = new System.Drawing.Point(203, 307);
            this.buttonNewFolder.Name = "buttonNewFolder";
            this.buttonNewFolder.Size = new System.Drawing.Size(64, 24);
            this.buttonNewFolder.TabIndex = 4;
            this.buttonNewFolder.Text = "新建目录";
            this.buttonNewFolder.UseVisualStyleBackColor = true;
            this.buttonNewFolder.Click += new System.EventHandler(this.buttonNewFolder_Click);
            // 
            // bookmarkTreeView
            // 
            this.bookmarkTreeView.FullRowSelect = true;
            this.bookmarkTreeView.HideSelection = false;
            this.bookmarkTreeView.Location = new System.Drawing.Point(12, 12);
            this.bookmarkTreeView.Name = "bookmarkTreeView";
            this.bookmarkTreeView.ShowNodeToolTips = true;
            this.bookmarkTreeView.Size = new System.Drawing.Size(382, 289);
            this.bookmarkTreeView.TabIndex = 5;
            this.bookmarkTreeView.UserEditable = true;
            // 
            // buttonAddSeparator
            // 
            this.buttonAddSeparator.Location = new System.Drawing.Point(273, 307);
            this.buttonAddSeparator.Name = "buttonAddSeparator";
            this.buttonAddSeparator.Size = new System.Drawing.Size(64, 24);
            this.buttonAddSeparator.TabIndex = 6;
            this.buttonAddSeparator.Text = "新建分栏";
            this.buttonAddSeparator.UseVisualStyleBackColor = true;
            this.buttonAddSeparator.Click += new System.EventHandler(this.buttonAddSeparator_Click);
            // 
            // textBoxFolderName
            // 
            this.textBoxFolderName.Location = new System.Drawing.Point(59, 307);
            this.textBoxFolderName.Name = "textBoxFolderName";
            this.textBoxFolderName.Size = new System.Drawing.Size(138, 23);
            this.textBoxFolderName.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 310);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "名称：";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(12, 349);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 17);
            this.label2.TabIndex = 12;
            this.label2.Text = "拖曳列表中项目即可改变顺序、从属";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.linkLabel1.Location = new System.Drawing.Point(221, 349);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(116, 17);
            this.linkLabel1.TabIndex = 13;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "打开Bookmark.xml";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // buttonNewBookmark
            // 
            this.buttonNewBookmark.Location = new System.Drawing.Point(343, 307);
            this.buttonNewBookmark.Name = "buttonNewBookmark";
            this.buttonNewBookmark.Size = new System.Drawing.Size(121, 24);
            this.buttonNewBookmark.TabIndex = 14;
            this.buttonNewBookmark.Text = "新建书签...";
            this.buttonNewBookmark.UseVisualStyleBackColor = true;
            this.buttonNewBookmark.Click += new System.EventHandler(this.buttonNewBookmark_Click);
            // 
            // BookmarkManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 380);
            this.Controls.Add(this.buttonNewBookmark);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxFolderName);
            this.Controls.Add(this.buttonAddSeparator);
            this.Controls.Add(this.bookmarkTreeView);
            this.Controls.Add(this.buttonNewFolder);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonDelete);
            this.DialogButtons = DialogButton.Close;
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BookmarkManager";
            this.ShowInTaskbar = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "书签管理器";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BookmarkManager_FormClosed);
            this.Load += new System.EventHandler(this.BookmarkManager_Load);
            this.Controls.SetChildIndex(this.buttonDelete, 0);
            this.Controls.SetChildIndex(this.buttonUp, 0);
            this.Controls.SetChildIndex(this.buttonDown, 0);
            this.Controls.SetChildIndex(this.buttonNewFolder, 0);
            this.Controls.SetChildIndex(this.bookmarkTreeView, 0);
            this.Controls.SetChildIndex(this.buttonAddSeparator, 0);
            this.Controls.SetChildIndex(this.textBoxFolderName, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.linkLabel1, 0);
            this.Controls.SetChildIndex(this.buttonNewBookmark, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonNewFolder;
        private BookmarkTreeView bookmarkTreeView;
        private System.Windows.Forms.Button buttonAddSeparator;
        private System.Windows.Forms.TextBox textBoxFolderName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button buttonNewBookmark;
    }
}