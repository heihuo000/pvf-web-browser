using System;
using System.Collections.Generic;
using System.Linq;
using static pvfUtility.Helper.PathsHelper;

namespace pvfUtility.Model.PvfOperation
{
    internal class PackEventArgs : EventArgs
    {
        public PvfPack Pack;

        public PackEventArgs(PvfPack pack)
        {
            Pack = pack;
        }
    }

    internal partial class PvfPack
    {
        private uint _fileTreeChecksum;

        private int _fileTreeLength;

        //private byte[] _sourceFile;
        private int _guidLen;

        private EncodingType _overAllEncodingType;
        public int FileVersion;
        public byte[] Guid;

        //public bool Working { get; set; }
        public PvfPack(EncodingType encodingType = EncodingType.TW)
        {
            OverAllEncodingType = encodingType;
            Strtable = new Stringtable();
            Strview = new StringView();
            ListFileTable = new ListFileTable();
        }

        /// <summary>
        ///     当前打开的pvf的路径
        /// </summary>
        public string PvfPackFilePath { get; set; }

        public Dictionary<string, PvfFile> FileList { get; private set; }
        public Stringtable Strtable { get; }
        public StringView Strview { get; }

        public ListFileTable ListFileTable { get; }

        /// <summary>
        ///     封包内置的默认全局编码
        /// </summary>
        public EncodingType OverAllEncodingType
        {
            get => _overAllEncodingType;
            set
            {
                _overAllEncodingType = value;
                if (Strtable == null)
                    return;
                if (Strtable.IsStringTableUpdated) //update string table if updated
                    GetFile("stringtable.bin")?.WriteFileData(Strtable.CreateStringTable());
                Strtable?.Loadstringtable(GetFile("stringtable.bin").Data, value);
                Strview?.InitStringData(GetFile("n_string.lst"), this, value);
            }
        }

        public string[] GetFiles(string path)
        {
            var path1 = PathFix(path);
            return (from item in FileList
                where IsPathMatch(item.Key, path1)
                select item.Key).ToArray();
        }

        public PvfFile[] GetFileObjs(string path)
        {
            var path1 = PathFix(path);
            return (from item in FileList
                where IsPathMatch(item.Key, path1)
                select item.Value).ToArray();
        }

        public PvfFile GetFile(string filePath)
        {
            return FileList.TryGetValue(filePath.Replace('\\', '/').ToLower(), out var file) ? file : null;
        }
    }
}