using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace pvfUtility.Dock.FileExplorer.Bookmark
{
    //Modified by wallace1300 Origin namespace:RyzStudio.Windows.Forms
    internal class BookmarkTreeView : TreeView
    {
        public enum BookmarkItemType
        {
            Bookmark,
            Folder,
            Separator
        }

        private readonly string AppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        protected readonly XmlDocument XmlDocument = new XmlDocument();

        public BookmarkTreeView()
        {
            InitializeComponent();
        }

        protected BookmarkTreeNode DraggingNode { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AllowDrop { get; set; }

        public bool UserEditable
        {
            get => AllowDrop;
            set => AllowDrop = value;
        }

        #region internals

        protected bool IsNodeChild(BookmarkTreeNode dragNode, BookmarkTreeNode dropNode)
        {
            var tn = (TreeNode) dropNode;
            while (true)
            {
                if (tn.Parent == null) break;

                if (tn.Equals(dragNode)) return true;

                tn = tn.Parent;
            }

            return false;
        }

        #endregion

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // BookmarkTreeView
            // 
            AllowDrop = true;
            FullRowSelect = true;
            HideSelection = false;
            HotTracking = true;
            ShowNodeToolTips = true;
            ResumeLayout(false);
        }

        public class BookmarkTreeNode : TreeNode
        {
            public BookmarkItemType BookmarkItemType { get; set; }
        }

        #region public methods

        public void LoadBookmark()
        {
            Nodes.Clear();
            if (!File.Exists(AppPath + "BookMark.xml"))
            {
                var xmldoc = new XmlDocument();
                var xmldecl = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmldoc.AppendChild(xmldecl);
                var xmlelem = xmldoc.CreateElement("", "root", "");
                xmldoc.AppendChild(xmlelem);
                xmldoc.Save(string.Concat(AppPath, "Bookmark.xml"));
            }

            XmlDocument.Load(string.Concat(AppPath, "BookMark.xml"));
            if (XmlDocument.DocumentElement == null) return;
            var t = XmlDocument.DocumentElement.SelectSingleNode("/root")?.ChildNodes;
            CreateBookmarkTree(t, Nodes);
        }

        private void CreateBookmarkTree(IEnumerable list, TreeNodeCollection tnc)
        {
            foreach (XmlNode x in list)
            {
                var xe = (XmlElement) x;
                var node = new BookmarkTreeNode
                {
                    Tag = xe.GetAttribute("FileName"),
                    Text = xe.GetAttribute("Title")
                };

                if (xe.GetAttribute("Title") == "-")
                {
                    node.ForeColor = Color.Green;
                    node.Text = @"-";
                    node.BookmarkItemType = BookmarkItemType.Separator;
                }
                else
                {
                    if (xe.GetAttribute("FileName") == "")
                    {
                        node.BookmarkItemType = BookmarkItemType.Folder;
                        node.ForeColor = Color.Chocolate;
                        CreateBookmarkTree(xe.ChildNodes, node.Nodes);
                    }
                    else
                    {
                        node.BookmarkItemType = BookmarkItemType.Bookmark;
                    }
                }

                tnc.Add(node);
            }
        }

        public Task SaveBookmark()
        {
            var xmldoc = new XmlDocument();
            var xmldecl = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmldoc.AppendChild(xmldecl);
            var rootElement = xmldoc.CreateElement("", "root", "");
            xmldoc.AppendChild(rootElement);
            SaveBookmarkTree(xmldoc, rootElement, Nodes);
            xmldoc.Save(string.Concat(AppPath, "Bookmark.xml"));
            return Task.FromResult(true);
        }

        private void SaveBookmarkTree(XmlDocument doc, XmlNode ele, IEnumerable collection)
        {
            foreach (TreeNode x in collection)
            {
                var element = doc.CreateElement("", "Bookmark", "");
                element.SetAttribute("Title", x.Text);
                element.SetAttribute("FileName", x.Text == @"-" ? @"-" : x.Tag.ToString());
                ele.AppendChild(element);
                if (x.Nodes.Count > 0)
                    SaveBookmarkTree(doc, element, x.Nodes);
            }
        }

        public void MoveNodeUp()
        {
            var tn = SelectedNode;

            if (tn.Index == 0) return;

            var n = tn.Index - 1;

            var tn1 = tn.Parent == null ? Nodes : tn.Parent.Nodes;
            tn1.Remove(tn);
            tn1.Insert(n, tn);

            SelectedNode = tn;
        }

        public void MoveNodeDown()
        {
            var tn = SelectedNode;

            var tn1 = tn.Parent == null ? Nodes : tn.Parent.Nodes;

            if (tn.Index >= tn1.Count - 1) return;

            var n = tn.Index + 1;

            tn1.Remove(tn);
            tn1.Insert(n, tn);

            SelectedNode = tn;
        }

        public async void AddFolder(string folderName)
        {
            var fn = new BookmarkTreeNode
            {
                Tag = "",
                Text = folderName,
                ForeColor = Color.Chocolate,
                BookmarkItemType = BookmarkItemType.Folder
            };
            var node = (BookmarkTreeNode) SelectedNode;
            if (node == null)
            {
                Nodes.Add(fn);
            }
            else if (node.BookmarkItemType == BookmarkItemType.Folder)
            {
                node.Nodes.Insert(node.Index, fn);
            }
            else
            {
                if (node.Parent == null)
                    Nodes.Insert(node.Index, fn);
                else
                    node.Parent.Nodes.Insert(node.Index, fn);
            }

            await SaveBookmark();
        }

        public async void AddSeparator()
        {
            var fn = new BookmarkTreeNode
            {
                Tag = "",
                ForeColor = Color.RoyalBlue,
                Text = @"-",
                BookmarkItemType = BookmarkItemType.Separator
            };
            var node = (BookmarkTreeNode) SelectedNode;

            if (node.BookmarkItemType == BookmarkItemType.Folder)
            {
                node.Nodes.Insert(node.Index, fn);
            }
            else
            {
                if (node.Parent == null)
                    Nodes.Insert(node.Index, fn);
                else
                    node.Parent.Nodes.Insert(node.Index, fn);
            }

            await SaveBookmark();
        }

        #endregion

        #region integrated behaviour

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);

            DraggingNode = (BookmarkTreeNode) e.Item;
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        protected override async void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);

            // if (draggingNode.Level <= 0)
            // {
            //     return;
            // }

            var en = (BookmarkTreeNode) GetNodeAt(PointToClient(new Point(e.X, e.Y)));
            if (en == null) return;

            if (IsNodeChild(DraggingNode, en)) return;

            var dn = DraggingNode;
            if (en.BookmarkItemType == BookmarkItemType.Folder)
            {
                if (en.Parent == null)
                    dn.Nodes.Remove(dn);
                else
                    dn.Parent.Nodes.Remove(dn);

                en.Nodes.Insert(0, dn);
            }
            else
            {
                switch (en.Parent)
                {
                    // bug fixed
                    //上拉
                    case null when en.Index == dn.Index - 1:
                        Nodes.Remove(dn);
                        Nodes.Insert(en.Index, dn);
                        break;
                    //下拉
                    case null:
                        Nodes.Remove(dn);
                        Nodes.Insert(en.Index + 1, dn);
                        break;
                    default:
                        if (en.Index == dn.Index - 1) //上拉
                        {
                            en.Parent.Nodes.Remove(dn);
                            en.Parent.Nodes.Insert(en.Index, dn);
                        }
                        else //下拉
                        {
                            en.Parent.Nodes.Remove(dn);
                            en.Parent.Nodes.Insert(en.Index + 1, dn);
                        }

                        break;
                }
            }

            await SaveBookmark();
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            e.Effect = DragDropEffects.Move;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            SelectedNode = GetNodeAt(e.Location);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            SelectedNode = GetNodeAt(PointToClient(new Point(e.X, e.Y)));
        }

        #endregion
    }
}