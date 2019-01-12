using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AskFm.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public string question { get; set; }
        public bool IsAnon { get; set; }

        [ForeignKey("AskedBy")]
        public string AskedBy_Id { get; set; }
        [ForeignKey("AskedTo")]
        public string AskedTo_Id { get; set; }

        virtual public ApplicationUser AskedBy { get; set; }
        virtual public ApplicationUser AskedTo { get; set; }
    }
}