using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AskFm.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public string Answer_question { get; set; }
        public string Answer_answer { get; set; }
        public Answer_Photo Answer_photo { get; set; }
        public DateTime Date { get; set; }
        public bool IsAnon { get; set; }
        public bool ContainsPhoto { get; set; }
        [ForeignKey("AskedBy")]
        public string AskedBy_Id { get; set; }
        [ForeignKey("AnsweredBy")]
        public string AnsweredBy_Id { get; set; }

        virtual public ApplicationUser AskedBy { get; set; }
        virtual public ApplicationUser AnsweredBy { get; set; }
        virtual public ICollection<Like> Likes { get; set; }
    }
}