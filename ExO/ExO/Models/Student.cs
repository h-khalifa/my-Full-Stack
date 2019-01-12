using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExO.Models
{
    public class Student
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        #region nav props
        public ApplicationUser ApplicationUser { get; set; }
        virtual public List<StudentExamStatus> Status { get; set; }
        #endregion
    }
}