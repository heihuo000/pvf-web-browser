using System;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace pvfUtility.Helper.Native
{
    internal static class ChineseHelper
    {
        private const int LocaleSystemDefault = 0x0800;
        private const uint LcmapSimplifiedChinese = 0x02000000;
        private const uint LcmapTraditionalChinese = 0x04000000;

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int LCMapStringEx(int lpLocaleName, uint dwMapFlags, string lpSrcStr, int cchSrc,
            [Out] string lpDestStr, int cchDest, IntPtr lpVersionInformation, IntPtr lpReserved, IntPtr sortHandle);

        /// <summary>
        ///     将字符转换成简体中文
        /// </summary>
        /// <param name="source">输入要转换的字符串</param>
        /// <returns>转换完成后的字符串</returns>
        public static string ToSimplified(string source)
        {
            // var target = new string(' ', source.Length);
            // var ret = LCMapStringEx(LocaleSystemDefault, LcmapSimplifiedChinese, source, source.Length, target, source.Length, new IntPtr(), new IntPtr(), new IntPtr(0));
            // return target;
            return Strings.StrConv(source, VbStrConv.SimplifiedChinese);
        }

        /// <summary>
        ///     将字符转换为繁体中文
        /// </summary>
        /// <param name="source">输入要转换的字符串</param>
        /// <returns>转换完成后的字符串</returns>
        public static string ToTraditional(string source)
        {
            // var target = new string(' ', source.Length);
            // var ret = LCMapStringEx(LocaleSystemDefault, LcmapTraditionalChinese, source, source.Length, target, source.Length, new IntPtr(), new IntPtr(), new IntPtr(0));
            // return target;
            return Strings.StrConv(source, VbStrConv.TraditionalChinese);
        }
    }
}