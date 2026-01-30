using System;
using System.Globalization;
using System.IO;
using System.Text;
using pvfUtility.Dock.Output;
using pvfUtility.Helper;

namespace pvfUtility.Service
{
    /// <summary>
    ///     日志服务
    /// </summary>
    internal static class Logger
    {
        private static readonly StringBuilder LoggerBuilder = new StringBuilder();

        public static void SaveLog()
        {
            PathsHelper.DirectoryExistCheck(PathsHelper.AppPath + "log\\");
            using (var writer = new StreamWriter(
                PathsHelper.AppPath + "log\\" + DateTime.Now.ToString("yy-MM-dd") + ".log", true))
            {
                writer.Write(LoggerBuilder.ToString());
            }
        }

        public static void Success(string text)
        {
            LoggerBuilder.AppendLine($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] SUCCESS {text}");
            OutputPresenter.Instance.Append(text);
        }

        public static void Error(string text)
        {
            LoggerBuilder.AppendLine($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] ERROR {text}");
            OutputPresenter.Instance.Append(text);
        }

        public static void Warning(string text)
        {
            LoggerBuilder.AppendLine($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] WARN {text}");
            OutputPresenter.Instance.Append(text);
        }

        public static void Info(string text)
        {
            LoggerBuilder.AppendLine($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] INFO {text}");
            OutputPresenter.Instance.Append(text);
        }

        public static void Debug(string text)
        {
            LoggerBuilder.AppendLine($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] DEBUG {text}");
#if DEBUG
            OutputPresenter.Instance.Append(text);
#endif
        }
    }
}