using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Be.Windows.Forms;
using pvfUtility.Document.TextEditor;
using pvfUtility.Interface.View;
using pvfUtility.Model.PvfOperation;
using pvfUtility.Properties;
using pvfUtility.Service;
using WeifenLuo.WinFormsUI.Docking;
#if DEBUG

#endif

namespace pvfUtility.Document.HexEditor
{
    internal partial class HexEditorFrom : DockContent, IEditor
    {
        private MemoryStream _mStream;

        public HexEditorFrom(string path)
        {
            InitializeComponent();
            MainPresenter.Instance.View.vs.SetStyle(toolStrip, VisualStudioToolStripExtender.VsVersion.Vs2015,
                MainPresenter.Instance.View.Theme);
            MainPresenter.Instance.View.vs.SetStyle(contextMenuStripEditor,
                VisualStudioToolStripExtender.VsVersion.Vs2015, MainPresenter.Instance.View.Theme);

            EncodingType = PackService.CurrentPack.OverAllEncodingType;
            FileName = path;
            Text = PackService.CurrentPack.FileList[path].ShortName;
            toolStripTextBoxPath.Text = path;
        }

        public string FileName { get; set; }
        public bool IsExternalFile { get; set; }
        public Guid ExternalContentId { get; set; }
        public EncodingType EncodingType { get; set; }
        public bool IsEdited { get; set; }
        public string Content { get; set; }

        public bool GoTo(int line)
        {
            throw new NotImplementedException();
        }

        public void SaveFile()
        {
            var obj = PackService.CurrentPack.GetFile(FileName);
            if (obj != null)
            {
                MainPresenter.Instance.View.SetStatusText(string.Format(lang.SaveSuccess, FileName));
                try
                {
                    var byteProvider = (DynamicFileByteProvider) hexBox.ByteProvider;
                    byteProvider?.ApplyChanges();
                }
                catch (Exception ex1)
                {
                    Logger.Error("保存发生错误：" + ex1.Message);
                    return;
                }

                PackService.CurrentPack.FileList[FileName].WriteFileData(_mStream.ToArray());
                MainPresenter.Instance.View.SetStatusText(string.Format(lang.SaveSuccess, FileName));
            }
            else
            {
                DialogService.Error("找不到文件", $"编辑器未能保存文件：{FileName}\r\n 检查它是否存在。", Handle);
            }
        }

        private Task<bool> Init()
        {
            if (PackService.CurrentPack.FileList[FileName].DataLen <= 0)
            {
                if (InvokeRequired)
                    Invoke(new MethodInvoker(() => label1.Text = "空文件"));
                else
                    label1.Text = "空文件";
                return Task.FromResult(false);
            }

            _mStream = new MemoryStream(PackService.CurrentPack.FileList[FileName].Data);
            _mStream.SetLength(PackService.CurrentPack.FileList[FileName].DataLen);
            var byteProvider = new DynamicFileByteProvider(_mStream);
            var act = new System.Action(() =>
            {
                hexBox.ByteProvider = byteProvider;
                label1.Visible = false;
            });
            if (InvokeRequired)
                Invoke(new MethodInvoker(act));
            else
                act();

            return Task.FromResult(true);
        }

        private async void HexEditorFrom_Load(object sender, EventArgs e)
        {
            label1.Text = $"正在打开{FileName}";
            label1.Location = new Point(Width / 2 - label1.Width / 2, Height / 2 - label1.Height / 2);
            await Task.Run(Init);
        }

        private void contextMenuStripEditor_Opening(object sender, CancelEventArgs e)
        {
            PasteToolStripMenuItem.Enabled = hexBox.CanPasteHex();
        }

        private void HexEditorFrom_SizeChanged(object sender, EventArgs e)
        {
            if (Width - 100 > 0)
                toolStripTextBoxPath.Width = Width - 100;
        }

        private void toolStripTextBoxPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                TextEditorPresenter.Instance.CreateInternal(toolStripTextBoxPath.Text, true);
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        #region Menus Cmds

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!hexBox.Focused) return;
            hexBox.CopyHex();
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!hexBox.Focused) return;
            hexBox.PasteHex();
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!hexBox.Focused) return;
            hexBox.SelectAll();
        }

        private void ShowInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO
            //FileExplorerManager.SetNodeTo(FileName);
        }

        public void SetText(string text)
        {
            throw new NotImplementedException();
        }

        #endregion Menus Cmds
    }
}