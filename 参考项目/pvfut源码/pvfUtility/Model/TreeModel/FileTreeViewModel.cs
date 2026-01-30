using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aga.Controls.Tree;
using pvfUtility.Helper;

namespace pvfUtility.Model.TreeModel
{
    internal class FileTreeViewModel : TreeModelBase
    {
        public readonly Dictionary<string, TreeFileModel> FileListTree;

        public FileTreeViewModel(Dictionary<string, TreeFileModel> tree)
        {
            FileListTree = tree;
        }

        public FileTreeViewModel(List<string> tree)
        {
            FileListTree = TreeFileHelper.CreateTreeList(tree);
        }

        public override IEnumerable GetChildren(TreePath treePath)
        {
            var items = new List<FileTreeViewNode>();
            if (treePath.IsEmpty())
            {
                var list0 = FileListTree;
                items.AddRange(from item in list0.OrderBy(x => x.Key)
                    where item.Value.List.Count != 0
                    select
                        new FileTreeViewNode(item.Key));
                items.AddRange(from item in list0.OrderBy(x => x.Key)
                    where item.Value.List.Count == 0
                    select
                        new FileTreeViewNode(item.Key, item.Key, false));
            }
            else
            {
                if (!(treePath.LastNode is FileTreeViewNode parent)) return items;
                var text = parent.ToString();
                var list = FileListTree;
                char[] chrArray = {'\\', '/'};
                var strArrays = text.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    list = strArrays.Aggregate(list, (current, str) => current[str].List);
                }
                catch
                {
                    list = new Dictionary<string, TreeFileModel>();
                }

                items.AddRange(from item in list.OrderBy(x => x.Key)
                    where item.Value.List.Count != 0
                    select
                        new FileTreeViewNode(item.Key, string.Concat(text, "/", item.Key), true));
                items.AddRange(from item in list.OrderBy(x => x.Key)
                    where item.Value.List.Count == 0
                    select
                        new FileTreeViewNode(item.Key, string.Concat(text, "/", item.Key), false));
            }

            return items;
        }

        public override bool IsLeaf(TreePath treePath)
        {
            var x = (FileTreeViewNode) treePath.LastNode;
            return !x.IsFolder;
        }

        public void RefreshTree(TreePath treePath)
        {
            OnStructureChanged(new TreePathEventArgs(treePath));
        }
    }
}