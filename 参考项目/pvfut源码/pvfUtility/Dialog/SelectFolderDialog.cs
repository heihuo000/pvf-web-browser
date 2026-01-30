using System;
using Aga.Controls.Tree;
using pvfUtility.Action.Search;
using pvfUtility.Actions;

namespace pvfUtility.Shell.Dialogs
{
    internal partial class SelectFolderDialog : DialogBase
    {
        public SelectFolderDialog()
        {
            InitializeComponent();
            treeViewPath.Model = SearchPresenter.Pathmodel;
            SelectedPath = "";
            btnOk.Click += buttonOK_Click;
        }

        public string SelectedPath { get; private set; }

        private string GetSelectedNode()
        {
            var selectedNode = treeViewPath.SelectedNode;
            return selectedNode?.ToString() ?? "";
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SelectedPath = GetSelectedNode();
        }

        private void treeViewPath_NodeMouseClick(object sender, TreeNodeAdvMouseEventArgs e)
        {
            labelCurrentSelected.Text = GetSelectedNode();
        }
    }
}