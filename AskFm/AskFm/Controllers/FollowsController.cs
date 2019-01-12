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
    public class FollowsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // POST: api/Follows
        public Guid PostFollow([FromBody]Follow follow)
        {
            Guid Id = Guid.NewGuid();
            follow.Id = Id;
            db.Follows.Add(follow);
            db.SaveChanges();
            return Id;
        }
        // DELETE: api/Follows/5
        public void DeleteFollow([FromUri]Guid id)
        {
            Follow follow = db.Follows.Find(id);
            db.Follows.Remove(follow);
            db.SaveChanges();
        }

        // POST: api/Follows
        public void PostFollow([FromUri]string followed_Id)
        {
            string clientId = User.Identity.GetUserId();
            Follow follow = new Follow() { FollowedId = followed_Id, FollowerId = clientId, Id = Guid.NewGuid() };
            db.Follows.Add(follow);
            db.SaveChanges();
            ///////////add notification\\\\\\\\\\\\\\\\
            string ClientUsername = User.Identity.GetUserName();
            Notification notification = new Notification()
            {
                Id = Guid.NewGuid(),
                UrlType = NotUrlType.ToProfile,
                message = ClientUsername + " followed you.",
                TriggerUserName = ClientUsername,
                UserId = followed_Id
            };
            //remove all notification about following the user if the client unfollowed then refollowed    ----->performance overload
            //db.notifications.RemoveRange(db.notifications.Where(n => n.UserId == model.AnsweredBy_Id && n.AnswerId == model.AnswerId));
            db.notifications.Add(notification);
            db.SaveChanges();
        }

        // DELETE: api/Follows/5
        public void DeleteFollow([FromUri]string followed_Id)
        {
            string clientId = User.Identity.GetUserId();
            Follow follow = db.Follows.Single(f => f.FollowedId == followed_Id && f.FollowerId == clientId);
            db.Follows.Remove(follow);
            db.SaveChanges();
        }

       
    }
}