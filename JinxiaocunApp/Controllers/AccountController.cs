using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinxiaocunApp.Models;
using WebMatrix.WebData;
using Microsoft.Web.WebPages.OAuth;
using System.Web.Security;
using JinxiaocunApp.Filters;
using System.IO;
using System.Transactions;

namespace JinxiaocunApp.Controllers
{
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private JinxiaocunAppContext db = new JinxiaocunAppContext();

        [AllowAnonymous][MultipleResponseFormats]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

       
        public ActionResult LoginInner()
        {
            ViewBag.isTong = false;
            return PartialView();
        }

        [HttpPost]
        [MultipleResponseFormats]
        [AllowAnonymous]
        public ActionResult LoginInner(LoginModel model)
        {
            ViewBag.isTong = false;
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                //return RedirectToLocal(returnUrl);
                if (db.BEmplyees.Where(m => m.NumberEmp == model.UserName && !(m.isDelete != null && m.isDelete == true)).Count() > 0)
                {
                    //return Content("登录成功！");
                    ViewBag.isTong = true;
                    ViewBag.uName = model.UserName;
                    return PartialView();
                    
                }
                else
                {
                    WebSecurity.Logout();
                    //return Content("该用户已被管理禁用.");
                    ModelState.AddModelError("", "该用户已被管理禁用.");
                    return PartialView(model);
                }

            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "用户名或密码错误.");
            //return Content("用户名或密码错误.");
            return PartialView(model);
        }
        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [MultipleResponseFormats]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                //return RedirectToLocal(returnUrl);
                if (db.BEmplyees.Where(m => m.NumberEmp == model.UserName && !(m.isDelete!=null&&m.isDelete == true)).Count() > 0)
                {
                    return RedirectToLocal("/");
                }else
                {
                    WebSecurity.Logout();
                    ModelState.AddModelError("", "该用户已被管理禁用.");
                    return View(model);
                }
                
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "用户名或密码错误.");

            return View(model);
            
        }

        [MultipleResponseFormats]
        public ActionResult Register()
        {
            ViewBag.CompanyType = new SelectList(db.Tender_CompanyTypes, "TypeName", "TypeName");
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel register)
        {
            if (ModelState.IsValid)
            {
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
                //****************  委托书 **************
                if (fCodeAtt == null || fTaxAtt == null || fCertAtt == null || fLicenceAtt == null || fDelegateBook == null || fPromiseAtt==null)
                {
                    return Content("error file is null");
                }
                if (Path.GetExtension(fCodeAtt.FileName).ToLower().EndsWith("exe")||
                    Path.GetExtension(fTaxAtt.FileName).ToLower().EndsWith("exe") ||
                    Path.GetExtension(fCertAtt.FileName).ToLower().EndsWith("exe") ||
                    Path.GetExtension(fCertAtt.FileName).ToLower().EndsWith("exe") ||
                    //****************  委托书 **************
                    Path.GetExtension(fPromiseAtt.FileName).ToLower().EndsWith("exe") ||
                    Path.GetExtension(fDelegateBook.FileName).ToLower().EndsWith("exe"))
                {
                    return Content("请上传办公文件或压缩文件");
                }
                //using( TransactionScope scope=new TransactionScope())
                //{
                    WebSecurity.CreateUserAndAccount(register.bemplyee.NumberEmp, register.Password);

                    BEmplyee emp = db.BEmplyees.Single(m => m.NumberEmp.Equals(register.bemplyee.NumberEmp));

                    emp.CopyFrom(register.bemplyee);

                    //****************  保密协议 **************
                    emp.SecretAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    if (fSecretAtt != null)
                    {
                        Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.SecretAtt.Substring(0, 4) + @"\" + emp.SecretAtt.Substring(4, 4) + @"\")));
                        fSecretAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.SecretAtt.Substring(0, 4) + @"\" + emp.SecretAtt.Substring(4, 4) + @"\" + emp.SecretAtt.Substring(8))));
                        GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.SecretAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fSecretAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fSecretAtt.FileName),"DbType.String",null },
                    { "@Size",fSecretAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                    }

                //*******************资格证明***********

                //****************  开户许可证 **************
                emp.OpenAccountAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                if (fOpenAccountAtt != null)
                {
                    Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.OpenAccountAtt.Substring(0, 4) + @"\" + emp.OpenAccountAtt.Substring(4, 4) + @"\")));
                    fOpenAccountAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.OpenAccountAtt.Substring(0, 4) + @"\" + emp.OpenAccountAtt.Substring(4, 4) + @"\" + emp.OpenAccountAtt.Substring(8))));
                    GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] {
                    { "@FileID",emp.OpenAccountAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fOpenAccountAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fOpenAccountAtt.FileName),"DbType.String",null },
                    { "@Size",fOpenAccountAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                }

                //****************  保密协议 **************
                emp.PeopleAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    if (fPeopleAtt != null)
                    {
                        Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.PeopleAtt.Substring(0, 4) + @"\" + emp.PeopleAtt.Substring(4, 4) + @"\")));
                        fPeopleAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.PeopleAtt.Substring(0, 4) + @"\" + emp.PeopleAtt.Substring(4, 4) + @"\" + emp.PeopleAtt.Substring(8))));
                        GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.PeopleAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fPeopleAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fPeopleAtt.FileName),"DbType.String",null },
                    { "@Size",fPeopleAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                    }

                    //****************  委托书 **************
                    emp.PromiseAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    if (fPromiseAtt != null)
                    {
                        Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.PromiseAtt.Substring(0, 4) + @"\" + emp.PromiseAtt.Substring(4, 4) + @"\")));
                        fPromiseAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.PromiseAtt.Substring(0, 4) + @"\" + emp.PromiseAtt.Substring(4, 4) + @"\" + emp.PromiseAtt.Substring(8))));
                        GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.PromiseAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fPromiseAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fPromiseAtt.FileName),"DbType.String",null },
                    { "@Size",fPromiseAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                    }

                    emp.CodeAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    if (fCodeAtt != null)
                    {
                        Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.CodeAtt.Substring(0, 4) + @"\" + emp.CodeAtt.Substring(4, 4) + @"\")));
                        fCodeAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.CodeAtt.Substring(0, 4) + @"\" + emp.CodeAtt.Substring(4, 4) + @"\" + emp.CodeAtt.Substring(8))));
                        GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.CodeAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fCodeAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fCodeAtt.FileName),"DbType.String",null },
                    { "@Size",fCodeAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                    }

                    emp.TaxAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    if (fTaxAtt != null)
                    {
                        Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.TaxAtt.Substring(0, 4) + @"\" + emp.TaxAtt.Substring(4, 4) + @"\")));
                        fTaxAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.TaxAtt.Substring(0, 4) + @"\" + emp.TaxAtt.Substring(4, 4) + @"\" + emp.TaxAtt.Substring(8))));
                        GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.TaxAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fTaxAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fTaxAtt.FileName),"DbType.String",null },
                    { "@Size",fTaxAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                    }

                    emp.CertAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    if (fCertAtt != null)
                    {
                        Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.CertAtt.Substring(0, 4) + @"\" + emp.CertAtt.Substring(4, 4) + @"\")));
                        fCertAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.CertAtt.Substring(0, 4) + @"\" + emp.CertAtt.Substring(4, 4) + @"\" + emp.CertAtt.Substring(8))));
                        GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.CertAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fCertAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fCertAtt.FileName),"DbType.String",null },
                    { "@Size",fCertAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                    }

                    emp.LicenceAtt = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    if (fLicenceAtt != null)
                    {
                        Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.LicenceAtt.Substring(0, 4) + @"\" + emp.LicenceAtt.Substring(4, 4) + @"\")));
                        fLicenceAtt.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.LicenceAtt.Substring(0, 4) + @"\" + emp.LicenceAtt.Substring(4, 4) + @"\" + emp.LicenceAtt.Substring(8))));
                        GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.LicenceAtt,"DbType.String",null },
                    { "@Name",Path.GetFileName(fLicenceAtt.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fLicenceAtt.FileName),"DbType.String",null },
                    { "@Size",fLicenceAtt.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                    }

                   
                    emp.DelegateBook = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
                    if (fDelegateBook != null)
                    {
                        Directory.CreateDirectory(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.DelegateBook.Substring(0, 4) + @"\" + emp.DelegateBook.Substring(4, 4) + @"\")));
                        fDelegateBook.SaveAs(Path.Combine(System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentRootPath"], (emp.DelegateBook.Substring(0, 4) + @"\" + emp.DelegateBook.Substring(4, 4) + @"\" + emp.DelegateBook.Substring(8))));
                        GenericDataAccess.UpdateBySql("Insert into YZAppAttachment(FileID,Name,Ext,Size,OwnerAccount) values(@FileID,@Name,@Ext,@Size,@OwnerAccount)", new string[,] { 
                    { "@FileID",emp.DelegateBook,"DbType.String",null },
                    { "@Name",Path.GetFileName(fDelegateBook.FileName),"DbType.String",null },
                    { "@Ext",Path.GetExtension(fDelegateBook.FileName),"DbType.String",null },
                    { "@Size",fDelegateBook.ContentLength.ToString(),"DbType.Int32",null },
                    { "@OwnerAccount","0","DbType.String",null } });
                    }
                    emp.RegisterDate = System.DateTime.Now;
                    db.SaveChanges();
                    //WebSecurity.Login(emp.NumberEmp, register.Password);

                    //System.Web.Routing.RouteValueDictionary routv= new System.Web.Routing.RouteValueDictionary();
                    //routv.Add("EmpID","6");
                    //return RedirectToAction("Create", "Tender_CompanyInfo", routv);
                    
                //    scope.Complete();
                //}

                return Content("注册成功！请登录");
                //return RedirectToLocal("/");
            }
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        [MultipleResponseFormats][Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [MultipleResponseFormats]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // 在某些出错情况下，ChangePassword 将引发异常，而不是返回 false。
                bool changePasswordSucceeded;
                try
                {
                    changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return Content("密码修改成功!");
                    //return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                else
                {
                    return Content("当前密码不正确或新密码无效");
                    
                }
            }
            return Content("坏小孩");
        }

        [MultipleResponseFormats]
        [Authorize(Roles="admin")]
        public ActionResult ResetPassword()
        {

            return View();
        }

        [MultipleResponseFormats][HttpPost]
        public ActionResult ResetPassword(ResetPwdModel model)
        {
            if(ModelState.IsValid)
            {
                if(db.BEmplyees.Where(m=>m.NumberEmp==model.UserName).Count()<=0)
                {
                    return Content("单位名称错误!");
                }
                if(WebSecurity.ResetPassword(WebSecurity.GeneratePasswordResetToken(model.UserName),model.NewPassword))
                {
                    return Content("重置密码成功！");
                }else
                {
                    return Content("重置密码错误！");
                }
            }
            return Content("坏小孩");
        }


        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

    }
}
