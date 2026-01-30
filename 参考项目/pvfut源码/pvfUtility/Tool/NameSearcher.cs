using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using pvfUtility.Action;
using pvfUtility.Action.Search;
using pvfUtility.Document.TextEditor;
using pvfUtility.Helper.Native;
using pvfUtility.Service;

namespace pvfUtility.Shell.Tools
{
    public partial class NameSearcher : Form
    {
        public NameSearcher()
        {
            InitializeComponent();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                this.BeginDo(() =>
                {
                    buttonSearch.Enabled = false;
                    listViewCodeSearcher.Items.Clear();
                });
                var keyword = textBoxKeyword.Text;
                var keyword2 = ChineseHelper.ToTraditional(keyword);
                foreach (var item in PackService.CurrentPack.FileList)
                    if (item.Value.SearchName(false, keyword, keyword2, false, null))
                    {
                        var i = new ListViewItem(item.Key);
                        i.SubItems.Add(PackService.CurrentPack.GetName(PackService.CurrentPack.FileList[item.Key]));
                        this.BeginDo(() => listViewCodeSearcher.Items.Add(i));
                    }

                this.BeginDo(() => buttonSearch.Enabled = true);
            });
        }

        private void listViewCodeSearcher_DoubleClick(object sender, EventArgs e)
        {
            if (listViewCodeSearcher.SelectedItems.Count == 0) return;

            TextEditorPresenter.Instance.CreateInternal(listViewCodeSearcher.SelectedItems[0].Text);
        }
    }
}