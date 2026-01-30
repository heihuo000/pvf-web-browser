using System.Collections.Generic;
using pvfUtility.Model;

namespace pvfUtility.Helper
{
    internal static class TreeFileHelper
    {
        public static Dictionary<string, TreeFileModel> CreateTreeList(IEnumerable<string> fileList)
        {
            var treelist = new Dictionary<string, TreeFileModel>();
            foreach (var s in fileList)
            {
                if (s == null) continue;
                var list0 = treelist;
                var chrArray = new[] {'\\', '/'};
                var strArrays = s.Split(chrArray);
                foreach (var x in strArrays)
                    if (list0.TryGetValue(x, out var tree))
                    {
                        list0 = tree.List;
                    }
                    else
                    {
                        var treeFile = new TreeFileModel {List = new Dictionary<string, TreeFileModel>()};
                        list0.Add(x, treeFile);
                        list0 = treeFile.List;
                    }
            }

            return treelist;
        }

        public static void AddString2TreeList(string str, Dictionary<string, TreeFileModel> treelist)
        {
            if (str == null) return;
            var list0 = treelist;
            var chrArray = new[] {'\\', '/'};
            var strArrays = str.Split(chrArray);
            foreach (var x in strArrays)
                if (list0.TryGetValue(x, out var tree))
                {
                    list0 = tree.List;
                }
                else
                {
                    var treeFile = new TreeFileModel {List = new Dictionary<string, TreeFileModel>()};
                    list0.Add(x, treeFile);
                    list0 = treeFile.List;
                }
        }

        public static void RemoveString2TreeList(string str, Dictionary<string, TreeFileModel> treelist)
        {
            var list0 = treelist;
            if (str.IndexOf('/') < 0)
            {
                list0.Remove(str);
            }
            else
            {
                var chrArray = new[] {'\\', '/'};
                var strArrays = str.Split(chrArray);
                for (var i = 0; i < strArrays.Length - 1; i++)
                {
                    var strA = strArrays[i];
                    if (list0.TryGetValue(strA, out var tree))
                        list0 = tree.List;
                }

                list0.Remove(strArrays[strArrays.Length - 1]);
            }
        }
    }
}