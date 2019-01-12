using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AskFm.Models
{
    public class Follow
    {
        public Guid Id { get; set; }

        [ForeignKey("Follower")]
        public string FollowerId { get; set; }

        [ForeignKey("Followed")]
        public string FollowedId { get; set; }

        virtual public ApplicationUser Follower { get; set; }
        virtual public ApplicationUser Followed { get; set; }
    }
}