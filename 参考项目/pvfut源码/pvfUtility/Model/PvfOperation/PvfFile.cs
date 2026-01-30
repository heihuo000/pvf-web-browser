using System;
using System.Text;
using pvfUtility.Helper;

namespace pvfUtility.Model.PvfOperation
{
    [Serializable]
    internal class PvfFile
    {
        public PvfFile()
        {
        }

        /// <summary>
        ///     Init File by name
        /// </summary>
        /// <param name="fileName"></param>
        public PvfFile(string fileName)
        {
            FileName = fileName;
            FileNameBytes = Encoding.GetEncoding(0x3b5).GetBytes(FileName);
            FileNameBytesChecksum = DataHelper.GetFileNameHashCode(FileNameBytes);
            Checksum = FileNameBytesChecksum;
            IsUpdated = true;
        }

        /// <summary>
        ///     初始化文件
        /// </summary>
        /// <param name="fileNameChecksum"></param>
        /// <param name="fileNameBytes"></param>
        /// <param name="dataLen"></param>
        /// <param name="checksum"></param>
        /// <param name="offset"></param>
        public PvfFile(uint fileNameChecksum, byte[] fileNameBytes, int dataLen, uint checksum, int offset)
        {
            FileNameBytesChecksum = fileNameChecksum;
            FileNameBytes = fileNameBytes;
            DataLen = dataLen;
            Checksum = checksum;
            Offset = offset;
        }

        public byte[] FileNameBytes { get; set; }
        public int FileNameLen => FileNameBytes.Length;
        public byte[] Data { get; private set; }
        public int DataLen { get; set; }
        public int Offset { get; set; }
        public uint Checksum { get; set; }
        public uint FileNameBytesChecksum { get; set; }

        /// <summary>
        ///     文件更新情况
        /// </summary>
        public bool IsUpdated { get; private set; }

        /// <summary>
        ///     文件名
        /// </summary>
        public string FileName
        {
            get => Encoding.GetEncoding(0x3b5).GetString(FileNameBytes)
                .TrimEnd(new char[1]); // Replace('\\', '/').ToLower()
            set
            {
                FileNameBytes = Encoding.GetEncoding(0x3b5).GetBytes(value.Replace('\\', '/').ToLower());
                FileNameBytesChecksum = DataHelper.GetFileNameHashCode(FileNameBytes);
                if (DataLen > 0)
                    Checksum = PvfAlgorithmHelper.CreateBuffKey(Data, GetBlockLength(), FileNameBytesChecksum);
                IsUpdated = true;
            }
        }

        /// <summary>
        ///     取文件短名
        /// </summary>
        public string ShortName
        {
            get
            {
                var fileName = FileName;
                return fileName.LastIndexOf('/') > 0 ? fileName.Substring(fileName.LastIndexOf('/') + 1) : fileName;
            }
        }

        /// <summary>
        ///     取文件路径
        /// </summary>
        public string PathName
        {
            get
            {
                var fileName = FileName;
                return fileName.LastIndexOf('/') > 0 ? fileName.Substring(0, fileName.LastIndexOf('/')) : "";
            }
        }

        /// <summary>
        ///     判断文件是否为脚本文件
        /// </summary>
        public bool IsScriptFile => Data?.Length >= 2 && BitConverter.ToUInt16(Data, 0) == 0xd0b0;

        /// <summary>
        ///     判断文件是否为二进制ANI文件
        /// </summary>
        public bool IsBinaryAniFile => !IsScriptFile && FileName.EndsWith(".ani");

        /// <summary>
        ///     判断文件是否为NUT
        /// </summary>
        public bool IsNutFile => !IsScriptFile && FileName.EndsWith(".nut");

        /// <summary>
        ///     判断文件是否为LST
        /// </summary>
        public bool IsListFile => IsScriptFile && FileName.EndsWith(".lst");

        public int GetBlockLength()
        {
            return (DataLen + 3) & -4;
        }

        public void WriteFileData(byte[] fileData)
        {
            DataLen = fileData.Length;
            if (DataLen <= 0) return;
            Data = new byte[GetBlockLength()];
            Buffer.BlockCopy(fileData, 0, Data, 0, DataLen);
            Checksum = PvfAlgorithmHelper.CreateBuffKey(Data, GetBlockLength(), FileNameBytesChecksum);
            IsUpdated = true;
        }

        public void InitFile(byte[] bytes)
        {
            // if (DataLen <= 0) 
            //     return;
            Data = bytes; //new byte[TrueLen];
            //Buffer.BlockCopy(bytes, Offset, Data, 0, TrueLen);
            Data = PvfAlgorithmHelper.DecryptionPvf(Data, GetBlockLength(), Checksum);
            for (var x = 0; x < GetBlockLength() - DataLen; x++)
                Data[DataLen + x] = 0;
        }

        /// <summary>
        ///     create new file from copy
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="name"></param>
        public void InitNewCopyFile(byte[] bytes, string name)
        {
            Rename(name);
            if (bytes != null)
                WriteFileData(bytes);
        }

        public void Rename(string newFileName)
        {
            FileName = newFileName.Replace('\\', '/').ToLower();
            ;
            FileNameBytes = Encoding.GetEncoding(0x3b5).GetBytes(FileName);
            FileNameBytesChecksum = DataHelper.GetFileNameHashCode(FileNameBytes);
            if (DataLen > 0)
                Checksum = PvfAlgorithmHelper.CreateBuffKey(Data, GetBlockLength(), FileNameBytesChecksum);
            IsUpdated = true;
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}