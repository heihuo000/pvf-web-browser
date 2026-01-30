using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace pvfUtility.Helper
{
    internal static class DataHelper
    {
        public static int GetResolve(int num)
        {
            if (num > 500000)
                return 768;
            if (num > 400000)
                return 512;
            if (num > 300000)
                return 384;
            if (num > 200000)
                return 256;
            if (num > 100000)
                return 128;
            if (num > 50000)
                return 64;
            if (num > 25000)
                return 32;
            if (num > 10000)
                return 16;
            return num > 5000 ? 8 : 2;
        }

        public static uint GetFileNameHashCode(IEnumerable<byte> dataBytes)
        {
            var num = dataBytes.Aggregate<byte, uint>(0x1505, (current, t) => 0x21 * current + t);
            return num * 0x21;
        }

        public static bool BytesEquals(byte[] b1, byte[] b2)
        {
            if ((b1 == null) & (b2 == null))
                return true;
            if ((b1 != null) & (b2 == null))
                return false;
            if ((b1 == null) & (b2 != null))
                return false;
            if (b1.Length != b2.Length) return false;
            return !b1.Where((t, i) => t != b2[i]).Any();
        }

        public static string GetDataFromFormat(string source, string header, string ending)
        {
            var length = header != "" ? source.IndexOf(header, StringComparison.Ordinal) : 0;
            if (length == -1)
                return "";
            length = length + header.Length;
            var str = source.Substring(length, source.Length - length);
            if (ending == "")
                return str;
            var num = str.IndexOf(ending, StringComparison.Ordinal);
            return num == -1 ? "" : str.Substring(0, num);
        }

        public static string FormatFloat(float f)
        {
            var str = f.ToString(CultureInfo.InvariantCulture);
            return str.IndexOf('.') <= 0 ? string.Concat(str, ".0") : str;
        }

        public static string FormatFloat(double f)
        {
            var str = f.ToString(CultureInfo.InvariantCulture);
            return str.IndexOf('.') <= 0 ? string.Concat(str, ".0") : str;
        }

        public static string Transform(string data)
        {
            var result = data;
            const char nullChar = (char) 0;
            const char cr = (char) 13;
            const char lf = (char) 10;
            const char tab = (char) 9;

            result = result.Replace("\\r\\n", Environment.NewLine);
            result = result.Replace("\\r", cr.ToString());
            result = result.Replace("\\n", lf.ToString());
            result = result.Replace("\\t", tab.ToString());
            result = result.Replace("\\0", nullChar.ToString());

            return result;
        }

        public static string SizeToText(long size)
        {
            if (size > 1024 * 1024)
                return ((double) size / 1024 / 1024).ToString("f3") + " MiB";
            if (size > 1024)
                return ((double) size / 1024).ToString("f3") + " KiB";
            return ((double) size).ToString("f3") + " Bytes";
        }
    }
}