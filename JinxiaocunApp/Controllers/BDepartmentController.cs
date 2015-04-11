using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinxiaocunApp.Models;
using JinxiaocunApp.Filters;
using JinxiaocunApp.Extensions;

namespace JinxiaocunApp.Controllers
{
    public class BDepartmentController : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();

        private int PageSize = 10;
        //
        // GET: /Default1/

        [MultipleResponseFormats]
        public ActionResult Index(string id = "")
        {
            ViewBag.SuperRef = id;
            ViewBag.PageDiv = Guid.NewGuid().ToString();
            ViewBag.Pagination = Guid.NewGuid().ToString();

            if (ViewBag.PageSize == null)
            { 
                ViewBag.PageSize = PageSize;
                ViewBag.CurrentPage = 1;
                ViewBag.TotalRecord = db.BDepartments.Count();
                ViewBag.TotalPage =Math.Ceiling( ViewBag.TotalRecord / (double)ViewBag.PageSize);
                
            }


            return View(db.BDepartments.Where(m => m.BID == 1));
        }

        public ActionResult Pages(int id=1,string SuperRef="")
        {
            ViewBag.SuperRef=SuperRef;
            ViewBag.CurrentPage = id;
            
            ViewBag.tid = id+"  "+new Random().Next();
            return PartialView(db.BDepartments.OrderBy(m=>m.BID).Skip((id-1) * PageSize).Take(PageSize).ToList());
            //return PartialView(db.BDepartments.ToList());
        }

        [MultipleResponseFormats]
        public ActionResult PartialIndex()
        {
            return View(db.BDepartments.ToList());
        }

        //
        // GET: /Default1/Details/5

        [MultipleResponseFormats]
        public ActionResult Details(int id = 0)
        {
            BDepartment bdepartment = db.BDepartments.Find(id);
            if (bdepartment == null)
            {
                return HttpNotFound();
            }
            return View(bdepartment);
        }

        //
        // GET: /Default1/Create

        [MultipleResponseFormats]
        public ActionResult Create()
        {

            if (db.BDepartments.Count() <= 0)
            {
                return View();
            }
           

            return View(new BDepartment { BNumber = db.BDepartments.OrderByDescending(m => m.BID).First().BNumber.GetAscNumberStr() });
            
        }

        //
        // POST: /Default1/Create

        [HttpPost]
        [MultipleResponseFormats]
        public ActionResult Create(BDepartment bdepartment)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //if (db.BDepartments.Find(bdepartment.BNumber) != null)
                    //{
                    //    return RedirectToAction("Error", "MyTart", new { err = "编号不能重复", Act = "Create", Con = "BDepartment" });

                    //}
                    db.BDepartments.Add(bdepartment);
                    
                    db.SaveChanges();

                    return RedirectToAction("Create");
                    //return View(new BDepartment { BNumber = bdepartment.BNumber.GetAscNumberStr() });
                }
                return View(bdepartment);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            

            //return View(bdepartment);
        }

        //
        // GET: /Default1/Edit/5

        [MultipleResponseFormats]
        public ActionResult Edit(int id = 0,string SuperRef="")
        {
            ViewBag.SuperRef=SuperRef;
            BDepartment bdepartment = db.BDepartments.Find(id);
            if (bdepartment == null)
            {
                return HttpNotFound();
            }
            return View(bdepartment);
            //return View(bdepartment);
        }

        //
        // POST: /Default1/Edit/5

        [HttpPost]
        [MultipleResponseFormats]
        public ActionResult Edit(BDepartment bdepartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bdepartment).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
            }
            return View(bdepartment);
        }

        //
        // GET: /Default1/Delete/5

        [MultipleResponseFormats]
        public ActionResult Delete(int id =0)
        {
            BDepartment bdepartment = db.BDepartments.Find(id);
            if (bdepartment == null)
            {
                return HttpNotFound();
            }
            return View(bdepartment);
        }

        //
        // POST: /Default1/Delete/5

        [HttpPost, ActionName("Delete")]
        [MultipleResponseFormats]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            BDepartment bdepartment = db.BDepartments.Find(id);
            db.BDepartments.Remove(bdepartment);
            db.SaveChanges();

            return Content("删除成功");
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}