using System;
using System.Collections.Generic;
using System.Text;
using pvfUtility.Service;
using static pvfUtility.Helper.DataHelper;

namespace pvfUtility.Model.PvfOperation.Praser
{
    internal class ScriptFileParser
    {
        private readonly PvfFile _file;
        private readonly PvfPack _pack;
        public List<ScriptItemBase> ScriptFile;
        public List<ScriptItemData> ScriptItems;

        public ScriptFileParser(PvfFile file, PvfPack pack)
        {
            _file = file;
            _pack = pack;
            ScriptItems = new List<ScriptItemData>();
            if (_file.Data == null || _file.DataLen < 7) return;
            for (var index = 2; index < _file.DataLen - 4; index += 5)
            {
                var type = _file.Data[index];
                var data = BitConverter.ToInt32(_file.Data, index + 1);
                var item = new ScriptItemData
                {
                    ScriptType = ScriptType.Int,
                    Data = data
                };
                if (type < 2 || type > 10)
                    // MainPresenter.Instance.View.DockErrorList.
                    //     AddError(_file.FileName, $"未能识别的类型值：{type}数据参考：{data}::`{GetStringItem(data)}`", "");
                    continue;
                item.ScriptType = (ScriptType) type;
                ScriptItems.Add(item);
            }
        }

        public string Init()
        {
            var str = GetText();
            //VerifyData(str);
            return str;
        }

        #region 校验

        // private bool VerifyData(string text)
        // {
        //     var bytes = new byte[_file.DataLen];
        //     Buffer.BlockCopy(_file.Data, 0, bytes, 0, _file.DataLen);    
        //     var bytes1 = new ScriptFileCompiler(_pack).Compile(_file, text);
        //     return BytesEquals(bytes, bytes1);
        // }
        private bool VerifyStructure()
        {
            var afterCount = 0;
            foreach (var item in ScriptFile)
                if (item.GetType() == typeof(ScriptItemBase))
                    afterCount++;
                else if (item.GetType() == typeof(ScriptSection))
                    afterCount += GetSectionCount((ScriptSection) item);
            return afterCount == ScriptItems.Count;
        }

        private int GetSectionCount(ScriptSection section)
        {
            var count = 0;
            foreach (var item in section.Children)
                if (item.GetType() == typeof(ScriptItemBase))
                    count++;
                else if (item.GetType() == typeof(ScriptSection))
                    count += GetSectionCount((ScriptSection) item);
                else if (item.GetType() == typeof(ScriptItemGroup)) count += ((ScriptItemGroup) item).Children.Count;

            return count;
        }

        #endregion

        #region 文本解析

        public string PraseText()
        {
            PraseStructure();
            if (!VerifyStructure())
                Logger.Error($"解析文件{_file.FileName}：：数量统计报告异常。");
            var text = GetText();
            //if (!VerifyData(text))
            //    AppLogger.AddConsoleErrorInfo(string.Format(lang.PraserVerifyDataFailed, _file.FileName));
            return text;
        }

        private string GetText()
        {
            var sb = new StringBuilder("#PVF_File");
            sb.Append(Environment.NewLine);
            for (var i = 0; i < ScriptFile.Count; i++)
            {
                var item = ScriptFile[i];
                if (item.GetType() == typeof(ScriptSection))
                {
                    sb.Append(ProcessSectionText((ScriptSection) item, 1));
                    sb.Append(Environment.NewLine);
                }
                else
                {
                    sb.Append(Environment.NewLine);
                    sb.Append(item.Item.ScriptType == ScriptType.StringLinkIndex
                        ? GetItemText(ScriptFile[i].Item, ScriptFile[i + 1].Item)
                        : GetItemText(ScriptFile[i].Item, new ScriptItemData()));
                }
            }

            return sb.ToString();
        }

        private string ProcessSectionText(ScriptSection section, int layer)
        {
            var sb = new StringBuilder();
            var count = section.Children.Count;
            if (section.HasEndTag) count--;
            sb.Append(Environment.NewLine);
            for (var i = 0; i < layer - 1; i++) sb.Append("\t");
            sb.Append(GetItemText(section.Children[0].Item, new ScriptItemData()));
            for (var index = 1; index < count; index++)
            {
                var item = section.Children[index];
                if (item.GetType() == typeof(ScriptSection))
                {
                    sb.Append(ProcessSectionText((ScriptSection) item, layer + 1));
                }
                else if (item.GetType() == typeof(ScriptItemGroup))
                {
                    sb.Append(Environment.NewLine);
                    for (var i = 0; i < layer; i++) sb.Append("\t");
                    var group = (ScriptItemGroup) item;
                    for (var i = 0; i < group.Children.Count; i++)
                    {
                        sb.Append(group.Children[i].Item.ScriptType == ScriptType.StringLinkIndex
                            ? GetItemText(group.Children[i].Item, group.Children[i + 1].Item)
                            : GetItemText(group.Children[i].Item, new ScriptItemData()));

                        //sb.Append(GetItemText(group.Children[i].Item, group.Children[i + 1].Item));
                        sb.Append("\t");
                    }
                }
                else
                {
                    if (index == 1)
                    {
                        sb.Append(Environment.NewLine);
                        for (var i = 0; i < layer; i++) sb.Append("\t");
                    }
                    else if (item.Item.ScriptType == ScriptType.Int || item.Item.ScriptType == ScriptType.Float)
                    {
                        sb.Append("\t");
                    }
                    else
                    {
                        sb.Append(Environment.NewLine);
                        for (var i = 0; i < layer; i++) sb.Append("\t");
                    }

                    sb.Append(item.Item.ScriptType == ScriptType.StringLinkIndex
                        ? GetItemText(item.Item, section.Children[index + 1].Item)
                        : GetItemText(item.Item, new ScriptItemData()));
                }
            }

            if (section.HasEndTag)
            {
                sb.Append(Environment.NewLine);
                for (var i = 0; i < layer - 1; i++) sb.Append("\t");
                sb.Append(section.Children[count].GetType() == typeof(ScriptSection)
                    ? ProcessSectionText((ScriptSection) section.Children[count], layer + 1)
                    : GetItemText(section.Children[count].Item, new ScriptItemData()));
            }

            return sb.ToString();
        }

        public string GetItemText(ScriptItemData item, ScriptItemData nextItem)
        {
            switch (item.ScriptType)
            {
                case ScriptType.Int:
                    return item.Data.ToString();
                case ScriptType.Float:
                    return FormatFloat(BitConverter.ToSingle(BitConverter.GetBytes(item.Data), 0));
                case ScriptType.IntEx:
                    return $"{{{(int) item.ScriptType}=`{item.Data}`}}";
                case ScriptType.Section:
                    return GetStringItem(item.Data);
                case ScriptType.String:
                    return $"`{GetStringItem(item.Data)}`";
                case ScriptType.Command:
                case ScriptType.CommandSeparator:
                    return $"{{{(int) item.ScriptType}=`{GetStringItem(item.Data)}`}}";
                case ScriptType.StringLinkIndex:
                    var strname = GetStringItem(nextItem.Data);
                    if (Config.Instance.AutoConvertStringLink)
                        return $"`{_pack.Strview.GetStrText(item.Data, strname).Replace("\\n", "\r\n")}`";
                    return $"<{item.Data}::{strname}`{_pack.Strview.GetStrText(item.Data, strname)}`>";
                case ScriptType.StringLink:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return "";
        }

        private string GetStringItem(int data)
        {
            return _pack.Strtable.GetStringItem(data);
        }

        #endregion

        #region 解析结构

        public void PraseStructure()
        {
            ScriptFile = new List<ScriptItemBase>();
            var dic = PraserInfoProvider.GetDic(_file.FileName);
            var pos = 0;
            var count = ScriptItems.Count;
            while (pos < count)
            {
                var item = ScriptItems[pos];
                if (item.ScriptType == ScriptType.Section)
                {
                    var sectionName = GetStringItem(item.Data);
                    var endTagName = @"[/" + sectionName.Remove(0, 1);
                    var description = "";
                    var groupLen = 0;
                    if (dic.ContainsKey(sectionName))
                    {
                        description = dic[sectionName].Description;
                        groupLen = dic[sectionName].GroupLength;
                    }

                    var section = new ScriptSection
                    {
                        Name = sectionName,
                        HasEndTag = GetHasEnding(ScriptItems, pos, endTagName),
                        Description = description,
                        GroupLength = groupLen
                    };
                    ScriptFile.Add(section);
                    var posSection = GetSectionLength(ScriptItems, pos, section.HasEndTag, endTagName);
                    if (section.HasEndTag)
                    {
                        ProcessSection(section, ScriptItems.GetRange(pos, posSection + 1), dic);
                        pos += posSection + 1;
                    }
                    else
                    {
                        ProcessSection(section, ScriptItems.GetRange(pos, posSection), dic);
                        pos += posSection;
                    }
                }
                else
                {
                    ScriptFile.Add(new ScriptItemBase(item));
                    pos++;
                }
            }
        }

        private void ProcessSection(ScriptSection section, List<ScriptItemData> dataList,
            Dictionary<string, PraserInfoProvider.SectionInfo> dic)
        {
            var count = dataList.Count;
            for (var i = 0; i < count; i++)
                if (dataList[i].ScriptType == ScriptType.Section && i != 0 && i != count - 1)
                {
                    var sectionName = GetStringItem(dataList[i].Data);
                    var endTagName = @"[/" + sectionName.Remove(0, 1);
                    var groupLen = 0;
                    var description = "";
                    if (dic.ContainsKey(sectionName))
                    {
                        description = dic[sectionName].Description;
                        groupLen = dic[sectionName].GroupLength;
                    }

                    var childSection = new ScriptSection
                    {
                        Name = sectionName,
                        HasEndTag = GetHasEnding(dataList, i, endTagName),
                        Description = description,
                        GroupLength = groupLen
                    };
                    section.Children.Add(childSection);
                    var posSection = GetSectionLength(dataList, i, childSection.HasEndTag, endTagName);
                    if (childSection.HasEndTag)
                    {
                        ProcessSection(childSection, dataList.GetRange(i, posSection + 1), dic);
                        if (section.HasEndTag)
                            i += posSection;
                        else
                            i += posSection + 1;
                    }
                    else
                    {
                        ProcessSection(childSection, dataList.GetRange(i, posSection), dic);
                        if (section.HasEndTag)
                            i += posSection - 1;
                        else
                            i += posSection;
                    }
                }
                else
                {
                    if (section.GroupLength >= 1 &&
                        (section.HasEndTag && count - i > section.GroupLength && i != 0 && i != count - 1
                         || !section.HasEndTag && count - i >= section.GroupLength && i != 0 && i != count - 1))
                    {
                        var itemGroup = new ScriptItemGroup {GroupName = ""};
                        var groupCount = section.GroupLength;
                        for (var j = 0; j < groupCount; j++)
                        {
                            itemGroup.Children.Add(new ScriptItemBase(dataList[i + j]));
                            if (dataList[i + j].ScriptType == ScriptType.StringLinkIndex)
                                groupCount++;
                        }

                        section.Children.Add(itemGroup);
                        i += groupCount - 1;
                    }
                    else
                    {
                        section.Children.Add(new ScriptItemBase(dataList[i]));
                    }
                }
        }

        private int GetSectionLength(IReadOnlyList<ScriptItemData> dataList, int startPos, bool hasEndTag,
            string endTagName)
        {
            var count = dataList.Count;
            for (var i = startPos + 1; i < count; i++)
            {
                if (dataList[i].ScriptType != ScriptType.Section) continue;
                var sName = GetStringItem(dataList[i].Data);
                if (!hasEndTag || sName == endTagName) return i - startPos;
            }

            return count - startPos;
        }

        private bool GetHasEnding(IReadOnlyList<ScriptItemData> dataList, int startpos, string endingName)
        {
            var pos = startpos;
            var ending = dataList.Count;
            while (pos < ending)
            {
                if (dataList[pos].ScriptType == ScriptType.Section)
                    if (GetStringItem(dataList[pos].Data) == endingName)
                        return true;
                pos++;
            }

            return false;
        }

        #endregion
    }
}