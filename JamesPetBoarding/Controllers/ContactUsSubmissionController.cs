using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using System;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    public class ContactUsSubmissionController : Controller
    {

        // GET: ContactUs
        public ActionResult Index()
        {
            return View(new ContactUsSubmissionVM());
        }

        // POST: ContactUs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ContactUsSubmissionVM contactUsSubmissionVM)
        {
            if (!ModelState.IsValid) 
            { 
                return View(contactUsSubmissionVM); 
            }

            ApplicationDbContext dbContext = new ApplicationDbContext();

            ContactUsSubmissionModel submission = new ContactUsSubmissionModel();

            submission.LastName = contactUsSubmissionVM.LastName;
            submission.FirstName = contactUsSubmissionVM.FirstName;
            submission.Phone = contactUsSubmissionVM.Phone;
            submission.Email = contactUsSubmissionVM.Email;
            submission.Message = contactUsSubmissionVM.Message;
            submission.SubmissionDateTime = DateTime.Now;

            dbContext.ContactUsSubmissions.Add(submission);
            dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Thank you for contacting Paws & Reservations. " +
                "We have received your message and a member of our team will reach out to you in 1-2 business days.";

            return RedirectToAction("ContactUsSuccessful");
        }

        public ActionResult ContactUsSuccessful() 
        { 
            return View(); 
        }
    }
}