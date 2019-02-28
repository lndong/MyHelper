using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApiSample.Helper
{
    /// <summary>
    /// api返回二进制流文件扩展
    /// </summary>
    public class FileResult : IHttpActionResult
    {
        private readonly string _filePath; //文件路径
        private readonly string _contentType; //文件mime类型

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="contentType"></param>
        public FileResult(string filePath, string contentType = null)
        {
            _filePath = filePath ?? throw new ArgumentException("filePath");
            _contentType = contentType;
        }

        /// <summary>
        /// 生成HttpResponseMessage
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(File.OpenRead(_filePath))
            };
            var contentType = _contentType ?? MimeMapping.GetMimeMapping(Path.GetExtension(_filePath));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return Task.FromResult(response);
        }
    }
}