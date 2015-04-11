using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinxiaocunApp.Models;
using JinxiaocunApp.Filters;

namespace JinxiaocunApp.Controllers.Tender
{
    public class Tender_GongGaoController : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();
        private int PageSize = 15;
        //
        // GET: /Tender_GongGao/

        [MultipleResponseFormats]
        public ActionResult Index(string id="")
        {
            ViewBag.SuperRef = id;
            ViewBag.PageDiv = Guid.NewGuid().ToString();
            ViewBag.Pagination = Guid.NewGuid().ToString();

            

            if (ViewBag.PageSize == null)
            {
                ViewBag.PageSize = PageSize;
                ViewBag.CurrentPage = 1;
                //ViewBag.TotalRecord = db.Tender_GongGaos.Count();
                //ViewBag.TotalPage = Math.Ceiling(ViewBag.TotalRecord / (double)ViewBag.PageSize);

            }

            if(!string.IsNullOrEmpty(id)&&int.Parse(id)==1)
            {
                ViewBag.TotalRecord = db.Tender_GongGaos.Where(m => (!(m.IsDelete != null && m.IsDelete == true)) && (!(m.IsZhongb != null && m.IsZhongb))).Count();
                ViewBag.TotalPage = Math.Ceiling(ViewBag.TotalRecord / (double)ViewBag.PageSize);
            }else
            {
                ViewBag.TotalRecord = db.Tender_GongGaos.Where(m => (!(m.IsDelete != null && m.IsDelete == true)) && (m.IsZhongb != null && m.IsZhongb)).Count();
                ViewBag.TotalPage = Math.Ceiling(ViewBag.TotalRecord / (double)ViewBag.PageSize);
                
            }

            return View(db.Tender_GongGaos.Where(m=>m.TaskID==1));
        }

        [MultipleResponseFormats]
        [Authorize(Roles = "admin")]
        public ActionResult IndexAdmin(string id = "")
        {
            ViewBag.SuperRef = id;
            ViewBag.PageDiv = Guid.NewGuid().ToString();
            ViewBag.Pagination = Guid.NewGuid().ToString();



            if (ViewBag.PageSize == null)
            {
                ViewBag.PageSize = PageSize;
                ViewBag.CurrentPage = 1;
                ViewBag.TotalRecord = db.Tender_GongGaos.Count();
                ViewBag.TotalPage = Math.Ceiling(ViewBag.TotalRecord / (double)ViewBag.PageSize);

            }
            return View(db.Tender_GongGaos.Where(m => m.TaskID == 1));
        }

        [MultipleResponseFormats]
        public ActionResult TopIndex(int id=0)
        {
            ViewBag.ycid = id;
            DateTime dtnow=DateTime.Parse(System.DateTime.Now.Date.ToShortDateString());
            if (id == 0)
            {
                return View(db.Tender_GongGaos.Where(m => (!(m.IsDelete != null && m.IsDelete == true)) && !(m.IsZhongb != null && m.IsZhongb)).OrderByDescending(m => m.TaskID).OrderByDescending(m => m.BeginDate).Take(15).ToList());
            }
            else
            {
                return View(db.Tender_GongGaos.Where(m => (!(m.IsDelete != null && m.IsDelete == true)) && (m.IsZhongb != null && m.IsZhongb)).OrderByDescending(m => m.TaskID).OrderByDescending(m => m.BeginDate).Take(8).ToList());
            }
            return View(db.Tender_GongGaos.OrderByDescending(m => m.TaskID).OrderByDescending(m => m.BeginDate).Take(5).ToList());
        }
        
        public ActionResult Pages(int id = 1, string SuperRef = "")
        {
            ViewBag.ProType = db.Tender_ProjectTypes.ToList();

            ViewBag.SuperRef = SuperRef;
            ViewBag.CurrentPage = id;

            if(SuperRef.Trim().Equals("1"))
            {
                return PartialView(db.Tender_GongGaos.Where(m => (!(m.IsDelete != null && m.IsDelete == true)) && (!(m.IsZhongb != null && m.IsZhongb))).OrderByDescending(m => m.TaskID).OrderByDescending(m => m.BeginDate).Skip((id - 1) * PageSize).Take(PageSize).ToList());
            }
            else
            {
                return PartialView(db.Tender_GongGaos.Where(m => (!(m.IsDelete != null && m.IsDelete == true)) && (m.IsZhongb != null && m.IsZhongb)).OrderByDescending(m => m.TaskID).OrderByDescending(m => m.BeginDate).Skip((id - 1) * PageSize).Take(PageSize).ToList());
            }
            //ViewBag.tid = id + "  " + new Random().Next();
            return PartialView(db.Tender_GongGaos.Where(m=>!(m.IsDelete!=null&&m.IsDelete==true)).OrderByDescending(m => m.TaskID).OrderByDescending(m => m.BeginDate).Skip((id - 1) * PageSize).Take(PageSize).ToList());
            //return PartialView(db.BDepartments.ToList());
        }

        [Authorize(Roles = "admin")]
        public ActionResult PagesAdmin(int id = 1, string SuperRef = "")
        {
            ViewBag.ProType = db.Tender_ProjectTypes.ToList();

            ViewBag.SuperRef = SuperRef;
            ViewBag.CurrentPage = id;

            //ViewBag.tid = id + "  " + new Random().Next();
            return PartialView(db.Tender_GongGaos.OrderByDescending(m => m.TaskID).OrderByDescending(m => m.BeginDate).Skip((id - 1) * PageSize).Take(PageSize).ToList());
            //return PartialView(db.BDepartments.ToList());
        }

        //[MultipleResponseFormats]
        //public ActionResult NoTenderIndex(string id="")
        //{
        //    ViewBag.SuperRef = id;
        //    ViewBag.PageDiv = Guid.NewGuid().ToString();
        //    ViewBag.Pagination = Guid.NewGuid().ToString();

        //    if (ViewBag.PageSize == null)
        //    {
        //        ViewBag.PageSize = PageSize;
        //        ViewBag.CurrentPage = 1;
        //        ViewBag.TotalRecord = db.Tender_GongGaos.Count();
        //        ViewBag.TotalPage = Math.Ceiling(ViewBag.TotalRecord / (double)ViewBag.PageSize);

        //    }
        //    return View(db.Tender_GongGaos.Where(m => m.TaskID == 1));
        //}


        //public ActionResult NoTenderPages(int id = 1, string SuperRef = "")
        //{
        //    ViewBag.SuperRef = SuperRef;
        //    ViewBag.CurrentPage = id;
        //    //db.Bemp_GongGaos.Include(m=>m.Tender_GongGaos).Where(m=>m.UserName==User.Identity.Name)
        //    return PartialView(db.Tender_GongGaos.Include(m=>m.Bemp_GongGaos).OrderByDescending(m => m.TaskID).Skip((id - 1) * PageSize).Take(PageSize).ToList());
            
        //}

        //
        // GET: /Tender_GongGao/Details/5

        [MultipleResponseFormats]
        public ActionResult Details(int id = 0)
        {
            ViewBag.ProType = db.Tender_ProjectTypes.ToList();

            Tender_GongGao tender_gonggao = db.Tender_GongGaos.Include("Tender_GongGao_Items").Include(m=>m.Tender_Purchase_GongGaos).Single(m=>m.TaskID==id);
            //ViewBag.bgid = null;
            ViewBag.bgid = 0;
            ViewBag.M1 = false;
            ViewBag.M2 = false;
            if (Request.IsAuthenticated)
            { 
                int empid=(db.BEmplyees.Single(n=>n.NumberEmp==User.Identity.Name).EmpID);
                
                if (db.Tender_ModelManage1.Where(m => m.Tid == id).Count() > 0)
                {
                    ViewBag.M1 = true;
                }
                if (db.Tender_ModelManage2.Where(m => m.Tid == id).Count() > 0)
                {
                    ViewBag.M2 = true;
                }

                if (db.Bemp_GongGaos.Where(m => m.UserId == empid && m.GongGaoId == id&&m.IsPassShen!=null&&m.IsPassShen==true).Count() > 0)
                { 
                    ViewBag.bgid= db.Bemp_GongGaos.Single(m => m.UserId == empid && m.GongGaoId==id).BGid;
                }
                
            }
            

            if (tender_gonggao == null)
            {
                return HttpNotFound();
            }
            return View(tender_gonggao);
        }

        [JinxcAuthorize]
        public ActionResult TenderRequest(int id=0)
        {

            //************* 添加  验证  已不能审核 时间超等****
            Tender_GongGao tender_gonggao = db.Tender_GongGaos.Include("Bemp_GongGaos.bemplyees").Single(m => m.TaskID == id);
            //if (tender_gonggao != null && tender_gonggao.Bemp_GongGaos.Count > 0)
            //{
            //    //if (tender_gonggao.Bemp_GongGaos.Contains(db.Bemp_GongGaos.Single(m => m.bemplyees.NumberEmp == User.Identity.Name&&m.GongGaoId==id)))
            //    //{
            //    //    return Content("请不要重复申请");
            //    //}
            if(tender_gonggao.IsShenhe!=null&&tender_gonggao.IsShenhe==true)
            {
                return Content("投标申请已结束！");
            }
                if (tender_gonggao.Bemp_GongGaos.Where(m=>m.GongGaoId==id&&m.UserName==User.Identity.Name).Count()>0)
                {
                    return Content("请不要重复申请");
                }

            //}
            Bemp_GongGao empG=new Bemp_GongGao();
            empG.GongGaoId=id;
            empG.UserId=db.BEmplyees.Single(emp=>emp.NumberEmp== User.Identity.Name).EmpID;
            empG.UserName =User.Identity.Name;
            tender_gonggao.Bemp_GongGaos.Add(empG);
            
            //db.Entry(tender_gonggao).State = EntityState.Modified;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            db.Configuration.ValidateOnSaveEnabled = true;
            return Content("申请成功，等待审核");
            
            
        }
        //
        // GET: /Tender_GongGao/Create

        [MultipleResponseFormats]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Tender_GongGao/Create

        [MultipleResponseFormats]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tender_GongGao tender_gonggao)
        {
            if (ModelState.IsValid)
            {
                db.Tender_GongGaos.Add(tender_gonggao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tender_gonggao);
        }

        //
        // GET: /Tender_GongGao/Edit/5
        [MultipleResponseFormats]
        public ActionResult Edit(int id = 0)
        {
            Tender_GongGao tender_gonggao = db.Tender_GongGaos.Find(id);
            if (tender_gonggao == null)
            {
                return HttpNotFound();
            }
            return View(tender_gonggao);
        }

        [Authorize(Roles = "admin")][MultipleResponseFormats]
        public ActionResult EditDelete(int id=0)
        {
            Tender_GongGao tender_gonggao = db.Tender_GongGaos.Find(id);
            if (tender_gonggao == null)
            {
                return HttpNotFound();
            }
            return View(tender_gonggao);
        }

        [MultipleResponseFormats]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult EditDelete(Tender_GongGao tender_gonggao)
        {
            if (ModelState.IsValid)
            {
                Tender_GongGao tgg = db.Tender_GongGaos.Find(tender_gonggao.TaskID);
                
                if(tgg!=null)
                {
                    tgg.IsDelete = tender_gonggao.IsDelete;
                    db.Entry(tgg).State = EntityState.Modified;
                    db.SaveChanges();
                    return Content("修改成功!");
                }
                
                //return RedirectToAction("Index");
            }
            return Content("修改失败!");
            //return View(tender_gonggao);
        }
        //
        // POST: /Tender_GongGao/Edit/5
        [MultipleResponseFormats]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tender_GongGao tender_gonggao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tender_gonggao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tender_gonggao);
        }

        //
        // GET: /Tender_GongGao/Delete/5
        [MultipleResponseFormats]
        public ActionResult Delete(int id = 0)
        {
            Tender_GongGao tender_gonggao = db.Tender_GongGaos.Find(id);
            if (tender_gonggao == null)
            {
                return HttpNotFound();
            }
            return View(tender_gonggao);
        }

        //
        // POST: /Tender_GongGao/Delete/5
        [MultipleResponseFormats]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tender_GongGao tender_gonggao = db.Tender_GongGaos.Find(id);
            db.Tender_GongGaos.Remove(tender_gonggao);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}