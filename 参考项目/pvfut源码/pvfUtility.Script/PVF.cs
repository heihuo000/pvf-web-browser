using pvfUtility.Core;
using System.Collections.Generic;
using static pvfUtility.Core.AppCore;
using System.Linq;

namespace pvfUtility.Script
{
    public static class PVF
    {
        public static List<string> GetCurretFileList()
        {
            return (from i in MainPvf.FileList select i.Key).ToList();
        }

//        public static List<string> GetCurretFileList(string path)
//        {
//            //return (from i in MainPvf.FileList  where IsPathMatch(i.Value.FileName, path) select i.Key).ToList();
//        }
        public static void UnzipFile(string[] filename)
        {
            //MainAction.ExtractFileFast(filename);
        }

        public static void Delete(string[] filename)
        {
            //MainAction.Delete(filename);
        }
    }
}
