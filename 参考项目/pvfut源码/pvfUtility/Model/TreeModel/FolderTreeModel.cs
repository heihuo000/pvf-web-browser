using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aga.Controls.Tree;

namespace pvfUtility.Model.TreeModel
{
    internal class FolderTreeModel : TreeModelBase
    {
        private readonly Dictionary<string, TreeFileModel> _fileList;

        public FolderTreeModel(Dictionary<string, TreeFileModel> source)
        {
            _fileList = source;
        }

        public override IEnumerable GetChildren(TreePath treePath)
        {
            var items = new List<FileTreeViewNode>();
            if (treePath.IsEmpty())
            {
                var list0 = _fileList;
                items.AddRange(from item in list0.OrderBy(x => x.Key)
                    where item.Value.List.Count != 0
                    select
                        new FileTreeViewNode(item.Key));
            }
            else
            {
                if (!(treePath.LastNode is FileTreeViewNode parent))
                    return items;
                var text = parent.ToString();
                var list0 = _fileList;
                char[] chrArray = {'\\', '/'};
                var strArrays = text.Split(chrArray);
                list0 = strArrays.Aggregate(list0, (current, str) => current[str].List);

                items.AddRange(from item in list0.OrderBy(x => x.Key)
                    where item.Value.List.Count != 0
                    select
                        new FileTreeViewNode(item.Key, string.Concat(text, "/", item.Key), true));
            }

            return items;
        }

        public override bool IsLeaf(TreePath treePath)
        {
            var x = (FileTreeViewNode) treePath.LastNode;
            return !x.IsFolder;
        }
    }
}