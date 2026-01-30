using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using pvfUtility.Action;
using pvfUtility.Action.Extract;
using pvfUtility.Action.Search;
using pvfUtility.Helper;
using pvfUtility.Model.PvfOperation;
using pvfUtility.Model.TreeModel;
using pvfUtility.Service;
using WeifenLuo.WinFormsUI.Docking;

namespace pvfUtility.Shell.Document
{
    /// <summary>
    ///     差异比较器
    /// </summary>
    internal partial class DummyPackComparer : DockContent
    {
        internal DummyPackComparer()
        {
            InitializeComponent();
            MainPresenter.Instance.View.vs.SetStyle(toolStrip1, VisualStudioToolStripExtender.VsVersion.Vs2015,
                MainPresenter.Instance.View.Theme);
        }

        /// <summary>
        ///     外部打开的
        /// </summary>
        private PvfPack OutPvf { get; set; }

        /// <summary>
        ///     存在Main但不存在于Out
        /// </summary>
        private List<string> MainDiff { get; set; }

        /// <summary>
        ///     存在Out但不存在于Main
        /// </summary>
        private List<string> OutDiff { get; set; }

        /// <summary>
        ///     共同存在但是内容不同
        /// </summary>
        private List<string> ContentDiff { get; set; }

        /// <summary>
        ///     外部封包的路径
        /// </summary>
        private string OutPvfPath { get; set; }

        /// <summary>
        ///     打开外部pvf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonOpenPVF_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog
            {
                Title = "打开封包",
                ValidateNames = true,
                DefaultExt = ".pvf",
                CheckFileExists = true,
                Filter = "Pvf Pack|*.pvf"
            })
            {
                var result = openFileDialog.ShowDialog();
                if (result != DialogResult.OK) return;

                OutPvf = new PvfPack(PackService.CurrentPack.OverAllEncodingType);
                OutPvfPath = openFileDialog.FileName;
                toolStripButtonOpenPVF.Enabled = false;

                MainPresenter.Instance.DoAction("打开外部封包", async progress =>
                {
                    if (!await Task.Run(() => OutPvf.OpenPvfPack(OutPvfPath, progress)))
                        DialogService.Error("无法打开pvf文件", "", Handle);
                    toolStripButtonOpenPVF.Enabled = toolStripButtonStartCompare.Enabled = true;
                });
            }
        }

        private void buttonUnzipMain_Click(object sender, EventArgs e)
        {
            new Extractor(PackService.CurrentPack).ShowDialog(MainDiff.ToArray());
        }

        private void buttonUnzipOut_Click(object sender, EventArgs e)
        {
            new Extractor(OutPvf).ShowDialog(OutDiff.ToArray());
        }

        private void buttonUnzipMainDiff_Click(object sender, EventArgs e)
        {
            new Extractor(PackService.CurrentPack).ShowDialog(ContentDiff.ToArray());
        }

        private void buttonUnzipOutDiff_Click(object sender, EventArgs e)
        {
            new Extractor(OutPvf).ShowDialog(ContentDiff.ToArray());
        }

        private void toolStripButtonStartCompare_Click(object sender, EventArgs e)
        {
            MainPresenter.Instance.DoAction("正在比较...",
                async progress => { await Task.Run(() => Comparing(progress)); });
        }

        /// <summary>
        ///     核心比较代码
        /// </summary>
        /// <param name="progress"></param>
        private void Comparing(IProgress<int> progress)
        {
            //开始比较
            var fileListOut = OutPvf.FileList.Keys.ToList();
            var fileListMain = PackService.CurrentPack.FileList.Keys.ToList();

            var intersectFiles = fileListMain.Intersect(fileListOut).ToList(); //取得两个pvf共同拥有的文件
            OutDiff = fileListOut.Except(intersectFiles).ToList();
            MainDiff = fileListMain.Except(intersectFiles).ToList();

            var workCount = 0;
            var count = intersectFiles.Count;
            var resolve = DataHelper.GetResolve(count);
            var contentDiffBag = new ConcurrentBag<string>();
            Parallel.ForEach(intersectFiles, item =>
            {
                if (!PackService.CurrentPack.FileList[item].CompareFile(OutPvf.FileList[item], OutPvf))
                    contentDiffBag.Add(item);
                workCount++;
                if (workCount % resolve == 0)
                    progress.Report(AppCore.GetProgressNum(workCount, count));
            });

            ContentDiff = contentDiffBag.ToList();
            this.BeginDo(() =>
            {
                labelMain.Text = $"不存在于：{OutPvfPath}";
                labelOut.Text = $"不存在于：{PackService.CurrentPack.PvfPackFilePath}";
                labeldiff.Text = $"两个封包之间存在{ContentDiff}个文件差异";
                buttonUnzipMain.Enabled = true;
                buttonUnzipMainDiff.Enabled = true;
                buttonUnzipOut.Enabled = true;
                buttonUnzipOutDiff.Enabled = true;
                OutListView.Model = new FileTreeViewModel(OutDiff);
                FileDifferent.Model = new FileTreeViewModel(ContentDiff);
                MainListView.Model = new FileTreeViewModel(MainDiff);
            });
        }

        private void toolStripButtonReset_Click(object sender, EventArgs e)
        {
            OutPvf = null;
            MainDiff = null;
            OutDiff = null;
            ContentDiff = null;
            FileDifferent.Model = null;
            OutListView.Model = null;
            MainListView.Model = null;
            buttonUnzipMain.Enabled = false;
            buttonUnzipMainDiff.Enabled = false;
            buttonUnzipOut.Enabled = false;
            buttonUnzipOutDiff.Enabled = false;
            toolStripButtonStartCompare.Enabled = false;
        }
    }
}