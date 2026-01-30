using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using pvfUtility.Action.Extract;
using pvfUtility.Actions;
using pvfUtility.Model;
using pvfUtility.Service;

namespace pvfUtility.Shell.Dialogs
{
    internal partial class ExtractDialog : DialogBase
    {
        private readonly Extractor _presenter;

        public ExtractDialog(IEnumerable<string> list, Extractor presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            //TODO 
            //FormLocationHelper.Apply(this, "ExtractDialog", true);

            btnOk.Click += buttonOK_Click;
            btnCancel.Click += buttonCancel_Click;

            checkBoxDecompileScript.Checked = Config.Instance.ExtractDecompileScript;
            checkBoxDecompileBinaryAni.Checked = Config.Instance.ExtractDecompileBinaryAni;
            checkBoxOpenFolder.Checked = Config.Instance.ExtractOpenFolder;
            checkBoxConvertChinese.Checked = Config.Instance.ExtractConvertSimplifiedChinese;

            textBoxTargetPath.Text = PackService.CurrentPackPathFolder;
            if (list == null) return;
            foreach (var x in list)
                listBoxExtract.Items.Add(new ExtractItem(x));
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            listBoxExtract.Items.Add(new ExtractItem(textBoxPath.Text, checkBoxUseLike.Checked));
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            _presenter.CurSetting.Items = new List<(string Path, bool UseLike)>();
            foreach (ExtractItem item in listBoxExtract.Items)
                _presenter.CurSetting.Items.Add((item.Path, item.UseLike));

            _presenter.CurSetting.DecompileScript = checkBoxDecompileScript.Checked;
            _presenter.CurSetting.DecompileBinaryAni = checkBoxDecompileBinaryAni.Checked;
            _presenter.CurSetting.ConvertConvertSimplifiedChinese = checkBoxConvertChinese.Checked;
            _presenter.CurSetting.OpenFolder = checkBoxOpenFolder.Checked;
            _presenter.CurSetting.TargetPath = textBoxTargetPath.Text;

            Close();
            if (checkBoxExtractAs7z.Checked)
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "保存文件",
                    ValidateNames = true,
                    CheckPathExists = true,
                    DefaultExt = ".7z",
                    Filter = "7z|*.7z"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    Task.Run(() => _presenter.ExtractFileTo7Z(saveFileDialog.FileName));
            }
            else
            {
                Task.Run(() => _presenter.ExtractFile());
            }

            Dispose();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void buttonSelectPath_Click(object sender, EventArgs e)
        {
            var openFileDialog = new FolderBrowserDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                textBoxTargetPath.Text = openFileDialog.SelectedPath;
        }

        private void checkBoxExtractAs7z_CheckedChanged(object sender, EventArgs e)
        {
            labelTargetPath.Visible = !checkBoxExtractAs7z.Checked;
            textBoxTargetPath.Visible = !checkBoxExtractAs7z.Checked;
            buttonSelectPath.Visible = !checkBoxExtractAs7z.Checked;
        }

        private void buttonChangeToFolder_Click(object sender, EventArgs e)
        {
            var items = listBoxExtract.CheckedItems;
            foreach (var item in items)
            {
                var x = PackService.CurrentPack?.GetFile(item.ToString());
                if (x != null)
                    ((ExtractItem) item).Path = x.PathName;
            }

            listBoxExtract.Refresh();
        }

        private void buttonSaveSetting_Click(object sender, EventArgs e)
        {
            Config.Instance.ExtractDecompileScript = checkBoxDecompileScript.Checked;
            Config.Instance.ExtractDecompileBinaryAni = checkBoxDecompileBinaryAni.Checked;
            Config.Instance.ExtractOpenFolder = checkBoxOpenFolder.Checked;
            Config.Instance.ExtractConvertSimplifiedChinese = checkBoxConvertChinese.Checked;
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            var count = listBoxExtract.CheckedItems.Count;
            for (var i = count - 1; i >= 0; i--)
                listBoxExtract.Items.Remove(listBoxExtract.CheckedItems[i]);
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            var count = listBoxExtract.Items.Count;
            for (var i = 0; i < count; i++)
                listBoxExtract.SetItemChecked(i, true);
        }

        private void buttonSelectAll2_Click(object sender, EventArgs e)
        {
            var count = listBoxExtract.Items.Count;
            for (var i = 0; i < count; i++)
                listBoxExtract.SetItemChecked(i, !listBoxExtract.GetItemChecked(i));
        }

        private void buttonImportFromTxt_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog
            {
                Title = "从txt导入",
                ValidateNames = true,
                CheckPathExists = true,
                DefaultExt = ".txt",
                CheckFileExists = true,
                Filter = "Text File|*.txt"
            })
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                var streamReader = File.OpenText(openFileDialog.FileName);
                streamReader.BaseStream.Seek(0L, SeekOrigin.Begin);
                var str = streamReader.ReadLine();
                while (str != null)
                {
                    listBoxExtract.Items.Add(new ExtractItem(str));
                    str = streamReader.ReadLine();
                }
            }
        }

        /// <summary>
        ///     extract item in listBoxExtract
        /// </summary>
        public class ExtractItem
        {
            public readonly bool UseLike;
            public string Path;

            public ExtractItem(string path, bool useLike = false)
            {
                Path = path;
                UseLike = useLike;
            }

            public override string ToString()
            {
                return Path;
            }
        }
    }
}