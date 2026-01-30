using System.Collections.Generic;
using System.Linq;
using pvfUtility.Action.Extract;
using pvfUtility.Helper;
using pvfUtility.Model;
using pvfUtility.Model.TreeModel;
using pvfUtility.Shell.Docks;
using WeifenLuo.WinFormsUI.Docking;
using static pvfUtility.Service.PackService;

namespace pvfUtility.Dock.CollectionExplorer
{
    internal class CollectionExplorerPresenter
    {
        public CollectionExplorerPresenter()
        {
            View = new CollectionExplorerDock(this);
            NewFileCollection("未命名");
        }

        public static CollectionExplorerPresenter Instance { get; } = new CollectionExplorerPresenter();

        public CollectionExplorerDock View { get; }

        public List<FileCollectionData> FileCollections { get; } = new List<FileCollectionData>();

        public FileCollectionData CurFileCollection { get; private set; }

        public FileTreeViewModel FileCollectionTreeModel { get; set; }

        public string Name => "文件集合资源管理器";

        public DockContent DockView => View;

        public bool SaveLocation => true;

        public DockState DefaultDockState => DockState.DockRight;

        /// <summary>
        ///     create a new file collection
        /// </summary>
        /// <param name="name"></param>
        public void NewFileCollection(string name)
        {
            var x = new FileCollectionData(name);
            FileCollections.Add(x);
            CurFileCollection = x;
            View.AddFileCollection(x);
        }

        public void NewFileCollection(FileCollectionData fileCollection)
        {
            FileCollections.Add(fileCollection);
            CurFileCollection = fileCollection;
            View.AddFileCollection(fileCollection);
        }

        /// <summary>
        ///     移除当前文件集
        /// </summary>
        public void RemoveCurrentFileCollection()
        {
            var findIndex = FileCollections.FindIndex(x => x == CurFileCollection);
            FileCollections.RemoveAt(findIndex);
            var newIndex = findIndex;
            if (findIndex > 0)
                newIndex--;
            if (FileCollections.Count == 0)
                NewFileCollection("未命名");
            else
                CurFileCollection = FileCollections[newIndex];
            View.RemoveFileCollection(findIndex, newIndex);
        }

        public void ChangeCurFileCollection(int index)
        {
            CurFileCollection = FileCollections[index];
        }

        public void RemoveFilesInCurFileCollection()
        {
        }

        public async void ImportAsNewResultData()
        {
            var n = new FileCollectionData(null);
            if (await n.ImportFromTxt())
                NewFileCollection(n);
        }

        public string[] GetFileListFromPath()
        {
            return null;
            //TODO
//            foreach (var s in lst)
//            {
//                if (PackService.FileList.ContainsKey(s))
//                    reallst.Add(s);
//                else
//                    reallst.AddRange(from x in _presenter.CurFileCollection.FileList
//                        where PathsHelper.IsPathMatch(x, s)
//                        select x);
//            }
        }

        /// <summary>
        ///     extract all files in current file collection
        /// </summary>
        public void ExtractAllFiles()
        {
            new Extractor(CurrentPack).ShowDialog(CurFileCollection.FileList.ToArray());
        }

        /// <summary>
        ///     从选择的搜索结果文件、文件夹中取得文件列表
        /// </summary>
        /// <returns></returns>
        public string[] GetRealFileList(string[] lst)
        {
            var reallst = new List<string>();

            if (lst == null) return null;
            foreach (var s in lst)
                if (CurrentPack.FileList.ContainsKey(s))
                    reallst.Add(s);
                else
                    reallst.AddRange(from x in CurFileCollection.FileList
                        where PathsHelper.IsPathMatch(x, s)
                        select x);
            return reallst.ToArray();
        }

        public void AddFileToCurrentCollection(string fileName)
        {
            CurFileCollection.AddFile(fileName);
        }

        public void OnPackOpened()
        {
        }

        public void OnPackClosed()
        {
        }
    }
}