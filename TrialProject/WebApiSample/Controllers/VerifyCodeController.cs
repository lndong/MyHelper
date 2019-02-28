using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using MyHelper.Helper;

namespace WebApiSample.Controllers
{
    /// <summary>
    /// 验证码API
    /// </summary>
    //[EnableCors(origins: "http://testhtml.com", headers: "*", methods: "*")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VerifyCodeController : ApiController
    {
        /// <summary>
        /// 获取纯数字验证
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns></returns>
        [Route("api/verifyCode/int")]
        [HttpGet]
        public string GetCode(int length)
        {
            return CaptchaHelper.GeneratorIntCode(length);
        }

        /// <summary>
        /// 获取图片验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns></returns>
        [Route("api/verifyCode/imageCode")]
        [HttpGet]
        public HttpResponseMessage GetImageCode(int length)
        {
            var code = CaptchaHelper.GeneratorMixtedCode(length);
            var imageCode = (new ImageCodeHelper()).CreateImageCode(code);
            var imageSteam = new MemoryStream();
            imageCode.Save(imageSteam,ImageFormat.Jpeg); //把图片写入到流中，此时流的当前位置是结束位置
            imageSteam.Position = 0; //把流当前位置重置到0即开始位置，然后写入到HttpResponseMessage中
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(imageSteam)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            return response;
        }
    }
}
