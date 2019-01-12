using AskFm.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AskFm.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //temp
            //ApplicationDbContext context = new ApplicationDbContext();
            //UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
            //UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);
            //String newPassword = "AskFm@123"; //"<PasswordAsTypedByUser>";
            //String hashedNewPassword = UserManager.PasswordHasher.HashPassword(newPassword);
            //foreach(var u in context.Users)
            //{
            //    if (u.SecurityStamp==null)
            //    u.SecurityStamp = hashedNewPassword;
            //}
            //context.SaveChanges();
          
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}