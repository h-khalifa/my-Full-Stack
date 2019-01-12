using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AskFm.Models
{
    public class Answer_Photo
    {
        [ForeignKey("Answer")]
        public Guid Id { get; set; }
        public byte[] Photo { get; set; }

        public virtual Answer Answer { get; set; }
    }
}