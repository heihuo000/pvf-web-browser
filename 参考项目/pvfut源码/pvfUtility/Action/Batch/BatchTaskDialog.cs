using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using pvfUtility.Action.Batch;

namespace pvfUtility.Actions.Batch
{
    internal partial class BatchTaskDialog : DialogBase
    {
        private readonly BatchCmd _cmd;

        public BatchTaskDialog(BatchCmd cmd)
        {
            InitializeComponent();
            btnOk.Click += buttonOK_Click;
            btnCancel.Click += buttonCancel_Click;
            AcceptButton = null;
            CancelButton = null;

            _cmd = cmd;
            labelText.Text = $"将对文件集：{_cmd.FileCollection.Name}进行操作。";
            ModeChanged(this, new EventArgs());
        }

        private void linkLabelLearningRegex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(
                "https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/regular-expression-language-quick-reference");
        }

        private void ModeChanged(object sender, EventArgs e)
        {
            if (radioButtonModeAdd.Checked)
                panelModeAdd.BringToFront();
            else if (radioButtonModeReplace.Checked)
                panelModeReplace.BringToFront();
            else if (radioButtonModeDelete.Checked)
                panelModeDelete.BringToFront();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (radioButtonModeAdd.Checked)
                _cmd.BatchMode = BatchMode.Add;
            else if (radioButtonModeReplace.Checked)
                _cmd.BatchMode = BatchMode.Replace;
            else if (radioButtonModeDelete.Checked)
                _cmd.BatchMode = BatchMode.Delete;
            _cmd.TextData = _cmd.BatchMode == BatchMode.Add ? textBoxAddContent.Text : textBoxFindContent.Text;
            _cmd.TextDataReplace = textBoxReplaceContent.Text;
            _cmd.TextDelSection = textBoxDeleteSectionName.Text;
            _cmd.TextDelContent = textBoxDeleteContent.Text;
            _cmd.IsDelContent = !radioButtonDeleteSection.Checked;


            Task.Run(() => BatchPresenter.StartBatch(_cmd));

            Close();
            Dispose();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void BatchTaskDialog_Load(object sender, EventArgs e)
        {
        }

        private void BatchTaskDialog_Load_1(object sender, EventArgs e)
        {
        }
    }
}