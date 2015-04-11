using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinxiaocunApp.Models;
using JinxiaocunApp.Filters;

namespace JinxiaocunApp.Controllers
{
    public class Bemp_GongGaoController : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();
        private int PageSize = 10;
        //
        // GET: /Bemp_GongGao/

        [MultipleResponseFormats]
        [Authorize]
        public ActionResult Index(string id = "")
        {
            ViewBag.SuperRef = id;
            ViewBag.PageDiv = Guid.NewGuid().ToString();
            ViewBag.Pagination = Guid.NewGuid().ToString();

            if (ViewBag.PageSize == null)
            {
                ViewBag.PageSize = PageSize;
                ViewBag.CurrentPage = 1;
                ViewBag.TotalRecord = db.Bemp_GongGaos.Where(m => m.UserName == User.Identity.Name).Count();
                ViewBag.TotalPage = Math.Ceiling(ViewBag.TotalRecord / (double)ViewBag.PageSize);

            }
            return View(db.Bemp_GongGaos.Include(m=>m.Tender_GongGaos).Where(m => m.BGid == 1));
        }


        [Authorize]
        public ActionResult Pages(int id = 1, string SuperRef = "")
        {
            ViewBag.SuperRef = SuperRef;
            ViewBag.CurrentPage = id;
            return PartialView(db.Bemp_GongGaos.Include(m=>m.Tender_GongGaos).Where(m=>m.UserName==User.Identity.Name).OrderByDescending(m => m.BGid).Skip((id - 1) * PageSize).Take(PageSize).ToList());
           
        }

        //
        // GET: /Bemp_GongGao/Details/5

        public ActionResult Details(int id = 0)
        {
            Bemp_GongGao bemp_gonggao = db.Bemp_GongGaos.Find(id);
            if (bemp_gonggao == null)
            {
                return HttpNotFound();
            }
            return View(bemp_gonggao);
        }

        //
        // GET: /Bemp_GongGao/Create

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.BEmplyees, "EmpID", "NumberEmp");
            ViewBag.GongGaoId = new SelectList(db.Tender_GongGaos, "TaskID", "SN");
            return View();
        }

        //
        // POST: /Bemp_GongGao/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Bemp_GongGao bemp_gonggao)
        {
            if (ModelState.IsValid)
            {
                db.Bemp_GongGaos.Add(bemp_gonggao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.BEmplyees, "EmpID", "NumberEmp", bemp_gonggao.UserId);
            ViewBag.GongGaoId = new SelectList(db.Tender_GongGaos, "TaskID", "SN", bemp_gonggao.GongGaoId);
            return View(bemp_gonggao);
        }

        //
        // GET: /Bemp_GongGao/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Bemp_GongGao bemp_gonggao = db.Bemp_GongGaos.Find(id);
            if (bemp_gonggao == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.BEmplyees, "EmpID", "NumberEmp", bemp_gonggao.UserId);
            ViewBag.GongGaoId = new SelectList(db.Tender_GongGaos, "TaskID", "SN", bemp_gonggao.GongGaoId);
            return View(bemp_gonggao);
        }

        //
        // POST: /Bemp_GongGao/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Bemp_GongGao bemp_gonggao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bemp_gonggao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.BEmplyees, "EmpID", "NumberEmp", bemp_gonggao.UserId);
            ViewBag.GongGaoId = new SelectList(db.Tender_GongGaos, "TaskID", "SN", bemp_gonggao.GongGaoId);
            return View(bemp_gonggao);
        }

        //
        // GET: /Bemp_GongGao/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Bemp_GongGao bemp_gonggao = db.Bemp_GongGaos.Find(id);
            if (bemp_gonggao == null)
            {
                return HttpNotFound();
            }
            return View(bemp_gonggao);
        }

        //
        // POST: /Bemp_GongGao/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bemp_GongGao bemp_gonggao = db.Bemp_GongGaos.Find(id);
            db.Bemp_GongGaos.Remove(bemp_gonggao);
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