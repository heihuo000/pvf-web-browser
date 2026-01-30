using System;
using System.Linq;
using System.Windows.Forms;
using pvfUtility.Dock.CollectionExplorer;
using pvfUtility.Document.TextEditor;
using pvfUtility.Service;

namespace pvfUtility.Shell.Tools
{
    /// <summary>
    ///     代码搜索器
    /// </summary>
    public partial class CodeSearcher : Form
    {
        public CodeSearcher()
        {
            InitializeComponent();
        }

        private void CodeSearcher_Load(object sender, EventArgs e)
        {
            foreach (var item in PackService.CurrentPack.ListFileTable.FileCodeDictionary)
                comboBoxLstFiles.Items.Add(item.Key);
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            foreach (var item in listViewCodeSearcher.Items)
                CollectionExplorerPresenter.Instance.CurFileCollection.AddFile(((ListViewItem) item).Text);
            CollectionExplorerPresenter.Instance.View.ShowSearchResult();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            listViewCodeSearcher.Items.Clear();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            listViewCodeSearcher.Items.Clear();
            var basePath = comboBoxLstFiles.Text;
            if (!PackService.CurrentPack.ListFileTable.FileCodeDictionary.ContainsKey(basePath))
            {
                DialogService.Error("没有找到lst,或该lst存在错误", "", Handle);
                return;
            }

            var dic = PackService.CurrentPack.ListFileTable.FileCodeDictionary[basePath];
            var list = textBoxCode.Text.Split(new[] {"\r\n", "\t"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in list)
            {
                int.TryParse(item, out var code);
                if (code < 0)
                    continue;
                var pair = dic.FirstOrDefault(x => x.Value == code);
                if (pair.Key == null)
                    continue;
                var listViewItem = new ListViewItem(pair.Key);
                listViewItem.SubItems.Add(PackService.CurrentPack.GetName(PackService.CurrentPack.FileList[pair.Key]));
                listViewItem.SubItems.Add(code.ToString());
                listViewCodeSearcher.Items.Add(listViewItem);
            }
        }

        private void listViewCodeSearcher_DoubleClick(object sender, EventArgs e)
        {
            if (listViewCodeSearcher.SelectedItems.Count == 0) return;

            TextEditorPresenter.Instance.CreateInternal(listViewCodeSearcher.SelectedItems[0].Text);
        }
    }
}