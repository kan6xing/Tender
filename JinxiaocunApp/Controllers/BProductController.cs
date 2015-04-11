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
    public class BProductController : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();

        //
        // GET: /BProduct/
        [MultipleResponseFormats]
        public ActionResult Index()
        {
            return View(db.BProducts.ToList());
        }

        //
        // GET: /BProduct/Details/5

        public ActionResult Details(int id = 0)
        {
            BProduct bproduct = db.BProducts.Find(id);
            if (bproduct == null)
            {
                return HttpNotFound();
            }
            return View(bproduct);
        }

        //
        // GET: /BProduct/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BProduct/Create

        [HttpPost]
        public ActionResult Create(BProduct bproduct)
        {
            if (ModelState.IsValid)
            {
                db.BProducts.Add(bproduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bproduct);
        }

        //
        // GET: /BProduct/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BProduct bproduct = db.BProducts.Find(id);
            if (bproduct == null)
            {
                return HttpNotFound();
            }
            return View(bproduct);
        }

        //
        // POST: /BProduct/Edit/5

        [HttpPost]
        public ActionResult Edit(BProduct bproduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bproduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bproduct);
        }

        //
        // GET: /BProduct/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BProduct bproduct = db.BProducts.Find(id);
            if (bproduct == null)
            {
                return HttpNotFound();
            }
            return View(bproduct);
        }

        //
        // POST: /BProduct/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            BProduct bproduct = db.BProducts.Find(id);
            db.BProducts.Remove(bproduct);
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