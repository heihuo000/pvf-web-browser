using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using pvfUtility.Helper;
using pvfUtility.Service;
using static pvfUtility.Action.AppCore;
#if DEBUG

#endif

namespace pvfUtility.Model.PvfOperation
{
    internal partial class PvfPack
    {
        private readonly byte[] _endingcontent =
        {
            0, 84, 104, 105, 115, 32, 112, 118, 102, 32, 80, 97, 99, 107, 32, 119, 97, 115, 32, 99, 114, 101, 97, 116,
            101, 100, 32, 98, 121, 32, 112, 118, 102, 85, 116, 105, 108, 105, 116, 121, 46
        };

        public Task<bool> SavePvfPack(string path, bool isFastMode, IProgress<int> progress)
        {
            lock (this)
            {
                // var stopwatch = new Stopwatch();
                // stopwatch.Start();
                Stream stream;
                try
                {
                    stream = File.Create(path);
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                    return Task.FromResult(false);
                }

                if (Strtable.IsStringTableUpdated)
                    GetFile("stringtable.bin")?.WriteFileData(Strtable.CreateStringTable());
                var fileList = FileList.Values.OrderBy(x => x.FileNameBytesChecksum).ToList();
                var fileCount = fileList.Count;
                using (var bWriter = new BinaryWriter(stream))
                {
                    bWriter.Write(BitConverter.GetBytes(_guidLen), 0, 4);
                    bWriter.Write(Guid, 0, _guidLen);
                    bWriter.Write(BitConverter.GetBytes(FileVersion), 0, 4);
                    var fileTreeBytes = CreateNewFileTree(fileList, progress);
                    _fileTreeChecksum =
                        PvfAlgorithmHelper.CreateBuffKey(fileTreeBytes, _fileTreeLength, (uint) FileList.Count);
                    bWriter.Write(BitConverter.GetBytes(_fileTreeLength), 0, 4);
                    bWriter.Write(BitConverter.GetBytes(_fileTreeChecksum), 0, 4);
                    bWriter.Write(BitConverter.GetBytes(FileList.Count), 0, 4);
                    bWriter.Write(PvfAlgorithmHelper.EncryptionPvf(fileTreeBytes, _fileTreeLength,
                        _fileTreeChecksum), 0, _fileTreeLength);

                    var nValue = 0;
                    foreach (var file in fileList)
                    {
                        var trueLen = file.GetBlockLength();
                        if (trueLen > 0)
                            // if (!isFastMode || file.IsUpdated)
                            bWriter.Write(PvfAlgorithmHelper.EncryptionPvf(file.Data, trueLen,
                                file.Checksum), 0, trueLen);
                        // else
                        //     bWriter.Write(_sourceFile, file.Offset, trueLen);

                        if (nValue % 512 == 0)
                            progress.Report(GetProgressNum(fileCount + nValue, fileCount * 2));
                        nValue++;
                    }

                    bWriter.Write(_endingcontent);
                    bWriter.Flush();
                }
                // stopwatch.Stop();
                // Logger.Info(stopwatch.ElapsedMilliseconds.ToString());

                return Task.FromResult(true);
            }
        }

        private byte[] CreateNewFileTree(IEnumerable<PvfFile> fileList, IProgress<int> progress)
        {
            _fileTreeLength =
                (FileList.Aggregate(0, (current, fileObj) => current + fileObj.Value.FileNameLen + 20) + 3) & -4;
            var nValue = 0;
            var fileDataOffset = 0;
            using (var fileTreeStream = new MemoryStream(_fileTreeLength))
            {
                fileTreeStream.SetLength(_fileTreeLength);
                foreach (var fileObj in fileList)
                {
                    var length = fileObj.FileNameLen;
                    var truelen = fileObj.GetBlockLength();
                    fileTreeStream.Write(BitConverter.GetBytes(fileObj.FileNameBytesChecksum), 0, 4);
                    fileTreeStream.Write(BitConverter.GetBytes((uint) length), 0, 4);
                    fileTreeStream.Write(fileObj.FileNameBytes, 0, length);
                    fileTreeStream.Write(BitConverter.GetBytes((uint) fileObj.DataLen), 0, 4);
                    fileTreeStream.Write(BitConverter.GetBytes(fileObj.Checksum), 0, 4);
                    fileTreeStream.Write(BitConverter.GetBytes((uint) fileDataOffset), 0, 4);
                    fileDataOffset += truelen;
                    if (nValue % 1024 == 0)
                        progress.Report(GetProgressNum(nValue, FileList.Count * 2));
                    nValue++;
                }

                return fileTreeStream.ToArray();
            }
        }

        private bool IsEditbyPu(Stream stream)
        {
            var len = _endingcontent.Length;
            stream.Seek(stream.Length - len, SeekOrigin.Begin);
            for (var i = 0; i < len; i++)
                if (_endingcontent[i] != stream.ReadByte())
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    return false;
                }

            stream.Seek(0, SeekOrigin.Begin);
            return true;
        }

        /// <summary>
        ///     打开封包
        /// </summary>
        /// <param name="path"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public Task<bool> OpenPvfPack(string path, IProgress<int> progress)
        {
            lock (this)
            {
                try
                {
                    PvfPackFilePath = path;
                    using (var reader = new BinaryReader(File.OpenRead(path)))
                    {
                        var diffLst = new HashSet<PvfFile>();
                        var isEditedbyPu = IsEditbyPu(reader.BaseStream);

                        _guidLen = reader.ReadInt32();
                        Guid = reader.ReadBytes(_guidLen);
                        FileVersion = reader.ReadInt32();
                        _fileTreeLength = reader.ReadInt32();
                        _fileTreeChecksum = reader.ReadUInt32();

                        var fileCount = reader.ReadInt32();
                        FileList = new Dictionary<string, PvfFile>(fileCount);
                        var fileTreeReader = new BinaryReader(new MemoryStream(
                            PvfAlgorithmHelper.DecryptionPvf(reader.ReadBytes(_fileTreeLength), _fileTreeLength,
                                _fileTreeChecksum)
                        ));

                        for (var i = 0; i < fileCount; i++)
                        {
                            var fileNameChecksum = fileTreeReader.ReadUInt32();
                            var fileNameBytes = fileTreeReader.ReadBytes(fileTreeReader.ReadInt32());

                            var dataLen = fileTreeReader.ReadInt32();
                            var checksum = fileTreeReader.ReadUInt32();
                            var offset = fileTreeReader.ReadInt32();

                            var item = new PvfFile(fileNameChecksum, fileNameBytes, dataLen, checksum, offset);

                            if (isEditedbyPu || !FileList.ContainsKey(item.FileName))
                            {
                                FileList.Add(item.FileName, item);
                            }
                            else
                            {
                                var str = item.FileName;
                                while (FileList.ContainsKey(str))
                                    str += "(diff)";
                                FileList.Add(str, item);
                                diffLst.Add(item);
                            }

                            if (i % 512 == 0)
                                progress.Report(GetProgressNum(i, fileCount));
                        }

                        var baseOffset = reader.BaseStream.Position;

                        foreach (var item in FileList.Where(item => item.Value.DataLen > 0))
                        {
                            item.Value.Offset += (int) baseOffset;
                            reader.BaseStream.Seek(item.Value.Offset, SeekOrigin.Begin);
                            item.Value.InitFile(reader.ReadBytes(item.Value.GetBlockLength()));
                            if (!isEditedbyPu && diffLst.Contains(item.Value))
                                item.Value.Rename(item.Key);
                        }
                    }

                    Task.Run(() =>
                    {
                        Strtable.Loadstringtable(GetFile("stringtable.bin").Data, OverAllEncodingType);
                        Strview.InitStringData(GetFile(Config.Instance.StringLstFileName), this, OverAllEncodingType);
                        ListFileTable.Init(this);
                    });

                    return Task.FromResult(true);
                }
                catch
                {
                    return Task.FromResult(false);
                }
            }
        }
    }
}