using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using pvfUtility.Helper;
using pvfUtility.Service;

namespace pvfUtility.Model.PvfOperation.Praser
{
    internal static class PraserInfoProvider
    {
        /// <summary>
        ///     文件名，节名
        /// </summary>
        private static Dictionary<string, Dictionary<string, SectionInfo>> _sectionDictionary;

        private static XmlDocument _xml;

        public static Dictionary<string, SectionInfo> GetDic(string fileName)
        {
            if (_sectionDictionary == null) return new Dictionary<string, SectionInfo>();
            foreach (var item in _sectionDictionary)
            {
                if (!LikeOperator.LikeString(fileName, item.Key, CompareMethod.Binary)) continue;
                return item.Value;
            }

            return new Dictionary<string, SectionInfo>();
        }

        public static async void InitParseInfoProvider()
        {
            _xml = new XmlDocument();
            _sectionDictionary = new Dictionary<string, Dictionary<string, SectionInfo>>();
            try
            {
                var data = await HttpHelper.Get("https://wallace1300.gitee.io/pvfutility/PraseInfo.xml").Result
                    .ReadAsStreamAsync();
                _xml.Load(data);
                foreach (XmlElement xmlElement in _xml.SelectNodes("/root/Section"))
                {
                    var file = xmlElement.GetAttribute("File");
                    if (!_sectionDictionary.ContainsKey(file))
                    {
                        var filekey = new Dictionary<string, SectionInfo>();
                        _sectionDictionary.Add(file, filekey);
                        AddSectionInfo(filekey, xmlElement);
                    }
                    else
                    {
                        AddSectionInfo(_sectionDictionary[file], xmlElement);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("读取解析器配置信息失败：：" + ex.Message);
            }
        }

        private static void AddSectionInfo(IDictionary<string, SectionInfo> dic, XmlElement xmlElement)
        {
            var item = new SectionInfo
            {
                Description = xmlElement.GetAttribute("Description"),
                GroupLength = int.Parse(xmlElement.GetAttribute("GroupLength"))
            };
            dic.Add(xmlElement.GetAttribute("SectionName"), item);
        }

        public class SectionInfo
        {
            public string Description;
            public int GroupLength;
        }
    }
}