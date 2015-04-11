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
    public class BPartMenuController : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();

        //
        // GET: /BPartMenu/

        [MultipleResponseFormats]
        public ActionResult Index()
        {
            return View(db.BPartMenus.ToList());
        }

        public ActionResult Menu(int id = 0, string tagetdiv="")
        {
            ViewBag.Bid = id;
            ViewBag.divid = Guid.NewGuid().ToString();
            //ViewBag.tagetdiv = Request["tagetdiv"]??"tabs-2";
            ViewBag.tagetdiv = tagetdiv;
            return PartialView(db.BPartMenus.ToList());
        }
        //
        // GET: /BPartMenu/Details/5

        public ActionResult Details(int id = 0)
        {
            BPartMenu bpartmenu = db.BPartMenus.Find(id);
            if (bpartmenu == null)
            {
                return HttpNotFound();
            }
            return View(bpartmenu);
        }

        //
        // GET: /BPartMenu/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BPartMenu/Create

        [HttpPost]
        public ActionResult Create(BPartMenu bpartmenu)
        {
            if (ModelState.IsValid)
            {
                db.BPartMenus.Add(bpartmenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bpartmenu);
        }

        //
        // GET: /BPartMenu/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BPartMenu bpartmenu = db.BPartMenus.Find(id);
            if (bpartmenu == null)
            {
                return HttpNotFound();
            }
            return View(bpartmenu);
        }

        //
        // POST: /BPartMenu/Edit/5

        [HttpPost]
        public ActionResult Edit(BPartMenu bpartmenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bpartmenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bpartmenu);
        }

        //
        // GET: /BPartMenu/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BPartMenu bpartmenu = db.BPartMenus.Find(id);
            if (bpartmenu == null)
            {
                return HttpNotFound();
            }
            return View(bpartmenu);
        }

        //
        // POST: /BPartMenu/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            BPartMenu bpartmenu = db.BPartMenus.Find(id);
            db.BPartMenus.Remove(bpartmenu);
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