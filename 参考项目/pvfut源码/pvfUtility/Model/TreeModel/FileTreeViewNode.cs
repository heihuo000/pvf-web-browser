using System.Drawing;
using pvfUtility.Dock.FileExplorer;
using pvfUtility.Model.PvfOperation;
using pvfUtility.Properties;
using pvfUtility.Service;

namespace pvfUtility.Model.TreeModel
{
    internal sealed class FileTreeViewNode
    {
        private readonly string _path;
        public readonly PvfFile File;
        public readonly bool IsFolder;
        private string _name;

        /// <summary>
        ///     Normal Node
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="isFolder"></param>
        public FileTreeViewNode(string name, string path, bool isFolder)
        {
            File = null;
            _name = name;
            _path = path;
            IsFolder = isFolder;
            if (!IsFolder)
                File = PackService.CurrentPack?.GetFile(path);
        }

        /// <summary>
        ///     Root Node
        /// </summary>
        /// <param name="name"></param>
        public FileTreeViewNode(string name)
        {
            File = null;
            _name = name;
            _path = name;
            IsFolder = true;
            if (!IsFolder)
                File = PackService.CurrentPack?.GetFile(name);
        }

        /// <summary>
        ///     项目路径
        /// </summary>
        public string ItemPath => IsFolder ? _path : File?.FileName;

        /// <summary>
        ///     名称
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                    return;
                _name = value.ToLower();
                FileExplorerPresenter.Instance.RenameItem(ItemPath, value);
                //ItemPath = ItemPath.IndexOf('/') < 0 ? _name : ItemPath.Remove(ItemPath.IndexOf('/') + 1) + _name;
            }
        }

        /// <summary>
        ///     图像Icon
        /// </summary>
        public Image Icon
        {
            get
            {
                if (IsFolder)
                    return IsExpanded ? Resources.FolderOpen_16x : Resources.Folder_16x;
                if (File == null)
                    return Resources.Document_16x;
                if (File.IsListFile)
                    return Resources.JSScript_16x;
                if (File.IsScriptFile)
                    return Resources.Script_16x;
                if (File.IsBinaryAniFile)
                    return Resources.BinaryFile_16x;
                if (File.IsNutFile)
                    return Resources.Code_16x;
                return Resources.Document_16x;
            }
        }


        /// <summary>
        ///     联动 判断是否展开
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        ///     注释
        /// </summary>
        public string Comment => File != null && File.IsUpdated
            ? string.Concat("* ", FileNameCommentService.GetComment(ItemPath))
            : FileNameCommentService.GetComment(ItemPath);

        /// <summary>
        ///     额外内容
        /// </summary>
        public string ExtraText
        {
            get
            {
                if (File == null)
                    return null;
                var str = Config.Instance.FileExplorerDisableScriptNameView
                    ? ""
                    : PackService.CurrentPack?.GetName(File);
                if (Config.Instance.FileExplorerDisableCodeView) return str;
                var code = PackService.CurrentPack?.ListFileTable.GetCode(File);
                str += code == -1 ? "" : $"<{code}>";
                return str;
            }
        }

        public override string ToString()
        {
            return ItemPath;
        }

        public override bool Equals(object obj)
        {
            return obj != null && ItemPath == obj.ToString();
        }

        public override int GetHashCode()
        {
            return ItemPath.GetHashCode();
        }
    }
}