using System;
using System.Windows.Forms;

namespace pvfUtility.Shell.Dialogs
{
    public sealed partial class InputBox : Form
    {
        /// <summary>
        ///     初始化一个InputBox
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="content">内容</param>
        /// <param name="name">输入类型名称</param>
        public InputBox(string caption, string content, string name, string text = "")
        {
            InitializeComponent();
            Text = caption;
            InputDescription.Text = content;
            InputName.Text = name;
            buttonOK.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
            InputContent.Text = text;
        }

        /// <summary>
        ///     获取输入的内容
        /// </summary>
        public string InputtedText => InputContent.Text;

        private void InputBox_Load(object sender, EventArgs e)
        {
            InputContent.Focus();
        }
    }
}