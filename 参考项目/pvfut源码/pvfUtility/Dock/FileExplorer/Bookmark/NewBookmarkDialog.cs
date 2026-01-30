using System;
using System.Xml;
using pvfUtility.Action;
using pvfUtility.Actions;

namespace pvfUtility.FileExplorer.Bookmark
{
    internal partial class NewBookmarkDialog : DialogBase
    {
        private readonly XmlDocument _xmlDocument = new XmlDocument();
        private string _fileName;

        public NewBookmarkDialog(string fileName)
        {
            InitializeComponent();
            if (fileName != null)
            {
                textBoxPath.Text = fileName;
                _fileName = textBoxPath.Text;
                textBoxPath.Enabled = false;
            }

            // Init Paths
            _xmlDocument.Load(string.Concat(AppCore.AppPath, "BookMark.xml"));
            if (_xmlDocument.DocumentElement == null) return;
            var t = _xmlDocument.DocumentElement.SelectSingleNode("/root");

            var path = new PathInfo
            {
                Name = "（根目录）",
                XmlPath = t
            };
            comboBoxFolder.Items.Add(path);
            comboBoxFolder.SelectedIndex = 0;

            LoadPath(t, 0);
            btnOk.Click += buttonOK_Click;
            btnCancel.Click += buttonCancel_Click;
        }

        private void LoadPath(XmlNode list, int layer)
        {
            var str = "";
            for (var i = 0; i < layer; i++)
                str += "─";

            foreach (XmlNode x in list.ChildNodes)
            {
                var xe = (XmlElement) x;
                if (xe.GetAttribute("FileName") == "" && xe.GetAttribute("Title") != "-")
                {
                    var path = new PathInfo
                    {
                        Name = str + xe.GetAttribute("Title"), XmlPath = x
                    };
                    comboBoxFolder.Items.Add(path);
                }

                if (x.HasChildNodes)
                    LoadPath(x, layer + 1);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            _fileName = textBoxPath.Text;
            var pathinfo = (PathInfo) comboBoxFolder.SelectedItem;
            var xe1 = _xmlDocument.CreateElement("BookMark");
            xe1.SetAttribute("FileName", _fileName);
            xe1.SetAttribute("Title", textBoxName.Text);
            pathinfo.XmlPath.AppendChild(xe1);
            _xmlDocument.Save(string.Concat(AppCore.AppPath, "BookMark.xml"));
            Close();
            Dispose();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private class PathInfo
        {
            public string Name;
            public XmlNode XmlPath;

            public override string ToString()
            {
                return Name;
            }
        }
    }
}