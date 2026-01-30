using System;
using System.IO;
using pvfUtility.Helper;
using pvfUtility.Model.PvfOperation.Encoder;

namespace pvfUtility.Model.PvfOperation
{
    internal partial class PvfPack
    {
        /// <summary>
        ///     获取脚本文件名称
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GetName(PvfFile file)
        {
            if (!file.IsScriptFile) return null;
            for (var i = 2; i < file.DataLen - 9; i += 5)
            {
                if (file.Data[i] != 5 || BitConverter.ToInt32(file.Data, i + 1) != Strtable.NameLabel) continue;
                if (i < file.DataLen - 14 && file.Data[i + 5] == 9 && file.Data[i + 10] == 10)
                    return Strview.GetStrText(file.Data[i + 6],
                        Strtable.GetStringItem(BitConverter.ToInt32(file.Data, i + 11)));
                if (file.Data[i + 5] == 7)
                    return Strtable.GetStringItem(BitConverter.ToInt32(file.Data, i + 6));
            }

            return null;
        }

        public bool DeleteScriptContent(PvfFile file, byte[] databytes)
        {
            var len = databytes.Length;
            if (!file.IsScriptFile) return false;
            if (file.DataLen < len) return false;
            for (var i = 2; i < file.DataLen; i += 5)
            {
                if (file.Data[i] != databytes[0] ||
                    BitConverter.ToInt32(file.Data, i + 1) != BitConverter.ToInt32(databytes, 1))
                    continue;
                if (i + len > file.DataLen) return false;
                var temp = new byte[len];
                Buffer.BlockCopy(file.Data, i, temp, 0, len);
                if (!DataHelper.BytesEquals(temp, databytes)) continue;
                temp = null;
                var newBytes = new byte[file.DataLen - len];
                Buffer.BlockCopy(file.Data, 0, newBytes, 0, i);
                if (i < file.DataLen - len)
                    Buffer.BlockCopy(file.Data, i + len, newBytes, i, file.DataLen - len - i);
                file.WriteFileData(newBytes);
                return true;
            }

            return false;
        }

        public (bool success, int times) ReplaceScriptContent(PvfFile file, byte[] findbytes, byte[] replaceBytes)
        {
            var len = findbytes.Length;
            var len2 = replaceBytes.Length;
            if (!file.IsScriptFile)
                return (false, 0);
            if (file.DataLen < len)
                return (false, 0);

            var times = 0;

            var stream = new MemoryStream();
            var reader = new BinaryReader(new MemoryStream(file.Data));
            stream.Write(file.Data, 0, 2);
            for (var i = 2; i < file.DataLen; i += 5)
            {
                if (file.Data[i] != findbytes[0] ||
                    BitConverter.ToInt32(file.Data, i + 1) != BitConverter.ToInt32(findbytes, 1))
                {
                    //不符合的
                    stream.Write(file.Data, i, 5);
                    continue;
                }

                if (i + len > file.DataLen)
                {
                    //过长,写入剩余内容
                    stream.Write(file.Data, i, file.DataLen - i);
                    break;
                }

                var temp = new byte[len];
                Buffer.BlockCopy(file.Data, i, temp, 0, len);
                if (!DataHelper.BytesEquals(temp, findbytes))
                {
                    stream.Write(file.Data, i, 5);
                    continue;
                }

                stream.Write(replaceBytes, 0, len2);
                i += len - 5;
                times++;
                // var newBytes = new byte[file.DataLen - len + len2];
                // Buffer.BlockCopy(file.Data, 0, newBytes, 0, i);
                // Buffer.BlockCopy(replaceBytes, 0, newBytes, i, len2);
                // if (i < file.DataLen - len)
                // {
                //     Buffer.BlockCopy(file.Data, i + len, newBytes, i + len2, file.DataLen - len - i);
                // }
            }

            if (times > 0)
            {
                file.WriteFileData(stream.ToArray());
                return (true, times);
            }

            return (false, 0);
        }

        public bool DeleteSectionContent(PvfFile file, int sectionStart, int sectionEnd)
        {
            if (!file.IsScriptFile) return false;
            for (var i = 2; i < file.DataLen - 9; i += 5)
            {
                if (file.Data[i] != 5 || BitConverter.ToInt32(file.Data, i + 1) != sectionStart) continue;
                for (var j = i + 5; j < file.DataLen - 4; j += 5)
                {
                    if (sectionEnd != -1 && file.Data[j] == 5 && BitConverter.ToInt32(file.Data, j + 1) == sectionEnd)
                    {
                        var data = new byte[i + file.DataLen - (j + 5)];

                        Buffer.BlockCopy(file.Data, 0, data, 0, i);
                        Buffer.BlockCopy(file.Data, j + 5, data, i, file.DataLen - (j + 5));
                        file.WriteFileData(data);
                        return true;
                    }

                    if (sectionEnd == -1 && file.Data[j] == 5)
                    {
                        var data = new byte[i + file.DataLen - j];
                        Buffer.BlockCopy(file.Data, 0, data, 0, i);
                        Buffer.BlockCopy(file.Data, j, data, i, file.DataLen - j);
                        file.WriteFileData(data);
                        return true;
                    }

                    if (j == file.DataLen - 5)
                    {
                        var data = new byte[i];
                        Buffer.BlockCopy(file.Data, 0, data, 0, i);
                        file.WriteFileData(data);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool AddScriptContent(PvfFile file, byte[] newDataBytes)
        {
            if (!file.IsScriptFile) return false;
            var newObjData = new byte[file.DataLen + newDataBytes.Length];
            Buffer.BlockCopy(file.Data, 0, newObjData, 0, file.DataLen);
            Buffer.BlockCopy(newDataBytes, 0, newObjData, file.DataLen, newDataBytes.Length);
            file.WriteFileData(newObjData);
            return true;
        }

        public string GetLabelData(PvfFile obj, int label, int labelStop)
        {
            if (!obj.IsScriptFile) return null;
            for (var i = 2; i < obj.DataLen - 9; i += 5)
            {
                if (obj.Data[i] != 5 || BitConverter.ToInt32(obj.Data, i + 1) != label) continue;
                for (var j = i + 5; j < obj.DataLen - 4; j += 5)
                {
                    if (labelStop != -1 && obj.Data[j] == 5 && BitConverter.ToInt32(obj.Data, j + 1) == labelStop)
                    {
                        var data = new byte[j - (i + 5)];
                        Buffer.BlockCopy(obj.Data, i + 5, data, 0, j - (i + 5));
                        return new ScriptFileCompiler(this).Decompile(data);
                    }

                    if (labelStop == -1 && obj.Data[j] == 5)
                    {
                        var data = new byte[j - (i + 5)];
                        Buffer.BlockCopy(obj.Data, i + 5, data, 0, j - (i + 5));
                        return new ScriptFileCompiler(this).Decompile(data);
                    }

                    if (j != obj.DataLen - 5) continue;
                    {
                        var data = new byte[obj.DataLen - (i + 5)];
                        Buffer.BlockCopy(obj.Data, i + 5, data, 0, obj.DataLen - (i + 5));
                        return new ScriptFileCompiler(this).Decompile(data);
                    }
                }
            }

            return null;
        }

        public int GetLabelDataStr(PvfFile obj, int label)
        {
            if (!obj.IsScriptFile) return -1;
            for (var i = 2; i < obj.DataLen - 9; i += 5)
            {
                if (obj.Data[i] != 5 || BitConverter.ToInt32(obj.Data, i + 1) != label ||
                    obj.Data[i + 5] != 7 && obj.Data[i + 5] != 10) continue;
                var num2 = BitConverter.ToInt32(obj.Data, i + 5 + 1);
                return num2;
            }

            return -1;
        }

        public int GetLabelDataInt(PvfFile obj, int label)
        {
            if (!obj.IsScriptFile) return -1;
            for (var i = 2; i < obj.DataLen - 9; i += 5)
            {
                if (obj.Data[i] != 5 || BitConverter.ToInt32(obj.Data, i + 1) != label ||
                    obj.Data[i + 5] != 2) continue;
                return BitConverter.ToInt32(obj.Data, i + 5 + 1);
            }

            return -1;
        }

        public bool SetLabelDataInt(PvfFile obj, int label, int data)
        {
            if (!obj.IsScriptFile) return false;
            for (var i = 2; i < obj.DataLen - 9; i += 5)
            {
                if (obj.Data[i] != 5 || BitConverter.ToInt32(obj.Data, i + 1) != label ||
                    obj.Data[i + 5] != 2 && obj.Data[i + 5] != 4) continue;
                var tmp = new byte[obj.DataLen];
                Buffer.BlockCopy(obj.Data, 0, tmp, 0, obj.DataLen);
                Buffer.BlockCopy(BitConverter.GetBytes(data), 0, tmp, i + 5 + 1, 4);
                obj.WriteFileData(tmp);
                return true;
            }

            return false;
        }

        public bool SetLabelDataStr(PvfFile obj, int label, string data)
        {
            if (!obj.IsScriptFile) return false;
            for (var i = 2; i < obj.DataLen - 9; i += 5)
            {
                if (obj.Data[i] != 5 || BitConverter.ToInt32(obj.Data, i + 1) != label ||
                    obj.Data[i + 5] != 7 && obj.Data[i + 5] != 10) continue;
                var num1 = Strtable.GetStringItem(data);
                var data2 = num1 != -1 ? num1 : Strtable.AddStringItem(data);
                var tmp = new byte[obj.DataLen];
                Buffer.BlockCopy(obj.Data, 0, tmp, 0, obj.DataLen);
                Buffer.BlockCopy(BitConverter.GetBytes((uint) data2), 0, tmp, i + 5 + 1, 4);
                obj.WriteFileData(tmp);
                return true;
            }

            return false;
        }

        //     public bool SetLabelData(PvfFile obj, int label, int labelStop, string text)
        //     {
        //         if (!obj.IsScriptFile) return false;
        //         for (var i = 2; i < obj.DataLen - 9; i += 5)
        //         {
        //             if (obj.Data[i] != 5 || BitConverter.ToInt32(obj.Data, i + 1) != label) continue;
        //             for (var j = i + 5; j < obj.DataLen - 4; j += 5)
        //             {
        //                 if (labelStop != -1 && obj.Data[j] == 5 && BitConverter.ToInt32(obj.Data, j + 1) == labelStop)
        //                 {
        //                     var newdata = new ScriptFileCompiler(this).EncryptScriptText(text, false);
        //                     var data = new byte[i + 5 + obj.DataLen - j + newdata.Length];
        //                     Buffer.BlockCopy(obj.Data, 0, data, 0, i + 5);
        //                     Buffer.BlockCopy(newdata, 0, data, i + 5, newdata.Length);
        //                     Buffer.BlockCopy(obj.Data, j, data, i + newdata.Length + 5, obj.DataLen - j);
        //                     obj.WriteFileData(data);
        //                     return true;
        //                 }
        //                 if (labelStop == -1 && obj.Data[j] == 5)
        //                 {
        //                     var newdata = new ScriptFileCompiler(this).EncryptScriptText(text, false);
        //                     var data = new byte[i + 5 + obj.DataLen - j + newdata.Length];
        //                     Buffer.BlockCopy(obj.Data, 0, data, 0, i + 5);
        //                     Buffer.BlockCopy(newdata, 0, data, i + 5, newdata.Length);
        //                     Buffer.BlockCopy(obj.Data, j, data, i + 5 + newdata.Length, obj.DataLen - j);
        //                     obj.WriteFileData(data);
        //                     return true;
        //                 }
        //                 if (j != obj.DataLen - 5) continue;
        //                 {
        //                     var newdata = new ScriptFileCompiler(this).EncryptScriptText(text, false);
        //                     var data = new byte[i + 5 + newdata.Length];
        //                     Buffer.BlockCopy(obj.Data, 0, data, 0, i + 5);
        //                     Buffer.BlockCopy(newdata, 0, data, i + 5, newdata.Length);
        //                     obj.WriteFileData(data);
        //                     return true;
        //                 }
        //
        //             }
        //         }
        //         return false;
        //     }
        //
    }
}