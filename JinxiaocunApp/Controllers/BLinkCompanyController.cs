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
    public class BLinkCompanyController : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();

        //
        // GET: /BLinkCompany/

        [MultipleResponseFormats]
        public ActionResult Index()
        {
            return View(db.BLinkCompanies.ToList());
        }

        //
        // GET: /BLinkCompany/Details/5

        public ActionResult Details(int id = 0)
        {
            BLinkCompany blinkcompany = db.BLinkCompanies.Find(id);
            if (blinkcompany == null)
            {
                return HttpNotFound();
            }
            return View(blinkcompany);
        }

        //
        // GET: /BLinkCompany/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BLinkCompany/Create

        [HttpPost]
        public ActionResult Create(BLinkCompany blinkcompany)
        {
            if (ModelState.IsValid)
            {
                db.BLinkCompanies.Add(blinkcompany);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blinkcompany);
        }

        //
        // GET: /BLinkCompany/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BLinkCompany blinkcompany = db.BLinkCompanies.Find(id);
            if (blinkcompany == null)
            {
                return HttpNotFound();
            }
            return View(blinkcompany);
        }

        //
        // POST: /BLinkCompany/Edit/5

        [HttpPost]
        public ActionResult Edit(BLinkCompany blinkcompany)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blinkcompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blinkcompany);
        }

        //
        // GET: /BLinkCompany/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BLinkCompany blinkcompany = db.BLinkCompanies.Find(id);
            if (blinkcompany == null)
            {
                return HttpNotFound();
            }
            return View(blinkcompany);
        }

        //
        // POST: /BLinkCompany/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            BLinkCompany blinkcompany = db.BLinkCompanies.Find(id);
            db.BLinkCompanies.Remove(blinkcompany);
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