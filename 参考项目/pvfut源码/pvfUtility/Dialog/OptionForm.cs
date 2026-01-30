using System;
using System.Linq;
using System.Windows.Forms;
using pvfUtility.Actions;
using pvfUtility.Model;
using static pvfUtility.Action.AppCore;

namespace pvfUtility.Shell
{
    internal partial class OptionForm : DialogBase
    {
        public OptionForm()
        {
            InitializeComponent();
            btnOk.Click += buttonOK_Click;
            btnCancel.Click += buttonCancel_Click;

            foreach (var item in typeof(Config).GetFields().Reverse())
            {
                if (!Config.Description.ContainsKey(item.Name))
                    continue;
                if (item.FieldType == false.GetType())
                {
                    var itemValue = false;
                    var checkItem = new CheckBox
                    {
                        Text = Config.Description[item.Name],
                        Dock = DockStyle.Top,
                        Tag = item.Name,
                        Checked = (bool) item.GetValue(Config.Instance)
                    };
                    panel1.Controls.Add(checkItem);
                }
                else if (item.FieldType == typeof(string))
                {
                    var title = new Label
                    {
                        Dock = DockStyle.Top,
                        Text = Config.Description[item.Name]
                    };
                    var textBox = new TextBox
                    {
                        Dock = DockStyle.Top,
                        Text = (string) item.GetValue(Config.Instance),
                        Tag = item.Name
                    };
                    panel1.Controls.Add(textBox);
                    panel1.Controls.Add(title);
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            foreach (var item in panel1.Controls)
                switch (item)
                {
                    case CheckBox checkBox:
                    {
                        var field = typeof(Config).GetField(checkBox.Tag.ToString());
                        field.SetValue(Config.Instance, checkBox.Checked);
                        break;
                    }
                    case TextBox textBox:
                    {
                        var field = typeof(Config).GetField(textBox.Tag.ToString());
                        field.SetValue(Config.Instance, textBox.Text);
                        break;
                    }
                }

            SaveSetting();
            Close();
            Dispose();
        }
    }
}