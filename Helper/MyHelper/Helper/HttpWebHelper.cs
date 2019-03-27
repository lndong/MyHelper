using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;
using System.IO;
using MyHelper.Extensions;
using System.Drawing;
using System.Web;

namespace MyHelper.Helper
{
    public static class HttpWebHelper
    {
        private const string DefaultUserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";

        #region 构造函数

        static HttpWebHelper()
        {
            //HTTPS时设置
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Ssl3
                | SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls;
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true);//HTTPS时设置
        }

        #endregion

        /// <summary>
        /// 下载图片并保存
        /// </summary>
        /// <param name="url">图片链接地址</param>
        /// <param name="path">图片保存地址</param>
        public static void GetImage(string url,string path)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                req.ServicePoint.Expect100Continue = false;
                req.Method = "GET";
                req.KeepAlive = false;
                req.ContentType = "image/jpg";
                HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
                Stream stream = null;
                stream = rep.GetResponseStream();
                Image.FromStream(stream).Save(path);
                stream?.Close();
                rep?.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="param">参数集合</param>
        /// <returns></returns>
        public static string Get(string url, Dictionary<string, object> param)
        {
            var body = CreateParameter(param);
            url = url + body;
            return RequestData("GET", url, body, "application/x-www-form-urlencoded"); ;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="param">参数集合</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, object> param)
        {
            var body = CreateParameter(param);
            return RequestData("post", url, body, "application/x-www-form-urlencoded");
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="body">参数拼接字符串（key1=value1&key2=value2）</param>
        /// <returns></returns>
        public static string Post(string url,string body)
        {
            return RequestData("post", url, body, "application/x-www-form-urlencoded");
        }

        #region HttpWebRequest上传文件请求包格式

        //POST http://localhost:29604/api/Test2 HTTP/1.1
        //Host: localhost:44187
        //Connection: keep-alive
        //            Content-Length: 19839
        //        Cache-Control: max-age=0
        //        Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8
        //Origin: chrome-extension://geobniafmelcledgickglbajofpkllpl
        //Upgrade-Insecure-Requests: 1
        //User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36
        //Content-Type: multipart/form-data; boundary=----WebKitFormBoundaryNSF3vGLxKBlk5kcB
        //Accept-Encoding: gzip, deflate
        //Accept-Language: zh-CN,zh;q=0.8


        //------WebKitFormBoundaryNSF3vGLxKBlk5kcB
        //Content-Disposition: form-data; name="userName"

        //admin
        //------WebKitFormBoundaryNSF3vGLxKBlk5kcB
        //Content-Disposition: form-data; name="userPwd"

        //123456
        //------WebKitFormBoundaryNSF3vGLxKBlk5kcB
        //Content-Disposition: form-data; name="photo"; filename="1.png"
        //Content-Type: image/png

        //<!--这一行是文件二进制数据-->
        //------WebKitFormBoundaryNSF3vGLxKBlk5kcB--

        //1、请求头中有一个Content-Type参数（默认值：application/x-www-form-urlencoded），
        //其中multipart/form-data值表示向服务器发送二进制数据，
        //boundary表示请求体的分界线，服务器就是依靠分界线分割请求体来读取数据，此参数值可自定义。

        //2、请求体依靠boundary有规则的排列参数。每一行字符串后面包含一个换行符“\r\n”，
        //有一个开始分界线（--boundary)和一个结束分界线（--boundary--），参数与参数之间通过--boundary分离，
        //每一个参数的键（key）和值（value）之间包含一个空行即：“\r\n"。

        #endregion

        /// <summary>
        /// Post请求上传文件并附带其它参数
        /// </summary>
        /// <param name="url">api地址</param>
        /// <param name="body">附带参数</param>
        /// <param name="files">文件集合(通过Request.Files获得)</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> body, HttpFileCollectionBase files)
        {
            //内存流，把参数和文件先放入内存流中暂存（视情况使用此流）
            //也可以直接写入requestStream流中，计算好request.ContentLength
            var memoryStream = new MemoryStream(); 
            //1.分界线时间戳(每个参数以及文件都用此分界线分隔原因暂不了解)
            var strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
            var boundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + strBoundary + "--\r\n");

            #region 2.把额外参数拼接并转化为bytes

            var bodyStr = CreateParam(body, strBoundary);
            var bodyBytes = string.IsNullOrEmpty(bodyStr) ? new byte[0] : Encoding.UTF8.GetBytes(bodyStr);
            memoryStream.Write(bodyBytes, 0, bodyBytes.Length);

            #endregion

            #region 3.组装上传文件

            var i = 0;
            var geffen = "\r\n";
            foreach (var key in files.AllKeys)
            {
                var file = files[key];
                if (file == null || file.ContentLength == 0) continue;
                var fileHeaderStr = CreateFileHeader(strBoundary, key, file.FileName, file.ContentType);
                var fileHeaderBytes = Encoding.UTF8.GetBytes(fileHeaderStr);
                memoryStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
                WriteFile(memoryStream, file.InputStream);//把文件写入内存流中
                if (i != files.AllKeys.Length - 1)
                {//只有不是最后一个文件流时才需要写入分割符                
                    var bytes = Encoding.UTF8.GetBytes(geffen);
                    memoryStream.Write(bytes, 0, bytes.Length); //然后写入一个回车到流中，在重新写入新的文件不然两个文件会合并为一个
                }
                i++;
            }

            #endregion

            //4.最后把结束分隔符写入到内存流中
            memoryStream.Write(boundaryBytes, 0, boundaryBytes.Length);

            #region 5.HttpWebRequest发送请求

            var request = CreateRequest(url);
            request.Method = "POST";
            request.ContentType = "multipart/form-data; boundary=" + strBoundary; //带上分隔标识符字符串
            var postBytes = memoryStream.ToArray();
            request.ContentLength = postBytes.Length; //不能确定大小时就不要设置此属性，不然会报错
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(postBytes, 0, postBytes.Length);
            }

            #endregion

            var v = request.GetResponse() as HttpWebResponse;
            var streamReader = new StreamReader(v.GetResponseStream(), Encoding.UTF8);
            var res = streamReader.ReadToEnd();
            memoryStream.Dispose();
            streamReader.Dispose();
            return res;

            #region API接收附带参数与文件方法

            //var files = HttpContext.Current.Request.Files;
            //foreach (var key in files.AllKeys)
            //{
            //    var file = files[key]
            //}

            //var param = HttpContext.Current.Request["key"]; //key为参数name

            #endregion
        }

        #region Private内部成员

        /// <summary>
        /// 请求数据
        /// 注：若使用证书,推荐使用X509Certificate2的pkcs12证书
        /// </summary>
        /// <param name="method">请求方法</param>
        /// <param name="url">请求地址</param>
        /// <param name="body">请求的body内容</param>
        /// <param name="contentType">请求数据类型</param>
        /// <param name="headers">请求头集合</param>
        /// <param name="cerFile">证书</param>
        /// <returns></returns>
        private static string RequestData(string method,string url,string body,string contentType,Dictionary<string,string> headers = null,X509Certificate cerFile = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("请求地址不能为null或空!");
            }
            HttpWebRequest request = CreateRequest(url);
            request.Method = method.ToUpper();
            request.ContentType = contentType;
            request.UserAgent = DefaultUserAgent;
            headers?.ForEach(aheader =>
            {
                request.Headers.Add(aheader.Key, aheader.Value);
            });
            ////HTTPS证书
            //if (cerFile != null)
            //    request.ClientCertificates.Add(cerFile);

            if(method.ToUpper() != "GET")
            {
                byte[] data = Encoding.UTF8.GetBytes(body);
                request.ContentLength = data.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }
            }

            using(HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using(Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    var resData = reader.ReadToEnd();
                    return resData;

                    #region 接收流文件，把流文件保存到指定目录

                    //var responseContentType = response.ContentType;
                    //if (!string.IsNullOrEmpty(responseContentType) &&
                    //    responseContentType.ToLower().Contains("application/octet-stream"))
                    //{
                    //    var fileNameEncode = response.Headers["Content-Disposition"].Split(new []{ "filename=" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    //    var fileName = string.Empty;
                    //    if (!string.IsNullOrEmpty(fileNameEncode))
                    //    {
                    //        fileName = HttpUtility.UrlDecode(fileNameEncode);//url解码，获取文件名
                    //    }

                    //    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                    //    //创建文件流把文件写入指定目录
                    //    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    //    {
                    //        var buff = new byte[1024 * 1024];
                    //        var size = responseStream.Read(buff, 0, buff.Length);
                    //        while (size > 0)
                    //        {
                    //            fileStream.Write(buff,0,size);
                    //            size = responseStream.Read(buff, 0, buff.Length);
                    //        }
                    //    }
                    //}

                    #endregion
                }
            }
        }

        /// <summary>
        /// 拼接参数
        /// </summary>
        /// <param name="parameters">参数集合</param>
        /// <returns></returns>
        private static string CreateParameter(IDictionary<string, object> parameters)
        {
            var buffer = new StringBuilder();
            var paramList = parameters.ToList() ?? new List<KeyValuePair<string, object>>();
            for (var i = 0; i < paramList.Count; i++)
            {
                var theParamter = paramList[i];
                var key = theParamter.Key;
                string value = theParamter.Value?.ToString();
                var head = string.Empty;
                if (i != 0)
                {
                    head = "&";
                }
                buffer.Append($@"{head}{key}={value}");
            }
            return buffer.ToString();
        }

        /// <summary>
        /// 判断url是否有参数
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static bool UrlHaveParam(string url)
        {
            return url.Contains("?");
        }

        /// <summary>
        /// 根据url创建HttpWebRequest实例
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="cerFile">证书</param>
        /// <returns></returns>
        private static HttpWebRequest CreateRequest(string url, X509Certificate cerFile = null)
        {

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            HttpWebRequest request = null;
            //如果是发送HTTPS请求
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    (sender, certificate, chain, sslPolicyErrors) => true;
                request = (HttpWebRequest) WebRequest.Create(url);
                if (cerFile != null)
                {
                    request.ClientCertificates.Add(cerFile);
                }

                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = (HttpWebRequest) WebRequest.Create(url);
            }

            return request;
        }

        /// <summary>
        /// 拼接参数（上传file时使用）
        /// </summary>
        /// <param name="body">额外参数集合</param>
        /// <param name="strBoundary">分割符</param>
        /// <returns>参数+分割符拼接字符串</returns>
        private static string CreateParam(Dictionary<string,string> body, string strBoundary)
        {
            if (body == null || body.Count <= 0) return string.Empty;
            var paramBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in body)
            {
                paramBuilder.Append("--");
                paramBuilder.Append(strBoundary); //先添加分隔符
                paramBuilder.Append("\r\n");
                paramBuilder.Append("Content-Disposition: form-data; name=\"");
                paramBuilder.Append(keyValuePair.Key);
                paramBuilder.Append("\"");
                paramBuilder.Append("\r\n");
                paramBuilder.Append("\r\n");
                paramBuilder.Append(keyValuePair.Value); //name与value使用两个回车分隔
                paramBuilder.Append("\r\n"); //回车分割下一个参数    
            }
            return paramBuilder.ToString();
        }

        /// <summary>
        /// 拼接file文件的文件头字符串
        /// </summary>
        /// <param name="strBoundary">分割符</param>
        /// <param name="key">文件的name，接收方获取该文件的key</param>
        /// <param name="fileName">文件全称带后缀</param>
        /// <param name="fileContentType">文件的ContentType</param>
        /// <returns></returns>
        private static string CreateFileHeader(string strBoundary,string key,string fileName,string fileContentType)
        {
            var sb = new StringBuilder();
            sb.Append("--");
            sb.Append(strBoundary); //分隔符
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append(key); //一个文件对应一个name
            sb.Append("\"; filename=\"");
            sb.Append(fileName);
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append(fileContentType);
            sb.Append("\r\n");
            sb.Append("\r\n");
            return sb.ToString();
        }

        /// <summary>
        /// 把File写入流中
        /// </summary>
        /// <param name="reqStream">写入的流</param>
        /// <param name="fileStream">文件流</param>
        /// <param name="bytes">每次写入流的大小，默认为4K</param>
        private static void WriteFile(Stream reqStream, Stream fileStream, int bytes = 1024 * 4)
        {
            var buff = new byte[bytes]; //每次写入
            var size = fileStream.Read(buff, 0, buff.Length);
            while (size > 0)
            {
                reqStream.Write(buff, 0, size);
                size = fileStream.Read(buff, 0, buff.Length);
            }
        }

        #endregion


    }
}
