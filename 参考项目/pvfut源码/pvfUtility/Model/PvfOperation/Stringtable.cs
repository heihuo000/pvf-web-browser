using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using pvfUtility.Helper.Native;
using pvfUtility.Properties;
using pvfUtility.Service;

namespace pvfUtility.Model.PvfOperation
{
    internal class Stringtable
    {
        private const string COMMENT = "此StringTable由 pvfUtility 2020 或更高版本生成.";
        private EncodingType _encodingType;
        private Dictionary<string, StringItem> _stringTable;

        private List<StringItem> _stringTableList;

        public int NameLabel { get; private set; }
        public bool IsStringTableUpdated { get; private set; }

        public void Loadstringtable(byte[] stBytes, EncodingType encoding)
        {
            _encodingType = encoding;
            _stringTableList = new List<StringItem>();
            _stringTable = new Dictionary<string, StringItem>();

            var num = BitConverter.ToInt32(stBytes, 0);
            for (var i = 0; i < num; i++)
            {
                var num1 = BitConverter.ToInt32(stBytes, i * 4 + 4);
                var num2 = BitConverter.ToInt32(stBytes, i * 4 + 8);
                var len = num2 - num1;
                var item = new StringItem
                {
                    Index = i,
                    StringData = new byte[len]
                };
                Buffer.BlockCopy(stBytes, num1 + 4, item.StringData, 0, len);
                item.StringContent = Encoding.GetEncoding((int) _encodingType).GetString(item.StringData)
                    .TrimEnd(new char[1]);

                _stringTableList.Add(item);

                if (!_stringTable.ContainsKey(item.StringContent))
                    _stringTable.Add(item.StringContent, item);
            }

            IsStringTableUpdated = false;
            NameLabel = GetStringItem("[name]");
            Logger.Success($"StringTable :: stringtable.bin 成功以编码{_encodingType}载入");
        }

        public string GetStringItem(int index)
        {
            if (index < 0 || index >= _stringTableList.Count)
                return $"{lang.StringError}::{index}";
            return _stringTableList[index].StringContent;
        }

        public int GetStringItem(string str)
        {
            return _stringTable.TryGetValue(str, out var item) ? item.Index : -1;
        }

        public int AddStringItem(string str)
        {
            var item = new StringItem
            {
                Index = _stringTableList.Count,
                StringContent = str,
                StringData = Encoding.GetEncoding((int) _encodingType).GetBytes(str)
            };
            _stringTableList.Add(item);
            _stringTable.Add(item.StringContent, item);
            IsStringTableUpdated = true;
            return item.Index;
        }

        public void FindStringItem(HashSet<int> list, string keyword, bool startMatch, bool useLike, Regex regex)
        {
            var keyWord2 = ChineseHelper.ToTraditional(keyword);
            var tmp = new List<int>();
            if (regex != null)
                tmp.AddRange(from item in _stringTableList where regex.IsMatch(item.StringContent) select item.Index);
            else if (useLike)
                tmp.AddRange(from item in _stringTableList
                    where LikeOperator.LikeString(item.StringContent, keyword, CompareMethod.Binary) ||
                          LikeOperator.LikeString(item.StringContent, keyWord2, CompareMethod.Binary)
                    select item.Index);
            else if (startMatch)
                tmp.AddRange(from item in _stringTableList
                    where item.StringContent.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) == 0 ||
                          item.StringContent.IndexOf(keyWord2, StringComparison.OrdinalIgnoreCase) == 0
                    select item.Index);
            else
                tmp.AddRange(from item in _stringTableList
                    where item.StringContent.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                          item.StringContent.IndexOf(keyWord2, StringComparison.OrdinalIgnoreCase) >= 0
                    select item.Index);

            foreach (var item in tmp)
                list.Add(item);
        }

        public byte[] CreateStringTable()
        {
            if (GetStringItem(COMMENT) < 0) AddStringItem(COMMENT);
            var length = 0;
            var count = _stringTableList.Count * 4 + 4; //计算偏移字典大小
            var stringtableStream = new MemoryStream();

            stringtableStream.Write(BitConverter.GetBytes((uint) _stringTableList.Count), 0, 4);

            //写入各个字符串的偏移
            for (var i = 0; i < _stringTableList.Count; i++)
            {
                stringtableStream.Write(BitConverter.GetBytes((uint) (count + length)), 0, 4);
                length = length + _stringTableList[i].StringData.Length;
            }

            //最后写入一个偏移，用于最后一个字符串的读取
            stringtableStream.Write(BitConverter.GetBytes((uint) (count + length)), 0, 4);
            //写入字符串内容
            for (var i = 0; i < _stringTableList.Count; i++)
                stringtableStream.Write(_stringTableList[i].StringData, 0, _stringTableList[i].StringData.Length);

            return stringtableStream.ToArray();
        }

        private struct StringItem
        {
            public int Index;
            public byte[] StringData;
            public string StringContent;
        }
    }
}