using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using pvfUtility.Action;
using pvfUtility.Action.Import;
using pvfUtility.Actions;
using pvfUtility.Model;
using pvfUtility.Model.TreeModel;

namespace pvfUtility.Shell.Dialogs
{
    internal partial class ImportDialog : DialogBase
    {
        private readonly Importer _presenter;

        public ImportDialog(Importer presenter, string[] targets, string rootPath, string targetPath)
        {
            InitializeComponent();
            _presenter = presenter;
            btnOk.Click += ButtonOK_Click;
            btnCancel.Click += ButtonCancel_Click;
            textBoxTargetPath.Text = targetPath;
            textBoxSourcePath.Text = rootPath;
            if (targets != null)
                listBoxImport.Items.AddRange(targets);

            Task.Run(RefreshTree);

            //默认配置
            checkBoxCompileFile.Checked = Config.Instance.ImportCompileScript;
            checkBoxCompileBinaryAni.Checked = Config.Instance.ImportCompileBinaryAni;
            checkBoxConvertChinese.Checked = Config.Instance.ImportConvertTraditionalChinese;
        }

        private string[] GetTargets()
        {
            return (from object x in listBoxImport.Items select x.ToString()).ToArray();
        }

        /// <summary>
        ///     开始导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            _presenter.CurSetting.CompileScript = checkBoxCompileFile.Checked;
            _presenter.CurSetting.CompileBinaryAni = checkBoxCompileBinaryAni.Checked;
            _presenter.CurSetting.ConvertToTraditionalChinese = checkBoxConvertChinese.Checked;
            _presenter.CurSetting.SourcePath = textBoxSourcePath.Text;
            _presenter.CurSetting.TargetPaths = textBoxTargetPath.Text;
            _presenter.CurSetting.Targets = GetTargets();

            Task.Run(() => _presenter.ImportFile());

            Close();
            Dispose();
        }

        private void ButtonSelectPath_Click(object sender, EventArgs e)
        {
            var openFileDialog = new FolderBrowserDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                textBoxSourcePath.Text = openFileDialog.SelectedPath;
            Task.Run(RefreshTree);
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDel_Click(object sender, EventArgs e)
        {
            listBoxImport.Items.RemoveAt(listBoxImport.SelectedIndex);
            Task.Run(RefreshTree);
        }

        /// <summary>
        ///     添加文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog
            {
                Title = "导入文件",
                ValidateNames = true,
                CheckPathExists = true,
                Multiselect = true,
                CheckFileExists = true,
                Filter = "All Files|*.*"
            })
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                var lst = openFileDialog.FileNames.ToArray();
                listBoxImport.Items.AddRange(lst);
                if (textBoxSourcePath.Text == "")
                {
                    var file = new FileInfo(lst[0]);
                    textBoxSourcePath.Text = file.DirectoryName;
                }

                Task.Run(RefreshTree);
            }
        }

        /// <summary>
        ///     保存设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSaveSetting_Click(object sender, EventArgs e)
        {
            Config.Instance.ImportCompileScript = checkBoxCompileFile.Checked;
            Config.Instance.ImportCompileBinaryAni = checkBoxCompileBinaryAni.Checked;
            Config.Instance.ImportConvertTraditionalChinese = checkBoxConvertChinese.Checked;
        }

        #region 拖曳处理

        private void ListBoxImport_DragDrop(object sender, DragEventArgs e)
        {
            var data = (string[]) e.Data.GetData(DataFormats.FileDrop);
            if (listBoxImport.Items.Count == 0)
                textBoxSourcePath.Text = data[0].Remove(data[0].LastIndexOf('\\'));
            listBoxImport.Items.AddRange(data);
            Task.Run(RefreshTree);
        }

        private void ListBoxImport_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
        }

        #endregion


        // private void CheckBoxImport7Z_CheckedChanged(object sender, EventArgs e)
        // {
        //     label1.Visible = !checkBoxImport7Z.Checked;
        //     textBoxSourcePath.Visible = !checkBoxImport7Z.Checked;
        //     buttonSelectPath.Visible = !checkBoxImport7Z.Checked;
        //     //Tips.Visible = !checkBoxImport7Z.Checked;
        //     listBoxImport.Visible = !checkBoxImport7Z.Checked;
        //     buttonDel.Visible = !checkBoxImport7Z.Checked;
        //     buttonAdd.Visible = !checkBoxImport7Z.Checked;
        //     buttonSelect7Z.Enabled = checkBoxImport7Z.Checked;
        //     if (label7ZFile.Text != "...")
        //         Task.Run(RefreshTree);
        // }
        // private void ButtonSelect7Z_Click(object sender, EventArgs e)
        // {
        //     var openFileDialog = new OpenFileDialog
        //     {
        //         Title = "选择7z文件",
        //         ValidateNames = true,
        //         CheckPathExists = true,
        //         CheckFileExists = true,
        //         Filter = "压缩文件|*.7z;*.rar;*.zip;*.tar;*.bz2;*.gz"
        //     };
        //     if (openFileDialog.ShowDialog() != DialogResult.OK) return;
        //     label7ZFile.Text = openFileDialog.FileName;
        //     Task.Run(RefreshTree);
        // }

        #region 刷新目录树

        /// <summary>
        ///     更新导入后的结构预览树
        /// </summary>
        private void RefreshTree()
        {
            this.Do(() => labelInfo.Text = "正在生成目录树...");
            _presenter.CurSetting.Targets = GetTargets();
            _presenter.CurSetting.SourcePath = textBoxSourcePath.Text;
            _presenter.CurSetting.TargetPaths = textBoxTargetPath.Text;
            var importObjects = _presenter.GetFullPaths();
            var model = new FileTreeViewModel(importObjects.Select(x => x.Target).ToList());
            this.Do(() => treeViewAdvImport.Model = model);
            this.Do(() => labelInfo.Text = "就绪");
        }

        private void TextBoxRootPath_Leave(object sender, EventArgs e)
        {
            Task.Run(RefreshTree);
        }

        private void TextBoxTargetPaths_Leave(object sender, EventArgs e)
        {
            Task.Run(RefreshTree);
        }

        private void ImportDialog_Load(object sender, EventArgs e)
        {
            Task.Run(RefreshTree);
        }

        private void ButtonRefreshTree_Click(object sender, EventArgs e)
        {
            Task.Run(RefreshTree);
        }

        /// <summary>
        ///     选择导入后的目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSelectPvfPath_Click(object sender, EventArgs e)
        {
            var dialog = new SelectFolderDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
                textBoxTargetPath.Text = dialog.SelectedPath;
        }

        #endregion
    }
}