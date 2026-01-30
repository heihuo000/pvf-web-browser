using pvfUtility.Actions;

namespace pvfUtility.Dialog
{
    internal partial class WelcomeDialog : DialogBase
    {
        public WelcomeDialog()
        {
            InitializeComponent();
        }

        public bool ChooseDark()
        {
            return radioButton2.Checked;
        }
    }
}