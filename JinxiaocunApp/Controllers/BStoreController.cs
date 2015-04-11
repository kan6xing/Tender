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
    public class BStoreController : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();
        int PageSize = 3;
        //
        // GET: /BStore/

        [MultipleResponseFormats]
        public ActionResult Index()
        {
            ViewBag.PageDiv = Guid.NewGuid().ToString();
            ViewBag.Pagination = Guid.NewGuid().ToString();

            if (ViewBag.PageSize == null)
            {
                ViewBag.PageSize = PageSize;
                ViewBag.CurrentPage = 1;
                ViewBag.TotalRecord = db.BStores.Count();
                ViewBag.TotalPage = Math.Ceiling(ViewBag.TotalRecord / (double)ViewBag.PageSize);

            }
            return View(db.BStores.Where(m => m.StoreID == 1));
        }

        public ActionResult Pages(int id = 1)
        {
            ViewBag.CurrentPage = id;
            return PartialView(db.BStores.OrderBy(m => m.StoreID).Skip((id - 1) * PageSize).Take(PageSize).ToList());
        }

        //
        // GET: /BStore/Details/5
        [MultipleResponseFormats]
        public ActionResult Details(int id = 0)
        {
            BStore bstore = db.BStores.Find(id);
            if (bstore == null)
            {
                return HttpNotFound();
            }
            return View(bstore);
        }

        //
        // GET: /BStore/Create
        [MultipleResponseFormats]
        public ActionResult Create()
        {
            if (db.BStores.Count() <= 0)
            {
                return View();
            }
            return View(new BStore { NumberStore = db.BStores.OrderByDescending(m => m.StoreID).First().NumberStore.GetAscNumberStr() });
            
        }

        //
        // POST: /BStore/Create

        [HttpPost]
        [MultipleResponseFormats]
        public ActionResult Create(BStore bstore)
        {
            if (ModelState.IsValid)
            {
                db.BStores.Add(bstore);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(bstore);
        }

        //
        // GET: /BStore/Edit/5
        [MultipleResponseFormats]
        public ActionResult Edit(int id = 0)
        {
            BStore bstore = db.BStores.Find(id);
            if (bstore == null)
            {
                return HttpNotFound();
            }
            return View(bstore);
        }

        //
        // POST: /BStore/Edit/5

        [HttpPost]
        [MultipleResponseFormats]
        public ActionResult Edit(BStore bstore)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bstore).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bstore);
        }

        //
        // GET: /BStore/Delete/5
        [MultipleResponseFormats]
        public ActionResult Delete(int id = 0)
        {
            BStore bstore = db.BStores.Find(id);
            if (bstore == null)
            {
                return HttpNotFound();
            }
            return View(bstore);
        }

        //
        // POST: /BStore/Delete/5

        [HttpPost, ActionName("Delete")]
        [MultipleResponseFormats]
        public ActionResult DeleteConfirmed(int id)
        {
            BStore bstore = db.BStores.Find(id);
            db.BStores.Remove(bstore);
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