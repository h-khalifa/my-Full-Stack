using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ExO
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Question",
               url: "Exam/{ExamCode}/Question/{action}/{id}",
               defaults: new { controller = "Exam", id = UrlParameter.Optional }
           );
            
            routes.MapRoute(
                name: "Exam",
                url: "Exam/{ExamCode}/{action}/{id}",
                defaults: new { controller = "Exam", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
