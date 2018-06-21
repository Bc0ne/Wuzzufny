using Job_offers.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Job_offers.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        public ActionResult Details(int JobId)
        {
            var job = db.Jobs.Find(JobId);
            if (job == null)
            {
                return HttpNotFound();
            }
            Session["JobId"] = JobId;
            return View(job);
        }
        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Apply(string message)
        {
            var UserId = User.Identity.GetUserId();
            var JobId = (int)Session["JobId"];
            var check = db.ApplyForJobs.Where(j => j.JobId == JobId && j.UserId == UserId).ToList();
            if (check.Count < 1)
            {
                var job = new ApplyForJob()
                {
                    UserId = UserId,
                    JobId = JobId,
                    Message = message,
                    ApplyDate = DateTime.Now
                };
                db.ApplyForJobs.Add(job);
                db.SaveChanges();
                ViewBag.Message = "You have successfully applied for this job.";
            }
            else
            {
                ViewBag.Message = "You have already applied before!";
            }
            return View();
        }

        [Authorize]
        public ActionResult GetJobsByUser()
        {
            var UserId = User.Identity.GetUserId();
            var Jobs = db.ApplyForJobs.Where(j => j.UserId == UserId);
            return View(Jobs.ToList());
        }

        [Authorize]
        public ActionResult DetailsOfJob(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        [Authorize]
        public ActionResult GetJobsByPublisher()
        {
            var UserID = User.Identity.GetUserId();

            var Jobs = from app in db.ApplyForJobs
                       join job in db.Jobs
                       on app.JobId equals job.Id
                       where job.User.Id == UserID
                       select app;

            var grouped = from j in Jobs
                          group j by j.job.JobTitle
                          into gr
                          select new JobsViewModel
                          {
                              JobTitle = gr.Key,
                              Items = gr
                          };

            return View(grouped.ToList());
        }


        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
                job.ApplyDate = DateTime.Now;
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetJobsByUser");
            }
            return View(job);
        }

        // GET
        public ActionResult Delete(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(ApplyForJob job)
        {

            // TODO: Add delete logic here
            if (job == null)
            {
                return HttpNotFound();
            }
            var myJob = db.ApplyForJobs.Find(job.Id);
            db.ApplyForJobs.Remove(myJob);
            db.SaveChanges();
            return RedirectToAction("GetJobsByUser");

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("bboyclaw@gmail.com", "wizzard1!");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("bboyclaw@gmail.com"));
            mail.Subject = contact.Subbject;
            mail.IsBodyHtml = true;
             
            string body = "Sender name: " + contact.Name + "<br>" +
                           "Sender Email " + contact.Email + "<br>" +
                           "Message title " + contact.Subbject + "<br>" +
                           "Message body " + contact.Message;
            mail.Body = body;
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(mail);
            return RedirectToAction("Index");
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchName)
        {
            var result = db.Jobs.Where(a => a.JobTitle. Contains(searchName)
            || a.JobContent.Contains(searchName)
            || a.Category.CategoryName.Contains(searchName)
            || a.Category.CategoryDescription.Contains(searchName)).ToList();
            return View(result);
        }
    }
}