using System.Collections.Generic;
using System.Linq;
using pvfUtility.Actions;
using pvfUtility.Helper;
using pvfUtility.Service;

namespace pvfUtility.Model.PvfOperation
{
    internal partial class FileProperties : DialogBase
    {
        public FileProperties(IReadOnlyList<string> fileName)
        {
            InitializeComponent();
            btnClose.Click += (sender, args) =>
            {
                Close();
                Dispose();
            };
            if (fileName.Count == 1)
            {
                labelName.Text = fileName[0];
                var file = PackService.CurrentPack.GetFile(fileName[0]);
                if (file != null)
                {
                    if (file.IsScriptFile)
                        listBoxProperties.Items.Add("类型：脚本文件");
                    else if (file.IsBinaryAniFile)
                        listBoxProperties.Items.Add("类型：二进制ANI文件");
                    else
                        listBoxProperties.Items.Add("类型：文本文件");
                    listBoxProperties.Items.Add("大小：" + DataHelper.SizeToText(file.DataLen));
                }
                else
                {
                    var files = PackService.CurrentPack.GetFileObjs(fileName[0]);
                    listBoxProperties.Items.Add("类型：文件夹");
                    listBoxProperties.Items.Add($"大小：{DataHelper.SizeToText(files.Sum(item => item.DataLen))}");
                    listBoxProperties.Items.Add($"包含：{files.Length} 个文件");
                }
            }
            else
            {
                labelName.Text = "多个项目";
                listBoxProperties.Items.Add("类型：多个项目");
            }
        }
    }
}