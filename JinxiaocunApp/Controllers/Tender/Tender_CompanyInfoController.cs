using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinxiaocunApp.Models;

namespace JinxiaocunApp.Controllers
{
    public class Tender_CompanyInfoController : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();

        //
        // GET: /Tender_CompanyInfo/

        public ActionResult Index()
        {
            var tender_companyinfo = db.Tender_CompanyInfo.Include(t => t.bemplyee);
            return View(tender_companyinfo.ToList());
        }

        //
        // GET: /Tender_CompanyInfo/Details/5

        public ActionResult Details(int id = 0)
        {
            Tender_CompanyInfo tender_companyinfo = db.Tender_CompanyInfo.Find(id);
            if (tender_companyinfo == null)
            {
                return HttpNotFound();
            }
            return View(tender_companyinfo);
        }

        //
        // GET: /Tender_CompanyInfo/Create

        public ActionResult Create(int EmpID=6)
        {
            ViewBag.EmpID = new SelectList(db.BEmplyees, "EmpID", "NumberEmp");
            
            return View();
        }

        //
        // POST: /Tender_CompanyInfo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tender_CompanyInfo tender_companyinfo)
        {
            
            if (ModelState.IsValid)
            {
                db.Tender_CompanyInfo.Add(tender_companyinfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmpID = new SelectList(db.BEmplyees, "EmpID", "NumberEmp", tender_companyinfo.EmpID);
            return View(tender_companyinfo);
        }

        //
        // GET: /Tender_CompanyInfo/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Tender_CompanyInfo tender_companyinfo = db.Tender_CompanyInfo.Find(id);
            if (tender_companyinfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpID = new SelectList(db.BEmplyees, "EmpID", "NumberEmp", tender_companyinfo.EmpID);
            return View(tender_companyinfo);
        }

        //
        // POST: /Tender_CompanyInfo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tender_CompanyInfo tender_companyinfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tender_companyinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpID = new SelectList(db.BEmplyees, "EmpID", "NumberEmp", tender_companyinfo.EmpID);
            return View(tender_companyinfo);
        }

        //
        // GET: /Tender_CompanyInfo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Tender_CompanyInfo tender_companyinfo = db.Tender_CompanyInfo.Find(id);
            if (tender_companyinfo == null)
            {
                return HttpNotFound();
            }
            return View(tender_companyinfo);
        }

        //
        // POST: /Tender_CompanyInfo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tender_CompanyInfo tender_companyinfo = db.Tender_CompanyInfo.Find(id);
            db.Tender_CompanyInfo.Remove(tender_companyinfo);
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