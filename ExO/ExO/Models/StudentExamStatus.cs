using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExO.Models
{
    public class StudentExamStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool IsComplete { get; set; }
        public int Score { get; set; }
        public int SolvedQuestions { get; set; }


        [ForeignKey("Exam")]
        public int ExamId { get; set; }
        [ForeignKey("Student")]
        public string StudentId { get; set; }


        //nav-props
        public Student Student { get; set; }
        public Exam Exam { get; set; }
    }
}