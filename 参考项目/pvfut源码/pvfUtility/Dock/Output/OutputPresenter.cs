using System;
using System.Globalization;
using pvfUtility.Shell;
using WeifenLuo.WinFormsUI.Docking;

namespace pvfUtility.Dock.Output
{
    internal class OutputPresenter
    {
        public OutputPresenter()
        {
            View = new OutputDockView();
        }

        public static OutputPresenter Instance { get; } = new OutputPresenter();

        public OutputDockView View { get; }

        public string Name => "输出";

        public DockContent DockView => View;

        public bool SaveLocation => true;

        public DockState DefaultDockState => DockState.DockBottom;

        // public void Error(string info)
        // {
        //     AddConsoleInfo_(info, Color.DarkRed);
        // }
        //
        // public void Warn(string info)
        // {
        //     AddConsoleInfo_(info, Color.DarkOrange);
        // }
        //
        // public void Success(string info)
        // {
        //     AddConsoleInfo_(info, Color.DarkGreen);
        // }
        //
        // public void Info(string info)
        // {
        //     AddConsoleInfo_(info, Color.Black);
        // }
        //
        // public void Debug(string info)
        // {
        //     AddConsoleInfo_(info, Color.DarkBlue);
        // }

        public void Append(string info)
        {
            //MainForm.View.ShowOnDockPanel(View, null);
            var text = string.Concat("[", DateTime.Now.ToString(CultureInfo.InvariantCulture), "]", info);
            View.AppendTextUtility(text);
        }

        public void OnPackOpened()
        {
        }

        public void OnPackClosed()
        {
        }
    }
}