using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using pvfUtility.Helper;
using pvfUtility.Service;

// ReSharper disable InconsistentNaming

namespace pvfUtility.Model.PvfOperation.Encoder
{
    internal static class BinaryAniCompiler
    {
        public static List<string> GetBinaryAniImgList(PvfFile file)
        {
            if (file.DataLen <= 0) return new List<string>();
            var imgList = new List<string>();
            var stream = new MemoryStream(file.Data);

            ReadUInt16(stream);
            var imgCount = ReadUInt16(stream);
            for (var i = 0; i < imgCount; i++)
                imgList.Add(ReadString(ReadInt32(stream), stream));
            stream.Dispose();
            return imgList;
        }

        /// <summary>
        ///     解密ANI
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static (bool success, string text) DecompileBinaryAni(PvfFile file)
        {
            try
            {
                // if (file.DataLen > 10) //少部分直接写入的明文ani
                // {
                //     var tmp = new byte[10];
                //     Buffer.BlockCopy(file.Data, 0, tmp, 0, 10);
                //     if (Encoding.ASCII.GetString(tmp) == "[FRAME MAX]")
                //         return (true, Encoding.ASCII.GetString(file.Data));
                // }
                if (file.DataLen <= 0)
                    return (true, "");

                var imgList = new List<string>();
                var stream = new MemoryStream(file.Data);
                var stringBuilder = new StringBuilder("#PVF_File\r\n"); //内容构建器
                var frameMax = ReadUInt16(stream);
                var imgCount = ReadUInt16(stream);
                //读取img列表
                for (var i = 0; i < imgCount; i++)
                    imgList.Add(ReadString(ReadInt32(stream), stream));
                //读取全局项目
                var aniOverallItem = ReadUInt16(stream);
                for (var j = 0; j < aniOverallItem; j++)
                {
                    var data = ReadUInt16(stream);
                    switch (data)
                    {
                        case (ushort) ANIData.LOOP:
                        case (ushort) ANIData.SHADOW:
                            stringBuilder.Append($"[{(ANIData) data}]\r\n\t{ReadByte(stream)}\r\n");
                            break;
                        case (ushort) ANIData.COORD:
                        case (ushort) ANIData.OPERATION:
                            stringBuilder.Append($"[{(ANIData) data}]\r\n\t{ReadUInt16(stream)}\r\n");
                            break;
                        case (ushort) ANIData.SPECTRUM:
                            stringBuilder.Append($"[SPECTRUM]\r\n\t{ReadByte(stream)}");
                            stringBuilder.Append($"\r\n\t[SPECTRUM TERM]\r\n\t\t{ReadInt32(stream)}");
                            stringBuilder.Append($"\r\n\t[SPECTRUM LIFE TIME]\r\n\t\t{ReadInt32(stream)}");
                            stringBuilder.Append("\r\n\t[SPECTRUM COLOR]\r\n\t\t");
                            stringBuilder.Append(
                                $"{Read256(stream)}\t{Read256(stream)}\t{Read256(stream)}\t{Read256(stream)}\r\n");
                            stringBuilder.Append(
                                $"\t[SPECTRUM EFFECT]\r\n\t\t`{(Effect_Item) ReadUInt16(stream)}`\r\n");
                            break;
                        default:
                            Logger.Error($"二进制Ani解析器 :: 文件 file://{file.FileName} 在读取全局数据时，第{stream.Position}字节错误。");
                            return (false, null);
                    }
                }

                //读取每一个image
                stringBuilder.Append($"[FRAME MAX]\r\n\t{frameMax}\r\n");
                for (var k = 0; k < frameMax; k++)
                {
                    stringBuilder.Append(string.Concat("\r\n[FRAME", k.ToString("D3"), "]\r\n"));
                    var aniBoxItem = ReadUInt16(stream);
                    var boxItemText = new StringBuilder();
                    for (var l = 0; l < aniBoxItem; l++)
                    {
                        var data = ReadUInt16(stream);
                        switch (data)
                        {
                            case (ushort) ANIData.ATTACK_BOX:
                                boxItemText.Append("\t[ATTACK BOX]\r\n\t");
                                break;
                            case (ushort) ANIData.DAMAGE_BOX:
                                boxItemText.Append("\t[DAMAGE BOX]\r\n\t");
                                break;
                            default:
                                Logger.Error(
                                    $"二进制Ani解析器 :: 文件 file://{file.FileName} 在读取[FRAME{k}]的DAMAGE BOX & ATTACK BOX 数据时时，第{stream.Position}字节错误。");
                                return (false, null);
                        }

                        boxItemText.Append(
                            $"{ReadInt32(stream)}\t{ReadInt32(stream)}\t{ReadInt32(stream)}\t{ReadInt32(stream)}\t{ReadInt32(stream)}\t{ReadInt32(stream)}\r\n");
                    }

                    stringBuilder.Append("\t[IMAGE]\r\n");
                    var imgIndex = (int) ReadInt16(stream);
                    if (imgIndex >= 0)
                    {
                        if (imgIndex > imgList.Count - 1)
                        {
                            Logger.Error(
                                $"二进制Ani解析器 :: 文件 file://{file.FileName} 在读取[FRAME{k}]的IMAGE数据时时，第{stream.Position}字节错误。" +
                                imgIndex);
                            return (false, null);
                        }

                        stringBuilder.Append($"\t\t`{imgList[imgIndex]}`\r\n\t\t{ReadUInt16(stream)}\r\n");
                    }
                    else
                    {
                        stringBuilder.Append("\t\t``\r\n\t\t0\r\n");
                    }

                    //读取特殊项目
                    stringBuilder.Append($"\t[IMAGE POS]\r\n\t\t{ReadInt32(stream)}\t{ReadInt32(stream)}\r\n");
                    var frameItem = ReadUInt16(stream);
                    for (var i = 0; i < frameItem; i++)
                    {
                        var data = ReadUInt16(stream);
                        switch (data)
                        {
                            case (ushort) ANIData.LOOP:
                            case (ushort) ANIData.SHADOW:
                            case (ushort) ANIData.INTERPOLATION:
                                stringBuilder.Append($"\t[{(ANIData) data}]\r\n\t\t{ReadByte(stream)}\r\n");
                                break;
                            case (ushort) ANIData.COORD:
                                stringBuilder.Append($"\t[COORD]\r\n\t\t{ReadUInt16(stream)}\r\n");
                                break;
                            case (ushort) ANIData.PRELOAD:
                                stringBuilder.Append("\t[PRELOAD]\r\n\t\t1\r\n");
                                break;
                            case (ushort) ANIData.IMAGE_RATE:
                                stringBuilder.Append(
                                    $"\t[IMAGE RATE]\r\n\t\t{ReadFloat(stream)}\t{ReadFloat(stream)}\r\n");
                                break;
                            case (ushort) ANIData.IMAGE_ROTATE:
                                stringBuilder.Append($"\t[IMAGE ROTATE]\r\n\t\t{ReadFloat(stream)}\r\n");
                                break;
                            case (ushort) ANIData.RGBA:
                                stringBuilder.Append(
                                    $"\t[RGBA]\r\n\t\t{Read256(stream)}\t{Read256(stream)}\t{Read256(stream)}\t{Read256(stream)}\r\n");
                                break;
                            case (ushort) ANIData.GRAPHIC_EFFECT:
                                stringBuilder.Append("\t[GRAPHIC EFFECT]\r\n");
                                var effectIndex = ReadUInt16(stream);
                                stringBuilder.Append($"\t\t`{(Effect_Item) effectIndex}`\r\n");
                                if (effectIndex == (int) Effect_Item.MONOCHROME)
                                    stringBuilder.Append(
                                        $"\t\t{Read256(stream)}\t{Read256(stream)}\t{Read256(stream)}\r\n");
                                if (effectIndex == (int) Effect_Item.SPACEDISTORT)
                                    stringBuilder.Append($"\t\t{ReadInt16(stream)}\t{ReadInt16(stream)}\r\n");
                                break;
                            case (ushort) ANIData.DELAY:
                                stringBuilder.Append($"\t[DELAY]\r\n\t\t{ReadInt32(stream)}\r\n");
                                break;
                            case (ushort) ANIData.DAMAGE_TYPE:
                                stringBuilder.Append(
                                    $"\t[DAMAGE TYPE]\r\n\t\t`{(DAMAGE_TYPE_Item) ReadUInt16(stream)}`\r\n");
                                break;
                            case (ushort) ANIData.PLAY_SOUND:
                                stringBuilder.Append(
                                    $"\t[PLAY SOUND]\r\n\t\t`{ReadString(ReadInt32(stream), stream)}`\r\n");
                                break;
                            case (ushort) ANIData.SET_FLAG:
                                stringBuilder.Append($"\t[SET FLAG]\r\n\t\t{ReadInt32(stream)}\r\n");
                                break;
                            case (ushort) ANIData.FLIP_TYPE:
                                stringBuilder.Append(
                                    $"\t[FLIP TYPE]\r\n\t\t`{(FLIP_TYPE_Item) ReadUInt16(stream)}`\r\n");
                                break;
                            case (ushort) ANIData.LOOP_START:
                                stringBuilder.Append("\t[LOOP START]\r\n");
                                break;
                            case (ushort) ANIData.LOOP_END:
                                stringBuilder.Append($"\t[LOOP END]\r\n\t\t{ReadInt32(stream)}\r\n");
                                break;
                            case (ushort) ANIData.CLIP:
                                stringBuilder.Append(
                                    $"\t[CLIP]\r\n\t\t{ReadInt16(stream)}\t{ReadInt16(stream)}\t{ReadInt16(stream)}\t{ReadInt16(stream)}\r\n");
                                break;
                            default:
                                Logger.Error(
                                    $"二进制Ani解析器 :: 文件 file://{file.FileName} 在读取[FRAME{k}]的子项数据时时，第{stream.Position}字节错误。");
                                return (false, null);
                        }
                    }

                    stringBuilder.Append(boxItemText);
                }

                return (true, stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                Logger.Error($"二进制Ani解析器 :: 文件 file://{file.FileName} 在读取时，发生错误:{ex.Message}");
                return (false, null);
            }
        }

        public static (bool success, byte[] data, ErrorListItem error) CompileBinaryAni(string text, string fileName)
        {
            try
            {
                var data = new List<byte>();
                var imgList = new List<string>();
                var split = text.Split
                    (new[] {'\r', '\n', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                var list = split.Select(t => t.TrimEnd())
                    .Where(text2 => text2 != "" && text2 != "#PVF_File").ToList();
                var count = list.Count;

                var frameIndex = new List<int>();
                for (var j = 0; j < count; j++)
                    if (list[j].Length > 6 && list[j].Substring(0, 6) == "[FRAME" && list[j] != "[FRAME MAX]")
                        frameIndex.Add(j);
                frameIndex.Add(count); //把文件结尾添加进来，确保最后一个FRAME没有遗漏
                data.AddRange(BitConverter.GetBytes((ushort) (frameIndex.Count - 1))); //写FRAME的个数

                for (var j = 0; j < count; j++)
                {
                    if (list[j] == "" || list[j] != "[IMAGE]") continue;
                    var imgFile = list[j + 1];
                    if (!imgList.Contains(imgFile) && imgFile != "``")
                        imgList.Add(imgFile);
                    if (imgFile == "``")
                        list[j + 1] = "-1";
                }

                data.AddRange(BitConverter.GetBytes((ushort) imgList.Count)); //写img个数
                //写所有img文件
                foreach (var t in imgList)
                {
                    var trueimgName = DataHelper.GetDataFromFormat(t, "`", "`"); //t.Substring(1, t.Length - 2);
                    data.AddRange(BitConverter.GetBytes(trueimgName.Length));
                    data.AddRange(Encoding.ASCII.GetBytes(trueimgName));
                }

                var overallTemp = new List<byte>();
                var overalltempCount = 0;
                for (var j = 0; j < frameIndex[0]; j++)
                {
                    if (list[j] == "" || list[j][0] != '[' ||
                        list[j].Length > 6 && list[j].Substring(0, 6) == "[FRAME") continue;
                    overalltempCount++;
                    switch (list[j])
                    {
                        case "[LOOP]":
                            overallTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.LOOP));
                            overallTemp.Add(byte.Parse(list[j + 1]));
                            break;
                        case "[SHADOW]":
                            overallTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.SHADOW));
                            overallTemp.Add(byte.Parse(list[j + 1]));
                            break;
                        case "[COORD]":
                            overallTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.COORD));
                            overallTemp.AddRange(BitConverter.GetBytes(ushort.Parse(list[j + 1])));
                            break;
                        case "[OPERATION]":
                            overallTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.OPERATION));
                            overallTemp.AddRange(BitConverter.GetBytes(ushort.Parse(list[j + 1])));
                            break;
                        case "[SPECTRUM]":
                            overallTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.SPECTRUM));
                            overallTemp.Add(byte.Parse(list[j + 1]));
                            if (list[j + 2][0] != '[' || list[j + 4][0] != '[' || list[j + 6][0] != '[' ||
                                list[j + 11][0] != '[')
                                break;
                            overallTemp.AddRange(BitConverter.GetBytes(uint.Parse(list[j + 3])));
                            overallTemp.AddRange(BitConverter.GetBytes(uint.Parse(list[j + 5])));
                            overallTemp.Add(byte.Parse(list[j + 7]));
                            overallTemp.Add(byte.Parse(list[j + 8]));
                            overallTemp.Add(byte.Parse(list[j + 9]));
                            overallTemp.Add(byte.Parse(list[j + 10]));
                            switch (list[j + 12])
                            {
                                case "`NONE`":
                                    overallTemp.AddRange(BitConverter.GetBytes((ushort) Effect_Item.NONE));
                                    break;
                                case "`DODGE`":
                                    overallTemp.AddRange(BitConverter.GetBytes((ushort) Effect_Item.DODGE));
                                    break;
                                case "`LINEARDODGE`":
                                    overallTemp.AddRange(BitConverter.GetBytes((ushort) Effect_Item.LINEARDODGE));
                                    break;
                                case "`DARK`":
                                    overallTemp.AddRange(BitConverter.GetBytes((ushort) Effect_Item.DARK));
                                    break;
                                case "`MONOCHROME`":
                                    overallTemp.AddRange(BitConverter.GetBytes((ushort) Effect_Item.MONOCHROME));
                                    break;
                                default:
                                    goto ERROR;
                            }

                            list[j + 2] = "";
                            list[j + 4] = "";
                            list[j + 6] = "";
                            list[j + 11] = "";
                            break;
                        default:
                            ERROR:
                            var error = new ErrorListItem
                            {
                                Description = $"保存文件`{fileName}`时，在全局数据中，未能识别数据或格式出错：`list[j]`。",
                                FileName = fileName,
                                FullText = text,
                                Line = 0
                            };
                            return (false, null, error);
                    }
                }

                data.AddRange(BitConverter.GetBytes((ushort) overalltempCount));
                data.AddRange(overallTemp);

                for (var i = 1; i < frameIndex.Count; i++) //从第一个分割点开始处理每个FRAME
                {
                    var boxTemp = new List<byte>();
                    var boxCount = 0;

                    var startIndex = frameIndex[i - 1];
                    var stopIndex = frameIndex[i];

                    var imgFile = "-1";
                    var imgFileIndex = "0";

                    var imgPos1 = "0";
                    var imgPos2 = "0";
                    //预处理 BOX IMAGE　POS
                    for (var j = startIndex; j < stopIndex; j++)
                    {
                        if (list[j] == "" || list[j][0] != '[')
                            continue;
                        if (list[j] == "[DAMAGE BOX]" || list[j] == "[ATTACK BOX]")
                        {
                            if (list[j] == "[DAMAGE BOX]")
                                boxTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.DAMAGE_BOX));
                            else if (list[j] == "[ATTACK BOX]")
                                boxTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.ATTACK_BOX));

                            boxTemp.AddRange(BitConverter.GetBytes(int.Parse(list[j + 1])));
                            boxTemp.AddRange(BitConverter.GetBytes(int.Parse(list[j + 2])));
                            boxTemp.AddRange(BitConverter.GetBytes(int.Parse(list[j + 3])));
                            boxTemp.AddRange(BitConverter.GetBytes(int.Parse(list[j + 4])));
                            boxTemp.AddRange(BitConverter.GetBytes(int.Parse(list[j + 5])));
                            boxTemp.AddRange(BitConverter.GetBytes(int.Parse(list[j + 6])));
                            list[j] = "";
                            boxCount++;
                        }

                        switch (list[j])
                        {
                            case "[IMAGE]":
                                list[j] = "";
                                imgFile = list[j + 1];
                                imgFileIndex = list[j + 2];
                                break;
                            case "[IMAGE POS]":
                                list[j] = "";
                                imgPos1 = list[j + 1];
                                imgPos2 = list[j + 2];
                                break;
                        }
                    }

                    data.AddRange(BitConverter.GetBytes((ushort) boxCount));
                    if (boxCount > 0)
                        data.AddRange(boxTemp);

                    if (imgFile != "-1") // 写img数据
                    {
                        var index = imgList.FindIndex(item => item == imgFile);
                        data.AddRange(BitConverter.GetBytes(Convert.ToInt16(index)));
                        data.AddRange(BitConverter.GetBytes(short.Parse(imgFileIndex)));
                    }
                    else
                    {
                        data.AddRange(BitConverter.GetBytes(short.Parse(imgFile)));
                    }

                    data.AddRange(BitConverter.GetBytes(int.Parse(imgPos1))); //写 IMAGE POS
                    data.AddRange(BitConverter.GetBytes(int.Parse(imgPos2)));

                    var frameItemTemp = new List<byte>();
                    var frameItemCount = 0;

                    for (var j = startIndex; j < stopIndex; j++) // 现在开始写一般项
                    {
                        if (list[j].Length > 6 && list[j].Substring(0, 6) == "[FRAME")
                            continue;
                        if (list[j] == "" || list[j][0] != '[') continue;
                        frameItemCount++;
                        switch (list[j])
                        {
                            case "[LOOP]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.LOOP));
                                frameItemTemp.Add(byte.Parse(list[j + 1]));
                                break;
                            case "[SHADOW]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.SHADOW));
                                frameItemTemp.Add(byte.Parse(list[j + 1]));
                                break;
                            case "[PRELOAD]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.PRELOAD));
                                break;
                            case "[COORD]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.COORD));
                                frameItemTemp.AddRange(BitConverter.GetBytes(ushort.Parse(list[j + 1])));
                                break;
                            case "[IMAGE RATE]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.IMAGE_RATE));
                                frameItemTemp.AddRange(BitConverter.GetBytes(float.Parse(list[j + 1])));
                                frameItemTemp.AddRange(BitConverter.GetBytes(float.Parse(list[j + 2])));
                                break;
                            case "[IMAGE ROTATE]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.IMAGE_ROTATE));
                                frameItemTemp.AddRange(BitConverter.GetBytes(float.Parse(list[j + 1])));
                                break;
                            case "[CLIP]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.CLIP));
                                frameItemTemp.AddRange(BitConverter.GetBytes(short.Parse(list[j + 1])));
                                frameItemTemp.AddRange(BitConverter.GetBytes(short.Parse(list[j + 2])));
                                frameItemTemp.AddRange(BitConverter.GetBytes(short.Parse(list[j + 3])));
                                frameItemTemp.AddRange(BitConverter.GetBytes(short.Parse(list[j + 4])));
                                break;
                            case "[RGBA]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.RGBA));
                                frameItemTemp.Add(byte.Parse(list[j + 1]));
                                frameItemTemp.Add(byte.Parse(list[j + 2]));
                                frameItemTemp.Add(byte.Parse(list[j + 3]));
                                frameItemTemp.Add(byte.Parse(list[j + 4]));
                                break;
                            case "[INTERPOLATION]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.INTERPOLATION));
                                frameItemTemp.Add(byte.Parse(list[j + 1]));
                                break;
                            case "[DELAY]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.DELAY));
                                frameItemTemp.AddRange(BitConverter.GetBytes(Convert.ToUInt32(list[j + 1])));
                                break;
                            case "[SET FLAG]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.SET_FLAG));
                                frameItemTemp.AddRange(BitConverter.GetBytes(uint.Parse(list[j + 1])));
                                break;
                            case "[LOOP START]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.LOOP_START));
                                break;
                            case "[LOOP END]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.LOOP_END));
                                frameItemTemp.AddRange(BitConverter.GetBytes(uint.Parse(list[j + 1])));
                                break;
                            case "[PLAY SOUND]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.PLAY_SOUND));
                                var tempSound = list[j + 1].Substring(1, list[j + 1].Length - 2);
                                frameItemTemp.AddRange(BitConverter.GetBytes(tempSound.Length));
                                frameItemTemp.AddRange(Encoding.ASCII.GetBytes(tempSound));
                                break;
                            case "[GRAPHIC EFFECT]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.GRAPHIC_EFFECT));
                                switch (list[j + 1].ToUpper())
                                {
                                    case "`NONE`":
                                        frameItemTemp.AddRange(BitConverter.GetBytes((ushort) Effect_Item.NONE));
                                        break;
                                    case "`DODGE`":
                                        frameItemTemp.AddRange(BitConverter.GetBytes((ushort) Effect_Item.DODGE));
                                        break;
                                    case "`LINEARDODGE`":
                                        frameItemTemp.AddRange(BitConverter.GetBytes((ushort) Effect_Item.LINEARDODGE));
                                        break;
                                    case "`DARK`":
                                        frameItemTemp.AddRange(BitConverter.GetBytes((ushort) Effect_Item.DARK));
                                        break;
                                    case "`XOR`":
                                        frameItemTemp.AddRange(BitConverter.GetBytes((ushort) Effect_Item.XOR));
                                        break;
                                    case "`MONOCHROME`":
                                        frameItemTemp.AddRange(BitConverter.GetBytes((ushort) Effect_Item.MONOCHROME));
                                        frameItemTemp.Add(byte.Parse(list[j + 2]));
                                        frameItemTemp.Add(byte.Parse(list[j + 3]));
                                        frameItemTemp.Add(byte.Parse(list[j + 4]));
                                        break;
                                    case "`SPACEDISTORT`":
                                        frameItemTemp.AddRange(
                                            BitConverter.GetBytes((ushort) Effect_Item.SPACEDISTORT));
                                        frameItemTemp.AddRange(BitConverter.GetBytes(short.Parse(list[j + 2])));
                                        frameItemTemp.AddRange(BitConverter.GetBytes(short.Parse(list[j + 3])));
                                        break;
                                    default:
                                        goto ERROR;
                                }

                                break;
                            case "[DAMAGE TYPE]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.DAMAGE_TYPE));
                                switch (list[j + 1])
                                {
                                    case "`SUPERARMOR`":
                                        frameItemTemp.AddRange(
                                            BitConverter.GetBytes((ushort) DAMAGE_TYPE_Item.SUPERARMOR));
                                        break;
                                    case "`NORMAL`":
                                        frameItemTemp.AddRange(BitConverter.GetBytes((ushort) DAMAGE_TYPE_Item.NORMAL));
                                        break;
                                    case "`UNBREAKABLE`":
                                        frameItemTemp.AddRange(
                                            BitConverter.GetBytes((ushort) DAMAGE_TYPE_Item.UNBREAKABLE));
                                        break;
                                    default:
                                        goto ERROR;
                                }

                                break;
                            case "[FLIP TYPE]":
                                frameItemTemp.AddRange(BitConverter.GetBytes((ushort) ANIData.FLIP_TYPE));
                                switch (list[j + 1])
                                {
                                    case "`ALL`":
                                        frameItemTemp.AddRange(BitConverter.GetBytes((ushort) FLIP_TYPE_Item.ALL));
                                        break;
                                    case "`HORIZON`":
                                        frameItemTemp.AddRange(BitConverter.GetBytes((ushort) FLIP_TYPE_Item.HORIZON));
                                        break;
                                    case "`VERTICAL`":
                                        frameItemTemp.AddRange(BitConverter.GetBytes((ushort) FLIP_TYPE_Item.VERTICAL));
                                        break;
                                    default:
                                        goto ERROR;
                                }

                                break;
                            default:
                                ERROR:
                                var error = new ErrorListItem
                                {
                                    Description = $"保存文件`{fileName}`时，在[FRAME{i}]中，未能识别数据`{list[j]}`",
                                    FileName = fileName,
                                    FullText = text,
                                    Line = 0
                                };
                                return (false, null, error);
                        }
                    }

                    data.AddRange(BitConverter.GetBytes((ushort) frameItemCount));
                    data.AddRange(frameItemTemp);
                }

                return (true, data.ToArray(), null);
            }
            catch (Exception ex)
            {
                Logger.Error($"保存文件 file://{fileName} 时，发生错误:{ex.Message}");
                return (false, null, null);
            }
        }

        //读6 个为读4个float，读3个为读2个Int
        private static byte ReadByte(Stream stream)
        {
            return (byte) stream.ReadByte();
        }

        private static ushort ReadUInt16(Stream stream)
        {
            var bytes = new byte[2];
            stream.Read(bytes, 0, 2);
            return BitConverter.ToUInt16(bytes, 0);
        }

        private static short ReadInt16(Stream stream)
        {
            var bytes = new byte[2];
            stream.Read(bytes, 0, 2);
            return BitConverter.ToInt16(bytes, 0);
        }

        private static int ReadInt32(Stream stream)
        {
            var bytes = new byte[4];
            stream.Read(bytes, 0, 4);
            return BitConverter.ToInt32(bytes, 0);
        }
//        private static uint ReadUInt32(byte[] data)
//        {
//            var array = new byte[4];
//            //Buffer.BlockCopy(data, _index, array, 0, 4);
//            //_index += 4;
//            return BitConverter.ToUInt32(array, 0);
//        }

        private static string ReadFloat(Stream stream)
        {
            var bytes = new byte[4];
            stream.Read(bytes, 0, 4);
            return DataHelper.FormatFloat((double) BitConverter.ToSingle(bytes, 0));
        }

        private static double Read256(Stream stream)
        {
            return (256.0 + ReadByte(stream)) % 256.0;
        }

        private static string ReadString(int len, Stream stream)
        {
            var temp = new byte[len];
            stream.Read(temp, 0, len);
            return Encoding.ASCII.GetString(temp);
        }

        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
        private enum ANIData
        {
            LOOP = 0,
            SHADOW = 1,
            COORD = 3,

            IMAGE_RATE = 7,
            IMAGE_ROTATE = 8,
            RGBA = 9,
            INTERPOLATION = 10,
            GRAPHIC_EFFECT = 11,
            DELAY = 12,
            DAMAGE_TYPE = 13,
            DAMAGE_BOX = 14,
            ATTACK_BOX = 15,
            PLAY_SOUND = 16,
            PRELOAD = 17,
            SPECTRUM = 18,

            SET_FLAG = 23,
            FLIP_TYPE = 24,
            LOOP_START = 25,
            LOOP_END = 26,
            CLIP = 27,
            OPERATION = 28
        }

        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
        private enum Effect_Item
        {
            NONE,
            DODGE,
            LINEARDODGE,
            DARK,
            XOR,
            MONOCHROME,
            SPACEDISTORT
        }

        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
        private enum DAMAGE_TYPE_Item
        {
            NORMAL,
            SUPERARMOR,
            UNBREAKABLE
        }

        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
        private enum FLIP_TYPE_Item
        {
            HORIZON = 1,
            VERTICAL,
            ALL
        }
    }
}