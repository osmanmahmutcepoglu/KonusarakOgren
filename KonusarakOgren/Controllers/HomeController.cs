using HtmlAgilityPack;
using KonusarakOgren.Context;
using KonusarakOgren.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;

namespace KonusarakOgren.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.ExamTitles = GetExamTitles().Select(x => new SelectListItem
            {
                Selected = x.Id == 1,
                Text = x.Title,
                Value = x.Href
            }).ToList();
            return View();
        }

        private List<ExamTitle> GetExamTitles()
        {
            var titleObjects = new List<ExamTitle>();
            if (!_context.ExamTitles.Any())
            {

                var urll = "https://www.wired.com/feed/rss";
                var reader = XmlReader.Create(urll);
                var feed = SyndicationFeed.Load(reader);
                var posts = feed.Items.OrderByDescending(x => x.PublishDate).Take(5).ToList();
                var result = posts[0].Title.Text;
                var resulTt = posts[0].Links[0].Uri;

                foreach (var item in posts)
                {
                    titleObjects.Add(new ExamTitle { Id = titleObjects.Count + 1, Title = item.Title.Text, Href = item.Links[0].Uri.ToString() });
                }

                _context.ExamTitles.AddRange(titleObjects);
                _context.SaveChanges();
            }
            else
            {
                titleObjects = _context.ExamTitles.ToList();
            }

            return titleObjects;
        }

        [HttpPost]
        public IActionResult GetExamText(string href)
        {
            var web = new HtmlWeb();
            var doc = web.Load(href);
            var text = doc.DocumentNode.SelectSingleNode("//article");
            return Json(text.InnerHtml);
        }

        [HttpPost]
        public IActionResult CreateExam(Exam exam)
        {
            exam.CreatedDate = DateTime.Now;
            _context.Exams.Add(exam);
            _context.SaveChanges();
            return RedirectToAction("ExamList");
        }
        [HttpPost]
        public IActionResult GetQuestions(int ExamId)
        {
            var a = PartialView("QuestionsView", _context.Questions.Where(x => x.ExamId == ExamId)
                .Select(x => new QuestionsViewModel { A = x.A, B = x.B, C = x.C, D = x.D, QuestionId = x.Id, Text = x.Text })
                .ToList());
            return a;
        }

        [HttpPost]
        public IActionResult GetRightAnswers(int ExamId)
        {
            var answers = _context.Questions.Where(x => x.ExamId == ExamId)
                 .Select(x => x.RightAnswer);
            return Json(answers);
        }

        public IActionResult ExamList()
        {
            return View(_context.Exams);
        }
        public IActionResult Delete(int Id)
        {
            var exam = _context.Exams.First(x => x.Id == Id);
            if (exam != null)
            {
                _context.Exams.Remove(exam);
                _context.SaveChanges();
                return RedirectToAction("ExamList");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult StartExam(int Id)
        {
            return View(_context.Exams.First(x => x.Id == Id));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
