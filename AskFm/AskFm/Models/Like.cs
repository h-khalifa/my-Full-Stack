using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AskFm.Models
{
    public class Like
    {
        public Guid Id { get; set; }
        public string AnsweredBy_Id { get; set; }    // only for better search since i cannot create view

        [ForeignKey("Liker")]
        public string LikerId { get; set; }

        [ForeignKey("Answer")]
        public Guid AnswerId { get; set; }

        virtual public ApplicationUser Liker { get; set; }
        virtual public Answer Answer { get; set; }
    }
}