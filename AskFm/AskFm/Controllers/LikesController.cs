using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AskFm.Models;
using Microsoft.AspNet.Identity;

namespace AskFm.Controllers
{
    public class LikesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

       

       

       

        // POST: api/Likes
        public void PostLike([FromBody]Like model)
        {
            //the coming model only contain answerid and the id of user answered

            string ClientId = User.Identity.GetUserId();
            model.LikerId = ClientId;
            model.Id = Guid.NewGuid();
            db.Likes.Add(model);
            db.SaveChanges();

            ///////////add notification\\\\\\\\\\\\\\\\
            string ClientUsername = User.Identity.GetUserName();
            Notification notification = new Notification()
            {
                Id = Guid.NewGuid(),
                UrlType = NotUrlType.ToAnswer,
                message = ClientUsername + " liked your answer.",
                AnswerId = model.AnswerId,
                UserId = model.AnsweredBy_Id,
            };
            //remove all notification about this answer to the user who answered it
            //db.notifications.RemoveRange(db.notifications.Where(n => n.UserId == model.AnsweredBy_Id && n.AnswerId == model.AnswerId));
            db.notifications.Add(notification);
            db.SaveChanges();
        }

        // DELETE: api/Likes/5
        public void DeleteLike([FromUri]Guid AnswerId)
        {
            string ClientId = User.Identity.GetUserId();
            db.Likes.RemoveRange(db.Likes.Where(l => l.AnswerId == AnswerId && l.LikerId == ClientId));
            db.SaveChanges();
        }

        
    }
}