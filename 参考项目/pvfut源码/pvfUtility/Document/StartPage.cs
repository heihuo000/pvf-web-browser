using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace pvfUtility.Shell.Document
{
    internal partial class StartPage : DockContent
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void DummyStartPage_Load(object sender, EventArgs e)
        {
            webBrowser.Navigate("https://wallace1300.github.io/pvfUtility/?version=" +
                                Assembly.GetExecutingAssembly().GetName().Version);
            webBrowser.NewWindow += webBrowser_NewWindow;
            webBrowser.Navigating += webBrowser_Navigating;
        }

        private void webBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            var browser = (WebBrowser) sender;
            Process.Start(browser.StatusText);
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            e.Cancel = true;
            Process.Start(e.Url.ToString());
        }
    }
}