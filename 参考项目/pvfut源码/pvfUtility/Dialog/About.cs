using System;
using System.Reflection;
using System.Windows.Forms;

namespace pvfUtility.Shell
{
    internal partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            label1.Text = "pvfUtility:" + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}