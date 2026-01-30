using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using pvfUtility.Helper;
using pvfUtility.Model.PvfOperation;
using pvfUtility.Service;

namespace pvfUtility.Action.Search
{
    internal static class SearchMethods
    {
        public static bool SearchString(this PvfFile file, HashSet<int> listint)
        {
            if (!file.IsScriptFile) return false;
            for (var i = 2; i < file.DataLen - 4; i += 5)
            {
                if (file.Data[i] == 5 || file.Data[i] == 7 || file.Data[i] == 10)
                    if (listint.Contains(BitConverter.ToInt32(file.Data, i + 1)))
                        return true;

                if (i <= 4 || file.Data[i] != 10 || file.Data[i - 5] != 9) continue;
                var item = file.Data[i - 4] * 0x1000000 + BitConverter.ToInt32(file.Data, i + 1);
                if (listint.Contains(item))
                    return true;
            }

            return false;
        }

        public static bool SearchNum(this PvfFile file, int num)
        {
            if (!file.IsScriptFile) return false;
            for (var i = 2; i < file.DataLen - 4; i += 5)
                if ((file.Data[i] == 2 || file.Data[i] == 4) && BitConverter.ToInt32(file.Data, i + 1) == num)
                    return true;

            return false;
        }

        public static bool SearchDataText(this PvfFile file, byte[] databytes)
        {
            var len = databytes.Length;
            if (!file.IsScriptFile) return false;
            if (file.DataLen < len) return false;
            for (var i = 2; i < file.DataLen; i += 5)
            {
                if (file.Data[i] != databytes[0] ||
                    BitConverter.ToInt32(file.Data, i + 1) != BitConverter.ToInt32(databytes, 1))
                    continue;
                if (databytes.Length == 5) return true;
                if (i + len > file.DataLen) return false;
                var temp = new byte[len];
                Buffer.BlockCopy(file.Data, i, temp, 0, len);
                if (!DataHelper.BytesEquals(temp, databytes)) continue;
                temp = null;
                return true;
            }

            return false;
        }

        public static bool SearchName(this PvfFile file, bool startMatch, string keyWord, string keyWord2, bool useLike,
            Regex regex)
        {
            var scriptName = PackService.CurrentPack.GetName(file);
            if (scriptName == null) return false;
            if (regex != null && regex.IsMatch(scriptName)) return true;
            if (useLike && LikeOperator.LikeString(scriptName, keyWord, CompareMethod.Binary) ||
                LikeOperator.LikeString(scriptName, keyWord2, CompareMethod.Binary))
                return true;
            if (startMatch && (scriptName.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) == 0 ||
                               scriptName.IndexOf(keyWord2, StringComparison.OrdinalIgnoreCase) == 0))
                return true;
            return scriptName.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) >= 0 ||
                   scriptName.IndexOf(keyWord2, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool CompareFile(this PvfFile file, PvfFile anotherfile, PvfPack anotherPvf)
        {
            if (file.IsScriptFile != anotherfile?.IsScriptFile) return false;

            if (!file.IsScriptFile && !anotherfile.IsScriptFile)
                return DataHelper.BytesEquals(anotherfile.Data, file.Data);

            if (anotherfile.DataLen != file.DataLen) return false;

            for (var i = 2; i < file.DataLen - 4; i += 5)
            {
                var x = file.Data[i];
                if (x == 5 || x == 6 || x == 7 || x == 8 || x == 10)
                    if (PackService.CurrentPack.Strtable.GetStringItem(BitConverter.ToInt32(file.Data, i + 1))
                        != anotherPvf.Strtable.GetStringItem(BitConverter.ToInt32(anotherfile.Data, i + 1)))
                        return false;

                if (x == 9 || x == 4 || x == 2)
                    if (BitConverter.ToInt32(file.Data, i + 1) != BitConverter.ToInt32(anotherfile.Data, i + 1))
                        return false;
            }

            return true;
        }
    }
}