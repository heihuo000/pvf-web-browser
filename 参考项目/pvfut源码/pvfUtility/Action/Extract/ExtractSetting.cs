using System.Collections.Generic;

namespace pvfUtility.Action.Extract
{
    internal class ExtractSetting
    {
        /// <summary>
        ///     目标目录
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        ///     提取对象
        /// </summary>
        public List<(string Path, bool UseLike)> Items { get; set; }

        /// <summary>
        ///     提取完成后是否打开explorer
        /// </summary>
        public bool OpenFolder { get; set; }

        /// <summary>
        ///     反编译脚本文件
        /// </summary>
        public bool DecompileScript { get; set; }

        /// <summary>
        ///     反编译二进制ani文件
        /// </summary>
        public bool DecompileBinaryAni { get; set; }

        /// <summary>
        ///     提取结果自动转换为简体中文
        /// </summary>
        public bool ConvertConvertSimplifiedChinese { get; set; }
    }
}