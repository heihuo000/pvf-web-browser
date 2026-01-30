#region Using Directives

using System;
using System.Drawing;
using System.Windows.Forms;
using ScintillaNET;

#endregion Using Directives

namespace pvfUtility.Shell.Document.TextEditor.GoTo
{
    public partial class GoToDialog : Form
    {
        #region Constructors

        public GoToDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        private void GoToDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void GoToDialog_Activated(object sender, EventArgs e)
        {
            var displayLine = (CurrentLineNumber + 1).ToString();

            txtCurrentLine.Text = displayLine;
            txtMaxLine.Text = MaximumLineNumber.ToString();
            txtGotoLine.Text = displayLine;

            txtGotoLine.Select();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        #region Fields

        private int _gotoLineNumber;

        #endregion Fields

        #region Methods

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtGotoLine.Text, out _gotoLineNumber))
            {
                //	Line #s are 0 based but the users don't think that way
                _gotoLineNumber--;
                if (_gotoLineNumber < 0 || _gotoLineNumber >= MaximumLineNumber)
                {
                    err.SetError(txtGotoLine,
                        "Go to line # must be greater than 0 and less than " + (MaximumLineNumber + 1));
                }
                else
                {
                    Scintilla.Lines[_gotoLineNumber].Goto();
                    //Line(GotoLineNumber);
                    Hide();
                    //DialogResult = DialogResult.OK;
                }
            }
            else
            {
                err.SetError(txtGotoLine, "Go to line # must be a numeric value");
            }
        }

        // This was taken from FindReplaceDialog. Obviously some refactoring is called for
        // since we have common code. However I'm holding off on this because I'm coming
        // up with some other ideas for the FindReplaceDialog. Right now every scintilla
        // gets its own FindReplaceDialog, but they really need to be sharable across
        // multiple scintillas much like how DropMarkers work.

        private void MoveFormAwayFromSelection()
        {
            if (!Visible)
                return;

            var pos = Scintilla.CurrentPosition;
            var x = Scintilla.PointXFromPosition(pos);
            var y = Scintilla.PointYFromPosition(pos);

            var cursorPoint = Scintilla.PointToScreen(new Point(x, y));

            var r = new Rectangle(Location, Size);
            if (r.Contains(cursorPoint))
            {
                Point newLocation;
                if (cursorPoint.Y < Screen.PrimaryScreen.Bounds.Height / 2)
                {
                    //TODO - replace lineheight with ScintillaNET command, when added
                    var SCI_TEXTHEIGHT = 2279;
                    var lineHeight = Scintilla.DirectMessage(SCI_TEXTHEIGHT, IntPtr.Zero, IntPtr.Zero).ToInt32();
                    // Top half of the screen
                    newLocation = Scintilla.PointToClient(
                        new Point(Location.X, cursorPoint.Y + lineHeight * 2)
                    );
                }
                else
                {
                    //TODO - replace lineheight with ScintillaNET command, when added
                    var SCI_TEXTHEIGHT = 2279;
                    var lineHeight = Scintilla.DirectMessage(SCI_TEXTHEIGHT, IntPtr.Zero, IntPtr.Zero).ToInt32();
                    // Bottom half of the screen
                    newLocation = Scintilla.PointToClient(
                        new Point(Location.X, cursorPoint.Y - Height - lineHeight * 2)
                    );
                }

                newLocation = Scintilla.PointToScreen(newLocation);
                Location = newLocation;
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            MoveFormAwayFromSelection();
        }

        #endregion Methods

        #region Properties

        public int CurrentLineNumber { get; set; }

        public int GotoLineNumber
        {
            get => _gotoLineNumber;
            set => _gotoLineNumber = value;
        }

        public int MaximumLineNumber { get; set; }

        public Scintilla Scintilla { get; set; }

        #endregion Properties
    }
}