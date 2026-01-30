using System;
using System.Net.Http;
using System.Threading.Tasks;
using pvfUtility.Service;

namespace pvfUtility.Helper
{
    /// <summary>
    ///     HTTP 网页访问服务
    /// </summary>
    internal static class HttpHelper
    {
        public static async Task<HttpContent> Get(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var resp = await httpClient.GetAsync(url); // 创建一个HTTP请求
                    return resp.Content;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"网络连接 :: 发生错误 {e.Message}");
                MainPresenter.Instance.View.SetStatusText("网络连接发生异常");
            }

            return null;
        }
    }
}