using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using pvfUtility.Action;
using pvfUtility.Dock.FileExplorer;
using WeifenLuo.WinFormsUI.Docking;

namespace pvfUtility.Shell
{
    internal partial class OutputDockView : DockContent
    {
        private readonly StringBuilder _pvfUtilityText = new StringBuilder();
        private StringBuilder _scriptText = new StringBuilder();

        public OutputDockView()
        {
            InitializeComponent();
            if (MainPresenter.Instance.View.IsDark)
            {
                textBoxOutput.ForeColor = Color.FromArgb(241, 241, 241);
                textBoxOutput.BackColor = Color.FromArgb(37, 37, 38);
            }
        }

        private void DummyOutput_Load(object sender, EventArgs e)
        {
            textBoxOutput.Text = "pvfUtility:\r\n";
        }

        public void AppendTextUtility(string text)
        {
            _pvfUtilityText.AppendLine(text);
            this.Do(() =>
            {
                // textBoxOutput.SelectionStart = textBoxOutput.TextLength;
                // textBoxOutput.SelectionLength = 0;
                // textBoxOutput.SelectionColor = color;
                textBoxOutput.AppendText(text);
                textBoxOutput.AppendText(Environment.NewLine);
            });
        }

        private void textBoxOutput_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (e.LinkText.StartsWith("http"))
                Process.Start(e.LinkText);
            else if (e.LinkText.StartsWith("file"))
                FileExplorerPresenter.Instance.View.SetNodeTo(e.LinkText.Substring(7));
        }
    }
}