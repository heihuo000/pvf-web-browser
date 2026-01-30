#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.WindowsAPICodePack.Dialogs;
using pvfUtility.Action.Batch;
using pvfUtility.Action.Extract;
using pvfUtility.Action.Import;
using pvfUtility.Action.Search;
using pvfUtility.Dock.CollectionExplorer;
using pvfUtility.Dock.ErrorList;
using pvfUtility.Dock.FileExplorer;
using pvfUtility.Dock.Output;
using pvfUtility.Document.TextEditor;
using pvfUtility.FileExplorer.Bookmark;
using pvfUtility.Interface.View;
using pvfUtility.Model;
using pvfUtility.Model.PvfOperation;
using pvfUtility.Service;
using pvfUtility.Shell;
using pvfUtility.Shell.Dialogs;
using pvfUtility.Shell.Document;
using pvfUtility.Shell.Tools;
using pvfUtility.Tool.FindReplace;
using WeifenLuo.WinFormsUI.Docking;
using static pvfUtility.Action.AppCore;
using static pvfUtility.Service.PackService;

#endregion

namespace pvfUtility
{
    internal partial class MainForm : Form
    {
        private readonly MainPresenter _presenter;

        private readonly List<ToolStripMenuItem> _recList = new List<ToolStripMenuItem>();

        private readonly ToolStripSeparator _separator1 = new ToolStripSeparator();
        private readonly ToolStripSeparator _separator2 = new ToolStripSeparator();
        private readonly VS2015BlueTheme _themeBlue = new VS2015BlueTheme();
        private readonly VS2015DarkTheme _themeDark = new VS2015DarkTheme();

        private readonly VS2015LightTheme _themeLight = new VS2015LightTheme();
        private DummyPackComparer DummyPackComparer;

        public FindReplaceService FnR = new FindReplaceService();

        public MainForm(MainPresenter p)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            if (Config.Instance.UseDarkMode)
                Theme = _themeDark;
            else
                Theme = _themeLight;
            _presenter = p;

            dockpanel.Theme = Theme;
            vs.SetStyle(MainMenuStrip, VisualStudioToolStripExtender.VsVersion.Vs2015, Theme);
            vs.SetStyle(toolStripMain, VisualStudioToolStripExtender.VsVersion.Vs2015, Theme);
            vs.SetStyle(statusStripMain, VisualStudioToolStripExtender.VsVersion.Vs2015, Theme);
            vs.SetStyle(contextMenuStripWin, VisualStudioToolStripExtender.VsVersion.Vs2015, Theme);

            if (dockpanel.Theme.ColorPalette != null)
                statusStripMain.BackColor = dockpanel.Theme.ColorPalette.MainWindowStatusBarDefault.Background;

            MainProgress.ProgressChanged += (sender, i) => ProgressBarShow(i, 100);
        }

        internal ThemeBase Theme { get; }
        internal bool IsDark => Theme == _themeDark;

        public Progress<int> MainProgress { get; } = new Progress<int>();

        /// <summary>
        ///     取出当前Document,生成列表
        /// </summary>
        public List<IEditor> Documents => dockpanel.Documents.Where(x => x is IEditor).OfType<IEditor>().ToList();

        /// <summary>
        ///     当前Document,无则返回null
        /// </summary>
        public IEditor CurrentDockDocument => dockpanel.ActiveDocument is IEditor editor ? editor : null;

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = _presenter.ExitConfirm();

            using (var stream = new GZipStream(
                File.OpenWrite(MainPresenter.LocationSettingFile), CompressionMode.Compress))
            {
                dockpanel.SaveAsXml(stream, Encoding.UTF8);
            }

            Config.Instance.MainInstanceSize = WindowState == FormWindowState.Maximized ? RestoreBounds.Size : Size;
            if (WindowState != FormWindowState.Minimized)
                Config.Instance.Location = Location;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DummyPackComparer = new DummyPackComparer();

            if (File.Exists(MainPresenter.LocationSettingFile))
            {
                using (var stream = new GZipStream(
                    File.OpenRead(MainPresenter.LocationSettingFile), CompressionMode.Decompress))
                {
                    dockpanel.LoadFromXml(stream, _presenter.GetContentFromPersistString);
                }
            }
            else
            {
                FileExplorerPresenter.Instance.View.Show(dockpanel, DockState.DockLeft);
                CollectionExplorerPresenter.Instance.View.Show(dockpanel, DockState.DockRight);
                ErrorListPresenter.Instance.View.Show(dockpanel, DockState.DockBottom);
                OutputPresenter.Instance.View.Show(dockpanel, DockState.DockBottom);
            }

            if (File.Exists(AppPath + "script-error.pvf"))
            {
                var str = AppPath + "script-error.pvf" + DateTime.Now.ToString("yyMMddhhss");
                File.Move(AppPath + "script-error.pvf", str);
                _presenter.OpenPvf(str);
            }
            else
            {
                new StartPage().Show(dockpanel);
            }

            if (Config.Instance.MainInstanceSize.Height <= 100 ||
                Config.Instance.MainInstanceSize.Width <= 100) //最小化后Bug修复
                Size = new Size(1280, 800);
            else
                Size = Config.Instance.MainInstanceSize;
            if (Config.Instance.Location.X > 0 && Config.Instance.Location.Y > 0)
                Location = Config.Instance.Location;

            ComboEncoding.Text = Config.Instance.DefaultEncoding.ToString();
            windowsToolStripMenuItem.DropDown = contextMenuStripWin;

            UpdateEnable();
            //LoadPlugins();

            var version = Assembly.GetEntryAssembly()?.GetName().Version;
            Logger.Info(version?.ToString());
            dockpanel.ActiveDocumentChanged += FnR.EditorChanged;
            _presenter.Load();
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (var item in _recList)
                fileToolStripMenuItem.DropDownItems.Remove(item);
            _recList.Clear();
            if (Config.Instance.RecentFiles == null) return;
            var count = Config.Instance.RecentFiles.Count;
            var list = new List<string>();
            for (var i = count - 1; i >= 0; i--)
                list.Add(Config.Instance.RecentFiles[i]);
            foreach (var toolStripMenuItem in list.Select(item => new ToolStripMenuItem(item) {Tag = item}))
            {
                toolStripMenuItem.Click += LoadPvfFile;
                fileToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);
                _recList.Add(toolStripMenuItem);
            }
        }


        private void LoadPvfFile(object sender, EventArgs e)
        {
            var str = ((ToolStripMenuItem) sender).Tag.ToString();
            _presenter.OpenPvf(str);
            _presenter.AddRecentRecord(str);
        }

        public void ShowOnDockPanel(DockContent dockContent, DockState? dockState)
        {
            if (dockState.HasValue)
                this.Do(() => dockContent.Show(dockpanel, dockState.Value));
            else
                this.Do(() => dockContent.Show(dockpanel));
        }

        #region ConsoleIO

        public void SetStatusText(string text)
        {
            this.Do(() => StatusLabel.Text = text);
        }

        #endregion

        public void ProgressBarShow(bool show)
        {
            this.BeginDo(() => toolStripProgressBar.Visible = show);
            if (show)
                this.BeginDo(() => toolStripProgressBar.Value = 0);
        }

        public void ProgressBarShow(int nValue, int nMaxValue)
        {
            var num = nValue * 100 / nMaxValue;
            this.BeginDo(() => toolStripProgressBar.Value = num);
        }


        private void Dock_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
        }

        private void Dock_DragDrop(object sender, DragEventArgs e)
        {
            var data = (string[]) e.Data.GetData(DataFormats.FileDrop);
            var lower = data[0];
            if (lower.IndexOf(".pvf", StringComparison.Ordinal) < 0)
                return;
            if (!DialogService.AskYesNo("载入此PVF文件？", lower, Handle))
                return;
            _presenter.OpenPvf(lower);
            _presenter.AddRecentRecord(lower);
        }

        public void UpdateFileCount()
        {
            this.BeginDo(() => StatusLabelCount.Text =
                CurrentPack != null ? string.Concat("文件总数:", CurrentPack.FileList?.Count) : "");
        }

        public void DisableSave()
        {
            this.Do(() =>
            {
                savePackAsFastToolStripMenuItem.Enabled = toolStripButtonSavePack.Enabled =
                    toolStripButtonSavePackAs.Enabled = savePackFastToolStripMenuItem.Enabled = false;
            });
        }

        public void UpdateEnable()
        {
            var enable = CurrentPack != null;
            this.Do(() =>
            {
                toolStripButtonComparer.Enabled = toolStripButtonExtract.Enabled = toolStripButtonImport.Enabled =
                    toolStripButtonNewBatch.Enabled = savePackFullToolStripMenuItem.Enabled =
                        savePackAsFullToolStripMenuItem.Enabled = newBatchToolStripMenuItem.Enabled =
                            fileComparerToolStripMenuItem.Enabled = extractAllFilesTo7zToolStripMenuItem.Enabled =
                                pvfHeaderEditorToolStripMenuItem.Enabled =
                                    extractCollectionToolStripMenuItem.Enabled = saveAllToolStripMenuItem1.Enabled =
                                        codeSearcherToolStripMenuItem.Enabled =
                                            toolStripButtonClosePack.Enabled =
                                                toolStripButtonNewSearch.Enabled =
                                                    newSearchToolStripMenuItem.Enabled =
                                                        toolStripButtonSavePack.Enabled =
                                                            toolStripButtonSavePackAs.Enabled =
                                                                toolStripButtonRefreshFileTree.Enabled =
                                                                    savePackFastToolStripMenuItem.Enabled =
                                                                        savePackAsFastToolStripMenuItem.Enabled
                                                                            = closePackToolStripMenuItem.Enabled
                                                                                = refreshFileTreeToolStripMenuItem
                                                                                        .Enabled =
                                                                                    ComboEncoding.Enabled = enable;
            });
        }

        public void UpdateNavigationEnable()
        {
            this.Do(() =>
            {
                toolStripButtonGo.Enabled = NavigationServices.CanForward;
                toolStripButtonBack.Enabled = NavigationServices.CanBack;
            });
        }

        private void startPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new StartPage().Show(dockpanel);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().Show(dockpanel);
        }

        private void dock_ActiveContentChanged(object sender, EventArgs e)
        {
            var dock = dockpanel.ActiveDocument;
            if (!(dock is IEditor))
            {
            }
            else
            {
                if (NavigationServices.CurrentHistory != (IEditor) dock)
                {
                    NavigationServices.Add((IEditor) dock);
                    UpdateNavigationEnable();
                }
            }
        }

        private void clearAllRecentFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.ClearRecentRecord();
        }

        private void dockpanel_ContentRemoved(object sender, DockContentEventArgs e)
        {
            if (e.Content is IEditor editor)
                NavigationServices.Remove(editor);
            UpdateNavigationEnable();
        }

        private void nameSearcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new NameSearcher().ShowDialog(this);
        }


        private void toolStripButtonStartClient_Click(object sender, EventArgs e)
        {
            _presenter.Debug();
        }

        private void goToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GoToFileDialog().ShowDialog(this);
        }

        #region Pvf操作

        #endregion

        #region pvf操作菜单

        private void OpenPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.OpenPvf(null);
        }

        private void SavePackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.SavePvf(false, true);
        }

        private void SavePackAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.SavePvf(true, true);
        }

        private void SavePackFullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.SavePvf(false, false);
        }

        private void SavePackAsFullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.SavePvf(true, false);
        }

        private void RefreshTree_Click(object sender, EventArgs e)
        {
            _presenter.RefreshTree();
        }

        private void ClosePvf_Click(object sender, EventArgs e)
        {
            _presenter.ClosePvf();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ChangeEncoding_Click(object sender, EventArgs e)
        {
            _presenter.ChangeEncoding(sender == ComboEncoding
                ? ComboEncoding.SelectedItem.ToString()
                : ((ToolStripMenuItem) sender).Text);
        }

        #endregion

        #region 封包菜单

        private void NewExtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Extractor(CurrentPack).ShowDialog();
        }

        private void NewImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Importer(CurrentPack).ShowDialog(null, null, null);
        }

        private void ExtractAllFilesTo7zToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Extractor(CurrentPack, Extractor.GetSetting(),
                CurrentPack.FileList.Select(x => x.Key).ToArray()).ExtractFileTo7ZFast();
        }

        private void OptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new OptionForm().Show(this);
        }

        private void pvfHeaderEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new PvfHeaderEditorDialog().Show(this);
        }

        private void FileComparerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DummyPackComparer.Show(dockpanel);
        }

        #endregion

        #region 搜索菜单

        private void NewCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var x = new InputBox("新建结果集", "输入结果集的名称（默认为未命名）", "名称：");
            if (x.ShowDialog(this) != DialogResult.OK) return;
            CollectionExplorerPresenter.Instance.NewFileCollection(x.InputtedText == "" ? "未命名" : x.InputtedText);
        }

        private void ExportTxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CollectionExplorerPresenter.Instance.CurFileCollection.ExportToTxt();
        }

        private void ImportTxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CollectionExplorerPresenter.Instance.ImportAsNewResultData();
        }

        private void ExtractCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CollectionExplorerPresenter.Instance.ExtractAllFiles();
        }

        private void DeleteCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CollectionExplorerPresenter.Instance.RemoveCurrentFileCollection();
        }

        private void NewSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SearchPresenter(CurrentPack).ShowDialog();
        }

        private void NewBatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new BatchPresenter().ShowDialog();
        }

        private void codeSearcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CodeSearcher().Show(this);
        }

        private void SearchToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            currentSearchResultToolStripMenuItem.Text = CollectionExplorerPresenter.Instance.CurFileCollection.Name;
        }

        #endregion

        #region ScriptCommand

        #endregion

        #region 前进后退

        private void toolStripButtonBack_Click(object sender, EventArgs e)
        {
            NavigationServices.Back();
            var editor = NavigationServices.CurrentHistory;
            ((DockContent) editor)?.Show(MainPresenter.Instance.View.dockpanel);
            UpdateNavigationEnable();
        }


        private void toolStripButtonGo_Click(object sender, EventArgs e)
        {
            NavigationServices.Forward();
            var editor = NavigationServices.CurrentHistory;
            ((DockContent) editor)?.Show(MainPresenter.Instance.View.dockpanel);
            UpdateNavigationEnable();
        }

        #endregion


        #region `Edit` Menu

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FnR.ShowFind();
        }

        private void 替换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FnR.ShowReplace();
        }

        #endregion

        #region 窗口菜单

        private readonly List<ToolStripMenuItem> _winList = new List<ToolStripMenuItem>();

        private void contextMenuStripWin_Opening(object sender, CancelEventArgs e)
        {
            foreach (var item in _winList)
                contextMenuStripWin.Items.Remove(item);
            _winList.Clear();

            foreach (var dockContent in MainPresenter.Instance.View.dockpanel.Documents)
            {
                var item = new ToolStripMenuItem
                {
                    Text = dockContent.DockHandler.TabText,
                    Tag = dockContent
                };
                item.Click += OpenWin;
                contextMenuStripWin.Items.Add(item);
                _winList.Add(item);
            }


            var en = dockpanel.ActiveDocument != null;
            saveFileToolStripMenuItem.Enabled = closeFileToolStripMenuItem.Enabled = saveAllToolStripMenuItem1.Enabled
                = closeAllToolStripMenuItem.Enabled = closeRToolStripMenuItem.Enabled = closeLToolStripMenuItem.Enabled
                    = closeOtherToolStripMenuItem.Enabled = en;
            if (!en)
                return;

            closeFileToolStripMenuItem.Text = "关闭 " + dockpanel.ActiveDocument.DockHandler.TabText;
            var enable = dockpanel.ActiveDocument is IEditor;
            saveFileToolStripMenuItem.Visible = enable;
            saveFileToolStripMenuItem.Text = "保存 " + dockpanel.ActiveDocument.DockHandler.TabText;
        }

        private void CloseDocument(IDockContent content)
        {
            this.Do(() =>
            {
                content.DockHandler.DockPanel = null;
                content.DockHandler.Close();
                content.DockHandler.Dispose();
            });
        }

        public void CloseAllDocuments(bool skipSaveConfirm = false)
        {
            if (!skipSaveConfirm)
            {
                if (!AskToSaveDocument(Documents))
                    return;
            }
            else
            {
                foreach (var dockContent in Documents) //修复了单个窗口弹出保存的bug
                    dockContent.IsEdited = false;
            }

            foreach (var dockContent in Documents)
                CloseDocument((DockContent) dockContent);
        }

        private void closeLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var leftlst = Documents.GetRange(0, Documents.IndexOf(CurrentDockDocument)).ToList();
            if (!AskToSaveDocument(leftlst))
                return;
            foreach (var item in leftlst)
                CloseDocument((DockContent) item);
        }

        private void closeRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = Documents.IndexOf(CurrentDockDocument) + 1;
            var rightlst = Documents.GetRange(index, Documents.Count - index);
            if (!AskToSaveDocument(rightlst))
                return;
            foreach (var item in rightlst)
                CloseDocument((DockContent) item);
        }

        private void OpenWin(object sender, EventArgs e)
        {
            ((IDockContent) ((ToolStripMenuItem) sender).Tag).DockHandler.Activate();
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllDocuments();
        }

        private void closeOtherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lst = Documents;
            lst.Remove(CurrentDockDocument);
            if (!AskToSaveDocument(lst))
                return;
            foreach (var item in lst)
                CloseDocument((DockContent) item);
        }


        private void closeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseDocument(dockpanel.ActiveDocument);
        }

        #endregion

        #region 窗口菜单

        /// <summary>
        ///     显示文档
        /// </summary>
        /// <param name="document"></param>
        public void ShowDocument(IEditor document)
        {
            this.BeginDo(() => ((DockContent) document).Show(dockpanel));
            NavigationServices.Add(document);
        }

        /// <summary>
        ///     询问是否保存文件
        /// </summary>
        /// <param name="documents"></param>
        /// <returns>是否取消</returns>
        public bool AskToSaveDocument(IEnumerable<IEditor> documents, bool needToClose = true)
        {
            var lst = documents.Where(item => item is IEditor content && content.IsEdited)
                .ToDictionary(item => item.FileName, item => item);
            if (lst.Count <= 0) return true;
            var sb = new StringBuilder();
            foreach (var item in lst) sb.AppendLine(item.Key);
            var dialogResult = DialogService.Message("存在未保存的文件，是否保存对以下各项的更改？", sb.ToString(),
                TaskDialogStandardIcon.Information,
                TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No | TaskDialogStandardButtons.Cancel,
                Handle);
            switch (dialogResult)
            {
                case TaskDialogResult.Yes:
                    foreach (var item in lst.Values)
                        item.SaveFile();
                    break;
                case TaskDialogResult.No:
                    if (needToClose)
                        foreach (var item in lst.Values)
                            item.IsEdited = false;
                    break;
                case TaskDialogResult.Cancel:
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     询问是否保存文件
        /// </summary>
        public bool AskToSaveDocument(bool needToClose)
        {
            return AskToSaveDocument(Documents, needToClose);
        }


        private void SaveAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (var dockContent in dockpanel.Documents.Where(x => x is IEditor))
                ((IEditor) dockContent).SaveFile();
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dockpanel.ActiveDocument is IEditor editor)
                editor.SaveFile();
        }

        #endregion

        #region 书签功能

        private void BookmarkToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            bookmarkToolStripMenuItem.DropDownItems.Clear();
            bookmarkToolStripMenuItem.DropDownItems.Add(bookmarkManagerToolStripMenuItem);
            bookmarkToolStripMenuItem.DropDownItems.Add(_separator1);
            var xmlDocument = new XmlDocument();
            if (!File.Exists(AppPath + "BookMark.xml"))
            {
                var xmldoc = new XmlDocument();
                var xmldecl = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmldoc.AppendChild(xmldecl);
                var xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                xmldoc.Save(string.Concat(AppPath, "Bookmark.xml"));
            }

            xmlDocument.Load(string.Concat(AppPath, "BookMark.xml"));
            if (xmlDocument.DocumentElement != null)
                ReadBookmarkFromXml(xmlDocument.DocumentElement.SelectSingleNode("/root")?.ChildNodes,
                    bookmarkToolStripMenuItem);
        }

        private void ReadBookmarkFromXml(IEnumerable nodeList, ToolStripDropDownItem menuItem)
        {
            foreach (XmlNode node in nodeList)
            {
                var xe = (XmlElement) node;
                if (xe.GetAttribute("Title") == "-")
                {
                    menuItem.DropDownItems.Add(new ToolStripSeparator());
                    continue;
                }

                var toolStripMenuItem = new ToolStripMenuItem
                {
                    Text = xe.GetAttribute("Title"),
                    Tag = xe.GetAttribute("FileName")
                };

                menuItem.DropDownItems.Add(toolStripMenuItem);
                if (node.HasChildNodes)
                    ReadBookmarkFromXml(node.ChildNodes, toolStripMenuItem);
                else
                    toolStripMenuItem.Click += LoadFile;
            }
        }

        private void LoadFile(object sender, EventArgs e)
        {
            TextEditorPresenter.Instance.CreateInternal((string) ((ToolStripMenuItem) sender).Tag, true);
        }

        private void BookmarkManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new BookmarkManager().Show();
        }

        #endregion

        #region 视图菜单

        private void fileExplorerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FileExplorerPresenter.Instance.View.Show(dockpanel);
        }

        private void collectionExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CollectionExplorerPresenter.Instance.View.Show(dockpanel);
        }

        private void outputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutputPresenter.Instance.View.Show(dockpanel);
        }

        private void errorListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ErrorListPresenter.Instance.View.Show(dockpanel);
        }

        #endregion
    }
}