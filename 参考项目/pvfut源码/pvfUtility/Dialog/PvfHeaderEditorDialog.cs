using System;
using System.Text;
using pvfUtility.Actions;
using pvfUtility.Service;

namespace pvfUtility.Model.PvfOperation
{
    internal partial class PvfHeaderEditorDialog : DialogBase
    {
        public PvfHeaderEditorDialog()
        {
            InitializeComponent();
            btnOk.Click += buttonOK_Click;
            btnCancel.Click += buttonCancel_Click;
        }

        private void PvfHeaderEditor_Load(object sender, EventArgs e)
        {
            textBoxGuid.Text = Encoding.UTF8.GetString(PackService.CurrentPack.Guid);
            textBoxFileVersion.Text = PackService.CurrentPack.FileVersion.ToString();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                PackService.CurrentPack.Guid = Encoding.UTF8.GetBytes(textBoxGuid.Text);
                PackService.CurrentPack.FileVersion = int.Parse(textBoxFileVersion.Text);
            }
            catch
            {
                // ignored
            }

            Close();
            Dispose();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}