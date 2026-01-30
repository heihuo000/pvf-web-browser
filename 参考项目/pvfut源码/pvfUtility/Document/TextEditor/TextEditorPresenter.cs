using System;
using System.Linq;
using System.Text;
using pvfUtility.Dock.ErrorList;
using pvfUtility.Dock.FileExplorer;
using pvfUtility.Document.HexEditor;
using pvfUtility.Helper.Native;
using pvfUtility.Interface.View;
using pvfUtility.Model;
using pvfUtility.Model.PvfOperation;
using pvfUtility.Model.PvfOperation.Encoder;
using pvfUtility.Model.PvfOperation.Praser;
using pvfUtility.Service;
using pvfUtility.Shell.Document.TextEditor;
using static pvfUtility.Service.PackService;

namespace pvfUtility.Document.TextEditor
{
    internal class TextEditorPresenter
    {
        private const string BinaryAniParseError = "PvfUtility BinaryAni Parse Error";
        public static TextEditorPresenter Instance { get; } = new TextEditorPresenter();

        public static TextEditorView CurrentTextEditorView =>
            MainPresenter.Instance.View.CurrentDockDocument is TextEditorView textEditorView ? textEditorView : null;

        /// <summary>
        ///     创建文本编辑器 (内部)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="setNode"></param>
        /// <param name="isHex"></param>
        /// <returns></returns>
        public (bool exist, bool isNew, IEditor editor) CreateInternal(string fileName, bool setNode = false,
            bool isHex = false)
        {
            fileName = fileName.ToLower().Replace('\\', '/');
            if (setNode && !Config.Instance.DisableNodeJump)
                FileExplorerPresenter.Instance.View.SetNodeTo(fileName);
            var current = MainPresenter.Instance.View.Documents.FirstOrDefault(
                editor => editor.FileName == fileName &&
                          (isHex && editor is HexEditorFrom || !isHex && editor is TextEditorView));
            if (current != null)
            {
                MainPresenter.Instance.View.ShowDocument(current);
                return (true, false, current);
            }

            if (CurrentPack?.GetFile(fileName) == null)
            {
                DialogService.Error("此文件不存在", fileName, MainPresenter.Instance.View.Handle);
                return (false, false, null);
            }

            if (!isHex)
            {
                var textEditor = new TextEditorView(fileName);
                textEditor.Show(MainPresenter.Instance.View.dockpanel);
                return (true, true, textEditor);
            }

            var hexEditor = new HexEditorFrom(fileName);
            hexEditor.Show(MainPresenter.Instance.View.dockpanel);
            return (true, true, hexEditor);
        }

        /// <summary>
        ///     创建文本编辑器(外部)
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="fileName"></param>
        /// <param name="externalId"></param>
        /// <returns></returns>
        public (bool isNew, IEditor editor) CreateExternal(string fileText, string fileName, Guid externalId)
        {
            var current = MainPresenter.Instance.View.Documents.FirstOrDefault(editor =>
                editor.IsExternalFile && editor.ExternalContentId == externalId);
            if (current != null)
            {
                MainPresenter.Instance.View.ShowDocument(current);
                return (false, current);
            }

            var newEditor = new TextEditorView(fileText, fileName, externalId);
            newEditor.Show(MainPresenter.Instance.View.dockpanel);
            return (true, newEditor);
        }

        /// <summary>
        ///     获取一个文件的文本策略
        /// </summary>
        /// <param name="file"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string GetFileText(PvfFile file, EncodingType encoding)
        {
            if (file.Data == null) return "";
            if (file.IsScriptFile)
                return Config.Instance.UseCompatibleDecompiler
                    ? new ScriptFileCompiler(CurrentPack).Decompile(file)
                    : new ScriptFileParser(file, CurrentPack).PraseText();
            if (!file.IsBinaryAniFile)
                return Encoding.GetEncoding((int) encoding).GetString(file.Data).TrimEnd(new char[1]);
            var (result, text) = BinaryAniCompiler.DecompileBinaryAni(file);
            if (result) return text;
            DialogService.Error("二进制Ani文件解析失败，查看“输出”窗口了解更多信息。", "", MainPresenter.Instance.View.Handle);
            return BinaryAniParseError;
        }

        /// <summary>
        ///     保存一个文件的逻辑策略
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileText"></param>
        /// <param name="encoding"></param>
        public bool SaveFileText(PvfFile file, string fileText, EncodingType encoding)
        {
            if (Config.Instance.EditorConvertTraditionalChinese) //如果设置里面有繁体中文转换,转换为繁体
                fileText = ChineseHelper.ToTraditional(fileText);

            if (file.IsScriptFile) //脚本文件检测
            {
                if (fileText.Length > 10 && fileText.Substring(0, 9) == "#PVF_File")
                    return SaveFileAsScript(file, fileText, encoding); //保存为脚本文件
                var dr = DialogService.AskYesNo(
                    "源文件显示它属于脚本文件，但未能找到标识符 #PVF_File ,是否仍保存为脚本文件？", "",
                    MainPresenter.Instance.View.Handle);
                return !dr ? SaveFileAsTextFile(file, fileText, encoding) : SaveFileAsScript(file, fileText, encoding);
            }

            //非脚本文件的情况
            if (file.IsBinaryAniFile)
                return SaveFileAsBinaryAni(file, fileText, encoding); //以ani形式保存
            if (fileText.Length > 10 && fileText.Substring(0, 9) == "#PVF_File")
                return SaveFileAsScript(file, fileText, encoding);
            return SaveFileAsTextFile(file, fileText, encoding);
        }

        /// <summary>
        ///     保存为脚本文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileText"></param>
        /// <returns></returns>
        public bool SaveFileAsScript(PvfFile file, string fileText, EncodingType encoding)
        {
            ErrorListPresenter.Instance.RemoveError(file.FileName); //移除错误列表中的错误
            var (bytes, errorList) = new ScriptFileCompiler(CurrentPack).Compile(file, fileText);
            if (bytes != null)
            {
                file.WriteFileData(bytes);
                if (file.IsListFile)
                {
                    if (file.FileName.IndexOf('/') < 0) return true;
                    var basePath = file.FileName.Substring(0, file.FileName.IndexOf('/'));
                    if (CurrentPack.ListFileTable.FileCodeDictionary.ContainsKey(basePath))
                    {
                        CurrentPack.ListFileTable.LoadList(basePath, file, CurrentPack.Strtable); // 修复bug
                        Logger.Success("成功重载lst：" + file.FileName);
                    }
                }

                return true;
            }

            foreach (var item in errorList)
            {
                item.IsExternal = false;
                item.Action = "保存文件";
                ErrorListPresenter.Instance.AddError(item);
            }

            //TODO 
            return false;
        }

        /// <summary>
        ///     保存为Ani
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileText"></param>
        /// <returns></returns>
        public bool SaveFileAsBinaryAni(PvfFile file, string fileText, EncodingType encoding)
        {
            ErrorListPresenter.Instance.RemoveError(file.FileName); //移除成为
            var (result, data, error) = BinaryAniCompiler.CompileBinaryAni(fileText, file.FileName);
            if (!result)
            {
                error.IsExternal = false;
                error.Action = "保存文件";
                ErrorListPresenter.Instance.AddError(error);

                //DialogService.Error("二进制Ani文件解析失败，查看“输出”窗口了解更多信息。", "", MainPresenter.Instance.View.Handle);
            }
            else
            {
                file.WriteFileData(data);
            }

            return true;
        }

        public bool SaveFileAsTextFile(PvfFile file, string fileText, EncodingType encoding)
        {
            switch (encoding)
            {
                case EncodingType.TW:
                    fileText = ChineseHelper.ToTraditional(fileText);
                    break;
                case EncodingType.CN:
                    fileText = ChineseHelper.ToSimplified(fileText);
                    break;
                case EncodingType.KR:
                    break;
                case EncodingType.JP:
                    break;
                case EncodingType.UTF8:
                    // if (Config.Instance.EditorConvertTraditionalChinese)
                    //     fileText = ChineseHelper.ToTraditional(fileText);
                    var bytesUtf = Encoding.UTF8.GetBytes(fileText);
                    file.WriteFileData(bytesUtf);
                    break;
                case EncodingType.Unicode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(encoding), encoding, null);
            }

            var bytesC = Encoding.GetEncoding((int) encoding).GetBytes(fileText);
            file.WriteFileData(bytesC);

            if (file.FileName.IndexOf(".str", StringComparison.OrdinalIgnoreCase) > 0) //reload Str
                CurrentPack.Strview.ReloadstrFile(file.FileName, fileText);
            return true;
        }
    }
}