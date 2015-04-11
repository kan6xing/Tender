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
    public class PermissionController : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();

        //
        // GET: /Permission/

        public ActionResult Index()
        {
            return View(db.Permissions.ToList());
        }

        //
        // GET: /Permission/Details/5

        public ActionResult Details(int id = 0)
        {
            Permission permission = db.Permissions.Find(id);
            if (permission == null)
            {
                return HttpNotFound();
            }
            return View(permission);
        }

        //
        // GET: /Permission/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Permission/Create

        [HttpPost]
        public ActionResult Create(Permission permission)
        {
            if (ModelState.IsValid)
            {
                db.Permissions.Add(permission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(permission);
        }

        //
        // GET: /Permission/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Permission permission = db.Permissions.Find(id);
            if (permission == null)
            {
                return HttpNotFound();
            }
            return View(permission);
        }

        //
        // POST: /Permission/Edit/5

        [HttpPost]
        public ActionResult Edit(Permission permission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(permission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(permission);
        }

        //
        // GET: /Permission/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Permission permission = db.Permissions.Find(id);
            if (permission == null)
            {
                return HttpNotFound();
            }
            return View(permission);
        }

        //
        // POST: /Permission/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Permission permission = db.Permissions.Find(id);
            db.Permissions.Remove(permission);
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