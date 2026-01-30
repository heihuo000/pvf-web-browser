using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using pvfUtility.Action;
using pvfUtility.Dock.FileExplorer;
using pvfUtility.Document.TextEditor;
using pvfUtility.Helper;
using pvfUtility.Interface.View;
using pvfUtility.Model;
using pvfUtility.Model.PvfOperation;
using pvfUtility.Service;
using ScintillaNET;
using WeifenLuo.WinFormsUI.Docking;

namespace pvfUtility.Shell.Document.TextEditor
{
    internal partial class TextEditorView : DockContent, IEditor
    {
        private PvfFile _file;
        private bool _isLst;

        /// <summary>
        ///     外部文件创建
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="fileName"></param>
        /// <param name="externalPath"></param>
        public TextEditorView(string fileText, string fileName, Guid externalId) : this()
        {
            IsExternalFile = true;
            ExternalContentId = externalId;
            FileName = fileName;
            Content = fileText;
            Text = (fileName.Contains("/") ? fileName.Substring(fileName.LastIndexOf('/') + 1) : fileName) + "(外部的)";
            if (PackService.CurrentPack.FileList.ContainsKey(fileName))
                _file = PackService.CurrentPack.FileList[fileName];
        }

        /// <summary>
        ///     内部文件创建
        /// </summary>
        /// <param name="path"></param>
        public TextEditorView(string fileName) : this()
        {
            FileName = fileName;
            _file = PackService.CurrentPack.FileList[fileName];
            Text = _file.ShortName;
            toolStripTextBoxPath.Text = fileName;
        }

        private TextEditorView()
        {
            InitializeComponent();
            MainPresenter.Instance.View.vs.SetStyle(toolStrip, VisualStudioToolStripExtender.VsVersion.Vs2015,
                MainPresenter.Instance.View.Theme);
            MainPresenter.Instance.View.vs.SetStyle(contextMenuStripEditor,
                VisualStudioToolStripExtender.VsVersion.Vs2015,
                MainPresenter.Instance.View.Theme);
            MainPresenter.Instance.View.vs.SetStyle(contextMenuStripSave,
                VisualStudioToolStripExtender.VsVersion.Vs2015,
                MainPresenter.Instance.View.Theme);
            if (MainPresenter.Instance.View.IsDark)
            {
                toolStripTextBoxPath.ForeColor = Color.FromArgb(241, 241, 241);
                toolStripTextBoxPath.BackColor = Color.FromArgb(63, 63, 70);
            }

            TabPageContextMenuStrip = MainPresenter.Instance.View.contextMenuStripWin;
            saveAdvToolStripMenuItem.DropDown = toolStripButtonSave.DropDown;
            EncodingType = PackService.CurrentPack.OverAllEncodingType;
        }

        public string FileName { get; set; }
        public bool IsExternalFile { get; set; }
        public EncodingType EncodingType { get; set; }

        public void SaveFile()
        {
            SaveFile(null);
        }

        /// <summary>
        ///     载入文本
        /// </summary>
        /// <returns></returns>
        private Task<bool> InitText()
        {
            if (!IsExternalFile && _file.DataLen > 0)
                Content = TextEditorPresenter.Instance.GetFileText(_file, EncodingType); //获取文本
            UpdateUI();
            return Task.FromResult(true);
        }

        private void UpdateUI()
        {
            //处理UI更新
            this.Do(() =>
            {
                scintilla.Visible = true;
                scintilla.WrapMode = WrapMode.None;
                scintilla.Text = Content;
                scintilla.WrapMode = WrapMode.Char; //提升载入速度

                if (_file != null)
                {
                    if (!_isLst && (_file.IsBinaryAniFile || _file.IsScriptFile))
                        ParseSectionFold(); //只有ani和脚本文件才会解析结构
                    else
                        toolStripComboBoxSection.Visible = false;
                    if (_file.IsScriptFile || _file.IsBinaryAniFile) //编码是否显示
                        toolStripSplitButtonEncoding.Visible = false;
                    scintilla.Delete += scintilla_Delete;
                    scintilla.Insert += scintilla_Insert;
                }

                showBeginningTabToolStripMenuItem.Checked = Config.Instance.EditorShowBeginningTab;
                label1.Visible = false;
            });
        }

        /// <summary>
        ///     保存文件
        /// </summary>
        /// <param name="doSave"></param>
        private void SaveFile(Func<PvfFile, string, EncodingType, bool> doSave)
        {
            var str = scintilla.Text;
            if (doSave == null)
                doSave = TextEditorPresenter.Instance.SaveFileText;

            if (IsExternalFile) //一般情况下是外部文件,添加文件
            {
                var newFile = new PvfFile(FileName);
                PackService.CurrentPack.FileList.Add(newFile.FileName, newFile);
                TreeFileHelper.AddString2TreeList(FileName, FileExplorerPresenter.Instance.FileTree);
                FileExplorerPresenter.Instance.View.UpdateNode(PathsHelper.PathFix(newFile.PathName));
                Logger.Info($"编辑器 :: 新建了文件`{FileName}`");
                _file = newFile;
            }

            if (doSave.Invoke(_file, str, EncodingType))
            {
                IsEdited = false; //恢复为正常状况
                Text = _file.ShortName;
                MainPresenter.Instance.SetStatusText($"保存文件{FileName}成功");
                if (IsExternalFile)
                {
                    IsExternalFile = false;
                    SetStyle();
                    Task.Run(InitText); //刷新文本
                }
            }
            else
            {
                MainPresenter.Instance.SetStatusText($"文件`{FileName}`存在错误,保存失败,请查看错误列表");
                Logger.Error($"编辑器 :: 文件 file://{FileName} 存在错误,保存失败,请查看错误列表");
            }
        }

        private async void TextEditorFrom_Load(object sender, EventArgs e)
        {
            label1.Text = $"正在打开{FileName}";
            label1.Location = new Point(Width / 2 - label1.Width / 2, Height / 2 - label1.Height / 2);
            SetStyle();
            await Task.Run(InitText);
        }


        /// <summary>
        ///     处理lst当中使用链接打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scintilla_HotspotClick(object sender, HotspotClickEventArgs e)
        {
            try
            {
                var ss = scintilla.Lines[scintilla.LineFromPosition(e.Position)].Text.TrimEnd('\r', '\n').Trim();
                var text = ss.Substring(ss.IndexOf('`') + 1, ss.Length - ss.IndexOf('`') - 2).ToLower();
                var fileName = _file.PathName + "/" + text;
                if (e.Modifiers != Keys.Control)
                {
                    scintilla.CallTipShow(e.Position,
                        $"{PackService.CurrentPack.GetName(PackService.CurrentPack.FileList[fileName])}\r\n{fileName}\r\n按住Ctrl后点击即可打开文件。");
                    return;
                }

                TextEditorPresenter.Instance.CreateInternal(fileName, true);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     自定义的语法高亮解析器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void scintilla_StyleNeeded(object sender, StyleNeededEventArgs e)
        {
            var startPos = scintilla.GetEndStyled();
            var endPos = e.Position;
            if (_isLst)
                await PvfScriptLexerFast.Style(scintilla, startPos, endPos);
            else
                await PvfScriptLexer.Style(scintilla, startPos, endPos);
        }

        /// <summary>
        ///     热键处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scintilla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab) //按下tab增加一个\t
            {
                scintilla.InsertText(scintilla.SelectionEnd, "\t");
                scintilla.SelectionEnd++;
                scintilla.SelectionStart++;
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                SaveFile();
                e.SuppressKeyPress = true;
            }
        }

        private void scintilla_ZoomChanged(object sender, EventArgs e)
        {
            Config.Instance.EditorZoom = scintilla.Zoom;
        }

        private void showBeginningTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showBeginningTabToolStripMenuItem.Checked = !showBeginningTabToolStripMenuItem.Checked;
            Config.Instance.EditorShowBeginningTab = showBeginningTabToolStripMenuItem.Checked;
            Logger.Info("修改成功,再次打开生效");
        }

        #region 编辑器样式

        /// <summary>
        ///     样式处理
        /// </summary>
        private void SetStyle()
        {
            scintilla.WhitespaceSize = 4;
            if (Config.Instance.EditorZoom.HasValue) scintilla.Zoom = Config.Instance.EditorZoom.Value;

            scintilla.ViewWhitespace = Config.Instance.EditorShowBeginningTab
                ? WhitespaceMode.VisibleAlways
                : WhitespaceMode.VisibleAfterIndent;
            scintilla.SetWhitespaceForeColor(Config.Instance.EditorShowSpaces, Color.DimGray);
            scintilla.StyleResetDefault();

            scintilla.Styles[Style.Default].SizeF = Config.Instance.EditorFont.Size;
            scintilla.Styles[Style.Default].Font = Config.Instance.EditorFont.Name;
            scintilla.StyleClearAll(); //不能缺失 否则无法使用Tips
            if (MainPresenter.Instance.View.IsDark)
            {
                scintilla.SetSelectionBackColor(true, Color.FromArgb(38, 70, 120));
                scintilla.CaretLineBackColor = Color.FromArgb(70, 70, 70);
                scintilla.Styles[Style.Default].ForeColor = Color.FromArgb(220, 220, 220);
                scintilla.Styles[Style.Default].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[0].ForeColor = Color.FromArgb(220, 220, 220);
                scintilla.Styles[0].BackColor = Color.FromArgb(30, 30, 30);
            }

            if (FileName.IndexOf(".lst", StringComparison.OrdinalIgnoreCase) > 0)
            {
                _isLst = true;
                scintilla.Styles[PvfScriptLexer.StyleString].Hotspot = true;
                scintilla.Styles[PvfScriptLexer.StyleString].Underline = true;
            }
            else if (FileName.IndexOf(".nut", StringComparison.OrdinalIgnoreCase) > 0)
            {
                SetStyleNut();
            }

            if (_file != null && (_file.IsScriptFile || _file.IsBinaryAniFile)) SetStyleScript();


            scintilla.Technology = Config.Instance.EditorEnableDirectDraw ? Technology.DirectWrite : Technology.Default;

            // Configure a margin to display folding symbols
            scintilla.Margins[0].Type = MarginType.Number;
            scintilla.Margins[0].Width = 50;
            if (MainPresenter.Instance.View.IsDark)
            {
                scintilla.Styles[Style.LineNumber].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[Style.LineNumber].ForeColor = Color.FromArgb(220, 220, 220);
            }

            scintilla.Margins[1].Type = MarginType.Symbol;
            scintilla.Margins[1].Mask = Marker.MaskFolders;
            scintilla.Margins[1].Sensitive = true;
            scintilla.Margins[1].Width = 20;
            if (MainPresenter.Instance.View.IsDark)
            {
                scintilla.SetFoldMarginColor(true, Color.FromArgb(51, 51, 51));
                scintilla.SetFoldMarginHighlightColor(true, Color.FromArgb(51, 51, 51));
            }

            // Set colors for all folding markers
            for (var i = 25; i <= 31; i++)
                if (MainPresenter.Instance.View.IsDark)
                {
                    scintilla.Markers[i].SetForeColor(Color.FromArgb(165, 165, 165));
                    scintilla.Markers[i].SetBackColor(Color.FromArgb(51, 51, 51));
                }
                else
                {
                    scintilla.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                    scintilla.Markers[i].SetBackColor(SystemColors.ControlDark);
                }

            // Configure folding markers with respective symbols
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            scintilla.AutomaticFold = AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change;
            scintilla.SetFoldFlags(FoldFlags.LineAfterContracted);
        }

        /// <summary>
        ///     脚本样式渲染
        /// </summary>
        private void SetStyleScript()
        {
            scintilla.Lexer = Lexer.Container;
            if (MainPresenter.Instance.View.IsDark)
            {
                scintilla.Styles[PvfScriptLexer.StyleDefault].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[PvfScriptLexer.StyleIdentifier].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[PvfScriptLexer.StyleNumber].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[PvfScriptLexer.StyleString].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[PvfScriptLexer.StyleSection].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[PvfScriptLexer.StyleStringLink].BackColor = Color.FromArgb(30, 30, 30);

                scintilla.Styles[PvfScriptLexer.StyleDefault].ForeColor = Color.FromArgb(220, 220, 220);
                scintilla.Styles[PvfScriptLexer.StyleIdentifier].ForeColor = Color.FromArgb(216, 160, 223);
                scintilla.Styles[PvfScriptLexer.StyleNumber].ForeColor = Color.FromArgb(156, 220, 254);
                scintilla.Styles[PvfScriptLexer.StyleString].ForeColor = Color.FromArgb(78, 201, 176);
                scintilla.Styles[PvfScriptLexer.StyleSection].ForeColor = Color.FromArgb(214, 157, 133);
                scintilla.Styles[PvfScriptLexer.StyleStringLink].ForeColor = Color.FromArgb(220, 220, 170);
            }
            else
            {
                scintilla.Styles[PvfScriptLexer.StyleIdentifier].ForeColor = Color.Gray;
                scintilla.Styles[PvfScriptLexer.StyleNumber].ForeColor = Color.Blue;
                scintilla.Styles[PvfScriptLexer.StyleString].ForeColor = Color.DarkGreen;
                scintilla.Styles[PvfScriptLexer.StyleSection].ForeColor = Color.Red;
                scintilla.Styles[PvfScriptLexer.StyleStringLink].ForeColor = Color.Maroon;
            }
        }

        /// <summary>
        ///     处理Nut样式
        /// </summary>
        private void SetStyleNut()
        {
            EncodingType = EncodingType.KR; //nut默认以韩文显示
            scintilla.Lexer = Lexer.Lua;
            scintilla.SetKeywords(1, "obj");
            scintilla.SetKeywords(2, "return function");
            scintilla.SetKeywords(3, "if else");

            if (MainPresenter.Instance.View.IsDark)
            {
                scintilla.Styles[Style.Lua.Default].ForeColor = Color.FromArgb(220, 220, 220);
                scintilla.Styles[Style.Lua.Identifier].ForeColor = Color.FromArgb(220, 220, 220);
                scintilla.Styles[Style.Lua.CommentLine].ForeColor = Color.FromArgb(87, 166, 74);
                scintilla.Styles[Style.Lua.Word3].ForeColor = Color.FromArgb(156, 220, 254);
                scintilla.Styles[Style.Lua.Word4].ForeColor = Color.FromArgb(216, 160, 223);
                scintilla.Styles[Style.Lua.Word2].ForeColor = Color.FromArgb(214, 157, 133);
                scintilla.Styles[Style.Lua.String].ForeColor = Color.FromArgb(78, 201, 176);
                scintilla.Styles[Style.Lua.Character].ForeColor = Color.FromArgb(78, 201, 176);
                scintilla.Styles[Style.Lua.Operator].ForeColor = Color.FromArgb(220, 220, 170);

                scintilla.Styles[Style.Lua.Default].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[Style.Lua.CommentLine].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[Style.Lua.Identifier].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[Style.Lua.Word3].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[Style.Lua.Word4].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[Style.Lua.Word2].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[Style.Lua.String].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[Style.Lua.Character].BackColor = Color.FromArgb(30, 30, 30);
                scintilla.Styles[Style.Lua.Operator].BackColor = Color.FromArgb(30, 30, 30);
            }
            else
            {
                scintilla.Styles[Style.Lua.CommentLine].ForeColor = Color.Maroon;
                scintilla.Styles[Style.Lua.Word3].ForeColor = Color.Blue;
                scintilla.Styles[Style.Lua.Word4].ForeColor = Color.Red;
                scintilla.Styles[Style.Lua.Word2].ForeColor = Color.FromArgb(214, 157, 133);
                scintilla.Styles[Style.Lua.String].ForeColor = Color.DarkGreen;
                scintilla.Styles[Style.Lua.Character].ForeColor = Color.DarkGreen;
                scintilla.Styles[Style.Lua.Operator].ForeColor = Color.Maroon;
            }
        }

        #endregion

        #region 节点导航

        private class ComboBoxItem
        {
            public readonly int Line;
            private readonly string Text;

            public ComboBoxItem(string text, int line)
            {
                Text = text;
                Line = line;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        /// <summary>
        ///     解析节点导航
        /// </summary>
        private void ParseSectionFold()
        {
            var count = scintilla.Lines.Count;
            scintilla.Lines[0].FoldLevel = 0;
            for (var i = 1; i < count; i++)
            {
                var text = scintilla.Lines[i].Text;
                var layer = GetLayer(text);
                scintilla.Lines[i].FoldLevel = layer;
                if (layer > scintilla.Lines[i - 1].FoldLevel)
                {
                    scintilla.Lines[i - 1].FoldLevelFlags = FoldLevelFlags.Header;
                    if (layer == 1)
                        toolStripComboBoxSection.Items.Add(new ComboBoxItem(scintilla.Lines[i - 1].Text, i - 1));
                }
            }

            if (toolStripComboBoxSection.Items.Count > 0)
                toolStripComboBoxSection.SelectedIndex = 0;
        }

        /// <summary>
        ///     获取层
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private int GetLayer(string text)
        {
            if (text.Length <= 1 || text[0] != '\t') return 0;
            if (text.Length <= 2 || text[1] != '\t') return 1;
            if (text.Length <= 3 || text[2] != '\t') return 2;
            if (text.Length <= 4 || text[3] != '\t') return 3;
            if (text.Length <= 5 || text[4] != '\t') return 4;
            if (text.Length <= 6 || text[5] != '\t') return 5;
            if (text.Length <= 7 || text[6] != '\t') return 6;
            return 0;
        }

        private void toolStripComboBoxSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = (ComboBoxItem) toolStripComboBoxSection.SelectedItem;
            scintilla.Lines[item.Line].Goto();
            scintilla.Focus();
        }

        #endregion

        #region 路径快速转到

        private void TextEditorFrom_SizeChanged(object sender, EventArgs e)
        {
            if (Width - 400 > 0) toolStripTextBoxPath.Width = Width - 400;
        }

        /// <summary>
        ///     路径快速转到
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBoxPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                TextEditorPresenter.Instance.CreateInternal(toolStripTextBoxPath.Text, true);
        }

        #endregion

        #region 更改编码

        private async void tWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EncodingType = EncodingType.TW;
            await InitText();
        }

        private async void cNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EncodingType = EncodingType.CN;
            await InitText();
        }

        private async void kRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EncodingType = EncodingType.KR;
            await InitText();
        }

        private async void jPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EncodingType = EncodingType.JP;
            await InitText();
        }

        #endregion ChangeEncoding

        #region Menus Cmds

        private void contextMenuStripEditor_Opening(object sender, CancelEventArgs e)
        {
            RedoToolStripMenuItem.Enabled = scintilla.CanRedo;
            UndoToolStripMenuItem.Enabled = scintilla.CanUndo;
            PasteToolStripMenuItem.Enabled = scintilla.CanPaste;
            ProcessTurning();
        }

        private void ProcessTurning()
        {
            var str = scintilla.SelectedText;
            if (str.Length < 0 || str.IndexOf('.') < 0 || str.IndexOf('\t') > 0 || str.IndexOf('\n') > 0)
            {
                TurningToolStripMenuItem.Text = "选择文件名以使用“转到”功能";
                return;
            }

            if (str[0] == '`') str = str.Remove(0, 1);
            if (str[str.Length - 1] == '`') str = str.Remove(str.Length - 1, 1);
            var shortname = toolStripTextBoxPath.Text;
            var num1 = shortname.LastIndexOf('/');
            if (num1 > 0) shortname = shortname.Remove(num1, shortname.Length - num1);
            while (str.IndexOf("../", StringComparison.Ordinal) >= 0)
            {
                str = str.Remove(0, 3);
                var x = shortname.LastIndexOf('/');
                if (x > 0) shortname = shortname.Remove(x, shortname.Length - x);
            }

            TurningToolStripMenuItem.Text = shortname + '/' + str;
        }

        private void TurningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextEditorPresenter.Instance.CreateInternal(TurningToolStripMenuItem.Text, true);
        }

        private void SaveTextToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "保存当前文本",
                ValidateNames = true,
                CheckPathExists = true,
                FileName = PackService.CurrentPack.FileList[FileName].ShortName
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                using (var streamWriter = File.CreateText(saveFileDialog.FileName))
                {
                    streamWriter.Write(scintilla.Text);
                }

            saveFileDialog.Dispose();
        }

        private async void refreshTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla.Visible = false;
            await InitText();
        }

        private void autoFoldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in scintilla.Lines)
                if (item.FoldLevel == 1)
                    item.ToggleFold();
        }

        private void ShowInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileExplorerPresenter.Instance.View.SetNodeTo(FileName);
        }

        #endregion Menus Cmds

        #region 保存菜单

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        /// <summary>
        ///     以GBK保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsTextGBKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EncodingType = EncodingType.CN;
            SaveFile(TextEditorPresenter.Instance.SaveFileAsTextFile);
        }

        /// <summary>
        ///     以Big5保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsTextBig5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EncodingType = EncodingType.TW;
            SaveFile(TextEditorPresenter.Instance.SaveFileAsTextFile);
        }

        /// <summary>
        ///     UTF8保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsUtf8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EncodingType = EncodingType.UTF8;
            SaveFile(TextEditorPresenter.Instance.SaveFileAsTextFile);
        }

        /// <summary>
        ///     以脚本文件保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(TextEditorPresenter.Instance.SaveFileAsScript);
        }

        /// <summary>
        ///     以ani保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsBinaryAniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(TextEditorPresenter.Instance.SaveFileAsBinaryAni);
        }

        #endregion

        #region 查找

        private void FindTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPresenter.Instance.View.FnR.ShowFind();
        }

        private void ReplaceTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPresenter.Instance.View.FnR.ShowReplace();
        }

        private void GoToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Tool.FindReplace.GoTo(scintilla).ShowGoToDialog();
        }

        private void toolStripSplitButtonFind_ButtonClick(object sender, EventArgs e)
        {
            MainPresenter.Instance.View.FnR.ShowFind();
        }

        #endregion

        #region 已修改检测

        public bool IsEdited { get; set; }
        public string Content { get; set; }
        public Guid ExternalContentId { get; set; }

        public bool GoTo(int line)
        {
            if (line < scintilla.Lines.Count)
            {
                scintilla.Lines[line - 1].Goto();
                scintilla.Focus();
                return true;
            }

            return false;
        }

        private void scintilla_Delete(object sender, ModificationEventArgs e)
        {
            IsEdited = true;
            Text = PackService.CurrentPack.FileList[FileName].ShortName + "*";
        }

        private void scintilla_Insert(object sender, ModificationEventArgs e)
        {
            IsEdited = true;
            Text = PackService.CurrentPack.FileList[FileName].ShortName + "*";
        }

        private void TextEditorFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsEdited) return;

            var c = DialogService.Message("文件已修改，是否保存？", FileName,
                TaskDialogStandardIcon.Information,
                TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No | TaskDialogStandardButtons.Cancel,
                Handle);
            switch (c)
            {
                case TaskDialogResult.Yes:
                    SaveFile();
                    break;
                case TaskDialogResult.No:
                    break;
                case TaskDialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }

        #endregion

        #region 编辑器基本操作

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla.SelectAll();
        }

        public void Undo()
        {
            if (scintilla.Focused)
                scintilla.Undo();
        }

        public void Redo()
        {
            if (scintilla.Focused)
                scintilla.Redo();
        }

        public void Cut()
        {
            if (scintilla.Focused)
                scintilla.Cut();
        }

        public void Copy()
        {
            if (scintilla.Focused)
                scintilla.Copy();
        }

        public void Paste()
        {
            if (scintilla.Focused)
                scintilla.Paste();
        }

        public void SetText(string text)
        {
            Content = text;
            UpdateUI();
        }

        #endregion
    }
}