using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinxiaocunApp.Models;
using JinxiaocunApp.Filters;
using System.Web.Security;

namespace JinxiaocunApp.Controllers
{
    //[AllowAnonymous]
    [InitializeSimpleMembership]
    public class RoleController : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();

        //
        // GET: /Role/

        
        [MultipleResponseFormats]
        public ActionResult Index()
        {
            return View(db.Roles.Include("Emplyees").ToList());
        }

        //
        // GET: /Role/Details/5

        [MultipleResponseFormats]
        [JinxcAuthorize("岗位列表")]
        public ActionResult Details(int id = 0)
        {
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // GET: /Role/Create
        [MultipleResponseFormats]
        public ActionResult Create()
        {
            ViewBag.user = null;
            return View();
        }

        //
        // POST: /Role/Create

        [HttpPost]
        [MultipleResponseFormats]
        public ActionResult Create(Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Roles.Add(role);
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                    Roles.AddUserToRole("海立美达", role.RoleName);
                  
                    return View(role);
                }

                return View(role);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            
        }

        [MultipleResponseFormats]
        public ActionResult GetEmp(int id=0)
        {
            
            return Content(id.ToString());
        }

        [HttpPost]
        [MultipleResponseFormats]
        public ActionResult EmpList(string[] EmpListCk)
        {
            try {
                //JinxiaocunApp.Models.Role role = (JinxiaocunApp.Models.Role)ViewData["vt"];
                int[] intEmps = new int[EmpListCk.Count()];
                for (int i = 0; i < EmpListCk.Count(); i++)
                {
                    intEmps[i] = int.Parse(EmpListCk[i]);
                }

                var empts = from es in db.BEmplyees where intEmps.Contains(es.EmpID) select es;
                return View("EmpLists",empts);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            
        }
        //
        // GET: /Role/Edit/5
        [MultipleResponseFormats]
        public ActionResult Edit(int id = 0)
        {
            try {
                Role role = db.Roles.Include("Emplyees").Single(m => m.RoleId == id);

                ViewData["emps"]= db.BEmplyees.ToList<BEmplyee>();

                if (role == null)
                {
                    return HttpNotFound();
                }
                return View(role);
            }
            catch (Exception ex) {
                return Content(ex.Message);
            }
            
        }

        //
        // POST: /Role/Edit/5

        [HttpPost]
        [MultipleResponseFormats]
        public ActionResult Edit(Role role, string[] EmpListH = null, string btnEmp = "")
        {
           
            ViewData["emps"] = db.BEmplyees.ToList<BEmplyee>();
            try {
                

                if (ModelState.IsValid)
                {
                    if (EmpListH != null && EmpListH.Count() > 0)
                    {

                        int[] intEmps = new int[EmpListH.Count()];
                        for (int i = 0; i < EmpListH.Count(); i++)
                        {
                            intEmps[i] = int.Parse(EmpListH[i]);
                        }

                        var empts = from es in db.BEmplyees where intEmps.Contains(es.EmpID) select es.NumberEmp;

                        //foreach (BEmplyee bemp in empts)
                        //{
                        //    if (!role.Emplyees.Contains(bemp))
                        //    {
                        //        role.Emplyees.Add(bemp);
                        //    }
                        //}
                        //role.Emplyees = empts.ToList<BEmplyee>();
                        Roles.RemoveUsersFromRole(Roles.GetUsersInRole(role.RoleName), role.RoleName);
                        Roles.AddUsersToRole(empts.ToArray<string>(),role.RoleName);
                        //return View(role);


                    }
                    else
                    {
                        return Content("没取到");
                    }

                    db.Entry(role).State = EntityState.Modified;
                    
                    
                    db.SaveChanges();

                    

                    return RedirectToAction("Index");
                }
                return View(role);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            
        }

        //
        // GET: /Role/Delete/5
        [MultipleResponseFormats]
        public ActionResult Delete(int id = 0)
        {
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Role/Delete/5

        [HttpPost, ActionName("Delete")]
        [MultipleResponseFormats]
        public ActionResult DeleteConfirmed(int id)
        {
            Role role = db.Roles.Find(id);
            db.Roles.Remove(role);
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