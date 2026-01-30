using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoUpdaterDotNET;
using pvfUtility.Dock.CollectionExplorer;
using pvfUtility.Dock.ErrorList;
using pvfUtility.Dock.FileExplorer;
using pvfUtility.Dock.Output;
using pvfUtility.Model;
using pvfUtility.Model.PvfOperation;
using pvfUtility.Model.PvfOperation.Praser;
using pvfUtility.Properties;
using pvfUtility.Service;
using pvfUtility.Shell;
using pvfUtility.Shell.Docks;
using WeifenLuo.WinFormsUI.Docking;
using static pvfUtility.Service.PackService;
using static pvfUtility.Action.AppCore;

namespace pvfUtility
{
    internal class MainPresenter
    {
        /// <summary>
        ///     单例
        /// </summary>
        public static MainPresenter Instance { get; } = new MainPresenter();

        /// <summary>
        ///     窗口
        /// </summary>
        public MainForm View { get; set; }

        /// <summary>
        ///     存储Tool窗口位置
        /// </summary>
        public static string LocationSettingFile =>
            AppPath + "DockPanel" + Assembly.GetExecutingAssembly().GetName().Version + ".bin";

        /// <summary>
        ///     初始化dock位置
        /// </summary>
        /// <param name="persistString"></param>
        /// <returns></returns>
        public IDockContent GetContentFromPersistString(string persistString)
        {
//            return (from item in ToolPresenter
//                where
//                    item.SaveLocation && item.DockView.GetType().ToString() == persistString
//                select item.DockView).FirstOrDefault();
            if (persistString == typeof(FileExplorerDock).ToString())
                return FileExplorerPresenter.Instance.View;
            if (persistString == typeof(CollectionExplorerDock).ToString())
                return CollectionExplorerPresenter.Instance.View;
            if (persistString == typeof(ErrorListDockView).ToString())
                return ErrorListPresenter.Instance.View;
            return persistString == typeof(OutputDockView).ToString() ? OutputPresenter.Instance.View : null;
        }

        /// <summary>
        ///     退出确认
        /// </summary>
        /// <returns></returns>
        public bool ExitConfirm()
        {
            if (CurrentPack != null &&
                !DialogService.AskYesNo("是否退出pvfUtility？", "未保存的更改将会丢失。", View.Handle))
                return true;
            SaveSetting();
            return false;
        }

        /// <summary>
        ///     所有行为走这个
        /// </summary>
        /// <param name="statusText"></param>
        /// <param name="action"></param>
        /// <param name="closeAllDocument"></param>
        public async void DoAction(string statusText, Func<IProgress<int>, Task> action, bool closeAllDocument = false)
        {
            View.ProgressBarShow(true);
            if (closeAllDocument) View.CloseAllDocuments();
            View.SetStatusText(statusText);
            await action(View.MainProgress);
            View.UpdateEnable();
            View.UpdateFileCount();
            View.ProgressBarShow(false);
            View.SetStatusText("就绪");
        }

        /// <summary>
        ///     设置状态栏
        /// </summary>
        /// <param name="text"></param>
        public void SetStatusText(string text)
        {
            Instance.View.SetStatusText(text);
        }

        /// <summary>
        ///     打开pvf
        /// </summary>
        /// <param name="filePath"></param>
        public void OpenPvf(string filePath)
        {
            lock (this)
            {
                if (filePath == null)
                    using (var openFileDialog = new OpenFileDialog
                    {
                        Title = "打开封包",
                        ValidateNames = true,
                        CheckPathExists = true,
                        DefaultExt = ".pvf",
                        CheckFileExists = true,
                        Filter = "Pack File|*.pvf"
                    })
                    {
                        if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                        AddRecentRecord(openFileDialog.FileName);
                        filePath = openFileDialog.FileName;
                    }

                if (CurrentPack != null)
                    if (!DialogService.AskYesNo("已经打开了一个PVF，继续打开将会使当前的更改丢失，继续吗？", "", View.Handle))
                        return;
                    else
                        ClosePvf(false);


                DoAction("打开中...", async progress =>
                {
                    Logger.Info("打开pvf：" + filePath);

                    if (!File.Exists(filePath))
                    {
                        DialogService.Error("文件不存在", "", View.Handle);
                        return;
                    }

                    CurrentPack = new PvfPack(CurrentEncodingType);
                    if (!await Task.Run(() => CurrentPack.OpenPvfPack(filePath, progress)))
                    {
                        DialogService.Error("无法打开此pvf文件", "该格式未受支持或已额外加密。", View.Handle);
                        CurrentPack = null;
                        return;
                    }

                    FileExplorerPresenter.Instance.OnPackOpened();
                    CollectionExplorerPresenter.Instance.OnPackOpened();
                    View.Text = $"[{CurrentPack.PvfPackFilePath}] - pvfUtility";
                    Logger.Info("提示:按住Ctrl后拖曳文件管理器中的文件可拖出到外面的文件夹中.");
                }, true);
            }
        }

        /// <summary>
        ///     主窗口启动成功
        /// </summary>
        public void Load()
        {
            Logger.Info(
                "pvfUtility :: 需要.NET 4.8 才能正常运行和使用全部功能,打开 https://go.microsoft.com/fwlink/?linkid=2088631 下载(已安装请忽略)");
            Task.Run(PraserInfoProvider.InitParseInfoProvider);
            HttpRestServerService.StartListen();
            AutoUpdater.Start("https://wallace1300.gitee.io/pvfutility/update.xml");

            Task.Run(() => //有些win10 用户打不开?
            {
                if (Environment.OSVersion.Version.Major == 10) Program.SetFeatures(999); //针对win10使用Edge 内核
            });
        }

        /// <summary>
        ///     保存pvf
        /// </summary>
        /// <param name="isOtherPath"></param>
        /// <param name="isFastMode"></param>
        public void SavePvf(bool isOtherPath, bool isFastMode)
        {
            if (!View.AskToSaveDocument(false))
                return;
            var path = CurrentPack.PvfPackFilePath;
            if (isOtherPath)
                using (var saveFileDialog = new SaveFileDialog
                {
                    Title = "封包另存为",
                    ValidateNames = true,
                    CheckPathExists = true,
                    DefaultExt = ".pvf",
                    FileName = "Script.pvf",
                    Filter = "Pack File|*.pvf"
                })
                {
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        path = saveFileDialog.FileName;
                    else
                        return;
                }

            lock (CurrentPack)
            {
                DoAction("保存中...", async progress =>
                {
                    View.DisableSave();
                    if (await Task.Run(() => CurrentPack.SavePvfPack(path, isFastMode, progress)))
                        Logger.Success("成功保存到：" + path);
                });
            }
        }

        public void Debug()
        {
            if (string.IsNullOrWhiteSpace(Config.Instance.DebugClientPath))
            {
                DialogService.Error("未设置客户端位置和参数", "在选项里面设置一下,例如 C:/xxx.exe QgElWj7lc5TJStZ+Z...vw==", View.Handle);
                return;
            }

            if (!View.AskToSaveDocument(false))
                return;
            var path = CurrentPack.PvfPackFilePath;
            lock (CurrentPack)
            {
                DoAction("启动中...", async progress =>
                {
                    View.DisableSave();
                    if (await Task.Run(() => CurrentPack.SavePvfPack(path, true, progress)))
                    {
                        Logger.Success("成功保存到：" + path);
                        Logger.Info($"启动 :: {Config.Instance.DebugClientPath} {Config.Instance.DebugClientArgs}");
                        Process.Start(Config.Instance.DebugClientPath, Config.Instance.DebugClientArgs);
                    }
                });
            }
        }

        /// <summary>
        ///     更新目录树
        /// </summary>
        public void RefreshTree()
        {
            FileExplorerPresenter.Instance.OnPackClosed();
            CollectionExplorerPresenter.Instance.OnPackClosed();
            FileExplorerPresenter.Instance.OnPackOpened();
            CollectionExplorerPresenter.Instance.OnPackOpened();
            View.UpdateFileCount();
            View.UpdateEnable();
        }

        /// <summary>
        ///     关闭pvf
        /// </summary>
        /// <param name="ask"></param>
        public void ClosePvf(bool ask = true)
        {
            if (ask && !DialogService.AskYesNo("是否关闭当前打开的封包文件？", "未保存的更改将可能会丢失。", View.Handle))
                return;

            lock (CurrentPack)
            {
                View.Text = "pvfUtility";
                FileExplorerPresenter.Instance.OnPackClosed();
                CollectionExplorerPresenter.Instance.OnPackClosed();
                View.CloseAllDocuments();
            }

            CurrentPack = null;
            View.UpdateEnable();
            GC.Collect();
        }

        /// <summary>
        ///     添加历史打开记录
        /// </summary>
        /// <param name="name"></param>
        public void AddRecentRecord(string name)
        {
            if (Config.Instance.RecentFiles.Contains(name))
                Config.Instance.RecentFiles.Remove(name);
            if (Config.Instance.RecentFiles.Count >= 10)
                Config.Instance.RecentFiles.RemoveAt(0);
            Config.Instance.RecentFiles.Add(name);
            SaveSetting();
        }

        /// <summary>
        ///     清理历史记录
        /// </summary>
        public void ClearRecentRecord()
        {
            Config.Instance.RecentFiles.Clear();
            SaveSetting();
        }

        /// <summary>
        ///     切换编码
        /// </summary>
        /// <param name="encoding"></param>
        public void ChangeEncoding(string encoding)
        {
            lock (this)
            {
                CurrentEncodingType = (EncodingType) Enum.Parse(typeof(EncodingType), encoding);
                Config.Instance.DefaultEncoding = CurrentEncodingType;
                if (CurrentPack == null)
                    return;
                DoAction("切换编码中...", async progress =>
                {
                    await Task.Run(() => CurrentPack.OverAllEncodingType = CurrentEncodingType);
                    Logger.Info("当前编码：" + CurrentEncodingType);
                });
            }
        }

        /// <summary>
        ///     就绪
        /// </summary>
        public static void SetReady()
        {
            Instance.View.SetStatusText(lang.Ready);
        }
    }
}