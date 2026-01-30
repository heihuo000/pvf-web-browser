using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using pvfUtility.Helper;
using pvfUtility.Model;
using pvfUtility.Tool.FindReplace;
using ScintillaNET;
using CharacterRange = pvfUtility.Tool.FindReplace.CharacterRange;

namespace pvfUtility.Document.TextEditor.FindReplace
{
    internal partial class FindReplaceDialog : Form
    {
        #region Constructors

        public FindReplaceDialog()
        {
            InitializeComponent();

            AutoPosition = true;
            MruFind = Config.Instance.FindHistory;
            MruReplace = Config.Instance.ReplaceHistory;
            lblStatus.ForeColor = Color.Red;
        }

        #endregion Constructors

        private void linkLabelLearningRegex1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(
                "https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/regular-expression-language-quick-reference");
        }


        public void ConvertMode(bool isFind)
        {
            panelReplace.Visible = txtReplace.Visible = cmdRecentReplace.Visible =
                cmdExtCharAndRegExReplace.Visible = lblReplace.Visible = !isFind;
            panelFind.Visible = isFind;
            AcceptButton = isFind ? btnFindNextF : btnReplaceNext;
            toolStripButtonFind.Checked = isFind;
            toolStripButtonReplace.Checked = !isFind;
        }

        private void toolStripButtonFind_Click(object sender, EventArgs e)
        {
            ConvertMode(true);
        }

        private void toolStripButtonReplace_Click(object sender, EventArgs e)
        {
            ConvertMode(false);
        }

        public void SetEnable(bool enabled)
        {
            btnFindAll.Enabled = btnFindNextF.Enabled = btnFindNextR.Enabled = btnFindPreviousF.Enabled =
                btnFindPreviousR.Enabled = btnReplaceAll.Enabled = btnReplaceNext.Enabled = btnReplacePrevious.Enabled =
                    btnClear.Enabled = enabled;
        }

        #region Properties

        /// <summary>
        ///     Gets or sets whether the dialog should automatically move away from the current
        ///     selection to prevent obscuring it.
        /// </summary>
        /// <returns>true to automatically move away from the current selection; otherwise, false.</returns>
        public bool AutoPosition { get; set; }

        public List<string> MruFind { get; set; }

        public int MruMaxCount { get; set; } = 10;

        public List<string> MruReplace { get; set; }

        public Scintilla Scintilla { get; set; }

        public FindReplaceService FindReplace { get; set; }

        #endregion Properties

        #region 窗口事件

        private void FindReplaceDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;
            Hide();
        }

        private void FindReplaceDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Hide();
        }

        #endregion Form Event Handlers

        #region Event Handlers

        private void btnClear_Click(object sender, EventArgs e)
        {
            Scintilla.MarkerDeleteAll(FindReplace.Marker.Index);
            FindReplace.ClearAllHighlights();
        }

        private void btnFindAll_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFind.Text)) return;

            AddFindMru();

            lblStatus.Text = string.Empty;

            btnClear_Click(null, null);
            var foundCount = 0;

            #region RegEx

            if (rdoRegex.Checked)
            {
                Regex rr = null;
                try
                {
                    rr = new Regex(txtFind.Text, GetRegexOptions());
                }
                catch (ArgumentException ex)
                {
                    lblStatus.Text = "正则表达式错误: " + ex.Message;
                    return;
                }

                foundCount = FindReplace.FindAll(rr, chkMarkLine.Checked, chkHighlightMatches.Checked).Count;
            }

            #endregion

            #region Non-RegEx

            if (!rdoRegex.Checked)
            {
                var textToFind = rdoExtended.Checked ? DataHelper.Transform(txtFind.Text) : txtFind.Text;
                foundCount = FindReplace.FindAll(textToFind, GetSearchFlags(), chkMarkLine.Checked,
                        chkHighlightMatches.Checked)
                    .Count;
            }

            #endregion

            lblStatus.Text = "查找结果数: " + foundCount;
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            FindNext();
        }

        private void btnFindPrevious_Click(object sender, EventArgs e)
        {
            FindPrevious();
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFind.Text)) return;

            AddReplaceMru();
            lblStatus.Text = string.Empty;
            var foundCount = 0;

            #region RegEx

            if (rdoRegex.Checked)
            {
                Regex rr = null;
                try
                {
                    rr = new Regex(txtFind.Text, GetRegexOptions());
                }
                catch (ArgumentException ex)
                {
                    lblStatus.Text = "正则表达式错误: " + ex.Message;
                    return;
                }

                foundCount = FindReplace.ReplaceAll(rr, DataHelper.Transform(txtReplace.Text), false, false);
            }

            #endregion

            #region Non-RegEx

            if (!rdoRegex.Checked)
            {
                var textToFind = rdoExtended.Checked ? DataHelper.Transform(txtFind.Text) : txtFind.Text;
                var textToReplace = rdoExtended.Checked ? DataHelper.Transform(txtReplace.Text) : txtReplace.Text;
                foundCount = FindReplace.ReplaceAll(textToFind, textToReplace, GetSearchFlags(), false, false);
            }

            #endregion

            lblStatus.Text = "替换结果数: " + foundCount;
        }

        private void btnReplaceNext_Click(object sender, EventArgs e)
        {
            ReplaceNext();
        }

        private void btnReplacePrevious_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFind.Text))
                return;

            AddReplaceMru();
            lblStatus.Text = string.Empty;

            CharacterRange nextRange;
            try
            {
                nextRange = ReplaceNext(true);
            }
            catch (ArgumentException ex)
            {
                lblStatus.Text = "正则表达式错误: " + ex.Message;
                return;
            }

            if (nextRange.cpMin == nextRange.cpMax)
            {
                lblStatus.Text = "找到不到匹配的文本";
            }
            else
            {
                if (nextRange.cpMin > Scintilla.AnchorPosition) lblStatus.Text = "查找到了当前文本的起始点";
                Scintilla.SetSel(nextRange.cpMin, nextRange.cpMax);
                MoveFormAwayFromSelection();
            }
        }


        private void cmdRecentFind_Click(object sender, EventArgs e)
        {
            mnuRecentFind.Items.Clear();
            foreach (var item in MruFind)
            {
                var newItem = mnuRecentFind.Items.Add(item);
                newItem.Tag = item;
            }

            mnuRecentFind.Items.Add("-");
            mnuRecentFind.Items.Add("清除历史记录");
            mnuRecentFind.Show(cmdRecentFind.PointToScreen(cmdRecentFind.ClientRectangle.Location));
        }

        private void cmdRecentReplace_Click(object sender, EventArgs e)
        {
            mnuRecentReplace.Items.Clear();
            foreach (var item in MruReplace)
            {
                var newItem = mnuRecentReplace.Items.Add(item);
                newItem.Tag = item;
            }

            mnuRecentReplace.Items.Add("-");
            mnuRecentReplace.Items.Add("清除历史记录");
            mnuRecentReplace.Show(cmdRecentReplace.PointToScreen(cmdRecentReplace.ClientRectangle.Location));
        }

        private void cmdExtendedCharFind_Click(object sender, EventArgs e)
        {
            if (rdoExtended.Checked)
                mnuExtendedChar.Show(
                    cmdExtCharAndRegExFind.PointToScreen(cmdExtCharAndRegExFind.ClientRectangle.Location));
            else if (rdoRegex.Checked)
                mnuRegExCharFind.Show(
                    cmdExtCharAndRegExFind.PointToScreen(cmdExtCharAndRegExFind.ClientRectangle.Location));
        }

        private void cmdExtendedCharReplace_Click(object sender, EventArgs e)
        {
            if (rdoExtended.Checked)
                mnuExtendedChar.Show(
                    cmdExtCharAndRegExReplace.PointToScreen(cmdExtCharAndRegExReplace.ClientRectangle.Location));
            else if (rdoRegex.Checked)
                mnuRegExCharReplace.Show(
                    cmdExtCharAndRegExReplace.PointToScreen(cmdExtCharAndRegExReplace.ClientRectangle.Location));
        }

        private void rdoStandard_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoRegex.Checked)
                pnlRegexpOptions.BringToFront();
            else
                pnlStandardOptions.BringToFront();

            cmdExtCharAndRegExFind.Enabled = !rdoStandard.Checked;
            cmdExtCharAndRegExReplace.Enabled = !rdoStandard.Checked;
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        ///     查找下一个
        /// </summary>
        public void FindNext()
        {
            if (string.IsNullOrEmpty(txtFind.Text))
                return;

            AddFindMru();
            lblStatus.Text = string.Empty;

            CharacterRange foundRange;

            try
            {
                var rr = new Regex(txtFind.Text, GetRegexOptions());
                foundRange = FindNext(false, ref rr);
            }
            catch (ArgumentException ex)
            {
                lblStatus.Text = "正则表达式错误: " + ex.Message;
                return;
            }

            if (foundRange.cpMin == foundRange.cpMax)
            {
                lblStatus.Text = "找到不到匹配的文本";
            }
            else
            {
                if (foundRange.cpMin < Scintilla.AnchorPosition) lblStatus.Text = "查找到了当前文本的起始点";

                Scintilla.SetSel(foundRange.cpMin, foundRange.cpMax);
                MoveFormAwayFromSelection();
            }
        }

        /// <summary>
        ///     查找上一个
        /// </summary>
        public void FindPrevious()
        {
            if (string.IsNullOrEmpty(txtFind.Text))
                return;

            AddFindMru();
            lblStatus.Text = string.Empty;
            CharacterRange foundRange;
            try
            {
                var rr = new Regex(txtFind.Text, GetRegexOptions());
                foundRange = FindNext(true, ref rr);
            }
            catch (ArgumentException ex)
            {
                lblStatus.Text = "正则表达式错误: " + ex.Message;
                return;
            }

            if (foundRange.cpMin == foundRange.cpMax)
            {
                lblStatus.Text = "找到不到匹配的文本";
            }
            else
            {
                if (foundRange.cpMin > Scintilla.CurrentPosition) lblStatus.Text = "查找到了当前文本的结尾处";
                Scintilla.SetSel(foundRange.cpMin, foundRange.cpMax);
                MoveFormAwayFromSelection();
            }
        }

        /// <summary>
        ///     获取正则表达式选项
        /// </summary>
        /// <returns></returns>
        private RegexOptions GetRegexOptions()
        {
            var ro = RegexOptions.None;
            ro |= RegexOptions.Compiled;
            if (chkIgnoreCase.Checked) ro |= RegexOptions.IgnoreCase;
            if (chkIgnorePatternWhitespace.Checked) ro |= RegexOptions.IgnorePatternWhitespace;
            if (chkMultiline.Checked) ro |= RegexOptions.Multiline;
            return ro;
        }

        /// <summary>
        ///     获取普通模式选项
        /// </summary>
        /// <returns></returns>
        private SearchFlags GetSearchFlags()
        {
            var sf = SearchFlags.None;
            if (chkMatchCase.Checked) sf |= SearchFlags.MatchCase;
            if (chkWholeWord.Checked) sf |= SearchFlags.WholeWord;
            if (chkWordStart.Checked) sf |= SearchFlags.WordStart;
            return sf;
        }

        /// <summary>
        ///     确保查找内容在屏幕上显示,移开窗口
        /// </summary>
        protected virtual void MoveFormAwayFromSelection()
        {
            if (!Visible)
                return;

            if (!AutoPosition)
                return;

            var pos = Scintilla.CurrentPosition;
            var x = Scintilla.PointXFromPosition(pos);
            var y = Scintilla.PointYFromPosition(pos);

            var cursorPoint = Scintilla.PointToScreen(new Point(x, y));

            var r = new Rectangle(Location, Size);
            if (!r.Contains(cursorPoint)) return;
            Point newLocation;
            if (cursorPoint.Y < Screen.PrimaryScreen.Bounds.Height / 2)
            {
                //TODO - replace lineheight with ScintillaNET command, when added
                var SCI_TEXTHEIGHT = 2279;
                var lineHeight = Scintilla.DirectMessage(SCI_TEXTHEIGHT, IntPtr.Zero, IntPtr.Zero).ToInt32();
                // int lineHeight = Scintilla.Lines[Scintilla.LineFromPosition(pos)].Height;

                // Top half of the screen
                newLocation = Scintilla.PointToClient(
                    new Point(Location.X, cursorPoint.Y + lineHeight * 2));
            }
            else
            {
                //TODO - replace lineheight with ScintillaNET command, when added
                var SCI_TEXTHEIGHT = 2279;
                var lineHeight = Scintilla.DirectMessage(SCI_TEXTHEIGHT, IntPtr.Zero, IntPtr.Zero).ToInt32();
                // int lineHeight = Scintilla.Lines[Scintilla.LineFromPosition(pos)].Height;

                // Bottom half of the screen
                newLocation = Scintilla.PointToClient(
                    new Point(Location.X, cursorPoint.Y - Height - lineHeight * 2));
            }

            newLocation = Scintilla.PointToScreen(newLocation);
            Location = newLocation;
        }

        public void ReplaceNext()
        {
            if (string.IsNullOrEmpty(txtFind.Text))
                return;

            AddReplaceMru();
            lblStatus.Text = string.Empty;

            CharacterRange nextRange;
            try
            {
                nextRange = ReplaceNext(false);
            }
            catch (ArgumentException ex)
            {
                lblStatus.Text = "正则表达式错误: " + ex.Message;
                return;
            }

            if (nextRange.cpMin == nextRange.cpMax)
            {
                lblStatus.Text = "找到不到匹配的文本";
                //TODO:打开下一个文档
            }
            else
            {
                //TODO:确定起始点
                if (nextRange.cpMin < Scintilla.AnchorPosition) lblStatus.Text = "查找到了当前文本的起始点";

                Scintilla.SetSel(nextRange.cpMin, nextRange.cpMax);
                MoveFormAwayFromSelection();
            }
        }


        private void AddFindMru()
        {
            var find = txtFind.Text;
            MruFind.Remove(find);

            MruFind.Insert(0, find);

            if (MruFind.Count > MruMaxCount)
                MruFind.RemoveAt(MruFind.Count - 1);
        }

        private void AddReplaceMru()
        {
            var find = txtFind.Text;
            MruFind.Remove(find);

            MruFind.Insert(0, find);

            if (MruFind.Count > MruMaxCount)
                MruFind.RemoveAt(MruFind.Count - 1);

            var replace = txtReplace.Text;
            if (!string.IsNullOrEmpty(replace))
            {
                MruReplace.Remove(replace);

                MruReplace.Insert(0, replace);

                if (MruReplace.Count > MruMaxCount)
                    MruReplace.RemoveAt(MruReplace.Count - 1);
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //Insert the string value held in the menu items Tag field (\t, \n, etc.)
            txtFind.SelectedText = e.ClickedItem.Tag.ToString();
        }

        private CharacterRange FindNext(bool searchUp, ref Regex rr)
        {
            CharacterRange foundRange;

            if (rdoRegex.Checked)
            {
                if (rr == null)
                    return new CharacterRange();

                foundRange = searchUp
                    ? FindReplace.FindPrevious("", GetSearchFlags(), rr)
                    : FindReplace.FindNext("", GetSearchFlags(), rr);
            }
            else
            {
                if (searchUp)
                {
                    var textToFind = rdoExtended.Checked ? DataHelper.Transform(txtFind.Text) : txtFind.Text;
                    foundRange = FindReplace.FindPrevious(textToFind, GetSearchFlags(), null);
                }
                else
                {
                    var textToFind = rdoExtended.Checked ? DataHelper.Transform(txtFind.Text) : txtFind.Text;
                    foundRange = FindReplace.FindNext(textToFind, GetSearchFlags(), null);
                }
            }

            return foundRange;
        }

        private void FindReplaceDialog_Activated(object sender, EventArgs e)
        {
            Opacity = 1.0;
        }

        private void FindReplaceDialog_Deactivate(object sender, EventArgs e)
        {
            Opacity = 0.6;
        }

        private void mnuRecentFindR_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //Insert the string value held in the menu items Tag field (\t, \n, etc.)
            if (e.ClickedItem.Text == "清除历史记录")
                MruFind.Clear();
            else
                txtFind.Text = e.ClickedItem.Tag.ToString();
        }

        private void mnuRecentReplace_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //Insert the string value held in the menu items Tag field (\t, \n, etc.)
            if (e.ClickedItem.Text == "清除历史记录")
                MruReplace.Clear();
            else
                txtReplace.Text = e.ClickedItem.Tag.ToString();
        }

        private CharacterRange ReplaceNext(bool searchUp)
        {
            Regex rr = null;
            var selRange = new CharacterRange(Scintilla.Selections[0].Start, Scintilla.Selections[0].End);

            //	We only do the actual replacement if the current selection exactly
            //	matches the find.
            if (selRange.cpMax - selRange.cpMin <= 0) return FindNext(searchUp, ref rr);
            if (rdoRegex.Checked)
            {
                rr = new Regex(txtFind.Text, GetRegexOptions());
                var selRangeText = Scintilla.GetTextRange(selRange.cpMin, selRange.cpMax - selRange.cpMin + 1);

                if (!selRange.Equals(FindReplace.Find(selRange, rr))) return FindNext(searchUp, ref rr);
                //	If searching up we do the replacement using the range object.
                //	Otherwise we use the selection object. The reason being if
                //	we use the range the caret is positioned before the replaced
                //	text. Conversely if we use the selection object the caret will
                //	be positioned after the replaced text. This is very important
                //	becuase we don't want the new text to be potentially matched
                //	in the next search.
                if (searchUp)
                {
                    Scintilla.SelectionStart = selRange.cpMin;
                    Scintilla.SelectionEnd = selRange.cpMax;
                    Scintilla.ReplaceSelection(rr.Replace(selRangeText, DataHelper.Transform(txtReplace.Text)));
                    Scintilla.GotoPosition(selRange.cpMin);
                }
                else
                {
                    Scintilla.ReplaceSelection(rr.Replace(selRangeText, DataHelper.Transform(txtReplace.Text)));
                }
            }
            else
            {
                var textToFind = rdoExtended.Checked ? DataHelper.Transform(txtFind.Text) : txtFind.Text;
                if (!selRange.Equals(FindReplace.Find(selRange, textToFind, GetSearchFlags())))
                    return FindNext(searchUp, ref rr);
                //	If searching up we do the replacement using the range object.
                //	Otherwise we use the selection object. The reason being if
                //	we use the range the caret is positioned before the replaced
                //	text. Conversely if we use the selection object the caret will
                //	be positioned after the replaced text. This is very important
                //	becuase we don't want the new text to be potentially matched
                //	in the next search.
                if (searchUp)
                {
                    var textToReplace = rdoExtended.Checked ? DataHelper.Transform(txtReplace.Text) : txtReplace.Text;
                    Scintilla.SelectionStart = selRange.cpMin;
                    Scintilla.SelectionEnd = selRange.cpMax;
                    Scintilla.ReplaceSelection(textToReplace);

                    Scintilla.GotoPosition(selRange.cpMin);
                }
                else
                {
                    var textToReplace = rdoExtended.Checked ? DataHelper.Transform(txtReplace.Text) : txtReplace.Text;
                    Scintilla.ReplaceSelection(textToReplace);
                }
            }

            return FindNext(searchUp, ref rr);
        }

        #endregion Methods
    }
}