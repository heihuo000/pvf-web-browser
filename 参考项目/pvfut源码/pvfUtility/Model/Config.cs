using System;
using System.Collections.Generic;
using System.Drawing;
using pvfUtility.Action.Search;
using pvfUtility.Model.PvfOperation;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable once ConvertToConstant.Global
// ReSharper disable once FieldCanBeMadeReadOnly.Global
namespace pvfUtility.Model
{
    [Serializable]
    internal class Config
    {
        public static Dictionary<string, string> Description = new Dictionary<string, string>
        {
            {"UseDarkMode", "使用暗黑颜色主题(需要重启)"},

            {"EditorConvertTraditionalChinese", "文本编辑器 ::保存文件时自动转换繁体"},
            {"EditorEnableDirectDraw", "文本编辑器 ::启用DirectDraw改善字体显示"},
            {"EditorShowSpaces", "文本编辑器 :: 显示空白区域"},
            {"EditorShowBeginningTab", "文本编辑器 :: 显示行首的Tab"},

            {"DisableScriptNameView", "文件资源管理器 :: 禁用文件资源管理器的脚本名称显示"},
            {"FileExplorerDisableCodeView", "文件资源管理器 :: 禁用文件代码显示<>"},
            {"DisableNodeJump", "文件资源管理器 :: 禁用在文件集浏览器打开文件时的自动跳转"},

            {"AutoConvertStringLink", "解析器 :: 自动转换字符串链接 <1::name_1`文字`>将会自动转换为`文字`"}, //,在`文字`里面的\\n将会被替换为\r\n
            {"UseCompatibleDecompiler", "解析器 :: 使用兼容性脚本文件反编译器"},

            {"ExtractDefaultPath", "固定提取目录在:(留空为无)"},
            {"DebugClientPath", "启动调试的客户端地址"},
            {"DebugClientArgs", "启动调试的客户端参数"}
        };


        public static Config Instance = new Config();

        //Script
        public bool AutoConvertStringLink = true;

        public int ConfigVer = 10;
        public string DebugClientArgs = "";

        //Debug
        public string DebugClientPath = "";

        //Main Instance

        public EncodingType DefaultEncoding = EncodingType.TW;

        public bool DisableNodeJump = false;
        public bool EditorConvertTraditionalChinese = true;

        public bool EditorEnableDirectDraw = true;

        //Editor
        public Font EditorFont = new Font("Consolas", 10.5f);

        //显示起始的Tab
        public bool EditorShowBeginningTab = false;
        public bool EditorShowSpaces = true;
        public int? EditorZoom = null;
        public bool ExtractConvertSimplifiedChinese = false;
        public bool ExtractDecompileBinaryAni = true;
        public bool ExtractDecompileScript = true;

        //Extract
        public string ExtractDefaultPath = "";
        public bool ExtractOpenFolder = false;
        public bool FileExplorerDisableCodeView = false;
        public bool FileExplorerDisableScriptNameView = false;

        public List<string> FindHistory = new List<string>();
        public bool ImportCompileBinaryAni = true;

        //Import
        public bool ImportCompileScript = true;
        public bool ImportConvertTraditionalChinese = false;
        public bool IsNewCollection = true;
        public bool IsStartMatch = false;

        //Search
        public bool IsUseLikeSearchPath = false;
        public Point Location = new Point(100, 100);
        public Size MainInstanceSize = new Size(1280, 800);
        public List<string> PathSearchHistory = new List<string>();

        //History
        public List<string> RecentFiles = new List<string>();
        public List<string> ReplaceHistory = new List<string>();
        public SearchPresenter.SearchMethod SearchMethod = SearchPresenter.SearchMethod.None;
        public SearchPresenter.SearchNormalUsing SearchNormalUsing = SearchPresenter.SearchNormalUsing.None;
        public SearchPresenter.SearchType SearchType = SearchPresenter.SearchType.SearchStrings;

        public string StringLstFileName = "n_string.lst";
        public bool UseCompatibleDecompiler = false;

        public bool UseDarkMode = true;
    }
}