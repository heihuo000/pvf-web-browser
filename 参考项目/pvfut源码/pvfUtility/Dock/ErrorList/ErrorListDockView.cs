using System;
using System.Drawing;
using System.Windows.Forms;
using pvfUtility.Document.TextEditor;
using pvfUtility.Model;
using WeifenLuo.WinFormsUI.Docking;

namespace pvfUtility.Shell
{
    internal partial class ErrorListDockView : DockContent
    {
        public ErrorListDockView()
        {
            InitializeComponent();
            MainPresenter.Instance.View.vs.SetStyle(toolStrip1, VisualStudioToolStripExtender.VsVersion.Vs2015,
                MainPresenter.Instance.View.Theme);
            if (MainPresenter.Instance.View.IsDark)
            {
                listViewError.ForeColor = Color.FromArgb(241, 241, 241);
                listViewError.BackColor = Color.FromArgb(37, 37, 38);
            }
        }

        public void AddError(ErrorListItem error)
        {
            var item = new ListViewItem
            {
                Text = error.Action,
                Tag = error
            };
            item.SubItems.Add(error.FileName);
            item.SubItems.Add(error.Description);
            item.SubItems.Add(error.Line.ToString());
            listViewError.Items.Add(item);
        }

        internal void RemoveError(ErrorListItem item)
        {
            var count = listViewError.Items.Count;
            for (var i = count - 1; i >= 0; i--)
                if (listViewError.Items[i].Tag.Equals(item))
                    listViewError.Items.RemoveAt(i);
        }

        private void toolStripButtonClearAll_Click(object sender, EventArgs e)
        {
            listViewError.Items.Clear();
        }

        /// <summary>
        ///     处理错误列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewError_DoubleClick(object sender, EventArgs e)
        {
            var item = (ErrorListItem) listViewError.SelectedItems[0].Tag;
            if (item.IsExternal)
            {
                var (success, editor) =
                    TextEditorPresenter.Instance.CreateExternal(item.FullText, item.FileName, item.Guid);
                editor.GoTo(item.Line);
            }
            else
            {
                var (success, isNew, editor) = TextEditorPresenter.Instance.CreateInternal(item.FileName);
                if (success)
                {
                    if (isNew) editor.SetText(item.FullText); //关闭后恢复错误时候的显示
                    editor.GoTo(item.Line);
                }
            }
        }
    }
}