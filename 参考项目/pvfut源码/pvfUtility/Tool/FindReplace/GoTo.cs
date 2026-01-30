#region Using Directives

using pvfUtility.Shell.Document.TextEditor.GoTo;
using ScintillaNET;

#endregion Using Directives

namespace pvfUtility.Tool.FindReplace
{
    public class GoTo
    {
        private readonly Scintilla _scintilla;
        private readonly GoToDialog _window;

        #region Constructors

        public GoTo(Scintilla scintilla)
        {
            _scintilla = scintilla;
            _window = new GoToDialog {Scintilla = scintilla};
        }

        #endregion Constructors

        #region Methods

        public void Line(int number)
        {
            _scintilla.Lines[number].Goto();
        }

        public void Position(int pos)
        {
            _scintilla.GotoPosition(pos);
        }

        public void ShowGoToDialog()
        {
            //GoToDialog gd = new GoToDialog();
            var gd = _window;

            gd.CurrentLineNumber = _scintilla.CurrentLine;
            gd.MaximumLineNumber = _scintilla.Lines.Count;
            gd.Scintilla = _scintilla;

            if (!_window.Visible)
                _window.Show(_scintilla.FindForm());

            //_window.ShowDialog(_scintilla.FindForm());
            //_window.Show(_scintilla.FindForm());

            //if (gd.ShowDialog() == DialogResult.OK)
            //Line(gd.GotoLineNumber);

            //gd.ShowDialog();
            //gd.Show();

            _scintilla.Focus();
        }

        #endregion Methods
    }
}