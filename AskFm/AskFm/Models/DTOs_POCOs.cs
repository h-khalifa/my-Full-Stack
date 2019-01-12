using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AskFm.Models
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string bio { get; set; }
        public string UserName { get; set; }
        public Gender Gender { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public bool IsClientFollower { get; set; } //in case of authenticated client
        public List<AnswerDTO> Answers { get; set; }
    }
    public class AnswerDTO   //used in diplaying an existing answer to anybody
        //data from two tables? join
    {
        //public AnswerDTO()
        //{
        //    AskedBy = new MiniUser();
        //    AnsweredBy = new MiniUser();
        //}

        public Guid Id { get; set; }
        public string Q { get; set; }
        public string A { get; set; }
        public int Likes { get; set; }
        public bool IsAnon { get; set; }
        public bool ContainsPhoto { get; set; }
        public bool DoClientLike { get; set; }
        public DateTime Date { get; set; }
        public MiniUser AskedBy { get; set; }
        public MiniUser AnsweredBy { get; set; }
        public List<MiniUser> Likers { get; set; }
    }
    public class MiniUser
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }
    }

    public class answerViewModel  //used for answering existing question
    {
        public Guid QuestionId { get; set; }
        public string Q { get; set; }
        public string A { get; set; }
        public string AskedBy_Id { get; set; }
        public string AnsweredBy_Id { get; set; }
        public HttpPostedFileBase img { get; set; }
        public bool IsAnon { get; set; }
        public bool ContainsPhoto { get; set; }
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
    }


}