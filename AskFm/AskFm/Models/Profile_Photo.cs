using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AskFm.Models
{
    public class ProfilePhoto
    {
        [ForeignKey("User")]
        public string Id { get; set; }
        public byte[] Photo { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}