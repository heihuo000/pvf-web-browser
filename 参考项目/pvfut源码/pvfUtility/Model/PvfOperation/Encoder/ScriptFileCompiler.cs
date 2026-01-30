using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using pvfUtility.Properties;
using pvfUtility.Service;
using static pvfUtility.Helper.DataHelper;

namespace pvfUtility.Model.PvfOperation.Encoder
{
    internal class ScriptFileCompiler
    {
        private readonly PvfPack _pack;

        public ScriptFileCompiler(PvfPack pack)
        {
            _pack = pack;
        }

        #region 反编译脚本文件

        /// <summary>
        ///     反编译脚本文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string Decompile(PvfFile file)
        {
            try
            {
                var main = new StringBuilder("#PVF_File\r\n");
                Decompile(file.Data, file.DataLen, file.FileName, main);
                return main.ToString();
            }
            catch (Exception ex)
            {
                return lang.ReadScriptException + "\r\n" + ex.Message;
            }
        }

        /// <summary>
        ///     反编译部分脚本文件内容
        /// </summary>
        /// <param name="scriptdata"></param>
        /// <returns></returns>
        public string Decompile(byte[] scriptdata)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                Decompile(scriptdata, scriptdata.Length, "", stringBuilder);
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                return lang.ReadScriptException + "\r\n" + ex.Message;
            }
        }

        private void Decompile(byte[] bytes, int len, string fileName, StringBuilder main)
        {
            if (bytes != null && len >= 7)
                for (var index = 2; index < len - 4; index += 5)
                {
                    var type = bytes[index];
                    var data = BitConverter.ToInt32(bytes, index + 1);
                    switch (type)
                    {
                        case 5:
                        {
                            main.Append($"\r\n{_pack.Strtable.GetStringItem(data)}\r\n");
                            continue;
                        }
                        case 10:
                        {
                            var strlistnum = BitConverter.ToInt32(bytes, index - 4);
                            var strname = _pack.Strtable.GetStringItem(data);
                            if (Config.Instance.AutoConvertStringLink)
                                main.Append(string.Concat("`",
                                    _pack.Strview.GetStrText(strlistnum, strname).Replace("\\n", "\r\n"), "`\r\n"));
                            else
                                main.Append(string.Concat("<", strlistnum.ToString(), "::", strname, "`",
                                    _pack.Strview.GetStrText(strlistnum, strname), "`>\r\n"));
                            continue;
                        }
                        case 7:
                            main.Append($"`{_pack.Strtable.GetStringItem(data)}`\r\n");
                            continue;
                        case 6:
                        case 8:
                            main.Append(string.Concat("{", type.ToString(), "=`", _pack.Strtable.GetStringItem(data),
                                "`}\r\n"));
                            continue;
                        case 3:
                            main.Append(string.Concat("{", type.ToString(), "=", data, "}\t"));
                            continue;
                        case 4:
                            main.Append(string.Concat(FormatFloat(BitConverter.ToSingle(bytes, index + 1)), "\t"));
                            continue;
                        case 2:
                            main.Append(string.Concat(data.ToString(), "\t"));
                            continue;
                        case 9:
                            continue;
                        default:
                            //MainPresenter.Instance.View.DockErrorList.AddError(fileName, $"未能识别的类型值：{type}", "");
                            //TODO
                            continue;
                    }
                }

            main.Append("\r\n");
        }

        #endregion

        #region 编译脚本文件

        /// <summary>
        ///     编译脚本文件(出现错误返回null)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="scriptText"></param>
        /// <returns></returns>
        public (byte[], ErrorListItem[]) Compile(PvfFile obj, string scriptText)
        {
            var stream = new MemoryStream();
            stream.WriteByte(176);
            stream.WriteByte(208); //Script File Header
            var result = Compile(obj.FileName, scriptText, false, stream);
            var errorList = new List<ErrorListItem>();
            var guid = Guid.NewGuid();
            foreach (var (input, line) in result)
            {
                var err = new ErrorListItem
                {
                    Guid = guid,
                    FileName = obj.FileName,
                    Description = $"未能识别的数据：{input}",
                    FullText = scriptText,
                    Line = line
                };
                errorList.Add(err);
            }

            if (errorList.Count == 0)
                return (stream.ToArray(), null);

            //Logger.Error($"脚本编译器 :: 编译脚本文件时检测到{errorCount}个错误，查看【错误列表】了解详情。本次操作不会应用到文件中");
            return (null, errorList.ToArray());
        }

        /// <summary>
        ///     编译脚本内容
        /// </summary>
        /// <param name="scriptText"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public (bool success, byte[] data) EncryptScriptText(string scriptText, bool readOnly)
        {
            var stream = new MemoryStream();
            var result = Compile("", scriptText, readOnly, stream);
            if (result.Length > 0)
            {
                Logger.Error($"脚本编译器 :: 编译脚本文件片段时发生{result.Length}个错误,请检查(注意关闭自动字符串链接转换,否则找不到某些字符串)");
                foreach (var (input, line) in result) Logger.Error($"{input} 在{line}行");
            }

            return (result.Length == 0, stream.ToArray());
        }

        /// <summary>
        ///     尝试编译并返回错误数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="scriptText"></param>
        /// <param name="readOnly"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        private (string input, int line)[] Compile(string fileName, string scriptText, bool readOnly, Stream stream)
        {
            //scriptText = new Regex(@"<(.*)::(.*)`.*`>").Replace(scriptText, "\t<$1::$2``>\t");
            scriptText = new Regex(@"<(\d+::.+?)`.+?`>").Replace(scriptText, @"<$1``>"); //Bug Fixed
            var str = scriptText.Split(new[] {"\r\n", "\n"}, StringSplitOptions.None);
            var count = str.Length;
            var errorList = new List<(string input, int line)>();
            var temp = "";
            for (var i = 0; i < count; i++)
            {
                var header = str[i].ToLower();
                if (header == "#pvf_file" || header == "#pvf_file_add" || str[i] == "")
                    continue;
                var lstStrings = str[i].Split(new[] {"\t"}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in lstStrings)
                {
                    temp += item;
                    var (type, data) = readOnly ? CompileItemReadOnly(temp) : CompileItem(temp);
                    switch (type)
                    {
                        case 0:
                            continue;
                        case 10:
                            CompileType10Item(temp, stream, readOnly);
                            temp = "";
                            continue;
                        case 81:
                            temp += "\r\n";
                            continue;
                        case 255:
                            errorList.Add((temp, i + 1));
                            temp = "";
                            continue;
                    }

                    stream.WriteByte(type);
                    stream.Write(data, 0, 4);
                    temp = "";
                }
            }

            return errorList.ToArray();
        }

        #endregion

        #region 编译单个项目

        /// <summary>
        ///     编译单个项目
        /// </summary>
        /// <param name="itemData"></param>
        /// <returns></returns>
        private (byte type, byte[] data) CompileItem(string itemData)
        {
            byte[] data;
            byte type;
            switch (itemData[0])
            {
                case '[' when itemData[itemData.Length - 1] == ']':
                {
                    type = 5;
                    var numIndex = _pack.Strtable.GetStringItem(itemData);
                    data = BitConverter.GetBytes(
                        (uint) (numIndex == -1 ? _pack.Strtable.AddStringItem(itemData) : numIndex));
                    break;
                }
                case '<' when itemData[itemData.Length - 1] == '>':
                    type = 10;
                    data = BitConverter.GetBytes(0);
                    break;
                case '`' when itemData[itemData.Length - 1] == '`':
                {
                    type = 7;
                    var str = GetDataFromFormat(itemData, "`", "`");
                    var num2 = _pack.Strtable.GetStringItem(str);
                    data = BitConverter.GetBytes((uint) (num2 != -1 ? num2 : _pack.Strtable.AddStringItem(str)));
                    break;
                }
                //处理特殊项目
                case '{' when itemData[itemData.Length - 1] == '}':
                {
                    var str1 = GetDataFromFormat(itemData, "{", "=");
                    var str2 = GetDataFromFormat(itemData, "=", "}");
                    byte.TryParse(str1, out type);
                    data = BitConverter.GetBytes(0);
                    if (type == 0)
                        return (type, data);
                    if (str2[0] != '`' || str2[str2.Length - 1] != '`')
                    {
                        var sParse = int.TryParse(str2, out var tmp);
                        data = BitConverter.GetBytes(tmp);
                        if (!sParse) type = 255;
                    }
                    else
                    {
                        var str3 = GetDataFromFormat(str2, "`", "`");
                        var num3 = _pack.Strtable.GetStringItem(str3);
                        data = BitConverter.GetBytes((uint) (num3 != -1 ? num3 : _pack.Strtable.AddStringItem(str3)));
                    }

                    break;
                }
                case '`':
                    type = 81;
                    data = BitConverter.GetBytes(0);
                    break;
                default:
                {
                    if (itemData.IndexOf('.') < 0)
                    {
                        type = 2;
                        var sParse = int.TryParse(itemData, out var tmp); //处理整数
                        data = BitConverter.GetBytes(tmp);
                        if (!sParse) type = 255;
                    }
                    else
                    {
                        type = 4;
                        var sParse = float.TryParse(itemData, out var f);
                        data = BitConverter.GetBytes(f);
                        if (!sParse)
                            type = 255;
                    }

                    break;
                }
            }

            return (type, data);
        }

        /// <summary>
        ///     编译单个项目（只读）
        /// </summary>
        /// <param name="itemData"></param>
        /// <returns></returns>
        private (byte type, byte[] data) CompileItemReadOnly(string itemData)
        {
            byte type = 255;
            var data = BitConverter.GetBytes(0);
            if (string.IsNullOrEmpty(itemData))
                return (type, data);
            switch (itemData[0])
            {
                case '[' when itemData[itemData.Length - 1] == ']':
                {
                    var num1 = _pack.Strtable.GetStringItem(itemData);
                    if (num1 == -1)
                        return (type, data);
                    type = 5;
                    data = BitConverter.GetBytes((uint) num1);
                    break;
                }
                case '<' when itemData[itemData.Length - 1] == '>':
                    type = 10;
                    data = BitConverter.GetBytes(0);
                    break;
                case '`' when itemData[itemData.Length - 1] == '`':
                {
                    var str = GetDataFromFormat(itemData, "`", "`");
                    var num2 = _pack.Strtable.GetStringItem(str);
                    if (num2 == -1)
                        return (type, data);
                    type = 7;
                    data = BitConverter.GetBytes((uint) num2);
                    break;
                }
                //处理特殊项目
                case '{' when itemData[itemData.Length - 1] == '}':
                {
                    var str1 = GetDataFromFormat(itemData, "{", "=");
                    var str2 = GetDataFromFormat(itemData, "=", "}");
                    byte.TryParse(str1, out type);
                    if (type == 0)
                        return (type, data);
                    if (str2[0] != '`' || str2[str2.Length - 1] != '`')
                    {
                        int.TryParse(str2, out var tmp);
                        data = BitConverter.GetBytes(tmp);
                    }
                    else
                    {
                        var str3 = GetDataFromFormat(str2, "`", "`");
                        var num3 = _pack.Strtable.GetStringItem(str3);
                        if (num3 != -1) data = BitConverter.GetBytes((uint) num3);
                    }

                    break;
                }
                default:
                {
                    if (itemData.IndexOf('.') < 0) //处理整数
                    {
                        type = 2;
                        int.TryParse(itemData, out var tmp);
                        data = BitConverter.GetBytes(tmp);
                    }
                    else
                    {
                        type = 4;
                        var sParse = float.TryParse(itemData, out var f);
                        data = BitConverter.GetBytes(f);
                        if (!sParse)
                            type = 255;
                    }

                    break;
                }
            }

            return (type, data);
        }

        /// <summary>
        ///     编译Type10
        /// </summary>
        /// <param name="item"></param>
        /// <param name="stream"></param>
        /// <param name="isReadonly"></param>
        private void CompileType10Item(string item, Stream stream, bool isReadonly)
        {
            var strid = GetDataFromFormat(item, "<", "::"); //取得id
            uint.TryParse(strid, out var num);
            var strname = GetDataFromFormat(item, "::", "`"); //取得名称
            stream.WriteByte(9);
            stream.Write(BitConverter.GetBytes(num), 0, 4);
            stream.WriteByte(10);
            var strcode = _pack.Strtable.GetStringItem(strname); //获取StringTable项目
            if (strcode != -1)
                stream.Write(BitConverter.GetBytes((uint) strcode), 0, 4);
            else if (!isReadonly)
                stream.Write(BitConverter.GetBytes((uint) _pack.Strtable.AddStringItem(strname)), 0, 4); //添加项目
            else
                stream.Write(BitConverter.GetBytes(0u), 0, 4);
        }

        #endregion
    }
}