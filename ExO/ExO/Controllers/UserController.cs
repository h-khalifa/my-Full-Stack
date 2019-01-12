using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ExO.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Host.SystemWeb;

namespace ExO.Controllers
{
    public class UserController : Controller
    {
        

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var user = UserManager.FindByEmail(model.Email);
            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }


         // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, EmailConfirmed = true };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //UserManager.AddToRole(user.Id, model.Role.ToString("F"));
                    ApplicationDbContext ctx = new ApplicationDbContext();
                    if (model.Role == Role.Teacher)
                    {
                        UserManager.AddToRole(user.Id, "Teacher");
                        Teacher teacher = new Teacher() { Id = user.Id };                        
                        ctx.Teachers.Add(teacher);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        UserManager.AddToRole(user.Id, "Student");
                        Student student = new Student() { Id = user.Id };
                        ctx.Students.Add(student);
                        ctx.SaveChanges();
                    }
                        //UserManager.AddToRole(user.Id, "Student");
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    
                    return RedirectToAction("Index", "Home");
                }
                
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        
        
        [Authorize]
        public ActionResult Profile(string username)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            string userId = User.Identity.GetUserId();
            string userName = User.Identity.GetUserName();
            if (User.IsInRole("Student"))
            {
                StudentProfileViewModel studentprofile = new StudentProfileViewModel();
                studentprofile.UserName = userName;
                studentprofile.Status = ctx.Exams.Join(ctx.Statuses.Where(s => s.StudentId == userId), e => e.Id, s => s.ExamId, (e, s) =>
                       new StatusInprofileVieModel()
                       {
                           score = s.Score,
                           IsComplete = s.IsComplete,
                           ExamCode = e.Code,
                           ExamName = e.Name,
                           ExamSize = e.Questions.Count
                       }).ToList();

                return View("Profile_Student", studentprofile);
            }
            else
            {
                TeacherProfileVeiwModel teacherProfile = new TeacherProfileVeiwModel();
                teacherProfile.UserName = userName;
                teacherProfile.Exams = ctx.Exams.Where(e => e.TeacherId == userId).Select(e => new ExamInProfileViewModel()
                {
                    ExamCode = e.Code,
                    ExamName = e.Name,
                    ExamSize = e.Questions.Count,
                    IsPosted = e.Isposted,
                    NumberOfStudents = (e.Isposted) ? ctx.Statuses.Where(s => s.IsComplete && s.ExamId == e.Id).Count() : 0,
                    //AverageScore = (e.Isposted ) ? Math.Round(ctx.Statuses.Where(s => s.IsComplete && s.ExamId == e.Id).AsEnumerable().Average(s => s.Score), 0) : 0
                }).ToList();

                foreach (ExamInProfileViewModel i in teacherProfile.Exams)
                {
                    int id = ctx.Exams.Single(ex => ex.Code == i.ExamCode).Id;
                    i.AverageScore = (i.NumberOfStudents > 0) ? Math.Round(ctx.Statuses.Where(s => s.ExamId == id).Average(e => e.Score),2) : 0;
                }

                return View("Profile_Teacher",teacherProfile);
            }
            
        }





        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}