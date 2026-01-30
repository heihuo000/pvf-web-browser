namespace pvfUtility.Action.Import
{
    internal class ImportSetting
    {
        /// <summary>
        ///     源目录
        /// </summary>
        public string SourcePath { get; set; }

        /// <summary>
        ///     目标目录
        /// </summary>
        public string TargetPaths { get; set; }

        /// <summary>
        ///     新的文件名
        /// </summary>
        public string[] Targets { get; set; }

        /// <summary>
        ///     编译脚本文件
        /// </summary>
        public bool CompileScript { get; set; }

        /// <summary>
        ///     编译二进制ani
        /// </summary>
        public bool CompileBinaryAni { get; set; }

        /// <summary>
        ///     转换繁体
        /// </summary>
        public bool ConvertToTraditionalChinese { get; set; }
    }
}