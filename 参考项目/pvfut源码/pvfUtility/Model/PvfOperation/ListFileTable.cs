using System;
using System.Collections.Generic;
using System.Linq;
using pvfUtility.Service;
using static pvfUtility.Helper.PathsHelper;

namespace pvfUtility.Model.PvfOperation
{
    internal class ListFileTable
    {
        /// <summary>
        ///     basePath - fileName - Code
        /// </summary>
        public Dictionary<string, Dictionary<string, int>> FileCodeDictionary;

        /// <summary>
        ///     basePath - fileName - Code - List
        /// </summary>
        public Dictionary<string, List<(string, int)>> FileCodeList;

        public void Init(PvfPack pack)
        {
            FileCodeDictionary = new Dictionary<string, Dictionary<string, int>>();
            FileCodeList = new Dictionary<string, List<(string, int)>>();
            foreach (var item in from item in pack.FileList
                where
                    item.Key == PathFix(item.Value.PathName) + item.Value.PathName + ".lst"
                select item)
            {
                var file = item.Value;
                var basePath = file.PathName;
                FileCodeDictionary.Add(basePath, new Dictionary<string, int>());
                FileCodeList.Add(basePath, new List<(string, int)>());
                LoadList(basePath, file, pack.Strtable);
            }

            Logger.Success("成功载入lst代码列表");
        }

        internal void LoadList(string basePath, PvfFile file, Stringtable stringtable)
        {
            var dic = FileCodeDictionary[basePath];
            var list = FileCodeList[basePath];
            dic.Clear();
            list.Clear();

            if (string.IsNullOrWhiteSpace(basePath) || !file.ShortName.Contains(basePath)) return;
            var length = file.DataLen;
            if (!file.IsScriptFile || length < 12)
                return;

            for (var i = 2; i < length - 5; i += 10)
            {
                var code = BitConverter.ToInt32(file.Data, i + 1);
                var fileName = (PathFix(basePath) +
                                stringtable.GetStringItem(BitConverter.ToInt32(file.Data, i + 1 + 5)))
                    .Replace('\\', '/').ToLower();
                list.Add((fileName, code));
                if (!dic.ContainsKey(fileName))
                {
                    dic.Add(fileName, code);
                }
            }
        }

        internal int GetCode(PvfFile file)
        {
            if (FileCodeDictionary == null) return -1;
            var path = PathFix(file.PathName);
            if (string.IsNullOrEmpty(path) || path.IndexOf('/') < 0) return -1;
            var basePath = path.Substring(0, path.IndexOf('/'));
            if (!FileCodeDictionary.ContainsKey(basePath)) return -1;
            var fileName = file.FileName;
            return !FileCodeDictionary[basePath].ContainsKey(fileName) ? -1 : FileCodeDictionary[basePath][fileName];
        }
    }
}