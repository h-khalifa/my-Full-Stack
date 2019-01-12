using AskFm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AskFm.Controllers
{
    public class AnswersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        //not needed
        //POST: Api/Answers
        //public void PostAnswer([FromBody] Answer model)
        //{
        //    model.Id = Guid.NewGuid();
        //   // db.Answers.
        //}

        //GET
        //Api/Answers/GetFiveAnswers
    }
}
