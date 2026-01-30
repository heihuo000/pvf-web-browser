using System;
using System.IO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace pvfUtility.Helper
{
    internal static class PathsHelper
    {
        /// <summary>
        ///     结尾 有  “\”
        /// </summary>
        internal static string AppPath => AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        public static void DirectoryExistCheck(string path)
        {
            var directoryName = Path.GetDirectoryName(path);
            if (Directory.Exists(directoryName))
                return;
            if (directoryName != null)
                Directory.CreateDirectory(directoryName);
        }

        public static bool IsPathMatch(string filename, string path)
        {
            if (path.Length <= 0) return true;
            if (filename.Length < path.Length) return false;
            return filename.Substring(0, path.Length) == path;
        }

        /// <summary>
        ///     使用通配符
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsPathMatchEx(string filename, string path)
        {
            if (path.Length <= 0) return true;
            return filename.Length >= path.Length &&
                   LikeOperator.LikeString(filename, path, CompareMethod.Binary);
        }

        /// <summary>
        ///     确保目录最后是 /
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PathFix(string path)
        {
            path = path.ToLower().Replace('\\', '/');
            if (path.Length < 1) return path;
            if (path[path.Length - 1] != '/') path += '/';
            return path;
        }

        public static string PathFixWin(string path)
        {
            if (path.Length < 1) return path;
            if (path[path.Length - 1] != '\\') path += '\\';
            return path;
        }

        public static string PathFixEx(string path)
        {
            if (path.Length < 1) return path.Replace('\\', '/');
            path = path.Replace('\\', '/');
            if (path.IndexOf('*') < 0) path += "/*";
            if (path.Substring(path.Length - 1, 1) == "/") path += "*";
            return path;
        }
    }
}