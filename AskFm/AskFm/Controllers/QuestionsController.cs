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
    public class QuestionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // POST: api/Questions 
        public void PostQuestion([FromBody]Question model)
        {

            if (!model.IsAnon)
            {
                string Client_Id = User.Identity.GetUserId();
                model.AskedBy_Id = Client_Id;
            }
            model.Id = Guid.NewGuid();
            db.Questions.Add(model);
            db.SaveChanges();
        }

        //GET
        // api/questions/getfiveQuestions?skip=5&Id
        public IEnumerable<Question> GetFiveQuestions(int? skip,string Id)
        {
            int x;
            if (skip == null)
             x = 0;
            else
              x = (int)skip;
            return db.Questions.AsEnumerable().Skip(x).Take(5).ToList();
        }

        // DELETE: api/Questions 
        public void DeleteQuestion([FromUri]Guid Id)
        {
            try {
                var q = db.Questions.Find(Id);
               // var x = new Guid();
                db.Questions.Remove(q);
                db.SaveChanges();
            }
            catch {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

    }
}