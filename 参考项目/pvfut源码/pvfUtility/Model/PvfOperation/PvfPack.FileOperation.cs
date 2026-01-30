using System.Collections.Generic;
using System.IO;
using System.Text;
using pvfUtility.Helper.Native;
using pvfUtility.Model.PvfOperation.Encoder;
using pvfUtility.Model.PvfOperation.Praser;
using pvfUtility.Service;
using static pvfUtility.Helper.PathsHelper;

namespace pvfUtility.Model.PvfOperation
{
    internal partial class PvfPack
    {
        /// <summary>
        ///     extract file to stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="file"></param>
        /// <param name="decompileBinaryAni"></param>
        /// <param name="decompileScript"></param>
        /// <returns></returns>
        public bool ExtractFile(Stream stream, PvfFile file, bool decompileBinaryAni, bool decompileScript,
            bool convertConvertSimplifiedChinese)
        {
            if (file == null)
                return false;
            if (file.DataLen <= 0)
                return true;
            if (file.IsBinaryAniFile && decompileBinaryAni)
            {
                var (result, text) = BinaryAniCompiler.DecompileBinaryAni(file);
                if (result)
                {
                    var bytes = Encoding.UTF8.GetBytes(text);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                }

                return result;
            }

            if (file.IsScriptFile && decompileScript)
            {
                // var bytes = Encoding.UTF8.GetBytes(Config.Instance.UseCompatibleDecompiler
                //     ? new ScriptFileCompiler(this).Decompile(file)
                //     : new ScriptFileParser(file, this).PraseText());
                // stream.Write(bytes, 0, bytes.Length);
                // stream.Seek(0, SeekOrigin.Begin);
                var text = Config.Instance.UseCompatibleDecompiler
                    ? new ScriptFileCompiler(this).Decompile(file)
                    : new ScriptFileParser(file, this).PraseText();
                if (convertConvertSimplifiedChinese)
                    text = ChineseHelper.ToSimplified(text);

                var bytes = Encoding.UTF8.GetBytes(text);
                stream.Write(bytes, 0, bytes.Length);
                return true;
            }

            if (PackService.CurrentEncodingType == EncodingType.TW && convertConvertSimplifiedChinese)
            {
                var text = Encoding.GetEncoding((int) EncodingType.TW).GetString(file.Data);
                text = ChineseHelper.ToSimplified(text);
                var bytes = Encoding.UTF8.GetBytes(text);
                stream.Write(bytes, 0, bytes.Length);
            }
            else
            {
                stream.Write(file.Data, 0, file.DataLen);
            }

            // stream.Write(file.Data, 0, file.DataLen);
            // stream.Seek(0, SeekOrigin.Begin);
            return true;
        }

        /// <summary>
        ///     update file form stream
        /// </summary>
        /// <param name="file"></param>
        /// <param name="stream"></param>
        /// <param name="compileBinaryAni"></param>
        /// <param name="compileScript"></param>
        /// <param name="useAppendMode"></param>
        /// <param name="convertChinese"></param>
        /// <returns></returns>
        public (bool success, ErrorListItem[] error) UpdateFile(PvfFile file, Stream stream, bool compileScript,
            bool compileBinaryAni, bool convertChinese)
        {
            if (stream == null || stream.Length <= 0L) return (true, null); // 空文件返回

            var buffer = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin); //7z 导入问题修复
            stream.Read(buffer, 0, buffer.Length);
            var str = Encoding.UTF8.GetString(buffer).TrimEnd(new char[1]);
            if (convertChinese)
            {
                str = ChineseHelper.ToTraditional(str);
                buffer = Encoding.UTF8.GetBytes(str);
            }

            //ani导入处理
            if (compileBinaryAni && file.IsBinaryAniFile && str.Length > 10 &&
                str.Substring(0, 9).ToLower() == "#pvf_file")
            {
                var (result, data, error) = BinaryAniCompiler.CompileBinaryAni(str, file.FileName);
                if (!result) return error == null ? (false, null) : (false, new[] {error});

                file.WriteFileData(data);
                return (true, null);
            }

            //
            // if (compileScript && useAppendMode && file.DataLen != 0 && str.Length > 14 &&
            //     str.Substring(0, 13).ToLower() == "#pvf_file_add")
            // {
            //     var newTextData = new ScriptFileCompiler(this).EncryptScriptText(str, false);
            //     var newObjData = new byte[file.DataLen + newTextData.Length];
            //     Buffer.BlockCopy(file.Data, 0, newObjData, 0, file.DataLen);
            //     Buffer.BlockCopy(newTextData, 0, newObjData, file.DataLen, newTextData.Length);
            //     file.WriteFileData(newObjData);
            // }
            if (compileScript && str.Length > 10 && str.Substring(0, 9).ToLower() == "#pvf_file")
            {
                var (data, error) = new ScriptFileCompiler(this).Compile(file, str);
                if (data == null)
                    return (false, error);
                file.WriteFileData(data);
                return (true, null);
            }
            // else if (useAppendMode && file.DataLen != 0 && str.Length > 5 && str.Substring(0, 4).ToLower() == "#add")
            // {
            //     var n = new byte[file.DataLen + buffer.Length - 4];
            //     Buffer.BlockCopy(file.Data, 0, n, 0, file.DataLen);
            //     Buffer.BlockCopy(buffer, 4, n, file.DataLen, buffer.Length - 4);
            //     file.WriteFileData(n);
            // }

            file.WriteFileData(buffer);
            return (true, null);
        }

        /// <summary>
        ///     移动目录下的文件到新的目录，返回新的文件名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newPath"></param>
        /// <param name="cut"></param>
        public IEnumerable<string> MoveFile(string path, string newPath, bool cut)
        {
            lock (FileList)
            {
                if (FileList.ContainsKey(path))
                {
                    string name;
                    newPath = PathFix(newPath);
                    if (cut)
                    {
                        var obj = GetFile(path);
                        if (obj == null) return null;
                        var newName = newPath + obj.ShortName;
                        while (GetFile(newName) != null) newName += "(copy)";
                        obj.Rename(newName);
                        FileList.Remove(path);
                        FileList.Add(newName, obj);
                        name = newName;
                    }
                    else
                    {
                        var obj = GetFile(path);
                        if (obj == null) return null;
                        var newName = newPath + obj.ShortName;
                        var newFile = new PvfFile();
                        while (GetFile(newName) != null) newName += "(copy)";
                        newFile.InitNewCopyFile(obj.Data, newName);
                        FileList.Add(newName, newFile);
                        name = newName;
                    }

                    var lst = new List<string> {name};
                    return lst;
                }
                else
                {
                    newPath = PathFix(newPath);
                    int delPath;
                    if (path.Contains("/"))
                        delPath = path.LastIndexOf('/') + 1;
                    else
                        delPath = 0;
                    path = PathFix(path);
                    var lst = new List<string>();
                    if (cut)
                        foreach (var x in GetFileObjs(path))
                        {
                            var newName = string.Concat(newPath, x.FileName.Remove(0, delPath));
                            while (GetFile(newName) != null) newName += "(copy)";
                            FileList.Remove(x.FileName);
                            x.Rename(newName);
                            lst.Add(newName);
                            FileList.Add(newName, x);
                        }
                    else
                        foreach (var x in GetFileObjs(path))
                        {
                            var newName = string.Concat(newPath, x.FileName.Remove(0, delPath));
                            while (GetFile(newName) != null) newName += "(copy)";
                            var newFile = new PvfFile();
                            newFile.InitNewCopyFile(x.Data, newName);
                            lst.Add(newFile.FileName);
                            FileList.Add(newName, newFile);
                        }

                    return lst;
                }
            }
        }
    }
}