using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using pvfUtility.Service;

namespace pvfUtility.Model
{
    /// <summary>
    ///     文件集
    /// </summary>
    internal class FileCollectionData
    {
        public HashSet<string> FileList;

        public string Name;

        public FileCollectionData(string name)
        {
            Name = name;
            FileList = new HashSet<string>();
        }

        public void AddFile(string path)
        {
            FileList.Add(path);
        }

        public void RemoveFile(string path)
        {
            FileList.Remove(path);
        }

        public async Task<bool> ImportFromTxt()
        {
            using (var openFileDialog = new OpenFileDialog
            {
                Title = "从txt导入",
                ValidateNames = true,
                CheckPathExists = true,
                DefaultExt = ".txt",
                CheckFileExists = true,
                Filter = "Text File|*.txt"
            })
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK) return false;
                var fileName = openFileDialog.FileName;
                var f = new FileInfo(fileName);
                Name = f.Name;
                await Task.Run(() =>
                {
                    var count = 0;
                    var streamReader = File.OpenText(fileName);
                    streamReader.BaseStream.Seek(0L, SeekOrigin.Begin);
                    var str = streamReader.ReadLine();
                    while (str != null)
                    {
                        count++;
                        AddFile(str.Replace('\\', '/').ToLower());
                        str = streamReader.ReadLine();
                    }

                    Logger.Success($"文件集::成功导入了{count}条数据至\"{Name}\"。");
                });
                return true;
            }
        }

        public async void ExportToTxt()
        {
            using (var saveFileDialog = new SaveFileDialog
            {
                Title = "导出到txt",
                ValidateNames = true,
                CheckPathExists = true,
                DefaultExt = ".txt",
                FileName = Name + ".txt",
                Filter = "Text File|*.txt"
            })
            {
                var count = 0;
                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
                await Task.Run(() =>
                {
                    // ReSharper disable once AccessToDisposedClosure
                    using (var streamWriter = File.CreateText(saveFileDialog.FileName))
                    {
                        foreach (var item in FileList)
                        {
                            count++;
                            streamWriter.WriteLine(item);
                        }
                    }
                });
                Logger.Success($"文件集::成功导出\"{Name}\"，共导出了{count}条数据。");
            }
        }
    }
}