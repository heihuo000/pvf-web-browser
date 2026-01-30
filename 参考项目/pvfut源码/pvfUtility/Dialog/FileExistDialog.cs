using System;
using System.Drawing;
using System.Windows.Forms;
using pvfUtility.Model.PvfOperation;

namespace pvfUtility.Shell.Dialogs
{
    internal partial class FileExistDialog : Form
    {
        public FileExistDialog(string source, string destination)
        {
            InitializeComponent();
            fileOperation = FileOperation.OverWrite;
            labelDestination.Text = destination;
            labelSource.Text = source;
            mErrorIconLabel.Image =
                new Bitmap(SystemIcons.Warning.ToBitmap(), mErrorIconLabel.Width, mErrorIconLabel.Height);
        }

        public FileOperation fileOperation { get; private set; }
        public bool applyToAll { get; private set; }

        private void buttonOverwrite_Click(object sender, EventArgs e)
        {
            fileOperation = FileOperation.OverWrite;
            applyToAll = checkBoxApplyToAll.Checked;
            Close();
        }

        private void buttonRename_Click(object sender, EventArgs e)
        {
            fileOperation = FileOperation.Rename;
            applyToAll = checkBoxApplyToAll.Checked;
            Close();
        }

        private void buttonSkip_Click(object sender, EventArgs e)
        {
            fileOperation = FileOperation.Skip;
            applyToAll = checkBoxApplyToAll.Checked;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            fileOperation = FileOperation.Cancel;
            applyToAll = checkBoxApplyToAll.Checked;
            Close();
        }
    }
}