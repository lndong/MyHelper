using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelValid.Models;
using MyHelper.Extensions;
using MyHelper.Helper;

namespace ModelValid.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Register()
        {
            var genderList = GenderType.Male.EnumListDic("0", "请选择性别");
            var genderSelectList = new SelectList(genderList, "key", "Value");
            ViewBag.GenderList = genderSelectList;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveInfo(UserInfo model)
        {
            ModelState.Remove("IgnorePhone"); //忽略对某字段的验证
            //ModelState.IsValidField("IgnorePhone");  对单个字段进行验证
            if (ModelState.IsValid)
            {
                ModelState.AddModelError("Phone","手机号已经使用过");
            }

            ModelState.AddModelError("Email", "邮箱已经使用过");
            var keys = new string[] {"Account", "Password", "UserName", "Phone", "Email", "Gender"};
            var modelStateErrors = GetModelStateErrors(keys, ModelState);
            var returnData = new {success = false, ModelStateErrors = modelStateErrors};
            return Json(returnData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 远程验证用户名，可以返回不同错误信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidAccount()
        {
            var account = Request["Account"];
            if (string.IsNullOrEmpty(account))
            {
                return Json("请填写账户名", JsonRequestBehavior.AllowGet);
            }

            var accs = new string[] {"aa", "bb", "cc", "dd"};
            if (accs.Contains(account))
            {
                return Json("该账号已存在", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 构建图形验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateCode()
        {
            var code = CaptchaHelper.GeneratorMixtedCode(4);
            var imageCode = (new ImageCodeHelper()).CreateImageCode(code);
            Session["VerifyCode"] = code;
            var ms = new MemoryStream();
            imageCode.Save(ms,ImageFormat.Png);
            return File(ms.ToArray(), "text/html");//返回text/html在浏览器中直接输入url会是二进制流，使用img或者css接收则是图片
        }

        /// <summary>
        /// 根据key获取验证错误信息
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        private Dictionary<string, string[]> GetModelStateErrors(string[] keys, ModelStateDictionary modelState)
        {
            return modelState.Where(x => keys.Contains(x.Key)).Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(k => k.Key, k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray());
        }
    }
}