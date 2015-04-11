using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinxiaocunApp.Filters;

namespace JinxiaocunApp.Controllers
{
    
    public class MyTartController : Controller
    {
        //
        // GET: /MyTart/

        JinxiaocunApp.Models.JinxiaocunAppContext db = new Models.JinxiaocunAppContext();
        [MultipleResponseFormats]
        public ActionResult Index()
        {
            return View();
        }

        [MultipleResponseFormats]
        public ActionResult Error(string err="错误",string Act="index",string Con="Home")
        {
            ViewBag.err = err;
            ViewBag.Act = Act;
            ViewBag.Con = Con;
            return View();
        }

        [MultipleResponseFormats]
        public ActionResult MenuPartial(string id="id",string tabName="新Tab",int tabid=-99)
        {
            ViewBag.divid = id;
            ViewBag.name = tabName;
            if(tabid!=-99)
            ViewBag.MenuM = db.BPartMenus.Where(m => m.BParentID == tabid).ToList();
            return View();
        }

        [MultipleResponseFormats]
        public ActionResult PaginationPartial(string id = "Paginationid", int TotalPage = 10, string tbodyPages = "", string ControllerName="",string PageName="Pages",string PageParam="")
        {
            ViewBag.divid = id;
            ViewBag.TotalPage = TotalPage==0?1:TotalPage;
            ViewBag.tbodyPages = tbodyPages;
            ViewBag.ControllerName = ControllerName;
            ViewBag.PageName = PageName;
            ViewBag.PageParam = PageParam;
            return View();
        }

        public ActionResult Part1()
        {
            return PartialView();
        }

        public ActionResult Part2()
        {
            return PartialView();
        }

        public ActionResult SystemM()
        {
            return PartialView();
        }

        [MultipleResponseFormats]
        public ActionResult IndexM()
        {
            return View();
        }

        public ActionResult Part3()
        {
            return Content("建设中...");
        }
        public ActionResult Part4()
        {
            return Content("Part4"+Request["aa"]+Request["bb"]);
        }
        public ActionResult Part5()
        {
            return Content("<div>Part5</div>");
        }
    }
}
