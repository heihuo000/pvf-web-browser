using System.Text;
using pvfUtility.Core;
using pvfUtility.PvfOperation.Encoder;

namespace pvfUtility.Script
{
    public static class File
    {
        public static byte[] GetBytes(string fileName)
        {
            var obj = AppCore.MainPvf.GetFile(fileName);
            return obj?.Data;
        }
        public static string GetText(string fileName,string encodings)
        {
            var obj = AppCore.MainPvf.GetFile(fileName);
            return obj != null ? Encoding.GetEncoding(encodings).GetString(obj.Data).TrimEnd(new char[1]) : null;
        }

        public static string GetDecryptionText(string fileName)
        {
            var obj = AppCore.MainPvf.GetFile(fileName);
            return obj != null ? new ScriptFileCompiler(AppCore.MainPvf).Decompile(obj) : null;
        }
        public static bool WriteBytes(string fileName, byte[] bytes)
        {
            var obj = AppCore.MainPvf.GetFile(fileName);
            if (obj == null) return false;
            obj.WriteFileData(bytes);
            return true;
        }
    }
}
