using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExO.Models
{
    public class Exam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Isposted { get; set; }

        [ForeignKey("Teacher")]
        public string TeacherId { get; set; }

        #region nav props
        public Teacher Teacher { get; set; }
        virtual public ICollection<Question> Questions { get; set; }
        virtual public ICollection<StudentExamStatus> Status { get; set; }
        #endregion
    }
}