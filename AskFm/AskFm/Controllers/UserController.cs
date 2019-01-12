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
    
    public class UserController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            string Client_UserName = User.Identity.GetUserName();
            return Redirect("/User/profile/" + Client_UserName);
        }

        // GET: User
        [Route("/User/{username}")]
        public ActionResult Profile(string username)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();

            //make sure the user name is right
            List<string> usernames = ctx.Users.Select(u => u.UserName.ToLower()).ToList();
            if (String.IsNullOrEmpty(username) || !usernames.Contains(username.ToLower()))
                return HttpNotFound();

            //if the client is authenticated and the profile not his ---> can follow and like
            //bool CanClientFollowAndLike = false;
            //if (User.Identity.IsAuthenticated)
            //{
            //    CanClientFollowAndLike = (User.Identity.GetUserName() == username);
            //}

            var user = ctx.Users.Single(u => u.UserName == username);
            UserDTO model = new UserDTO()
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FirstName + " " + user.LastName,
                bio = user.biography,
                Gender = user.Gender,
                Followers = user.Followers.Count,
                Following = user.Following.Count,
            };

            // add answers to the model
            // the following code indicates that, the models navigation props in 'many' side must be hash set, otherwise 
            //the performance would be disaster in case of long lists.
            //model.Answers = ctx.Answers.Where(a => a.AnsweredBy_Id == user.Id).OrderByDescending(a => a.Date)
            //    .Select(a => new AnswerDTO()
            //    {
            //        Q = a.Answer_question,
            //        A = a.Answer_answer,
            //        Likes = a.Likes.Count,
            //        Id = a.Id,
            //        Date = a.Date,
            //        ContainsPhoto = a.ContainsPhoto,
            //        IsAnon=a.IsAnon,

            //    }).ToList();
            model.Answers = ctx.Answers.Where(a => a.AnsweredBy_Id == user.Id).GroupJoin(ctx.Users, a => a.AskedBy_Id, u => u.Id,
                (a, u) => new AnswerDTO()
                {
                    Q = a.Answer_question,
                    A = a.Answer_answer,
                    Likes = a.Likes.Count,
                    Id = a.Id,
                    Date = a.Date,
                    ContainsPhoto = a.ContainsPhoto,
                    IsAnon = a.IsAnon,
                    AskedBy = (a.IsAnon)? null: new MiniUser()
                    {
                        Id = u.FirstOrDefault().Id,
                        UserName = u.FirstOrDefault().UserName,
                        FullName = u.FirstOrDefault().FirstName + " " + u.FirstOrDefault().LastName,
                    }
                }
                ).OrderByDescending(a => a.Date).ToList();
            //model.Answers = ctx.Answers.Where(a => a.AnsweredBy_Id == user.Id).Join(ctx.Users, a => a.AskedBy_Id, u => u.Id,
            //    (a, u) => new AnswerDTO()
            //    {
            //        Q = a.Answer_question,
            //        A = a.Answer_answer,
            //        Likes = a.Likes.Count,
            //        Id = a.Id,
            //        Date = a.Date,
            //        ContainsPhoto = a.ContainsPhoto,
            //        IsAnon = a.IsAnon,
            //        AskedBy = (a.IsAnon) ? null : new MiniUser()
            //        {
            //            Id = u.Id,
            //            UserName = u.UserName,
            //            FullName = u.FirstName + " " + u.LastName,
            //        }
            //    }
            //    ).ToList();

            //client:
            //1. not authenticated
            //2. authenticated and requesting profile of other user
            //3. authenticated and requesting his/her profile 
            if (!User.Identity.IsAuthenticated)
            {
                return View(model);
            }
            else
            {
                string Client_UserName = User.Identity.GetUserName();
                if (Client_UserName == user.UserName)
                    return View("Profile1", model);
                else
                {
                    //AUTHENTICATED CLIENT
                    //determine if the client is follower ----------------->follows table
                    //determine which answers he has liked before ---------> likes  table // very costy ? solution: dont include all answers on first load profile and use javascript ajax to query more answers on scroll down
                    string ClientId = User.Identity.GetUserId();
                    model.IsClientFollower = ctx.Follows.Where(f => f.FollowerId == ClientId && f.FollowedId == model.Id).Count() > 0;
                    List<Guid> AllLikesFromClientToProfile = ctx.Likes.Where(l => l.LikerId == ClientId && l.AnsweredBy_Id == model.Id).Select(l => l.AnswerId).ToList();
                    foreach(var answer in model.Answers)
                    {
                        answer.DoClientLike = AllLikesFromClientToProfile.Contains(answer.Id);
                    }
                    return View("Profile2", model);
                }
            }
                   
        }

        //get edit
        [Authorize]
        public ActionResult Edit(string username)
        {
            //make sure the client requesting his own profile to edit.
            //string ClientId = User.Identity.GetUserId();
            string Client_UserName = User.Identity.GetUserName();
            if (username != Client_UserName)
            {
                return HttpNotFound();
            }


            ApplicationDbContext ctx = new ApplicationDbContext();
            ApplicationUser user = ctx.Users.SingleOrDefault(u => u.UserName == username);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(ApplicationUser user)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            if (ModelState.IsValid)
            {
                ctx.Entry<ApplicationUser>(user).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                
                return Redirect("/User/profile/" + user.UserName);
            }
            else
            {
                return View(user);
            }
        }
            
        [Authorize]
        public ActionResult MyQuestions()
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            string ClientId = User.Identity.GetUserId();
            //ApplicationUser client = ctx.Users.Find(ClientId);
            //List<Question> Qlist = client.Questions_To;
            List<Question> Qlist = ctx.Questions.Where(q => q.AskedTo_Id == ClientId).ToList();
            return View(Qlist);
        }


        //to display an existing answer
        public ActionResult Answer(Guid Id)
        {
            ApplicationDbContext ctx = (new ApplicationDbContext());
            Answer answer = ctx.Answers.Find(Id);
            AnswerDTO model = new AnswerDTO()
            {
                Id = answer.Id,
                Q = answer.Answer_question,
                A = answer.Answer_answer,
                IsAnon = answer.IsAnon,
                ContainsPhoto = answer.ContainsPhoto,
                Likers = answer.Likes.Select(L => new MiniUser() { Id = L.LikerId }).ToList(),
                Likes = answer.Likes.Count,
                Date = answer.Date,
                AnsweredBy = new MiniUser(),
                AskedBy = new MiniUser(),
            };
            ApplicationUser x;
            model.AnsweredBy.Id = answer.AnsweredBy_Id;
            x = ctx.Users.Find(model.AnsweredBy.Id);
            model.AnsweredBy.UserName = x.UserName;
            model.AnsweredBy.FullName = x.FirstName + " " + x.LastName;
            if (!model.IsAnon)  //if the question wasn't asked anonimousy --> display info about asker
            {
                model.AskedBy.Id = answer.AskedBy_Id;
                x = ctx.Users.Find(model.AskedBy.Id);
                model.AskedBy.UserName = x.UserName;
                model.AskedBy.FullName = x.FirstName + " " + x.LastName;
            }
            // getting likers names and usernames for acount redirction and id for getting profile picture
            foreach (MiniUser liker in model.Likers)
            {
                x = ctx.Users.Find(liker.Id);
                liker.UserName = x.UserName;
                liker.FullName = x.FirstName + " " + x.LastName;
            }
            //if the client requesting the page wan authenticated ---> check if he/she had liked the answer before
            if (User.Identity.IsAuthenticated)
            {
                model.DoClientLike = model.Likers.Select(p => p.Id).ToList().Contains(User.Identity.GetUserId());
            }
            return View(model);
            //try
            //{

            //}
            //catch(Exception e)
            //{
            //    return HttpNotFound();
            //}
        }

        //solve question
        [Authorize]
        [HttpGet]
        public ActionResult MyAnswer(Guid Id)
        {
            Guid QId = Id;
            ApplicationDbContext ctx = new ApplicationDbContext();
            
            //1.fetch the question from db or return not found
            Question question = ctx.Questions.Find(QId);

            //2. make sure the client requesting the question is the right person
            string ClietId = User.Identity.GetUserId();
            if (question.AskedTo_Id != ClietId)
                return HttpNotFound();

            //3. construct the model and return it to the view
            answerViewModel model = new answerViewModel()
            {
                QuestionId = question.Id,
                Q = question.question,
                IsAnon = question.IsAnon,
                AnsweredBy_Id = question.AskedTo_Id
            };
            if (!model.IsAnon)
                model.AskedBy_Id = question.AskedBy_Id;
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MyAnswer(answerViewModel model)
        {
            HttpPostedFileBase img = model.img;

            ApplicationDbContext ctx = new ApplicationDbContext();

            //1.validate
            //no validation

            //2.insert answer
            Answer a = new Answer()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Answer_question = model.Q,
                Answer_answer = model.A,
                IsAnon = model.IsAnon,
                AnsweredBy_Id=User.Identity.GetUserId(),
            };
            if (!model.IsAnon)
            {
                a.AskedBy_Id = model.AskedBy_Id;
            }
            if (img != null)
            {
                a.ContainsPhoto = true;
                Answer_Photo ph = new Answer_Photo();
                ph.Id = a.Id;
                ph.Photo = new byte[img.ContentLength];
                img.InputStream.Read(ph.Photo, 0, img.ContentLength);
                ctx.Answers.Add(a);
                ctx.SaveChanges();
                ctx.answerPhotos.Add(ph);
                
            }
            else
            {
                a.ContainsPhoto = false;
                ctx.Answers.Add(a);
            }
            ctx.SaveChanges();
            //3.delete Question
            ctx.Questions.Remove(ctx.Questions.Find(model.QuestionId));
            ctx.SaveChanges();

            //4. make notification to the asker if not anon
            if (!model.IsAnon)
            {
                var temp = ctx.Users.Find(a.AnsweredBy_Id);
                Notification notification = new Notification()
                {
                    Id = Guid.NewGuid(),
                    message = temp.FirstName + " " + temp.LastName + " answered your Question.",
                    UrlType = NotUrlType.ToAnswer,
                    AnswerId = a.Id,
                    UserId = model.AskedBy_Id
                };
                ctx.notifications.Add(notification);
                ctx.SaveChanges();
            }
            return RedirectToAction("MyQuestions");
        }

        

        [Authorize]
        public ActionResult MyNotifications()
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            string ClientId = User.Identity.GetUserId();
            List<Notification> nots = ctx.notifications.RemoveRange(ctx.notifications.Where(n => n.UserId == ClientId)).ToList();
            //ctx.SaveChanges();
            //once you requested your notifications, they were removed.
            return View(nots);
        }

        [Authorize]
        public ActionResult MyFollowers()
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            string ClientId = User.Identity.GetUserId();
            List<MiniUser> followers = ctx.Users.Find(ClientId).Followers.Select(f => new MiniUser()
            {
                Id = f.Follower.Id,
                UserName = f.Follower.UserName,
                FullName = f.Follower.FirstName + " " + f.Follower.LastName,
            }).ToList();
            //join between follows and users tables should be used instead of nav-props querying
            return View(followers);
        }

        [Authorize]
        public ActionResult MyFollowings()
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            string ClientId = User.Identity.GetUserId();
            List<MiniUser> followings = ctx.Users.Find(ClientId).Following.Select(f => new MiniUser()
            {
                Id = f.Followed.Id,
                UserName = f.Followed.UserName,
                FullName = f.Followed.FirstName + " " + f.Follower.LastName,
            }).ToList();
            //join between follows and users tables should be used instead of nav-props querying
            return View(followings);
        }


        /// <summary>
        /// //////////////////////////
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public FileContentResult GetAnswerPhoto(Guid Id)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            byte[] p = ctx.answerPhotos.Find(Id).Photo;
            return new FileContentResult(p, "image/jpeg");
        }

        public FileContentResult GetProfilePhoto(string Id)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            try
            {
                byte[] p = ctx.profilePhotos.FirstOrDefault(ph => ph.Id == Id).Photo;
                return new FileContentResult(p, "image/jpeg");
            }
            catch
            {
                var user = ctx.Users.Single(u => u.Id == Id);
                ProfilePhoto p;
                if (user.Gender == Gender.Female)
                {
                    p = new ProfilePhoto() { Photo = imageToByteArray(System.Drawing.Image.FromFile(Server.MapPath("~/img/woman.png"))), Id = user.Id };
                    ctx.profilePhotos.Add(p);

                }
                else
                {
                    p = new ProfilePhoto() { Photo = imageToByteArray(System.Drawing.Image.FromFile(Server.MapPath("~/img/man.png"))), Id = user.Id };
                    ctx.profilePhotos.Add(p);
                }
                ctx.SaveChanges();
                return new FileContentResult(p.Photo, "image/jpeg");
            }
            
            
        }

        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
    }
}