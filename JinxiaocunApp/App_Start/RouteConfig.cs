using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JinxiaocunApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //        "Edit",
            //        "{controller}/{action}/{id}/{SuperRef}",
            //        new { Controller = "BPartMenu", action = "Edit", id = UrlParameter.Optional, SuperRef = UrlParameter.Optional }
            //    );

            //routes.MapRoute(
            //        "Menu",
            //        "BPartMenu/Menu/{id}/{tagetdiv}",
            //        new { Controller = "BPartMenu", action = "Menu", id = UrlParameter.Optional, tagetdiv = UrlParameter.Optional }
            //    );

            //routes.MapRoute(
            //        "Tabs",
            //        "MyTart/MenuPartial/{id}/{tabName}",
            //        new { Controller = "MyTart", action = "MenuPartial", id = UrlParameter.Optional, tabName = UrlParameter.Optional }
            //    );

            //routes.MapRoute(
            //        "Pagination",
            //        "MyTart/PaginationPartial/{id}/{TotalPage}/{tbodyPages}/{ContrllerName}",
            //        new { Controller = "MyTart", action = "PaginationPartial", id = UrlParameter.Optional, TotalPage = UrlParameter.Optional, tbodyPages=UrlParameter.Optional,ContrllerName=UrlParameter.Optional }
            //    );

        }
    }
}