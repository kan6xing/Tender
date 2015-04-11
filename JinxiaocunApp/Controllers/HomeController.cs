using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinxiaocunApp.Filters;

namespace JinxiaocunApp.Controllers
{
   //[JinxcAuthorize]
    public class HomeController : Controller
    {
       //[JinxcAuthorize]
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return Content(System.DateTime.Now.TimeOfDay.ToString());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
