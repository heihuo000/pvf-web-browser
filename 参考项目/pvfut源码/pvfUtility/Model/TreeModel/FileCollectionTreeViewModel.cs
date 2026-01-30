using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aga.Controls.Tree;

namespace pvfUtility.Model.TreeModel
{
    internal class FileCollectionTreeViewModel : TreeModelBase
    {
        private readonly List<string> _fileList;

        public FileCollectionTreeViewModel(List<string> list)
        {
            _fileList = list;
        }

        public override IEnumerable GetChildren(TreePath treePath)
        {
            var items = new List<FileTreeViewNode>();
            items.AddRange(from item in _fileList.OrderBy(x => x)
                select new FileTreeViewNode(item, item, false));
            return items;
        }

        public override bool IsLeaf(TreePath treePath)
        {
            return true;
        }
    }
}