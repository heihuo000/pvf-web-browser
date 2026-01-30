using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using pvfUtility.Action.Import;
using pvfUtility.Dock.CollectionExplorer;
using pvfUtility.Dock.FileExplorer;
using pvfUtility.Model;
using pvfUtility.Model.PvfOperation;

namespace pvfUtility.Service
{
    internal static class HttpRestServerService
    {
        private static HttpListener _listener;

        /// <summary>
        ///     启动HTTP监听服务
        /// </summary>
        public static void StartListen(int port = 27000)
        {
            try
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add($"http://127.0.0.1:{port}/");
                _listener.Prefixes.Add($"http://localhost:{port}/");
                _listener.Start(); // 创建Http监听器
                Logger.Info($"HTTP服务 :: 服务已启动在 {port} 端口,支持第三方访问,打开多个软件端口会变化,请注意");
            }
            catch (HttpListenerException e)
            {
                StartListen(port + 1);
                return;
            }

            //处理请求
            Task.Run(HandleIncomingConnections);
        }

        public static void CloseListen()
        {
            //关闭HTTP服务
            _listener.Close();
        }

        private static async Task HandleIncomingConnections()
        {
            while (_listener.IsListening)
            {
                var ctx = await _listener.GetContextAsync(); //等待接收请求

                var req = ctx.Request;
                var resp = ctx.Response; //拆分响应和请求
                try
                {
                    resp.ContentType = "text/plain";
                    resp.ContentEncoding = Encoding.UTF8;

                    var stream = new MemoryStream();
                    switch (req.Url.AbsolutePath)
                    {
                        case "/file":
                        {
                            var fileName = req.QueryString.Get("name");
                            var file = PackService.CurrentPack.GetFile(fileName);

                            switch (req.HttpMethod)
                            {
                                case "GET":
                                    HttpExtractFile(file, resp, stream, fileName);
                                    break;
                                case "POST":
                                    await HttpImportFile(req, file, fileName, resp);
                                    break;
                                case "DELETE":
                                    HttpDeleteFile(stream, fileName, resp);
                                    break;
                            }

                            break;
                        }
                        case "/list":
                            HttpListFile(req, stream, resp);
                            break;
                        case "/listSearch":
                            HttpListSearch(req, stream, resp);
                            break;
                    }

                    resp.ContentLength64 = stream.Length;
                    //先设定长度 再写入数据
                    await resp.OutputStream.WriteAsync(stream.GetBuffer(), 0, (int) stream.Length);
                }
                catch (Exception e)
                {
                    Logger.Error($"HTTP服务 :: 发生异常{e.Message}");
                }
                finally
                {
                    resp.Close(); //关闭流
                }
            }
        }

        private static void HttpListSearch(HttpListenerRequest req, MemoryStream stream, HttpListenerResponse resp)
        {
            var listWriter = new StreamWriter(stream);
            foreach (var item in CollectionExplorerPresenter.Instance.CurFileCollection.FileList)
                listWriter.WriteLine(item);

            listWriter.Flush();
            resp.StatusCode = (int) HttpStatusCode.OK;
            Logger.Success("HTTP服务 :: 成功列出搜索结果");
        }

        /// <summary>
        ///     HTTP请求列出文件
        /// </summary>
        /// <param name="req"></param>
        /// <param name="stream"></param>
        /// <param name="resp"></param>
        private static void HttpListFile(HttpListenerRequest req, MemoryStream stream, HttpListenerResponse resp)
        {
            var pathName = req.QueryString.Get("path");
            var listWriter = new StreamWriter(stream);
            foreach (var item in PackService.CurrentPack.GetFileObjs(pathName)) listWriter.WriteLine(item.FileName);

            listWriter.Flush();
            resp.StatusCode = (int) HttpStatusCode.OK;
            Logger.Success($"HTTP服务 :: 成功列出文件夹{pathName}");
        }

        /// <summary>
        ///     HTTP请求删除文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <param name="resp"></param>
        private static void HttpDeleteFile(MemoryStream stream, string fileName, HttpListenerResponse resp)
        {
            lock (PackService.CurrentPack)
            {
                var writer = new StreamWriter(stream);
                foreach (var item in FileExplorerPresenter.Instance.DeleteFile(fileName)) writer.WriteLine(item);
            }

            resp.StatusCode = (int) HttpStatusCode.OK;
            Logger.Success($"HTTP服务 :: 成功删除文件(夹){fileName}");
        }

        /// <summary>
        ///     HTTP请求导入文件
        /// </summary>
        /// <param name="req"></param>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <param name="resp"></param>
        /// <returns></returns>
        private static async Task HttpImportFile(HttpListenerRequest req, PvfFile file, string fileName,
            HttpListenerResponse resp)
        {
            var source = new MemoryStream();
            await req.InputStream.CopyToAsync(source);
            lock (PackService.CurrentPack)
            {
                new Importer(PackService.CurrentPack).ImportFile(fileName, source);
            }

            resp.StatusCode = (int) HttpStatusCode.OK;
            Logger.Success($"HTTP服务 :: 成功写入文件(记得刷新){fileName}");
        }

        /// <summary>
        ///     HTTP请求提取文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="resp"></param>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        private static void HttpExtractFile(PvfFile file, HttpListenerResponse resp, Stream stream, string fileName)
        {
            if (file == null)
            {
                resp.StatusCode = (int) HttpStatusCode.NotFound;
                return;
            }

            var data = PackService.CurrentPack.ExtractFile(stream, file,
                Config.Instance.ExtractDecompileBinaryAni, Config.Instance.ExtractDecompileScript,
                Config.Instance.ExtractConvertSimplifiedChinese);
            if (!data)
            {
                resp.StatusCode = (int) HttpStatusCode.InternalServerError;
                return;
            }

            resp.ContentType = "application/octet-stream";
            resp.ContentEncoding = null;

            Logger.Success($"HTTP服务 :: 成功读取文件{fileName}");
        }
    }
}