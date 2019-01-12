using ExO.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace ExO.Controllers
{

    public class ExamController : Controller
    {
        [Authorize]
        // GET: Exam
        public ActionResult Index()
        {
            //teacher or student?
            bool IsTeacher = User.IsInRole("Teacher");
            if (IsTeacher)
            {
                return RedirectToAction("Create");
            }
            else
            {
                return RedirectToAction("Solve");
            }
            
        }

        #region Teacher Domain

        [Authorize (Roles ="Teacher")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(Exam exam)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            //make sure the code is unique
            List<string> ExamCodes = ctx.Exams.Select(e => e.Code).ToList();
            if (ExamCodes.Contains(exam.Code))
            {
                ModelState.AddModelError("Code", "the code already exists.");
                return View(exam);
            }
            //new instance of exam
            Exam newExam = new Exam() {Name=exam.Name, Code=exam.Code, Isposted=false, TeacherId=User.Identity.GetUserId() };
            ctx.Exams.Add(newExam);
            ctx.SaveChanges();
            //go to add question
            return Redirect("/exam/"+exam.Code+"/addquestion");
        }

        //Get AddQuestion
        [Authorize(Roles ="Teacher")]
       // [Route("exam/{examcode}/addquestion")]
        public ActionResult AddQuestion(string ExamCode)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            QuestionViewModel question = new QuestionViewModel();

            //1. make sure the exam is existing & initialized by current user & not posted yet
            Exam exam = ctx.Exams.Single(e => e.Code == ExamCode);
            if ((exam == null) || (exam.TeacherId != User.Identity.GetUserId()) || exam.Isposted )
                return HttpNotFound();

            //2. initialize the question data
            question.Q_num = exam.Questions.Count + 1;
            question.ExamCode = ExamCode;

            ViewBag.ExamName = exam.Name;


            return View(question);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        // [Route("exam/{examcode}/addquestion")]
        public ActionResult AddQuestion(QuestionViewModel modelQuestion)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            Exam exam = ctx.Exams.Single(e => e.Code == modelQuestion.ExamCode);
            if (ModelState.IsValid)
            {
                ctx.Questions.Add(new Question()
                {
                    question = modelQuestion.question,
                    AnswerA = modelQuestion.AnswerA,
                    AnswerB = modelQuestion.AnswerB,
                    AnswerC = modelQuestion.AnswerC,
                    AnswerD = modelQuestion.AnswerD,
                    CorrectAnswer = modelQuestion.CorrectAnswer,
                    ExamId = exam.Id
                });
                ctx.SaveChanges();
            }
            return Redirect("/exam/" + exam.Code + "/addQuestion");
        }

        [Authorize(Roles = "Teacher")]
        //[Route("exam/{examcode}/Finish")]
        public ActionResult Finish(string examcode)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            Exam exam = ctx.Exams.Where(e => e.Code == examcode).Include(e => e.Questions).Single();
            
            //make sure the question is in unposted exam & the user is the real teacher who created the exam
            if (exam.Isposted || exam.TeacherId != User.Identity.GetUserId())
                return HttpNotFound();

            return View(exam);
        }

        [Authorize(Roles ="Teacher")]
        public ActionResult SaveExam(string examcode)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            Exam exam = ctx.Exams.Single(e => e.Code == examcode);
            exam.Isposted = true;
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteQuestion(string examcode, int? id)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();

            //make sure the question is in unposted exam & the user is the real teacher who created the exam
            Exam exam = ctx.Exams.Single(e => e.Code == examcode);
            if (exam.Isposted || exam.TeacherId != User.Identity.GetUserId())
                return HttpNotFound();


            Question question = ctx.Questions.Single(q => q.Id == id);
            ctx.Questions.Remove(question);
            ctx.SaveChanges();
            return Redirect("/Exam/" + examcode + "/Finish");
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult EditQuestion(string examcode, int? id)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();

            if (id == null)
                return HttpNotFound();
            Exam exam = ctx.Exams.Single(e => e.Code == examcode);
            //make sure the question is in unposted exam & the user is the real teacher who created the exam
            if (exam.Isposted || exam.TeacherId != User.Identity.GetUserId())
                return HttpNotFound();

            Question Q = ctx.Questions.Single(q => q.Id == id);
            return View(Q);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult EditQuestion(Question Q)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            Exam exam = ctx.Exams.Single(e => e.Id == Q.ExamId);

            ctx.Entry<Question>(Q).State = EntityState.Modified;
            //Question editedQuestion = ctx.Questions.Single(q => q.Id == Q.Id);
            //editedQuestion = Q;
            ctx.SaveChanges();
            return Redirect("/Exam/" + exam.Code + "/Finish");
        }


        [Authorize(Roles ="Teacher")]
        public ActionResult Grades(string ExamCode)
        {
            XSSFWorkbook wb = GenerateSheet(ExamCode);

            byte[] fileContents = null;
            using (var memoryStream = new MemoryStream())
            {
                wb.Write(memoryStream);
                fileContents = memoryStream.ToArray();
            }
            return File(fileContents, System.Net.Mime.MediaTypeNames.Application.Octet, ExamCode + ".xlsx");
        }

        private XSSFWorkbook GenerateSheet(string ExamCode)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();

            Exam exam = ctx.Exams.Single(e => e.Code == ExamCode);
            List<Grade> grades = ctx.Statuses.Where(s => s.ExamId == exam.Id).Select(s => new Grade() {
                StudentName = s.Student.ApplicationUser.UserName,
                Score = s.Score
            }).ToList();

            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet Grades = workbook.CreateSheet("Grades");
            var HeadRow = Grades.CreateRow(0);
            HeadRow.CreateCell(0).SetCellValue("Student Name");
            HeadRow.CreateCell(1).SetCellValue("Grades");
            int i = 0;
            foreach  (Grade grade in grades)
                {
                    var row = Grades.CreateRow(++i);
                    row.CreateCell(0).SetCellValue(grade.StudentName);
                    row.CreateCell(1).SetCellValue(grade.Score);
                }

            return workbook;
        }

        #endregion

        #region Student Domain

        [Authorize(Roles = "Student")]
        public ActionResult Solve()
        {
            
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public ActionResult Solve(QuestionViewModel Q)
        {
            if (String.IsNullOrEmpty(Q.ExamCode))
            {
                ModelState.AddModelError("ExamCode", "Code Cannot be empty.");
                return View(Q);
            }
            ApplicationDbContext ctx = new ApplicationDbContext();
            List<string> ExamCodes = ctx.Exams.Select(e => e.Code).ToList();
            if (!ExamCodes.Contains(Q.ExamCode))
            {
                ModelState.AddModelError("ExamCode", "There is no Exam with such Code.");
                return View(Q);
            }
            return Redirect("/Exam/"+Q.ExamCode+"/Get");
        }

        [Authorize(Roles ="Student")]
        //get status
        public ActionResult Get(string examCode)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();

            //1.get status or create one for start
            Exam exam = ctx.Exams.Single(e => e.Code == examCode);
            string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            StudentExamStatus studentExamStatus = ctx.Statuses.SingleOrDefault(s => (s.ExamId == exam.Id) && (s.StudentId == userId));
            if(studentExamStatus == null)
            {
                studentExamStatus = new StudentExamStatus()
                {
                    IsComplete = false,
                    Score = 0,
                    SolvedQuestions = 0,
                    ExamId = exam.Id,
                    StudentId = userId,

                };
                ctx.Statuses.Add(studentExamStatus);
                ctx.SaveChanges();
            }
            //2.if it's complete redirect to finished action
            if (studentExamStatus.IsComplete)
            {
                return Redirect("/Exam/" + examCode + "/Finished");
            }

            //3.pass it to the view
            StudentExamStatusViewModel S = new StudentExamStatusViewModel(studentExamStatus);
            S.ExamSize = exam.Questions.Count;
            S.ExamName = exam.Name;
            S.ExamCode = exam.Code;   //the view gets the exam id only for redirecting purpose
            return View(S);
        }

        [Authorize(Roles = "Student")]
        public ActionResult Question(string examcode)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            
            //1.make sure .......
            Exam exam = ctx.Exams.Single(e => e.Code == examcode);
            if (exam == null)
            {
                return HttpNotFound();
            }
            string userId = User.Identity.GetUserId();
            StudentExamStatus status = ctx.Statuses.Single(s => s.ExamId == exam.Id && s.StudentId == userId);
            if (status.IsComplete  || status.SolvedQuestions == exam.Questions.Count)
                return Redirect("/Exam/" + exam.Code + "/Finished");

           
            Question nextQuestion = ctx.Questions.Where(q => q.ExamId == exam.Id).AsEnumerable().Skip(status.SolvedQuestions).First();
            QuestionViewModel nextQuestionVM = new QuestionViewModel(nextQuestion);
            nextQuestionVM.ExamName = exam.Name;
            nextQuestionVM.ExamCode = examcode;
            return View(nextQuestionVM);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public ActionResult Question(string examcode, string answer)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();

            //get exam + status + question
            Exam exam = ctx.Exams.Single(e => e.Code == examcode);
            string userId = User.Identity.GetUserId();
            StudentExamStatus status = ctx.Statuses.Single(s => s.ExamId == exam.Id && s.StudentId == userId);
            Question targetQuestion = ctx.Questions.Where(q => q.ExamId == exam.Id).AsEnumerable().Skip(status.SolvedQuestions).First();

            //verify submitted solution
            status.SolvedQuestions++;
            if (answer == targetQuestion.CorrectAnswer)
                status.Score++;
            ctx.Entry(status).State = EntityState.Modified;
            ctx.SaveChanges();


            //refresh page
            return Redirect("/exam/" + examcode + "/Question");
        }

        [Authorize(Roles = "Student")]
        public ActionResult Finished(string examcode)
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
            
            //make sure all questions are solved or return to question
            Exam exam = ctx.Exams.Single(e => e.Code == examcode);
            string userId = User.Identity.GetUserId();
            StudentExamStatus status = ctx.Statuses.Single(s => s.ExamId == exam.Id && s.StudentId == userId);
            if (status.SolvedQuestions < exam.Questions.Count)
                return Redirect("/exam/" + examcode + "/Question");

            //finish
            status.IsComplete = true;
            ctx.Entry(status).State = EntityState.Modified;
            ctx.SaveChanges();

            //make view
            StudentExamStatusViewModel statusVM = new StudentExamStatusViewModel(status);
            statusVM.ExamName = exam.Name;
            statusVM.ExamSize = exam.Questions.Count;

            return View(statusVM);
        }

        #endregion
    }
}