using System;
using pvfUtility.Core;

namespace pvfUtility.Script
{
    public static class ScriptFile
    {
        public static string GetName(string fileName)
        {
            return AppCore.MainPvf.GetName(AppCore.MainPvf.GetFile(fileName));
        }
/*        public static string GetLabelDataStr(string fileName,string labelText)
        {
            var num1 = AppCore.MainPvf.Strtable.GetStringItem(labelText);
            if (num1 == -1)
                throw new Exception("could not find label:"+ labelText);

            var obj = AppCore.MainPvf.GetFile(fileName);
            return obj != null ? AppCore.MainPvf.Strtable.GetStringItem(obj.GetLabelDataStr(num1)) : null;
        }
        public static int GetLabelDataInt(string fileName, string labelText)
        {
            var num1 = AppCore.MainPvf.Strtable.GetStringItem(labelText);
            if (num1 == -1)
                throw new Exception("could not find label:" + labelText);

            return AppCore.MainPvf.GetFile(fileName)?.GetLabelDataInt(num1) ?? -1;
        }
        public static bool SetLabelDataInt(string fileName, string labelText,int data)
        {
            var num1 = AppCore.MainPvf.Strtable.GetStringItem(labelText);
            if (num1 == -1)
                throw new Exception("could not find label:" + labelText);

            var obj = AppCore.MainPvf.GetFile(fileName);
            return obj != null && obj.SetLabelDataInt(num1,data);
        }
        public static bool SetLabelDataStr(string fileName, string labelText, string data)
        {
            var num1 = AppCore.MainPvf.Strtable.GetStringItem(labelText);
            if (num1 == -1)
                throw new Exception("could not find label:" + labelText);

            var obj = AppCore.MainPvf.GetFile(fileName);
            return obj != null && obj.SetLabelDataStr(num1, data, AppCore.MainPvf);
        }
        public static string GetLabelData(string fileName, string labelText)
        {
            var num1 = AppCore.MainPvf.Strtable.GetStringItem(labelText);
            if (num1 == -1)
                throw new Exception("could not find label:" + labelText);

            labelText = labelText.Remove(0,1);
            var num2 = AppCore.MainPvf.Strtable.GetStringItem(@"[/" + labelText);

            var obj = AppCore.MainPvf.GetFile(fileName);
            return obj?.GetLabelData(num1,num2 ,AppCore.MainPvf);
        }
        public static bool SetLabelData(string fileName, string labelText, string text)
        {
            var num1 = AppCore.MainPvf.Strtable.GetStringItem(labelText);
            if (num1 == -1)
                throw new Exception("could not find label:" + labelText);
            labelText = labelText.Remove(0, 1);
            var num2 = AppCore.MainPvf.Strtable.GetStringItem(@"[/" + labelText);

            var obj = AppCore.MainPvf.GetFile(fileName);
            return obj != null && obj.SetLabelData(num1, num2, AppCore.MainPvf, text);
        }

        public static bool DelLabel(string fileName, string labelText )
        {
            var num1 = AppCore.MainPvf.Strtable.GetStringItem(labelText);
            if (num1 == -1)
                throw new Exception("could not find label:" + labelText);
            labelText = labelText.Remove(0, 1);
            var num2 = AppCore.MainPvf.Strtable.GetStringItem(@"[/" + labelText);

            var obj = AppCore.MainPvf.GetFile(fileName);
            return obj != null && obj.DelLabel(num1, num2, AppCore.MainPvf);
        }
        public static bool NewScriptContent(string fileName, string text)
        {
            var obj = AppCore.MainPvf.GetFile(fileName);
            return obj != null && obj.NewScriptContent(AppCore.MainPvf, text);
        }*/
    }

}
