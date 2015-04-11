using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using System.Web;
using WebMatrix.WebData;
using JinxiaocunApp.Tools;

namespace JinxiaocunApp.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembership : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<JinxiaocunApp.Models.JinxiaocunAppContext>(null);

                try
                {
                    using (var context = new JinxiaocunApp.Models.JinxiaocunAppContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }
                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class JinxcAuthorizeAttribute : AuthorizeAttribute
    {
        private  bool _authorize;

        private bool _isPermissionFail = false;

        public JinxcAuthorizeAttribute()
        {
            if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                _authorize = false;
            }
            else
            {
                _authorize = true;
            }
        }

        public JinxcAuthorizeAttribute(string permission)
        {
            if (HttpContext.Current.User.Identity.Name != "")
            {
                _authorize = PermissionManager.CheckUserHasPermision(HttpContext.Current.User.Identity.Name, permission);
                if (_authorize == false)
                {
                    _isPermissionFail = true;
                }
            }
            else
            {
                _authorize = false;
            }
            //_authorize = true;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            //httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            if (_isPermissionFail||string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                _authorize = false;
            }
            else
            {
                _authorize = true;
            }
            return _authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (_isPermissionFail)
            {
                
                //filterContext.HttpContext.Response.Redirect("/MyTart/Part3");

                ContentResult cont = new ContentResult();
                cont.Content = "没有权限" + filterContext.Controller.ControllerContext.RouteData.Values["controller"]+filterContext.ActionDescriptor.ActionName;
                filterContext.Result = cont;
            }
            else
            {
                
                //PartialViewResult pv = new PartialViewResult();
                //pv.View = new RazorView(filterContext, "/Account/Login", null, false, null);
                //ContentResult cont = new ContentResult();
                //cont.Content = "没有权限" + filterContext.Controller.ControllerContext.RouteData.Values["controller"] + filterContext.ActionDescriptor.ActionName;
                //filterContext.Controller=
                ////filterContext.Result = ViewEngines.Engines.FindPartialView(filterContext.Controller=new Controller(, "Login").View;

                base.HandleUnauthorizedRequest(filterContext);
            }

        }
    }
}