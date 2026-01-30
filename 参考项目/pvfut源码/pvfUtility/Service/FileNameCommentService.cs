using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using pvfUtility.Action;
using pvfUtility.Helper;
using pvfUtility.Shell.Dialogs;

namespace pvfUtility.Service
{
    /// <summary>
    ///     文件名称注释服务,包括云端和本地端
    /// </summary>
    internal static class FileNameCommentService
    {
        private static Dictionary<string, string> UserList;
        private static Dictionary<string, string> CloudList;
        private static XmlDocument Xml = new XmlDocument();

        /// <summary>
        ///     从云端和本地载入文件名称注释
        /// </summary>
        static FileNameCommentService()
        {
            Task.Run(Init);
        }

        /// <summary>
        ///     取文件名注释
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string Get(string path)
        {
            if (CloudList == null || UserList == null) return "";
            if (UserList.ContainsKey(path))
                return UserList[path];
            return CloudList.ContainsKey(path) ? CloudList[path] : "";
        }

        public static string GetComment(string path)
        {
            var result = Get(path);
            return string.IsNullOrEmpty(result) ? "" : $"({result})";
        }

        private static async Task Init()
        {
            UserList = new Dictionary<string, string>();
            CloudList = new Dictionary<string, string>();
            try
            {
                var xmlDocument = new XmlDocument();
                var data = await HttpHelper.Get("https://wallace1300.gitee.io/pvfutility/Comment.xml").Result
                    .ReadAsStreamAsync();
                xmlDocument.Load(data);
                var xmlNodeList = xmlDocument.SelectNodes("/root/Comment");
                if (xmlNodeList != null)
                    foreach (XmlElement xmlElement in xmlNodeList)
                        CloudList.Add(xmlElement.GetAttribute("FileName"), xmlElement.GetAttribute("Text"));

                if (File.Exists(AppCore.AppPath + "Comment.xml"))
                {
                    Xml.Load(string.Concat(AppCore.AppPath, "Comment.xml"));
                    var selectNodes = Xml.SelectNodes("/root/Comment");
                    if (selectNodes == null) return;
                    foreach (XmlElement xmlElement in selectNodes)
                        UserList.Add(xmlElement.GetAttribute("FileName"), xmlElement.GetAttribute("Text"));
                }
                else
                {
                    Xml = new XmlDocument();
                    var xmldecl = Xml.CreateXmlDeclaration("1.0", "UTF-8", null);
                    Xml.AppendChild(xmldecl);
                    var xmlelem = Xml.CreateElement("", "root", "");
                    Xml.AppendChild(xmlelem);
                    Xml.Save(string.Concat(AppCore.AppPath, "Comment.xml"));
                }
            }
            catch (Exception ex)
            {
                Logger.Error("文件名称注释 :: 读取注释文件（Comment.xml）失败：：" + ex.Message);
            }
        }

        /// <summary>
        ///     修改文件名注释
        /// </summary>
        /// <param name="fileName"></param>
        public static void Edit(string fileName)
        {
            var dialog = new InputBox("编辑注释", $"在下方输入{fileName}的注释", "注释：", Get(fileName));
            if (dialog.ShowDialog() == DialogResult.Cancel) return;

            var xmlNodeList = Xml.SelectNodes("/root/Comment");
            if (xmlNodeList != null)
                foreach (XmlElement xmlElement in xmlNodeList)
                {
                    if (xmlElement.GetAttribute("FileName") != fileName) continue;
                    xmlElement.SetAttribute("Text", dialog.InputtedText);
                    Xml.Save(string.Concat(AppCore.AppPath, "Comment.xml"));
                    UserList[fileName] = dialog.InputtedText;
                    return;
                }

            var root = Xml.SelectSingleNode("root");

            var xe1 = Xml.CreateElement("Comment");
            xe1.SetAttribute("FileName", fileName);
            xe1.SetAttribute("Text", dialog.InputtedText);
            root?.AppendChild(xe1);

            Xml.Save(string.Concat(AppCore.AppPath, "Comment.xml"));
            UserList.Add(fileName, dialog.InputtedText);
        }
    }
}