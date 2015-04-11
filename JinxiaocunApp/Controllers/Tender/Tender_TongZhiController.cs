using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinxiaocunApp.Models;
using JinxiaocunApp.Filters;
using System.IO;

namespace JinxiaocunApp.Controllers.Tender
{
    public class Tender_TongZhiController : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();
        private int PageSize = 15;
        //
        // GET: /Tender_TongZhi/

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
                ViewBag.TotalRecord = db.Tender_TongZhi.Count();
                ViewBag.TotalPage = Math.Ceiling(ViewBag.TotalRecord / (double)ViewBag.PageSize);

            }
            return View(db.Tender_TongZhi.Where(m => m.TZID == 0));
        }

        public ActionResult Pages(int id = 1, string SuperRef = "")
        {
            //ViewBag.ProType = db.Tender_ProjectTypes.ToList();
            ViewBag.SuperRef = SuperRef;
            ViewBag.CurrentPage = id;

            //ViewBag.tid = id + "  " + new Random().Next();
            return PartialView(db.Tender_TongZhi.OrderByDescending(m => m.TZID).Skip((id - 1) * PageSize).Take(PageSize).ToList());
            //return PartialView(db.BDepartments.ToList());
        }

        [MultipleResponseFormats]
        public ActionResult IndexTop(int id=0)
        {
            if(id>0)
            {
                return View(db.Tender_TongZhi.OrderByDescending(m => m.CreateDateT).ToList());
            }
            return View(db.Tender_TongZhi.OrderByDescending(m => m.CreateDateT).Take(5).ToList());
        }

        [MultipleResponseFormats]
        public ActionResult MoreIndex(int id = 0)
        {
            
                return View(db.Tender_TongZhi.OrderByDescending(m => m.CreateDateT).ToList());
            
        }
        //
        // GET: /Tender_TongZhi/Details/5

        [MultipleResponseFormats]
        public ActionResult Details(int id = 0)
        {
            Tender_TongZhi tender_tongzhi = db.Tender_TongZhi.Find(id);
            if (tender_tongzhi == null)
            {
                return HttpNotFound();
            }
            return View(tender_tongzhi);
        }

        //
        // GET: /Tender_TongZhi/Create

        [MultipleResponseFormats]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Tender_TongZhi/Create

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Tender_TongZhi custf, HttpPostedFileBase[] fileToUpload,int id=0)
        {
            //if (ModelState.IsValid)
            //{
            //    tender_tongzhi.CreateDateT = DateTime.Now;
            //    db.Tender_TongZhi.Add(tender_tongzhi);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(tender_tongzhi);
            if (id == 3)
                return Content("");

            HttpPostedFileBase file = null;
            if (fileToUpload != null)
                file = fileToUpload[0];
            if (file != null && !String.IsNullOrEmpty(file.FileName))
            {
                custf.CustFile = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (custf.CustFile.Substring(0, 4) + @"\" + custf.CustFile.Substring(4, 4) + @"\")));
                file.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (custf.CustFile.Substring(0, 4) + @"\" + custf.CustFile.Substring(4, 4) + @"\" + custf.CustFile.Substring(8))));
                GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",custf.CustFile,"DbType.String",null },
                    { "@Name",Path.GetFileName(file.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(file.FileName),"DbType.String",null },
                    { "@Size",file.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                custf.CustName = Path.GetFileName(file.FileName);
                
            }

            custf.CreateDateT = System.DateTime.Now;

            db.Tender_TongZhi.Add(custf);
            db.SaveChanges();
            //string fPath = @"D://myupload/" + file.FileName;
            //file.SaveAs(fPath);
            //string custT="nul";
            //if(custf!=null)
            //{
            //    custT = custf.CustText;
            //}
            return Content("保存成功");
        }

        //
        // GET: /Tender_TongZhi/Edit/5

        [MultipleResponseFormats]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id = 0)
        {
            Tender_TongZhi tender_tongzhi = db.Tender_TongZhi.Find(id);
            if (tender_tongzhi == null)
            {
                return HttpNotFound();
            }
            return View(tender_tongzhi);
        }

        //
        // POST: /Tender_TongZhi/Edit/5

        [HttpPost]
        [MultipleResponseFormats]
        [ValidateInput(false)]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Tender_TongZhi tender_tongzhi)
        {
            if (ModelState.IsValid)
            {
                Tender_TongZhi ttz = db.Tender_TongZhi.Find(tender_tongzhi.TZID);
                //tender_tongzhi.CreateDateT = ttz.CreateDateT;
                //tender_tongzhi.CustFile = ttz.CustFile;
                //tender_tongzhi.CustName = ttz.CustName;
                ttz.TitleT = tender_tongzhi.TitleT;
                ttz.ContentT = tender_tongzhi.ContentT;
                db.Entry(ttz).State = EntityState.Modified;
                db.SaveChanges();
                return Content("修改成功!");
            }
            return Content("修改失败!");
        }

        //
        // GET: /Tender_TongZhi/Delete/5

        [MultipleResponseFormats]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id = 0)
        {
            Tender_TongZhi tender_tongzhi = db.Tender_TongZhi.Find(id);
            if (tender_tongzhi == null)
            {
                return HttpNotFound();
            }
            return View(tender_tongzhi);
        }

        //
        // POST: /Tender_TongZhi/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tender_TongZhi tender_tongzhi = db.Tender_TongZhi.Find(id);
            db.Tender_TongZhi.Remove(tender_tongzhi);
            db.SaveChanges();
            return RedirectToAction("index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}