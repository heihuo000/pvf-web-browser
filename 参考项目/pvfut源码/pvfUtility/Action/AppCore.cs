using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using pvfUtility.Model;
using pvfUtility.Service;

namespace pvfUtility.Action
{
    internal static class AppCore
    {
        public static string AppPath => AppContext.BaseDirectory;

        public static void Do(this Control control, MethodInvoker methodInvoker)
        {
            if (control.InvokeRequired)
                control.Invoke(methodInvoker);
            else
                methodInvoker();
        }

        public static void BeginDo(this Control control, MethodInvoker methodInvoker)
        {
            if (control.InvokeRequired)
                control.BeginInvoke(methodInvoker);
            else
                methodInvoker();
        }

        public static void SaveSetting()
        {
            using (var fs = new GZipStream(File.OpenWrite(AppPath + "Setting.bin"), CompressionMode.Compress))
            {
                new BinaryFormatter().Serialize(fs, Config.Instance);
            }
        }


        public static string GetPvfPath()
        {
            try
            {
                return PackService.CurrentPack.PvfPackFilePath.Remove(
                    PackService.CurrentPack.PvfPackFilePath.LastIndexOf('\\'));
            }
            catch
            {
                return "";
            }
        }

        public static int GetProgressNum(int nValue, int nMaxValue)
        {
            var num = nValue * 100 / nMaxValue;
            return num;
        }
    }
}