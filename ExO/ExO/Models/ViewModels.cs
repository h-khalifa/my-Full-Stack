using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExO.Models
{
    
        public class LoginViewModel
        {
            [Required]
            [Display(Name = "Email")]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public class RegisterViewModel
        {
            [Required]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name ="Use your account as: ")]
            public Role Role { get; set; }
        }

        public enum Role
        {
            Teacher,
            Student
        }

    
    public class QuestionViewModel
    {
        [Required]
        public string question { get; set; }

        [Required]
        [Display(Name = "A")]
        public string AnswerA { get; set; }


        [Required]
        [Display(Name = "B")]
        public string AnswerB { get; set; }

        [Required]
        [Display(Name = "C")]
        public string AnswerC { get; set; }

        [Required]
        [Display(Name = "D")]
        public string AnswerD { get; set; }

        [Required]
        [Display(Name = "Correct Answer")]
        public string CorrectAnswer { get; set; }

        public int Q_num { get; set; }

        public string ExamCode { get; set; }
        public string ExamName { get; set; }
        public int Id { get; set; }

        //constructor as a mapper
        public QuestionViewModel(Question QQ)
        {
            this.question = QQ.question;
            this.AnswerA = QQ.AnswerA;
            this.AnswerB = QQ.AnswerB;
            this.AnswerC = QQ.AnswerC;
            this.AnswerD = QQ.AnswerD;
        }
        public QuestionViewModel()
        {

        }
    }
    public class StudentExamStatusViewModel
    {
        //public int Id { get; set; }
        public bool IsComplete { get; set; }
        public int Score { get; set; }
        public int SolvedQuestions { get; set; }
        //public int ExamId { get; set; }
        //public string StudentId { get; set; }
        public int ExamSize { get; set; }
        public string ExamName { get; set; }
        public string ExamCode { get; set; }

        //constructors instead of using external mapper
        public StudentExamStatusViewModel(StudentExamStatus studentExamStatus)
        {
            //this.Id = studentExamStatus.Id;
            this.IsComplete = studentExamStatus.IsComplete;
            this.Score = studentExamStatus.Score;
            this.SolvedQuestions = studentExamStatus.SolvedQuestions;
            //this.ExamId = studentExamStatus.ExamId;
            //this.StudentId = studentExamStatus.StudentId;
        }
    }

    public class TeacherProfileVeiwModel {
        public string UserName { get; set; }
        public List<ExamInProfileViewModel> Exams { get; set; }
    }
    public class StudentProfileViewModel {
        public string UserName { get; set; }
        public List<StatusInprofileVieModel> Status { get; set; }
    }

    public class ExamInProfileViewModel {
        public int ExamSize { get; set; }
        public string ExamName { get; set; }
        public string ExamCode { get; set; }
        public double AverageScore { get; set; }
        public int NumberOfStudents { get; set; }
        public bool IsPosted { get; set; }
    }
    public class StatusInprofileVieModel {
        public int score { get; set; }
        public string ExamName { get; set; }
        public string ExamCode { get; set; }
        public bool IsComplete { get; set; }
        public int ExamSize { get; set; }
    }

    public class Grade
    {
        public int Score { get; set; }
        public string StudentName { get; set; }
    }
}