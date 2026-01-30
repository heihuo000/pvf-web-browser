using pvfUtility.Dialog;

namespace pvfUtility.Shell.Dialogs
{
    partial class SelectFolderDialog
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
            this.labelCurrentSelected = new System.Windows.Forms.Label();
            this.treeViewPath = new Aga.Controls.Tree.TreeViewAdv();
            this._Name = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.SuspendLayout();
            // 
            // labelCurrentSelected
            // 
            this.labelCurrentSelected.Location = new System.Drawing.Point(10, 9);
            this.labelCurrentSelected.Name = "labelCurrentSelected";
            this.labelCurrentSelected.Size = new System.Drawing.Size(369, 18);
            this.labelCurrentSelected.TabIndex = 59;
            this.labelCurrentSelected.Text = "选择文件夹";
            // 
            // treeViewPath
            // 
            this.treeViewPath.AsyncExpanding = true;
            this.treeViewPath.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeViewPath.ColumnHeaderHeight = 0;
            this.treeViewPath.DefaultToolTipProvider = null;
            this.treeViewPath.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewPath.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewPath.FullRowSelect = true;
            this.treeViewPath.FullRowSelectActiveColor = System.Drawing.SystemColors.Highlight;
            this.treeViewPath.FullRowSelectInactiveColor = System.Drawing.SystemColors.ControlDark;
            this.treeViewPath.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
            this.treeViewPath.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeViewPath.LoadOnDemand = true;
            this.treeViewPath.Location = new System.Drawing.Point(13, 36);
            this.treeViewPath.Margin = new System.Windows.Forms.Padding(4);
            this.treeViewPath.Model = null;
            this.treeViewPath.Name = "treeViewPath";
            this.treeViewPath.NodeControls.Add(this._Name);
            this.treeViewPath.NodeFilter = null;
            this.treeViewPath.SelectedNode = null;
            this.treeViewPath.ShowLines = false;
            this.treeViewPath.ShowNodeToolTips = true;
            this.treeViewPath.Size = new System.Drawing.Size(368, 257);
            this.treeViewPath.TabIndex = 58;
            this.treeViewPath.NodeMouseClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this.treeViewPath_NodeMouseClick);
            // 
            // _Name
            // 
            this._Name.DataPropertyName = "Name";
            this._Name.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F);
            this._Name.IncrementalSearchEnabled = true;
            this._Name.LeftMargin = 3;
            this._Name.ParentColumn = null;
            // 
            // SelectFolderDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(395, 345);
            this.Controls.Add(this.labelCurrentSelected);
            this.Controls.Add(this.treeViewPath);
            this.DialogButtons = pvfUtility.Dialog.DialogButton.OkCancel;
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectFolderDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择文件夹";
            this.TopMost = true;
            this.Controls.SetChildIndex(this.treeViewPath, 0);
            this.Controls.SetChildIndex(this.labelCurrentSelected, 0);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelCurrentSelected;
        private Aga.Controls.Tree.TreeViewAdv treeViewPath;
        private Aga.Controls.Tree.NodeControls.NodeTextBox _Name;
    }
}