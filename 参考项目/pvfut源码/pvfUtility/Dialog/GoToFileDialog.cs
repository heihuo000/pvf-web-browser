using System;
using pvfUtility.Actions;
using pvfUtility.Document.TextEditor;
using pvfUtility.Model;

namespace pvfUtility.Shell.Dialogs
{
    internal partial class GoToFileDialog : DialogBase
    {
        public GoToFileDialog()
        {
            InitializeComponent();
            btnOk.Click += buttonOK_Click;
            checkBox1.Checked = !Config.Instance.DisableNodeJump;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            TextEditorPresenter.Instance.CreateInternal(textBox1.Text, checkBox1.Checked);
        }

        private void GoToFileDialog_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}