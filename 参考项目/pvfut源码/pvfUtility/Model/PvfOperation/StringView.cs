using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using pvfUtility.Helper;
using pvfUtility.Helper.Native;
using pvfUtility.Properties;
using pvfUtility.Service;

namespace pvfUtility.Model.PvfOperation
{
    internal class StringView
    {
        private StrListFile[] _pvfstrlist;

        public void InitStringData(PvfFile file, PvfPack objs, EncodingType type)
        {
            if (!file.IsScriptFile)
                return;
            var length = file.DataLen;
            _pvfstrlist = new StrListFile[(length - 2) / 10];
            var id = 0;
            for (var i = 2; i < length; i += 10)
            {
                var strFileName = objs.Strtable.GetStringItem(BitConverter.ToInt32(file.Data, i + 1 + 5));
                var index = objs.GetFile(strFileName);
                if (index != null)
                    LoadstrFile(Encoding.GetEncoding((int) type).GetString(index.Data).TrimEnd(new char[1]), id,
                        strFileName);
                id++;
            }

            Logger.Success($"StringLink :: 成功载入{(length - 2) / 10}个Str文件。");
        }

        private void LoadstrFile(string data, int codeIndex, string fileName)
        {
            try
            {
                _pvfstrlist[codeIndex] = new StrListFile(fileName);
                var chArray = new[] {'\r', '\n'};
                var lst = new Dictionary<string, string>();
                foreach (var stritem in data.Split(chArray, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (stritem.IndexOf('>') <= 0 ||
                        stritem.Length > 2 && stritem[0] == '/' && stritem[1] == '/') continue;
                    var str2 = DataHelper.GetDataFromFormat(stritem, "", ">");
                    var str3 = DataHelper.GetDataFromFormat(stritem, ">", "");
                    if (str2.Length > 0)
                        lst.Add(str2, str3);
                }

                _pvfstrlist[codeIndex].StringLst = lst;
            }
            catch (Exception e)
            {
                Logger.Error($"载入 file://{fileName} ，发生错误：{e.Message}，此文件损坏或存在重复项。");
            }
        }

        public void ReloadstrFile(string fileName, string fileData)
        {
            foreach (var t in _pvfstrlist)
            {
                if (t == null) continue;
                if (t.StrFileName.Replace('\\', '/').ToLower() != fileName) continue;
                t.StringLst.Clear();
                try
                {
                    var chArray = new[] {'\r', '\n'};
                    var lst = new Dictionary<string, string>();
                    foreach (var stritem in fileData.Split(chArray, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (stritem.IndexOf('>') <= 0 || stritem.Length > 2 && stritem[0] == '/' && stritem[1] == '/')
                            continue;
                        var str2 = DataHelper.GetDataFromFormat(stritem, "", ">");
                        var str3 = DataHelper.GetDataFromFormat(stritem, ">", "");
                        if (str2.Length <= 0) continue;
                        if (!lst.ContainsKey(str2))
                            lst.Add(str2, str3);
                        else
                            Logger.Error($"重载 file://{fileName} 时，发现重复项{str2}，这将导致文件损坏。");
                    }

                    t.StringLst = lst;
                }
                catch (Exception e)
                {
                    Logger.Error($"载入 file://{fileName} ，发生错误：{e.Message}，此文件损坏或存在重复项。");
                }

                Logger.Success($"成功重载：file://{fileName} ");
                return;
            }
        }

        public string GetStrText(int strid, string strname)
        {
            try
            {
                if (strid < 0 || strid >= _pvfstrlist.Length)
                    return string.Format(lang.StrIndexError, strid);
                return _pvfstrlist[strid].StringLst.TryGetValue(strname, out var str)
                    ? str
                    : string.Format(lang.CouldnotFindStr, strname);
            }
            catch (Exception ex)
            {
                Logger.Error($"StringLink :: 读取<{strid}::{strname}>发生错误:{ex.Message}");
                return "";
            }
        }

        public void SearchstrInFiles(HashSet<int> nums, Stringtable stringtable,
            string keyWord, bool startMatch, bool useLike, Regex regex)
        {
            var keyWord2 = ChineseHelper.ToTraditional(keyWord);
            for (var i = 0; i < _pvfstrlist.Length; i++)
            {
                //try
                {
                    var stringList = new List<string>();
                    if (_pvfstrlist[i] == null) continue;
                    if (regex != null)
                        stringList.AddRange(from item in _pvfstrlist[i].StringLst
                            where regex.IsMatch(keyWord)
                            select item.Key);
                    else if (useLike)
                        stringList.AddRange(from item in _pvfstrlist[i].StringLst
                            where LikeOperator.LikeString(item.Value, keyWord, CompareMethod.Binary) ||
                                  LikeOperator.LikeString(item.Value, keyWord2, CompareMethod.Binary)
                            select item.Key);
                    else if (startMatch)
                        stringList.AddRange(from item in _pvfstrlist[i].StringLst
                            where item.Value.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) == 0
                                  || item.Value.IndexOf(keyWord2, StringComparison.OrdinalIgnoreCase) == 0
                            select item.Key);
                    else
                        stringList.AddRange(from item in _pvfstrlist[i].StringLst
                            where item.Value.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) >= 0
                                  || item.Value.IndexOf(keyWord2, StringComparison.OrdinalIgnoreCase) >= 0
                            select item.Key);

                    foreach (var num in stringList.Select(stringtable.GetStringItem).Where(num => num != -1))
                        nums.Add(num + i * 16777216);
                }
                //catch
                {
                    // ignored
                }
            }
        }

        private class StrListFile
        {
            public readonly string StrFileName;
            public Dictionary<string, string> StringLst;

            public StrListFile(string strFileName)
            {
                StrFileName = strFileName;
                StringLst = new Dictionary<string, string>();
            }
        }
    }
}