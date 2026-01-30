using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using pvfUtility.Document.TextEditor.FindReplace;
using pvfUtility.Shell.Document.TextEditor;
using ScintillaNET;

namespace pvfUtility.Tool.FindReplace
{
    internal class FindReplaceService
    {
        #region Fields

        private int _lastReplaceAllOffset;
        private CharacterRange _lastReplaceAllRangeToSearch;
        private string _lastReplaceAllReplaceString = "";
        private int _lastReplaceCount;
        private Scintilla _currentScintilla;

        #endregion Fields

        #region Constructors

        /// <summary>
        ///     Creates an instance of the FindReplace class.
        /// </summary>
        /// <param name="scintilla">The Scintilla class to which the FindReplace class is attached.</param>
        public FindReplaceService(Scintilla scintilla)
        {
            Window1 = CreateWindowInstance();

            if (scintilla != null)
                Scintilla = scintilla;
        }

        /// <summary>
        ///     Creates an instance of the FindReplace class.
        /// </summary>
        public FindReplaceService() : this(null)
        {
        }

        #endregion Constructors

        #region Properties

        public Scintilla Scintilla
        {
            get => _currentScintilla;
            set
            {
                _currentScintilla = value;
                if (_currentScintilla == null)
                    return;
                Marker = _currentScintilla.Markers[10];
                Marker.Symbol = MarkerSymbol.Circle;
                Marker.SetForeColor(Color.Black);
                Marker.SetBackColor(Color.Blue);
                Indicator = _currentScintilla.Indicators[16];
                Indicator.ForeColor = Color.Red;
                //_indicator.ForeColor = Color.LawnGreen; //Smart highlight
                Indicator.Alpha = 100;
                Indicator.Style = IndicatorStyle.RoundBox;
                Indicator.Under = true;

                Window1.Scintilla = _currentScintilla;
                Window1.FindReplace = this;
            }
        }


        public Indicator Indicator { get; set; }

        public Marker Marker { get; set; }

        public bool _lastReplaceHighlight { get; set; }

        public int _lastReplaceLastLine { get; set; }

        public bool _lastReplaceMark { get; set; }

        public FindReplaceDialog Window1 { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Clears highlights from the entire document
        /// </summary>
        public void ClearAllHighlights()
        {
            var currentIndicator = _currentScintilla.IndicatorCurrent;

            _currentScintilla.IndicatorCurrent = Indicator.Index;
            _currentScintilla.IndicatorClearRange(0, _currentScintilla.TextLength);

            _currentScintilla.IndicatorCurrent = currentIndicator;
        }

        /// <summary>
        ///     Highlight ranges in the document.
        /// </summary>
        /// <param name="Ranges">List of ranges to which highlighting should be applied.</param>
        public void HighlightAll(IEnumerable<CharacterRange> Ranges)
        {
            _currentScintilla.IndicatorCurrent = Indicator.Index;

            foreach (var r in Ranges) _currentScintilla.IndicatorFillRange(r.cpMin, r.cpMax - r.cpMin);
        }

        /// <summary>
        ///     查找（普通模式）实现
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="searchString"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public CharacterRange Find(CharacterRange r, string searchString, SearchFlags flags) //unsafe
        {
            if (string.IsNullOrEmpty(searchString))
                return new CharacterRange();

            _currentScintilla.TargetStart = r.cpMin;
            _currentScintilla.TargetEnd = r.cpMax;
            _currentScintilla.SearchFlags = flags;
            var pos = _currentScintilla.SearchInTarget(searchString);
            return pos != -1
                ? new CharacterRange(_currentScintilla.TargetStart, _currentScintilla.TargetEnd)
                : new CharacterRange();
        }

        /// <summary>
        ///     查找（Regex模式）实现
        /// </summary>
        /// <param name="r"></param>
        /// <param name="findExpression"></param>
        /// <param name="searchUp"></param>
        /// <returns></returns>
        public CharacterRange Find(CharacterRange r, Regex findExpression, bool searchUp = false)
        {
            // Single line and Multi Line in RegExp doesn't really effect
            // whether or not a match will include newline characters. This
            // means we can't do a line by line search. We have to search
            // the entire range because it could potentially match the
            // entire range.

            var text = _currentScintilla.GetTextRange(r.cpMin, r.cpMax - r.cpMin + 1);
            var m = findExpression.Match(text);
            if (!m.Success) return new CharacterRange();
            if (searchUp)
            {
                // Since we can't search backwards with RegExp we
                // have to search the entire string and return the
                // last match. Not the most efficient way of doing
                // things but it works.
                var range = new CharacterRange();
                while (m.Success)
                {
                    //TODO - check that removing the byte count does not upset anything
                    //int start = r.cpMin + _scintilla.Encoding.GetByteCount(text.Substring(0, m.Index));
                    //int end = _scintilla.Encoding.GetByteCount(text.Substring(m.Index, m.Length));
                    var start = r.cpMin + text.Substring(0, m.Index).Length;
                    var end = text.Substring(m.Index, m.Length).Length;

                    range = new CharacterRange(start, start + end);
                    m = m.NextMatch();
                }

                return range;
            }

            {
                //TODO - check that removing the byte count does not upset anything
                //int start = r.cpMin + _scintilla.Encoding.GetByteCount(text.Substring(0, m.Index));
                //int end = _scintilla.Encoding.GetByteCount(text.Substring(m.Index, m.Length));
                var start = r.cpMin + text.Substring(0, m.Index).Length;
                var end = text.Substring(m.Index, m.Length).Length;

                return new CharacterRange(start, start + end);
            }
        }


        #region FindAll

        public List<CharacterRange> FindAll(int startPos, int endPos, string searchString, SearchFlags flags, bool Mark,
            bool Highlight)
        {
            var Results = new List<CharacterRange>();

            _currentScintilla.IndicatorCurrent = Indicator.Index;

            var findCount = 0;
            var lastLine = -1;
            while (true)
            {
                var r = Find(new CharacterRange(startPos, endPos), searchString, flags);
                if (r.cpMin == r.cpMax) break;

                Results.Add(r);
                findCount++;
                if (Mark)
                {
                    //	We can of course have multiple instances of a find on a single
                    //	line. We don't want to mark this line more than once.
                    var line = new Line(_currentScintilla, _currentScintilla.LineFromPosition(r.cpMin));
                    if (line.Position > lastLine)
                        line.MarkerAdd(Marker.Index);
                    lastLine = line.Position;
                }

                if (Highlight) _currentScintilla.IndicatorFillRange(r.cpMin, r.cpMax - r.cpMin);
                startPos = r.cpMax;
            }
            //return findCount;

            //FindAllResults?.Invoke(this, new FindResultsEventArgs(this, Results));

            return Results;
        }

        public List<CharacterRange> FindAll(CharacterRange rangeToSearch, Regex findExpression, bool Mark,
            bool Highlight)
        {
            var Results = new List<CharacterRange>();

            _currentScintilla.IndicatorCurrent = Indicator.Index;

            var findCount = 0;
            var lastLine = -1;

            while (true)
            {
                var r = Find(rangeToSearch, findExpression);
                if (r.cpMin == r.cpMax) break;

                Results.Add(r);
                findCount++;
                if (Mark)
                {
                    //	We can of course have multiple instances of a find on a single
                    //	line. We don't want to mark this line more than once.
                    var line = new Line(_currentScintilla, _currentScintilla.LineFromPosition(r.cpMin));
                    if (line.Position > lastLine)
                        line.MarkerAdd(Marker.Index);
                    lastLine = line.Position;
                }

                if (Highlight) _currentScintilla.IndicatorFillRange(r.cpMin, r.cpMax - r.cpMin);
                rangeToSearch = new CharacterRange(r.cpMax, rangeToSearch.cpMax);
            }
            //return findCount;
            //FindAllResults?.Invoke(this, new FindResultsEventArgs(this, Results));

            return Results;
        }


        public List<CharacterRange> FindAll(Regex findExpression, bool mark, bool highlight)
        {
            return FindAll(new CharacterRange(0, _currentScintilla.TextLength), findExpression, mark, highlight);
        }

        public List<CharacterRange> FindAll(string searchString, SearchFlags flags, bool Mark, bool Highlight)
        {
            return FindAll(0, _currentScintilla.TextLength, searchString, flags, Mark, Highlight);
        }

        #endregion

        #region 查找

        /// <summary>
        ///     查找下一个
        /// </summary>
        public CharacterRange FindNext(string searchString, SearchFlags flags, Regex findExpression)
        {
            var r = findExpression != null
                ? Find(new CharacterRange(_currentScintilla.CurrentPosition, _currentScintilla.TextLength),
                    findExpression)
                : Find(new CharacterRange(_currentScintilla.CurrentPosition, _currentScintilla.TextLength),
                    searchString, flags);
            return r.cpMin != r.cpMax
                ? r
                : Find(new CharacterRange(0, _currentScintilla.CurrentPosition), searchString, flags);
        }

        /// <summary>
        ///     查找上一个
        /// </summary>
        /// <returns></returns>
        public CharacterRange FindPrevious(string searchString, SearchFlags flags, Regex findExpression)
        {
            var r = findExpression != null
                ? Find(new CharacterRange(_currentScintilla.AnchorPosition, 0), findExpression)
                : Find(new CharacterRange(_currentScintilla.AnchorPosition, 0), searchString, flags);
            return r.cpMin != r.cpMax
                ? r
                : Find(new CharacterRange(_currentScintilla.TextLength, _currentScintilla.CurrentPosition),
                    searchString, flags);
            ;
        }

        #endregion

        public int ReplaceAll(int startPos, int endPos, Regex findExpression, string replaceString, bool Mark,
            bool Highlight)
        {
            return ReplaceAll(new CharacterRange(startPos, endPos), findExpression, replaceString, Mark, Highlight);
        }

        public int ReplaceAll(int startPos, int endPos, string searchString, string replaceString, SearchFlags flags,
            bool Mark, bool Highlight)
        {
            _currentScintilla.IndicatorCurrent = Indicator.Index;

            var findCount = 0;
            var lastLine = -1;

            _currentScintilla.BeginUndoAction();

            var diff = replaceString.Length - searchString.Length;
            while (true)
            {
                // TODO:
                var r = Find(new CharacterRange(startPos, endPos), searchString, flags);
                if (r.cpMin == r.cpMax) break;

                _currentScintilla.SelectionStart = r.cpMin;
                _currentScintilla.SelectionEnd = r.cpMax;
                _currentScintilla.ReplaceSelection(replaceString);
                r.cpMax = startPos = r.cpMin + replaceString.Length;
                endPos += diff;

                findCount++;

                if (Mark)
                {
                    //	We can of course have multiple instances of a find on a single
                    //	line. We don't want to mark this line more than once.
                    var line = new Line(_currentScintilla, _currentScintilla.LineFromPosition(r.cpMin));
                    if (line.Position > lastLine)
                        line.MarkerAdd(Marker.Index);
                    lastLine = line.Position;
                }

                if (Highlight) _currentScintilla.IndicatorFillRange(r.cpMin, r.cpMax - r.cpMin);
            }

            _currentScintilla.EndUndoAction();
            return findCount;
        }

        public int ReplaceAll(CharacterRange rangeToSearch, Regex findExpression, string replaceString, bool Mark,
            bool Highlight)
        {
            _currentScintilla.IndicatorCurrent = Indicator.Index;
            _currentScintilla.BeginUndoAction();

            //	I tried using an anonymous delegate for this but it didn't work too well.
            //	It's too bad because it was a lot cleaner than using member variables as
            //	psuedo globals.
            _lastReplaceAllReplaceString = replaceString;
            _lastReplaceAllRangeToSearch = rangeToSearch;
            _lastReplaceAllOffset = 0;
            _lastReplaceCount = 0;
            _lastReplaceMark = Mark;
            _lastReplaceHighlight = Highlight;

            var text = _currentScintilla.GetTextRange(rangeToSearch.cpMin,
                rangeToSearch.cpMax - rangeToSearch.cpMin + 1);
            findExpression.Replace(text, ReplaceAllEvaluator);

            _currentScintilla.EndUndoAction();

            //	No use having these values hanging around wasting memory :)
            _lastReplaceAllReplaceString = null;
            _lastReplaceAllRangeToSearch = new CharacterRange();

            return _lastReplaceCount;
        }

        public int ReplaceAll(Regex findExpression, string replaceString, bool Mark, bool Highlight)
        {
            return ReplaceAll(0, _currentScintilla.TextLength, findExpression, replaceString, Mark, Highlight);
        }

        public int ReplaceAll(string searchString, string replaceString, SearchFlags flags, bool Mark, bool Highlight)
        {
            return ReplaceAll(0, _currentScintilla.TextLength, searchString, replaceString, flags, Mark, Highlight);
        }

        public void EditorChanged(object sender, EventArgs eventArgs)
        {
            Scintilla = MainPresenter.Instance.View.CurrentDockDocument is TextEditorView textEditorView
                ? textEditorView.scintilla
                : null;
            Window1.SetEnable(Scintilla != null);
        }

        public void ShowFind()
        {
            if (!Window1.Visible)
                Window1.Show(); //_scintilla.FindForm()

            Window1.ConvertMode(true);

            Window1.Scintilla = _currentScintilla;
            Window1.FindReplace = this;
            if (_currentScintilla.Selections[0].End > _currentScintilla.Selections[0].Start)
                Window1.txtFind.Text = _currentScintilla.SelectedText;

            Window1.txtFind.Select();
            Window1.txtFind.SelectAll();
        }

        public void ShowReplace()
        {
            if (!Window1.Visible)
                Window1.Show(_currentScintilla.FindForm());
            Window1.ConvertMode(false);
            Window1.Scintilla = _currentScintilla;
            Window1.FindReplace = this;

            if (_currentScintilla.Selections[0].End > _currentScintilla.Selections[0].Start)
                Window1.txtFind.Text = _currentScintilla.SelectedText;

            Window1.txtFind.Select();
            Window1.txtFind.SelectAll();
        }

        protected virtual FindReplaceDialog CreateWindowInstance()
        {
            return new FindReplaceDialog();
        }

        private string ReplaceAllEvaluator(Match m)
        {
            //	So this method is called for every match

            //	We make a replacement in the range based upon
            //	the match range.
            var replacement = m.Result(_lastReplaceAllReplaceString);
            var start = _lastReplaceAllRangeToSearch.cpMin + m.Index + _lastReplaceAllOffset;
            var end = start + m.Length;

            var r = new CharacterRange(start, end);
            _lastReplaceCount++;
            _currentScintilla.SelectionStart = r.cpMin;
            _currentScintilla.SelectionEnd = r.cpMax;
            _currentScintilla.ReplaceSelection(replacement);

            if (_lastReplaceMark)
            {
                //	We can of course have multiple instances of a find on a single
                //	line. We don't want to mark this line more than once.
                // TODO - Is determining the current line any more efficient that just setting the duplicate marker? LineFromPosition appears to have more code that MarkerAdd!
                var line = new Line(_currentScintilla, _currentScintilla.LineFromPosition(r.cpMin));
                if (line.Position > _lastReplaceLastLine)
                    line.MarkerAdd(Marker.Index);
                _lastReplaceLastLine = line.Position;
            }

            if (_lastReplaceHighlight) _currentScintilla.IndicatorFillRange(r.cpMin, r.cpMax - r.cpMin);

            //	But because we've modified the document, the RegEx
            //	match ranges are going to be different from the
            //	document ranges. We need to compensate
            _lastReplaceAllOffset += replacement.Length - m.Value.Length;

            return replacement;
        }

        #endregion Methods
    }
}