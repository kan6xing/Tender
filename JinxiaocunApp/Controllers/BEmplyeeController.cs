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
using WebMatrix.WebData;
using System.IO;

namespace JinxiaocunApp.Controllers
{
    [InitializeSimpleMembership]
    public class BEmplyeeController : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();
        int PageSize = 3;
        //
        // GET: /BEmplyee/

        [MultipleResponseFormats]
        public ActionResult Index()
        {
            ViewBag.PageDiv = Guid.NewGuid().ToString();
            ViewBag.Pagination = Guid.NewGuid().ToString();

            if (ViewBag.PageSize == null)
            {
                ViewBag.PageSize = PageSize;
                ViewBag.CurrentPage = 1;
                ViewBag.TotalRecord = db.BEmplyees.Count();
                ViewBag.TotalPage = Math.Ceiling(ViewBag.TotalRecord / (double)ViewBag.PageSize);

            }

            return View(db.BEmplyees.Where(m=>m.EmpID==1));
        }


        public ActionResult Pages(int id = 1)
        {
            try
            {

            
            ViewBag.CurrentPage = id;

            //new BEmplyee{ EmpID=emp.EmpID,NumberEmp=emp.NumberEmp,FullNameEmp=emp.FullNameEmp,SmallNameEmp=emp.SmallNameEmp,PinyinEmp=emp.PinyinEmp,TelEmp=emp.TelEmp,StateEmp=emp.StateEmp,RemarkEmp=emp.RemarkEmp,bdepartment=dep,bdepartment_BID=dep.BID}

            //return PartialView(db.BEmplyees.Join(db.BDepartments, emp => emp.bdepartment_BID, dep => dep.BID, (emp, dep) => emp).OrderBy(m => m.EmpID).Skip((id - 1) * PageSize).Take(PageSize).ToList());
           
            return PartialView(db.BEmplyees.Include("BDepartment").OrderBy(m => m.EmpID).Skip((id - 1) * PageSize).Take(PageSize).ToList());

            }
            catch (Exception ex)
            { return Content(ex.Message); }

        }
        //
        // GET: /BEmplyee/Details/5

        [MultipleResponseFormats][Authorize]
        public ActionResult Details(int id = 0)
        {
            //BEmplyee bemplyee = db.BEmplyees.Find(id);
            //if (bemplyee == null)
            //{
            //    return HttpNotFound();
            //}

            BEmplyee bemplyee=  db.BEmplyees.Single(m => m.NumberEmp == User.Identity.Name);
            return View(bemplyee);
        }

        //
        // GET: /BEmplyee/Create

        [MultipleResponseFormats]
        public ActionResult Create()
        {
            try{
            if (db.BEmplyees.Count() <= 0)
            { 
                return View();
            }
            return View(new BEmplyee { NumberEmp=db.BEmplyees.OrderByDescending(m=>m.EmpID).First().NumberEmp.GetAscNumberStr()});
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        //
        // POST: /BEmplyee/Create

        [HttpPost]
        [MultipleResponseFormats]
        public ActionResult Create(BEmplyee bemplyee)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    WebSecurity.CreateUserAndAccount(bemplyee.NumberEmp, "111111");

                    BEmplyee emp = db.BEmplyees.Single(m => m.NumberEmp.Equals(bemplyee.NumberEmp));

                    emp.CopyFrom(bemplyee);
                    emp.RegisterDate = bemplyee.RegisterDate = System.DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            
            //else
            //{
            //    string err = "";
            //    foreach (string k in ModelState.Keys)
            //    {
            //        if (ModelState[k].Errors.Count > 0)
            //        {
            //            err += k + "\n ";
            //            foreach (ModelError eor in ModelState[k].Errors)
            //            {
            //                err += ">>" + eor.ErrorMessage + ":" + eor.Exception.Message + "\n" + eor.Exception.StackTrace ;
            //            }
            //        }
            //    }
            //    return Content(err);
            //}

            return View(bemplyee);
        }

        //
        // GET: /BEmplyee/Edit/5

        [MultipleResponseFormats]
        public ActionResult Edit(int? id = 0)
        {
            
            BEmplyee bemplyee = null;
            try
            {
                bemplyee = db.BEmplyees.Single(m => m.NumberEmp == User.Identity.Name);
                //if(bemplyee.isPass!=null&&bemplyee.isPass==true)
                //{
                //    return Content("你的资料已验证通过，不能进行修改");
                //}
            }catch(Exception ex)
            {
                return HttpNotFound();
            }
            
            if (bemplyee == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyType = new SelectList(db.Tender_CompanyTypes, "TypeName", "TypeName",bemplyee.CompanyType);
            return View(bemplyee);
        }

        //
        // POST: /BEmplyee/Edit/5

        [HttpPost]
        [MultipleResponseFormats]
        public ActionResult Edit(BEmplyee emp)
        {
            if (ModelState.IsValid)
            {
                int mid = emp.EmpID;
                BEmplyee emp1;
                try {
                     emp1= db.BEmplyees.Find(mid);
                }catch(Exception ex)
                {
                    return Content("用户不存在");
                }
                
                HttpPostedFileBase fCodeAtt = Request.Files["fCodeAtt"];
                HttpPostedFileBase fTaxAtt = Request.Files["fTaxAtt"];
                HttpPostedFileBase fCertAtt = Request.Files["fCertAtt"];
                HttpPostedFileBase fLicenceAtt = Request.Files["fLicenceAtt"];
                HttpPostedFileBase fDelegateBook = Request.Files["fDelegateBook"];
                //****************  委托书 **************
                HttpPostedFileBase fPromiseAtt = Request.Files["fPromiseAtt"];
                HttpPostedFileBase fSecretAtt = Request.Files["fSecretAtt"];
                HttpPostedFileBase fPeopleAtt = Request.Files["fPeopleAtt"];
                HttpPostedFileBase fOpenAccountAtt = Request.Files["fOpenAccountAtt"];

                HttpPostedFileBase fprojectAtt = Request.Files["fprojectAtt"];

                //****************  开户许可证 **************

                if (fOpenAccountAtt != null && !String.IsNullOrEmpty(fOpenAccountAtt.FileName))
                {
                    emp.OpenAccountAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.OpenAccountAtt.Substring(0, 4) + @"\" + emp.OpenAccountAtt.Substring(4, 4) + @"\")));
                    fOpenAccountAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.OpenAccountAtt.Substring(0, 4) + @"\" + emp.OpenAccountAtt.Substring(4, 4) + @"\" + emp.OpenAccountAtt.Substring(8))));
                    GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] {
                    { "@FileID",emp.OpenAccountAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fOpenAccountAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fOpenAccountAtt.FileName),"DbType.String",null },
                    { "@Size",fOpenAccountAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                }

                //****************  类似项目的经营业绩 **************

                if (fprojectAtt != null && !String.IsNullOrEmpty(fprojectAtt.FileName))
                {
                    emp.projectAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.projectAtt.Substring(0, 4) + @"\" + emp.projectAtt.Substring(4, 4) + @"\")));
                    fprojectAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.projectAtt.Substring(0, 4) + @"\" + emp.projectAtt.Substring(4, 4) + @"\" + emp.projectAtt.Substring(8))));
                    GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] {
                    { "@FileID",emp.projectAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fprojectAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fprojectAtt.FileName),"DbType.String",null },
                    { "@Size",fprojectAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                }
                //****************  保密协议 **************

                if (fSecretAtt != null && !String.IsNullOrEmpty(fSecretAtt.FileName))
                {
                    emp.SecretAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.SecretAtt.Substring(0, 4) + @"\" + emp.SecretAtt.Substring(4, 4) + @"\")));
                    fSecretAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.SecretAtt.Substring(0, 4) + @"\" + emp.SecretAtt.Substring(4, 4) + @"\" + emp.SecretAtt.Substring(8))));
                    GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.SecretAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fSecretAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fSecretAtt.FileName),"DbType.String",null },
                    { "@Size",fSecretAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                }

                //*****************************************
                if (fPeopleAtt != null && !String.IsNullOrEmpty(fPeopleAtt.FileName))
                {
                    emp.PeopleAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.PeopleAtt.Substring(0, 4) + @"\" + emp.PeopleAtt.Substring(4, 4) + @"\")));
                    fPeopleAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.PeopleAtt.Substring(0, 4) + @"\" + emp.PeopleAtt.Substring(4, 4) + @"\" + emp.PeopleAtt.Substring(8))));
                    GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.PeopleAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fPeopleAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fPeopleAtt.FileName),"DbType.String",null },
                    { "@Size",fPeopleAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                }

                if (fPromiseAtt != null && !String.IsNullOrEmpty(fPromiseAtt.FileName))
                {
                    emp.PromiseAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.PromiseAtt.Substring(0, 4) + @"\" + emp.PromiseAtt.Substring(4, 4) + @"\")));
                    fPromiseAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.PromiseAtt.Substring(0, 4) + @"\" + emp.PromiseAtt.Substring(4, 4) + @"\" + emp.PromiseAtt.Substring(8))));
                    GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.PromiseAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fPromiseAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fPromiseAtt.FileName),"DbType.String",null },
                    { "@Size",fPromiseAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                }

                if (fCodeAtt != null&&!String.IsNullOrEmpty(fCodeAtt.FileName))
                {
                    emp.CodeAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.CodeAtt.Substring(0, 4) + @"\" + emp.CodeAtt.Substring(4, 4) + @"\")));
                    fCodeAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.CodeAtt.Substring(0, 4) + @"\" + emp.CodeAtt.Substring(4, 4) + @"\" + emp.CodeAtt.Substring(8))));
                    GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.CodeAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fCodeAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fCodeAtt.FileName),"DbType.String",null },
                    { "@Size",fCodeAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                }


                if (fTaxAtt != null && !String.IsNullOrEmpty(fTaxAtt.FileName))
                {
                    emp.TaxAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.TaxAtt.Substring(0, 4) + @"\" + emp.TaxAtt.Substring(4, 4) + @"\")));
                    fTaxAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.TaxAtt.Substring(0, 4) + @"\" + emp.TaxAtt.Substring(4, 4) + @"\" + emp.TaxAtt.Substring(8))));
                    GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.TaxAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fTaxAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fTaxAtt.FileName),"DbType.String",null },
                    { "@Size",fTaxAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                }


                if (fCertAtt != null && !String.IsNullOrEmpty(fCertAtt.FileName))
                {
                    emp.CertAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.CertAtt.Substring(0, 4) + @"\" + emp.CertAtt.Substring(4, 4) + @"\")));
                    fCertAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.CertAtt.Substring(0, 4) + @"\" + emp.CertAtt.Substring(4, 4) + @"\" + emp.CertAtt.Substring(8))));
                    GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.CertAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fCertAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fCertAtt.FileName),"DbType.String",null },
                    { "@Size",fCertAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                }


                if (fLicenceAtt != null && !String.IsNullOrEmpty(fLicenceAtt.FileName))
                {
                    emp.LicenceAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.LicenceAtt.Substring(0, 4) + @"\" + emp.LicenceAtt.Substring(4, 4) + @"\")));
                    fLicenceAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.LicenceAtt.Substring(0, 4) + @"\" + emp.LicenceAtt.Substring(4, 4) + @"\" + emp.LicenceAtt.Substring(8))));
                    GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.LicenceAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fLicenceAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fLicenceAtt.FileName),"DbType.String",null },
                    { "@Size",fLicenceAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                }


                if (fDelegateBook != null && !String.IsNullOrEmpty(fDelegateBook.FileName))
                {
                    emp.DelegateBook = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.DelegateBook.Substring(0, 4) + @"\" + emp.DelegateBook.Substring(4, 4) + @"\")));
                    fDelegateBook.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.DelegateBook.Substring(0, 4) + @"\" + emp.DelegateBook.Substring(4, 4) + @"\" + emp.DelegateBook.Substring(8))));
                    GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.DelegateBook,"DbType.String",null },
                    { "@Name",Path.GetFileName(fDelegateBook.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fDelegateBook.FileName),"DbType.String",null },
                    { "@Size",fDelegateBook.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                }
                emp1.CopyFrom(emp);
                db.Entry(emp1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
                //return Content("修改成功!");
            }
            else
            {
                string err = "";
                foreach (string k in ModelState.Keys)
                {
                    if (ModelState[k].Errors.Count > 0)
                    {
                        err += k + "\n ";
                        foreach (ModelError eor in ModelState[k].Errors)
                        {
                            err += ">>" + eor.ErrorMessage ;
                        }
                    }
                }
                return Content(err);
            }
            return Content("修改失败");
            //return View(emp);
        }

        //
        // GET: /emp/Delete/5

        [MultipleResponseFormats]
        public ActionResult Delete(int id = 0)
        {
            BEmplyee bemplyee = db.BEmplyees.Find(id);
            if (bemplyee == null)
            {
                return HttpNotFound();
            }
            return View(bemplyee);
        }

        //
        // POST: /BEmplyee/Delete/5

        [HttpPost, ActionName("Delete")]
        [MultipleResponseFormats]
        public ActionResult DeleteConfirmed(int id)
        {
            BEmplyee bemplyee = db.BEmplyees.Find(id);
            db.BEmplyees.Remove(bemplyee);
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