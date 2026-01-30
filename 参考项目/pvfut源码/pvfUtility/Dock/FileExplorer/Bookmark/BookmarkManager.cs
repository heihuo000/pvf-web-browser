using System;
using System.Diagnostics;
using System.Windows.Forms;
using pvfUtility.Action;
using pvfUtility.Actions;

namespace pvfUtility.FileExplorer.Bookmark
{
    internal partial class BookmarkManager : DialogBase
    {
        public BookmarkManager()
        {
            InitializeComponent();
            btnClose.Click += buttonClose_Click;
        }

        private void BookmarkManager_Load(object sender, EventArgs e)
        {
            bookmarkTreeView.LoadBookmark();
            if (bookmarkTreeView.SelectedNode == null)
                bookmarkTreeView.SelectedNode = bookmarkTreeView.TopNode;
        }

        private void BookmarkManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            bookmarkTreeView.SaveBookmark();
        }

        private void buttonNewFolder_Click(object sender, EventArgs e)
        {
            if (bookmarkTreeView.SelectedNode == null)
                bookmarkTreeView.SelectedNode = bookmarkTreeView.TopNode;
            bookmarkTreeView.AddFolder(textBoxFolderName.Text);
        }

        private void buttonAddSeparator_Click(object sender, EventArgs e)
        {
            if (bookmarkTreeView.SelectedNode == null)
                bookmarkTreeView.SelectedNode = bookmarkTreeView.TopNode;
            bookmarkTreeView.AddSeparator();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (bookmarkTreeView.SelectedNode == null) return;
            bookmarkTreeView.Nodes.Remove(bookmarkTreeView.SelectedNode);
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (bookmarkTreeView.SelectedNode == null) return;
            bookmarkTreeView.MoveNodeUp();
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            if (bookmarkTreeView.SelectedNode == null) return;
            bookmarkTreeView.MoveNodeDown();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(string.Concat(AppCore.AppPath, "Bookmark.xml"));
        }

        private void buttonNewBookmark_Click(object sender, EventArgs e)
        {
            if (bookmarkTreeView.SelectedNode == null)
                bookmarkTreeView.SelectedNode = bookmarkTreeView.TopNode;
            new NewBookmarkDialog(null).ShowDialog(this);
        }
    }
}