using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace MyHelper.Helper
{
    public class HttpClientUtility
    {
        private static HttpClient _myHttpClient;

        private static DecompressionMethods _decompression = DecompressionMethods.None;

        public static HttpClient MyHttpClient => _myHttpClient ?? (_myHttpClient = GetHttpClient(_decompression));

        /// <summary>
        /// 设定内容解压缩方法的处理程序
        /// </summary>
        /// <param name="compression"></param>
        /// <returns></returns>
        private static HttpClientHandler GetHandler(DecompressionMethods compression)
        {
            return new HttpClientHandler
            {
                AutomaticDecompression = compression
            };
        }

        /// <summary>
        /// 生成HttpClient实例
        /// </summary>
        /// <param name="compression"></param>
        /// <returns></returns>
        private static HttpClient GetHttpClient(DecompressionMethods compression)
        {
            var httpHandler = GetHandler(compression);
            return new HttpClient(httpHandler);
        }

        #region 异步发送Get请求

        public static string GetHttpRequest(string url)
        {
            return GetHttpRequest(url, null);
        }

        /// <summary>
        /// 异步get请求
        /// </summary>
        /// <param name="url">请求URL，不带参数</param>
        /// <param name="dictionary">请求参数集合</param>
        /// <param name="compression">压缩方式,默认无压缩</param>
        /// <returns></returns>
        public static string GetHttpRequest(string url, Dictionary<string, string> dictionary,
            DecompressionMethods compression = DecompressionMethods.None)
        {
            try
            {
                _decompression = compression;
                var param = CreateParams(dictionary);
                if (!string.IsNullOrEmpty(param))
                {
                    url = url + "?" + param;
                }
                var res = MyHttpClient.GetAsync(url).Result;
                return res.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            finally
            {
                Dispose();
            }
        }

        #endregion

        #region 异步发送发送post请求

        /// <summary>
        /// 异步发送发送post请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static string PostHttpRequest(string url, object entity)
        {
            var objStr = JsonConvert.SerializeObject(entity);
            //var httpContent = new StringContent(objStr, Encoding.UTF8, "application/json");
            var httpContent = new StringContent(objStr, Encoding.UTF8, "application/x-www-form-urlencoded");
            return PostHttpRequest(url, httpContent);
        }

        /// <summary>
        /// 异步发送发送post请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="dictionary">参数集合</param>
        /// <param name="compression">压缩方式(默认没有)</param>
        /// <returns></returns>
        public static string PostHttpRequest(string url, IDictionary<string, string> dictionary,
            DecompressionMethods compression = DecompressionMethods.None)
        {
            var httpContent = new FormUrlEncodedContent(dictionary);
            return PostHttpRequest(url, httpContent);
        }

        /// <summary>
        /// 上传文件请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="files"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string PostHttpRequest(string url, HttpFileCollectionBase files, IDictionary<string, string> dic)
        {
            var httpContent = GetMultipartFormDataContent(files, dic);
            return PostHttpRequest(url, httpContent);
        }

        /// <summary>
        /// 共用异步发送发送post请求
        /// </summary>
        /// <param name="url">请求url地址</param>
        /// <param name="httpContent">发送到服务器的 HTTP 请求内容</param>
        /// <param name="compression"></param>
        /// <returns></returns>
        public static string PostHttpRequest(string url, HttpContent httpContent, DecompressionMethods compression = DecompressionMethods.None)
        {
            try
            {
                _decompression = compression;
                var res = MyHttpClient.PostAsync(url, httpContent).Result;
                return res.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            finally
            {
                Dispose();
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        ///创建multipart/form-data的Http请求内容
        /// </summary>
        /// <param name="files">文件集合</param>
        /// <param name="dic">额外参数列表集合</param>
        /// <returns></returns>
        private static MultipartFormDataContent GetMultipartFormDataContent(HttpFileCollectionBase files,
            IDictionary<string, string> dic)
        {
            var strBoundary = DateTime.Now.Ticks.ToString("x"); //分割符
            var resultContent = new MultipartFormDataContent(strBoundary);

            //委托
            Action<List<ByteArrayContent>> act = (dataContents) =>
            {
                foreach (var content in dataContents)
                {
                    resultContent.Add(content);
                }
            };
            var fileByteContents = GetFileByteArrayContents(files);
            act(fileByteContents); //添加附件
            var paramByteContents = GetParamByteArrayContent(dic);
            act(paramByteContents); //添加附加参数
            return resultContent;
        }

        /// <summary>
        /// 把发送附件时附带的参数转换成二进制流内容
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        private static List<ByteArrayContent> GetParamByteArrayContent(IDictionary<string, string> dic)
        {
            var list = new List<ByteArrayContent>();
            if (dic == null || dic.Count == 0) return list;
            foreach (KeyValuePair<string, string> keyValuePair in dic)
            {
                var valueBytes = Encoding.UTF8.GetBytes(keyValuePair.Value);
                var byteArray = new ByteArrayContent(valueBytes);
                //byteArray.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                byteArray.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    Name = keyValuePair.Key
                };
                list.Add(byteArray);
            }
            return list;
        }

        /// <summary>
        /// 把附件集合转为二进制集合
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private static List<ByteArrayContent> GetFileByteArrayContents(HttpFileCollectionBase files)
        {
            var list = new List<ByteArrayContent>();
            foreach (var key in files.AllKeys)
            {
                var file = files[key];
                if (file == null || file.ContentLength <= 0) continue;
                var byteArrayContent = GetFileByteArrayContent(key, file);
                list.Add(byteArrayContent);
            }

            return list;
        }

        /// <summary>
        /// 把附件转换成字节数组的http内容
        /// </summary>
        /// <param name="key">文件key，服务器获取文件的name（即Request.Files[key]）</param>
        /// <param name="fileBase">文件HttpPostedFileBase对象</param>
        /// <returns></returns>
        private static ByteArrayContent GetFileByteArrayContent(string key, HttpPostedFileBase fileBase)
        {
            var fileName = fileBase.FileName;
            var inputStream = fileBase.InputStream;
            var fileContent = fileBase.ContentType;
            var fileLength = fileBase.ContentLength;
            var buff = new byte[fileLength];
            inputStream.Read(buff, 0, fileLength);
            return GetFileByteArrayContent(key, fileName, fileContent, buff);
        }

        /// <summary>
        /// 把附件转换成字节数组的http内容
        /// </summary>
        /// <param name="key">文件key，服务器获取文件的name（即Request.Files[key]）</param>
        /// <param name="fileName">文件的名称带后缀</param>
        /// <param name="fileContent">文件的ContentType</param>
        /// <param name="fileBytes">文件的二进制流</param>
        /// <returns></returns>
        private static ByteArrayContent GetFileByteArrayContent(string key, string fileName, string fileContent,
            byte[] fileBytes)
        {
            var fileByteArrayContent = new ByteArrayContent(fileBytes);
            fileByteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(fileContent);
            fileByteArrayContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                Name = key,
                FileName = fileName
            };
            return fileByteArrayContent;
        }

        /// <summary>
        /// 拼接url参数
        /// </summary>
        /// <param name="dic">参数集合</param>
        /// <returns></returns>
        private static string CreateParams(IDictionary<string, string> dic)
        {
            if (dic == null || dic.Count <= 0) return string.Empty;
            var buff = new StringBuilder();
            var i = 0;
            foreach (KeyValuePair<string, string> param in dic)
            {
                var key = param.Key;
                var value = HttpUtility.UrlEncode(param.Value);
                var head = string.Empty;
                if (i > 0)
                {
                    head = "&";
                }
                buff.Append($"{head}{key}={value}");
            }

            return buff.ToString();

        }

        #endregion

        /// <summary>
        /// 回收资源
        /// </summary>
        private static void Dispose()
        {
            _myHttpClient?.Dispose();
            _myHttpClient = null;
            _decompression = DecompressionMethods.None;
        }
    }
}
