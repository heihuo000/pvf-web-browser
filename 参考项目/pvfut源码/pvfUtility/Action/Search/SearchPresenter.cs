using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using pvfUtility.Dock.CollectionExplorer;
using pvfUtility.Helper;
using pvfUtility.Helper.Native;
using pvfUtility.Model;
using pvfUtility.Model.PvfOperation;
using pvfUtility.Model.PvfOperation.Encoder;
using pvfUtility.Model.PvfOperation.Praser;
using pvfUtility.Model.TreeModel;
using pvfUtility.Service;
using pvfUtility.Shell.Dialogs.Search;

namespace pvfUtility.Action.Search
{
    internal class SearchPresenter
    {
        public enum SearchMethod
        {
            None,
            Match,
            Remove
        }

        public enum SearchNormalUsing
        {
            None,
            Like,
            Regex
        }

        public enum SearchType
        {
            SearchNum,
            SearchStrings,
            SearchFileName,
            SearchScript,
            SearchName
        }

        private readonly PvfPack _pvf;

        public SearchPresenter(PvfPack pvf)
        {
            _pvf = pvf;
            CurSetting = new SearchSetting();
        }

        public static FolderTreeModel Pathmodel { get; set; }
        private SearchDialog SearchDialog { get; set; }
        public SearchSetting CurSetting { get; set; }

        public void ShowDialog(FileCollectionData data = null)
        {
            if (data == null) data = CollectionExplorerPresenter.Instance.CurFileCollection;
            SearchDialog = new SearchDialog(data, this);
            SearchDialog.Show();
        }

        public void StartSearch()
        {
            MainPresenter.Instance.DoAction("搜索中...", progress =>
            {
                byte[] bytes = null;
                var keyNum = 0;
                var nums = new HashSet<int>();

                var listBag = new ConcurrentBag<string>();
                Action<PvfFile> act;
                switch (CurSetting.SearchType)
                {
                    case SearchType.SearchNum:
                        if (CurSetting.Keyword.IndexOf('.') > 0)
                        {
                            var single = float.Parse(CurSetting.Keyword);
                            keyNum = BitConverter.ToInt32(BitConverter.GetBytes(single), 0);
                        }
                        else
                        {
                            keyNum = int.Parse(CurSetting.Keyword);
                        }

                        if (CurSetting.SearchMethod != SearchMethod.None)
                            act = file =>
                            {
                                if (file.SearchNum(keyNum))
                                    handleFile(CurSetting, file.FileName);
                            };
                        else
                            act = file =>
                            {
                                if (file.SearchNum(keyNum)) listBag.Add(file.FileName);
                            };
                        break;
                    case SearchType.SearchStrings:
                        switch (CurSetting.SearchNormalUsing)
                        {
                            case SearchNormalUsing.None:
                                PackService.CurrentPack.Strtable.FindStringItem(nums, CurSetting.Keyword,
                                    CurSetting.IsStartMatch, false, null);
                                PackService.CurrentPack.Strview.SearchstrInFiles(nums, PackService.CurrentPack.Strtable,
                                    CurSetting.Keyword,
                                    CurSetting.IsStartMatch, false, null);
                                break;
                            case SearchNormalUsing.Like:
                                PackService.CurrentPack.Strtable.FindStringItem(nums, CurSetting.Keyword,
                                    CurSetting.IsStartMatch, true, null);
                                PackService.CurrentPack.Strview.SearchstrInFiles(nums, PackService.CurrentPack.Strtable,
                                    CurSetting.Keyword,
                                    CurSetting.IsStartMatch, true, null);
                                break;
                            case SearchNormalUsing.Regex:
                                PackService.CurrentPack.Strtable.FindStringItem(nums, CurSetting.Keyword,
                                    CurSetting.IsStartMatch, false,
                                    CurSetting.Regex);
                                PackService.CurrentPack.Strview.SearchstrInFiles(nums, PackService.CurrentPack.Strtable,
                                    CurSetting.Keyword,
                                    CurSetting.IsStartMatch, false, CurSetting.Regex);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        if (CurSetting.SearchMethod != SearchMethod.None)
                            act = file =>
                            {
                                if (file.SearchString(nums))
                                    handleFile(CurSetting, file.FileName);
                            };
                        else
                            act = file =>
                            {
                                if (file.SearchString(nums))
                                    listBag.Add(file.FileName);
                            };
                        break;
                    case SearchType.SearchFileName:
                        if (CurSetting.SearchMethod != SearchMethod.None)
                            act = file =>
                            {
                                switch (CurSetting.SearchNormalUsing)
                                {
                                    case SearchNormalUsing.None:
                                        if (CurSetting.IsStartMatch &&
                                            file.ShortName.IndexOf(CurSetting.Keyword,
                                                StringComparison.OrdinalIgnoreCase) == 0)
                                            handleFile(CurSetting, file.FileName);
                                        if (!CurSetting.IsStartMatch &&
                                            file.ShortName.IndexOf(CurSetting.Keyword,
                                                StringComparison.OrdinalIgnoreCase) >= 0)
                                            handleFile(CurSetting, file.FileName);
                                        break;
                                    case SearchNormalUsing.Like:
                                        if (LikeOperator.LikeString(file.ShortName, CurSetting.Keyword,
                                            CompareMethod.Binary))
                                            handleFile(CurSetting, file.FileName);
                                        break;
                                    case SearchNormalUsing.Regex:
                                        if (CurSetting.Regex != null && CurSetting.Regex.IsMatch(file.ShortName))
                                            handleFile(CurSetting, file.FileName);
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            };
                        else
                            act = file =>
                            {
                                switch (CurSetting.SearchNormalUsing)
                                {
                                    case SearchNormalUsing.None:
                                        if (CurSetting.IsStartMatch &&
                                            file.ShortName.IndexOf(CurSetting.Keyword,
                                                StringComparison.OrdinalIgnoreCase) == 0)
                                            listBag.Add(file.FileName);
                                        if (!CurSetting.IsStartMatch &&
                                            file.ShortName.IndexOf(CurSetting.Keyword,
                                                StringComparison.OrdinalIgnoreCase) >= 0)
                                            listBag.Add(file.FileName);
                                        break;
                                    case SearchNormalUsing.Like:
                                        if (LikeOperator.LikeString(file.ShortName, CurSetting.Keyword,
                                            CompareMethod.Binary))
                                            listBag.Add(file.FileName);
                                        break;
                                    case SearchNormalUsing.Regex:
                                        if (CurSetting.Regex != null && CurSetting.Regex.IsMatch(file.ShortName))
                                            listBag.Add(file.FileName);
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            };
                        break;
                    case SearchType.SearchName:
                        var keyword2 = ChineseHelper.ToTraditional(CurSetting.Keyword);
                        if (CurSetting.SearchMethod != SearchMethod.None)
                            act = file =>
                            {
                                switch (CurSetting.SearchNormalUsing)
                                {
                                    case SearchNormalUsing.None:
                                        if (file.SearchName(CurSetting.IsStartMatch, CurSetting.Keyword, keyword2,
                                            false, null))
                                            handleFile(CurSetting, file.FileName);
                                        break;
                                    case SearchNormalUsing.Like:
                                        if (file.SearchName(CurSetting.IsStartMatch, CurSetting.Keyword, keyword2, true,
                                            null))
                                            handleFile(CurSetting, file.FileName);
                                        break;
                                    case SearchNormalUsing.Regex:
                                        if (file.SearchName(CurSetting.IsStartMatch, CurSetting.Keyword, keyword2,
                                            false, CurSetting.Regex))
                                            handleFile(CurSetting, file.FileName);
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            };
                        else
                            act = file =>
                            {
                                switch (CurSetting.SearchNormalUsing)
                                {
                                    case SearchNormalUsing.None:
                                        if (file.SearchName(CurSetting.IsStartMatch, CurSetting.Keyword, keyword2,
                                            false, null))
                                            listBag.Add(file.FileName);
                                        break;
                                    case SearchNormalUsing.Like:
                                        if (file.SearchName(CurSetting.IsStartMatch, CurSetting.Keyword, keyword2, true,
                                            null))
                                            listBag.Add(file.FileName);
                                        break;
                                    case SearchNormalUsing.Regex:
                                        if (file.SearchName(CurSetting.IsStartMatch, CurSetting.Keyword, keyword2,
                                            false, CurSetting.Regex))
                                            listBag.Add(file.FileName);
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            };
                        break;
                    case SearchType.SearchScript:
                        if (!CurSetting.IsUseRegexInScriptSearch)
                        {
                            var (success, resultBytes) =
                                new ScriptFileCompiler(PackService.CurrentPack).EncryptScriptText(CurSetting.Keyword,
                                    true);
                            if (!success)
                            {
                                Logger.Error("搜索 :: 片段编译错误,搜索失败");
                                return Task.FromResult(false);
                            }

                            bytes = resultBytes;
                        }

                        if (CurSetting.SearchMethod != SearchMethod.None)
                            act = file =>
                            {
                                if (!file.IsScriptFile) return;
                                if (!CurSetting.IsUseRegexInScriptSearch && file.SearchDataText(bytes))
                                {
                                    handleFile(CurSetting, file.FileName);
                                }
                                else if (CurSetting.IsUseRegexInScriptSearch && CurSetting.Regex != null)
                                {
                                    var praser = new ScriptFileParser(file, PackService.CurrentPack);
                                    if (CurSetting.Regex.IsMatch(praser.PraseText()))
                                        handleFile(CurSetting, file.FileName);
                                    praser = null;
                                }
                            };
                        else
                            act = file =>
                            {
                                if (!file.IsScriptFile) return;
                                if (!CurSetting.IsUseRegexInScriptSearch && file.SearchDataText(bytes))
                                {
                                    listBag.Add(file.FileName);
                                }
                                else if (CurSetting.IsUseRegexInScriptSearch && CurSetting.Regex != null)
                                {
                                    var praser = new ScriptFileParser(file, PackService.CurrentPack);
                                    if (CurSetting.Regex.IsMatch(praser.PraseText()))
                                        listBag.Add(file.FileName);
                                    praser = null;
                                }
                            };
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(CurSetting.SearchType), CurSetting.SearchType,
                            null);
                }

                if (CurSetting.SearchMethod != SearchMethod.None)
                {
                    var list = new List<string>(CurSetting.FileCollection.FileList);
                    if (CurSetting.SearchMethod == SearchMethod.Match)
                        CurSetting.FileCollection.FileList = new HashSet<string>();
                    foreach (var item in list)
                    {
                        var file = PackService.CurrentPack.GetFile(item);
                        if (file == null)
                            continue;
                        act(file);
                    }
                }
                else
                {
                    CurSetting.FileCollection.FileList = new HashSet<string>();
                    Parallel.ForEach(PackService.CurrentPack.FileList, (item, parallelLoopState) =>
                    {
                        if (CheckPath(CurSetting.IsUseLikeSearchPath, item.Key, CurSetting.SearchPath))
                            act(item.Value);
                    });
                    foreach (var item in listBag)
                        handleFile(CurSetting, item);
                    listBag = null;
                }

                nums = null;
                Logger.Success($"查找完毕，找到{CurSetting.FileCollection.FileList.Count}个文件。");
                CollectionExplorerPresenter.Instance.View.ShowSearchResult();

                return Task.FromResult(true);
            });
        }

        private static void handleFile(SearchSetting cmd, string fileName)
        {
            if (cmd.SearchMethod == SearchMethod.Remove)
                cmd.FileCollection.RemoveFile(fileName);
            else
                cmd.FileCollection.AddFile(fileName);
        }

        private static bool CheckPath(bool isUseLikeSearchPath, string fileName, string path)
        {
            return isUseLikeSearchPath && PathsHelper.IsPathMatchEx(fileName, path) ||
                   !isUseLikeSearchPath && PathsHelper.IsPathMatch(fileName, path);
        }
    }
}