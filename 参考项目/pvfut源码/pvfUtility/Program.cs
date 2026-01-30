using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using pvfUtility.Action;
using pvfUtility.Dialog;
using pvfUtility.Model;
using pvfUtility.Service;
using pvfUtility.Shell;

namespace pvfUtility
{
    internal static class Program
    {
        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
#if !DEBUG
            //全局异常捕捉
            //UI线程异常
            Application.ThreadException += Application_ThreadException; 
            //多线程异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif
            // 长路径支持
            // string reallyLongDirectory = @"D:\Test\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // reallyLongDirectory = reallyLongDirectory + @"\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmno123pqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // reallyLongDirectory = reallyLongDirectory + @"\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //
            // Console.WriteLine($"Creating a directory that is {reallyLongDirectory.Length} characters long");
            // Directory.CreateDirectory(reallyLongDirectory);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!IsDotnet47Installed())
            {
                MessageBox.Show("pvfUtility 需要.NET 4.8 才能正常运行和使用全部功能 \r\n 按\"确定\"下载.");
                Process.Start("https://go.microsoft.com/fwlink/?linkid=2088631");
            }

            ReadSetting();
            MainPresenter.Instance.View = new MainForm(MainPresenter.Instance);
            Application.Run(MainPresenter.Instance.View);
            HttpRestServerService.CloseListen();
            Logger.SaveLog();
        }

        /// <summary>
        ///     读取配置文件
        /// </summary>
        private static void ReadSetting()
        {
            var fileName = AppContext.BaseDirectory + "Setting.bin";
            if (!File.Exists(fileName))
            {
                using (var welcomeDialog = new WelcomeDialog())
                {
                    if (welcomeDialog.ShowDialog() == DialogResult.OK)
                        Config.Instance = new Config
                        {
                            UseDarkMode = welcomeDialog.ChooseDark()
                        };
                }

                return;
            }

            try
            {
                using (var fs = new GZipStream(File.OpenRead(fileName), CompressionMode.Decompress)) //配置文件经过压缩
                {
                    Config.Instance = (Config) new BinaryFormatter().Deserialize(fs);
                    if (Config.Instance.ConfigVer == new Config().ConfigVer) return;
                    using (var welcomeDialog = new WelcomeDialog())
                    {
                        if (welcomeDialog.ShowDialog() == DialogResult.OK)
                            Config.Instance = new Config
                            {
                                UseDarkMode = welcomeDialog.ChooseDark()
                            };
                    }
                }
            }
            catch
            {
                Config.Instance = new Config();
            }
        }

        /// <summary>
        ///     修改注册表信息使WebBrowser使用指定版本IE内核
        /// </summary>
        public static void SetFeatures(uint ieMode)
        {
            //获取程序及名称
            var appName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
            var featureControlRegKey =
                "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\";
            //设置浏览器对应用程序(appName)以什么模式(ieMode)运行
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, ieMode,
                RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1,
                RegistryValueKind.DWord);
        }

        private static bool IsDotnet47Installed()
        {
            // 4.7
            // https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
            return GetDotnet4Release() >= 460805;
        }

        /// <summary>
        ///     Gets the .NET 4.x release number.
        ///     The numbers are documented on http://msdn.microsoft.com/en-us/library/hh925568.aspx
        /// </summary>
        private static int? GetDotnet4Release()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full"))
            {
                if (key != null)
                    return key.GetValue("Release") as int?;
            }

            return null;
        }

        //UI线程异常
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var fileName = "_error-" + DateTime.Now.ToString("yy-MM-dd") + ".log";
            using (var errorfile = new StreamWriter("log\\" + fileName, true))
            {
                errorfile.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                errorfile.WriteLine(e.Exception);
                errorfile.WriteLine(
                    "=====================================================================================");
                Logger.Error($"抱歉,主线程发生错误,请把log文件夹下的文件{fileName}发给 作者QQ:1300271842 解决,并告知具体操作内容,感谢您对pvfUtility的支持:" +
                             e.Exception.Message);
            }
        }

        //多线程异常
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var fileName = "_error-" + DateTime.Now.ToString("yy-MM-dd") + ".log";
            using (var errorfile = new StreamWriter("log\\" + fileName, true))
            {
                errorfile.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                errorfile.WriteLine(e.ExceptionObject);
                errorfile.WriteLine(
                    "=====================================================================================");
                Logger.Error($"抱歉,其他线程发生错误,请把log文件夹下的文件{fileName}发给 作者QQ:1300271842 解决,并告知具体操作内容,感谢您对pvfUtility的支持:" +
                             e.ExceptionObject);
            }

            if (!e.IsTerminating) return;
            DialogService.Error(
                $"抱歉,其他线程发生错误,请把log文件夹下的文件{fileName}" +
                "\r\n发给 作者QQ:1300271842 解决,并告知具体操作内容,感谢您对pvfUtility的支持,当前内存中的内容将会保存.",
                e.ExceptionObject.ToString(), MainPresenter.Instance.View.Handle);

            PackService.CurrentPack?.SavePvfPack(AppCore.AppPath + "script-temp.pvf", false, new Progress<int>());
            Application.Restart();
        }
    }
}