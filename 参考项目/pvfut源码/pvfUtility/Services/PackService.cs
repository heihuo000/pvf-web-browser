using System;
using System.Threading.Tasks;
using ICSharpCode.Core;
using pvfUtility.app.View;
using pvfUtility.Core;
using pvfUtility.Core.TaskManager;
using pvfUtility.PvfOperation;
using pvfUtility.Shell;
using pvfUtility.Shell.Commands;
using pvfUtility.Shell.Services;

namespace pvfUtility.Services
{
    public delegate void PackEventHandler(PackEventArgs e);
    public static class PackService
    {
        /// <summary>
        /// 当前pvf
        /// </summary>
        public static PvfPack CurrentPack { get; private set; }
        /// <summary>
        /// 当前默认编码
        /// </summary>
        private static EncodingType _currentEncodingType;

        /// <summary>
        /// 当前pvf的路径
        /// </summary>
        public static string CurrentPackPathFolder
        {
            get
            {
                if (AppCore.Setting.ExtractDefaultPath != "")
                    return AppCore.Setting.ExtractDefaultPath;
                return CurrentPack != null ? CurrentPack.PvfPackPath.Remove(CurrentPack.PvfPackPath.Length - 4, 4) : "";
            }

        }

        public static event PackEventHandler PackOpened;
        public static event EventHandler PackClosed;

        public static EncodingType CurrentEncodingType
        {
            get => _currentEncodingType;
            set
            {
                _currentEncodingType = value;
                if (CurrentPack == null) return;
                CurrentPack.MainEncoding = value;
                if (CurrentPack.Strtable.IsStringTableUpdated) //修复切换编码bug
                    CurrentPack.GetFile("stringtable.bin")?.WriteFileData(CurrentPack.Strtable.CreateStringTable());
                CurrentPack.Strtable.Loadstringtable(CurrentPack.GetFile("stringtable.bin").Data, CurrentPack.MainEncoding);
                CurrentPack.Strview.InitStringData(CurrentPack.GetFile("n_string.lst"), CurrentPack, CurrentPack.MainEncoding);
            }
        }
        public static bool IsProcessing = false;

        public static void OpenPvf(string filePath)
        {
            if (IsProcessing)
                return;
            if (CurrentPack != null)
                if (!TaskService.AskMessage(
                        StringParser.Parse("${res:pvfUtility.PackService.OpenPvfWarning}"), ""))
                    return;
                else
                {
                    CurrentPack = null;
                    PackClosed?.Invoke(null, EventArgs.Empty);
                }
            TaskService.StartTaskAsync(async () =>
            {
                Bootstrap.Instance.CloseAllDocuments(true);
                RecentFilesMenuBuilder.AddRecentRecord(filePath);
                AppLogger.AddConsoleFileInfo("打开pvf：" + filePath);
                CurrentPack = new PvfPack
                {
                    MainEncoding = CurrentEncodingType,
                    PvfPackPath = filePath
                };

                foreach (var item in AppForm.Tools)
                    item.OnPackInit(CurrentPack);
                PackOpened?.Invoke(new PackEventArgs(CurrentPack));
                CurrentPack.FileTreePrased += (sender,e) =>
                {
                    StatusService.UpdateFileCount();                    
                    AppLogger.AddConsoleSuccessInfo("成功在内存中缓存全部文件。");
                };
                if (!await Task.Run(() => CurrentPack.OpenPvfPack(filePath, StatusService.ProgressHandler)))
                {
                    TaskService.ErrorMessage("无法打开pvf文件", "不受支持的格式或算法扩展配置错误。");
                    CurrentPack = null;
                }
                if (CurrentPack != null)
                    AppForm.Instance.Text = string.Concat(CurrentPack.PvfPackPath, " - pvfUtility");
            });


        }
        public static void RefreshTree()
        {
            if (IsProcessing)
                return;
            
            StatusService.UpdateFileCount();
        }

        /// <summary>
        /// 保存pvf
        /// </summary>
        /// <param name="isFastMode">是否快速模式</param>
        /// <param name="path">指定目录</param>
        public static void SavePvf(bool isFastMode, string path)
        {
            if (IsProcessing) return;

            if(string.IsNullOrEmpty(path))
                path = CurrentPack.PvfPackPath;

            TaskService.StartTaskAsync(async () => {
                if (await Task.Run(() => CurrentPack.SavePvfPack(path, isFastMode, StatusService.ProgressHandler)))
                    AppLogger.AddConsoleFileInfo("成功保存到：" + path);
            });      
        }
        public static void ClosePvf()
        {
            if (IsProcessing)
                return;
            if (!TaskService.AskMessage("是否关闭封包文件？", "未保存的更改将会丢失。")) return;
            AppForm.Instance.Text = "pvfUtility";
            foreach (var item in AppForm.Tools)
                item.OnPackClosing(CurrentPack);
            Bootstrap.Instance.CloseAllDocuments(true);
            CurrentPack = null;
            PackClosed?.Invoke(null, EventArgs.Empty);
            GC.Collect();
        }
    }
}
