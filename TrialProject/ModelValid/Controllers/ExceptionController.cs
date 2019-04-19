using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyHelper.Custom;

namespace ModelValid.Controllers
{
    public class ExceptionController : Controller
    {
        // GET: Exception
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [CustomErrorFilter]
        public ActionResult CustomError()
        {
            var i = int.Parse(Request["param"]);
            return Json(new {data = 10 / i}, JsonRequestBehavior.AllowGet);
        }
    }
}