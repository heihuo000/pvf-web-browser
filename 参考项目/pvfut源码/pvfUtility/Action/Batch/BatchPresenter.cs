using System;
using System.Linq;
using System.Threading.Tasks;
using pvfUtility.Actions.Batch;
using pvfUtility.Dock.CollectionExplorer;
using pvfUtility.Model.PvfOperation.Encoder;
using pvfUtility.Service;

namespace pvfUtility.Action.Batch
{
    internal class BatchPresenter
    {
        private readonly BatchCmd cmd = new BatchCmd();

        public void ShowDialog()
        {
            cmd.FileCollection = CollectionExplorerPresenter.Instance.CurFileCollection;
            new BatchTaskDialog(cmd).Show();
        }

        public static void StartBatch(BatchCmd cmd)
        {
            MainPresenter.Instance.DoAction("正在处理文件...", progress =>
            {
                switch (cmd.BatchMode)
                {
                    case BatchMode.Add:
                        var (s, data) =
                            new ScriptFileCompiler(PackService.CurrentPack).EncryptScriptText(cmd.TextData, false);
                        if (!s)
                        {
                            Logger.Error("批处理 :: 片段编译错误,操作终止");
                            return Task.FromResult(false);
                        }

                        foreach (var file in cmd.FileCollection.FileList
                            .Select(item => PackService.CurrentPack.GetFile(item))
                            .Where(file => file != null)) PackService.CurrentPack.AddScriptContent(file, data);
                        break;
                    case BatchMode.Replace:
                        var (f, findBytes) =
                            new ScriptFileCompiler(PackService.CurrentPack).EncryptScriptText(cmd.TextData, true);
                        var (b, replaceBytes) =
                            new ScriptFileCompiler(PackService.CurrentPack).EncryptScriptText(cmd.TextDataReplace,
                                false);
                        if (!f || !b)
                        {
                            Logger.Error("批处理 :: 片段编译错误,操作终止");
                            return Task.FromResult(false);
                        }

                        foreach (var item in cmd.FileCollection.FileList)
                        {
                            var file = PackService.CurrentPack.GetFile(item);
                            if (file == null) continue;
                            var (success, times) =
                                PackService.CurrentPack.ReplaceScriptContent(file, findBytes, replaceBytes);
                            if (success) Logger.Info($"批处理 :: 在文件 file://{item} 中替换了{times}次");
                        }

                        break;
                    case BatchMode.Delete:
                        if (!cmd.IsDelContent)
                        {
                            var num1 = PackService.CurrentPack.Strtable.GetStringItem(cmd.TextDelSection);
                            if (num1 == -1)
                                throw new Exception("could not find label:" + cmd.TextDelSection);
                            var endText = cmd.TextDelSection.Remove(0, 1);
                            var num2 = PackService.CurrentPack.Strtable.GetStringItem(@"[/" + endText);
                            foreach (var file in cmd.FileCollection.FileList
                                .Select(item => PackService.CurrentPack.GetFile(item)).Where(file => file != null))
                                PackService.CurrentPack.DeleteSectionContent(file, num1, num2);
                        }
                        else
                        {
                            var (d, deleteBytes) =
                                new ScriptFileCompiler(PackService.CurrentPack).EncryptScriptText(cmd.TextDelContent,
                                    true);
                            if (!d)
                            {
                                Logger.Error("批处理 :: 片段编译错误,操作终止");
                                return Task.FromResult(false);
                            }

                            foreach (var file in cmd.FileCollection.FileList
                                .Select(item => PackService.CurrentPack.GetFile(item)).Where(file => file != null))
                                PackService.CurrentPack.DeleteScriptContent(file, deleteBytes);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Logger.Success($"批处理 :: 已处理“{cmd.FileCollection.Name}”中所有文件。");
                return Task.FromResult(true);
            });
        }
    }
}