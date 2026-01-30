using System;

namespace pvfUtility.Actions
{
    internal partial class DetailInfoDialog : DialogBase
    {
        public DetailInfoDialog(string text)
        {
            InitializeComponent();
            textBoxData.Text = text;
            btnClose.Click += buttonCancel_Click;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}