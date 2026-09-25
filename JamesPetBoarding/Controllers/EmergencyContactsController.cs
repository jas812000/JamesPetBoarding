using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using Microsoft.Owin.BuilderProperties;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection.Emit;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace JamesPetBoarding.Controllers
{
    [Authorize]
    public class EmergencyContactsController : Controller
    {

        // GET: EmergencyContacts/Create
        public ActionResult Create(Guid customerId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            EmergencyContactFormVM emergencyContactForm = new EmergencyContactFormVM();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer == null)
            {
                return Content("Customer ID #" + customerId + " does not exist.");
            }

            emergencyContactForm.CustomerId = customerId;

            return View(emergencyContactForm);
        }

        //POST: EmergencyContacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmergencyContactFormVM emergencyContactForm)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            if (!ModelState.IsValid)
            {
                return View(emergencyContactForm);
            }

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == emergencyContactForm.CustomerId);

            if (customer == null)
            {
                return Content("Customer ID #" + emergencyContactForm.CustomerId + " does not exist.");
            }

            EmergencyContactModel emergencyContact = new EmergencyContactModel();

            emergencyContact.CustomerId = emergencyContactForm.CustomerId;
            emergencyContact.LastName = FormatName(emergencyContactForm.LastName);
            emergencyContact.FirstName = FormatName(emergencyContactForm.FirstName);
            emergencyContact.Address = emergencyContactForm.Address;
            emergencyContact.City = emergencyContactForm.City;
            emergencyContact.State = emergencyContactForm.State;
            emergencyContact.ZipCode = emergencyContactForm.ZipCode;
            emergencyContact.Phone = emergencyContactForm.Phone;
            emergencyContact.Email = emergencyContactForm.Email;
            emergencyContact.RelationshipType = emergencyContactForm.RelationshipType;
            emergencyContact.IsActive = true;
            emergencyContact.Notes = emergencyContactForm.Notes;

            dbContext.EmergencyContacts.Add(emergencyContact);

            dbContext.SaveChanges();
            return RedirectToAction("Read", "Customers", new { customerId = emergencyContactForm.CustomerId });

        }


        // GET: EmergencyContacts/Read
        public ActionResult Read(Guid emergencyContactId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            EmergencyContactModel emergencyContact = dbContext.EmergencyContacts.FirstOrDefault(x => x.EmergencyContactId == emergencyContactId);

            if (emergencyContact == null)
            {
                return Content("Emergency Contact ID #" + emergencyContactId + " does not exist.");
            }

            string notesDisplay = string.IsNullOrWhiteSpace(emergencyContact.Notes)
                ? "No notes"
                : emergencyContact.Notes;

            EmergencyContactDetailsVM emergencyContactDetails = new EmergencyContactDetailsVM();

            emergencyContactDetails.EmergencyContactId = emergencyContact.EmergencyContactId;
            emergencyContactDetails.CustomerId = emergencyContact.CustomerId;
            emergencyContactDetails.FullNameDisplay = emergencyContact.FirstName + " " + emergencyContact.LastName;
            emergencyContactDetails.RelationshipDisplay = emergencyContact.RelationshipType.ToString();
            emergencyContactDetails.PhoneDisplay = emergencyContact.Phone;
            emergencyContactDetails.EmailDisplay = emergencyContact.Email;
            emergencyContactDetails.AddressDisplay = emergencyContact.Address;
            emergencyContactDetails.CityStateZipDisplay = emergencyContact.City + ", " + emergencyContact.State + " " + emergencyContact.ZipCode;
            emergencyContactDetails.NotesDisplay = notesDisplay;
            emergencyContactDetails.IsActive = emergencyContact.IsActive;

            emergencyContactDetails.StatusDisplay = emergencyContact.IsActive
                ? "Active"
                : "Inactive";

            emergencyContactDetails.InactivationReasonDisplay =
                emergencyContact.InactivationReason.HasValue
                    ? GetEnumDisplayName(emergencyContact.InactivationReason.Value)
                    : "Not Applicable";

            emergencyContactDetails.InactivationDateDisplay =
                emergencyContact.InactivationDate.HasValue
                    ? emergencyContact.InactivationDate.Value.ToShortDateString()
                    : "Not Applicable";

            emergencyContactDetails.InactivationNotesDisplay =
                string.IsNullOrWhiteSpace(emergencyContact.InactivationNotes)
                        ? "No inactivation notes"
                        : emergencyContact.InactivationNotes;

            emergencyContactDetails.ReactivationDateDisplay =
                emergencyContact.ReactivationDate.HasValue
                    ? emergencyContact.ReactivationDate.Value.ToShortDateString()
                    : "Not Applicable";

            emergencyContactDetails.ReactivationNotesDisplay =
                string.IsNullOrWhiteSpace(emergencyContact.ReactivationNotes)
                        ? "No reactivation notes"
                        : emergencyContact.ReactivationNotes;

            return View(emergencyContactDetails);

        }


        // GET: EmergencyContacts/Update
        public ActionResult Update(Guid emergencyContactId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            EmergencyContactModel emergencyContact = dbContext.EmergencyContacts.FirstOrDefault(x => x.EmergencyContactId == emergencyContactId);

            if (emergencyContact == null)
            {

                return Content("Emergency Contact ID #" + emergencyContactId + " does not exist.");

            }

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == emergencyContact.CustomerId);

            if (customer == null)
            {
                return Content("Customer ID #" + emergencyContact.CustomerId + " does not exist.");
            }

            EmergencyContactFormVM emergencyContactForm = new EmergencyContactFormVM();

            emergencyContactForm.EmergencyContactId = emergencyContact.EmergencyContactId;
            emergencyContactForm.CustomerId = emergencyContact.CustomerId;
            emergencyContactForm.LastName = emergencyContact.LastName;
            emergencyContactForm.FirstName = emergencyContact.FirstName;
            emergencyContactForm.Address = emergencyContact.Address;
            emergencyContactForm.City = emergencyContact.City;
            emergencyContactForm.State = emergencyContact.State;
            emergencyContactForm.ZipCode = emergencyContact.ZipCode;
            emergencyContactForm.Phone = emergencyContact.Phone;
            emergencyContactForm.Email = emergencyContact.Email;
            emergencyContactForm.RelationshipType = emergencyContact.RelationshipType;
            emergencyContactForm.Notes = emergencyContact.Notes;

            return View(emergencyContactForm);

        }


        // POST: EmergencyContacts/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(EmergencyContactFormVM emergencyContactForm)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            if (!ModelState.IsValid)
            {
                return View(emergencyContactForm);
            }

            EmergencyContactModel emergencyContact = dbContext.EmergencyContacts.FirstOrDefault(x => x.EmergencyContactId == emergencyContactForm.EmergencyContactId);

            if (emergencyContact == null)
            {

                return Content("Emergency Contact ID #" + emergencyContactForm.EmergencyContactId + " does not exist.");

            }

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == emergencyContact.CustomerId);

            if (customer == null)
            {
                return Content("Customer ID #" + emergencyContact.CustomerId + " does not exist.");
            }

            emergencyContact.LastName = FormatName(emergencyContactForm.LastName);
            emergencyContact.FirstName = FormatName(emergencyContactForm.FirstName);
            emergencyContact.Address = emergencyContactForm.Address;
            emergencyContact.City = emergencyContactForm.City;
            emergencyContact.State = emergencyContactForm.State;
            emergencyContact.ZipCode = emergencyContactForm.ZipCode;
            emergencyContact.Phone = emergencyContactForm.Phone;
            emergencyContact.Email = emergencyContactForm.Email;
            emergencyContact.RelationshipType = emergencyContactForm.RelationshipType;
            emergencyContact.Notes = emergencyContactForm.Notes;

            dbContext.SaveChanges();
            return RedirectToAction("Read", "EmergencyContacts", new { emergencyContactId = emergencyContact.EmergencyContactId });

        }


        // GET: EmergencyContacts/Delete
        public ActionResult Delete(Guid emergencyContactId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            EmergencyContactModel emergencyContact = dbContext.EmergencyContacts.FirstOrDefault(x => x.EmergencyContactId == emergencyContactId);

            if (emergencyContact == null)
            {

                return Content("Emergency Contact ID #" + emergencyContactId + " does not exist.");

            }

            if (!emergencyContact.IsActive)
            {

                return Content("Emergency Contact ID #" + emergencyContactId + " is already inactive.");

            }

            EmergencyContactDeactivateVM emergencyContactDelete = new EmergencyContactDeactivateVM();

            emergencyContactDelete.EmergencyContactId = emergencyContact.EmergencyContactId;
            emergencyContactDelete.CustomerId = emergencyContact.CustomerId;
            emergencyContactDelete.FullNameDisplay = emergencyContact.FirstName + " " + emergencyContact.LastName;
            emergencyContactDelete.RelationshipDisplay = emergencyContact.RelationshipType.ToString();
            emergencyContactDelete.AddressDisplay = emergencyContact.Address;
            emergencyContactDelete.CityStateZipDisplay = emergencyContact.City + ", " + emergencyContact.State + " " + emergencyContact.ZipCode;
            emergencyContactDelete.PhoneDisplay = emergencyContact.Phone;
            emergencyContactDelete.EmailDisplay = emergencyContact.Email;
            emergencyContactDelete.NotesDisplay = string.IsNullOrWhiteSpace(emergencyContact.Notes)
                ? "No notes"
                : emergencyContact.Notes;
            emergencyContactDelete.IsActive = emergencyContact.IsActive;
            emergencyContactDelete.StatusDisplay = emergencyContact.IsActive
                ? "Active"
                : "Inactive";

            return View(emergencyContactDelete);

        }


        // POST: EmergencyContacts/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(EmergencyContactDeactivateVM emergencyContactDelete)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            EmergencyContactModel emergencyContact = dbContext.EmergencyContacts.FirstOrDefault(x => x.EmergencyContactId == emergencyContactDelete.EmergencyContactId);

            if (emergencyContact == null)
            {

                return Content("Emergency Contact ID #" + emergencyContactDelete.EmergencyContactId + " does not exist.");

            }

            if (!ModelState.IsValid)
            {
                emergencyContactDelete.EmergencyContactId = emergencyContact.EmergencyContactId;
                emergencyContactDelete.CustomerId = emergencyContact.CustomerId;
                emergencyContactDelete.FullNameDisplay = emergencyContact.FirstName + " " + emergencyContact.LastName;
                emergencyContactDelete.RelationshipDisplay = emergencyContact.RelationshipType.ToString();
                emergencyContactDelete.AddressDisplay = emergencyContact.Address;
                emergencyContactDelete.CityStateZipDisplay = emergencyContact.City + ", " + emergencyContact.State + " " + emergencyContact.ZipCode;
                emergencyContactDelete.PhoneDisplay = emergencyContact.Phone;
                emergencyContactDelete.EmailDisplay = emergencyContact.Email;
                emergencyContactDelete.NotesDisplay = string.IsNullOrWhiteSpace(emergencyContact.Notes)
                    ? "No notes"
                    : emergencyContact.Notes;
                emergencyContactDelete.IsActive = emergencyContact.IsActive;
                emergencyContactDelete.StatusDisplay = emergencyContact.IsActive
                    ? "Active"
                    : "Inactive";

                return View(emergencyContactDelete);
            }

            emergencyContact.IsActive = false;
            emergencyContact.InactivationReason = emergencyContactDelete.InactivationReason;
            emergencyContact.InactivationDate = DateTime.Now;
            emergencyContact.InactivationNotes = emergencyContactDelete.InactivationNotes;

            emergencyContact.ReactivationDate = null;
            emergencyContact.ReactivationNotes = null;

            dbContext.SaveChanges();

            return RedirectToAction("Read", "Customers", new { customerId = emergencyContact.CustomerId });

        }


        // GET: EmergencyContacts/Reactivate
        public ActionResult Reactivate(Guid emergencyContactId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            EmergencyContactModel emergencyContact = dbContext.EmergencyContacts.FirstOrDefault(x => x.EmergencyContactId == emergencyContactId);

            if (emergencyContact == null)
            {

                return Content("Emergency Contact ID #" + emergencyContactId + " does not exist.");

            }

            if (emergencyContact.IsActive)
            {

                return Content("Emergency Contact ID #" + emergencyContactId + " is already active.");

            }

            EmergencyContactReactivateVM emergencyContactReactivate = new EmergencyContactReactivateVM();

            emergencyContactReactivate.EmergencyContactId = emergencyContact.EmergencyContactId;
            emergencyContactReactivate.CustomerId = emergencyContact.CustomerId;
            emergencyContactReactivate.FullNameDisplay = emergencyContact.FirstName + " " + emergencyContact.LastName;
            emergencyContactReactivate.RelationshipDisplay = emergencyContact.RelationshipType.ToString();
            emergencyContactReactivate.PhoneDisplay = emergencyContact.Phone;
            emergencyContactReactivate.EmailDisplay = emergencyContact.Email;
            emergencyContactReactivate.AddressDisplay = emergencyContact.Address;
            emergencyContactReactivate.CityStateZipDisplay = emergencyContact.City + ", " + emergencyContact.State + " " + emergencyContact.ZipCode;

            emergencyContactReactivate.NotesDisplay = string.IsNullOrWhiteSpace(emergencyContact.Notes)
                ? "No notes"
                : emergencyContact.Notes;

            emergencyContactReactivate.InactivationReasonDisplay =
                emergencyContact.InactivationReason.HasValue
                    ? GetEnumDisplayName(emergencyContact.InactivationReason.Value)
                    : "Not Applicable";

            emergencyContactReactivate.InactivationDateDisplay = emergencyContact.InactivationDate.HasValue
                ? emergencyContact.InactivationDate.Value.ToShortDateString()
                : "Not Applicable";

            emergencyContactReactivate.InactivationNotesDisplay = string.IsNullOrWhiteSpace(emergencyContact.InactivationNotes)
                ? "No inactive notes"
                : emergencyContact.InactivationNotes;

            return View(emergencyContactReactivate);
        }


        // POST: EmergencyContacts/Reactivate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reactivate(EmergencyContactReactivateVM emergencyContactReactivate)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            EmergencyContactModel emergencyContact = dbContext.EmergencyContacts.FirstOrDefault(x => x.EmergencyContactId == emergencyContactReactivate.EmergencyContactId);

            if (emergencyContact == null)
            {

                return Content("Emergency Contact ID #" + emergencyContactReactivate.EmergencyContactId + " does not exist.");

            }

            if (emergencyContact.IsActive)
            {

                return Content("Emergency Contact ID #" + emergencyContactReactivate.EmergencyContactId + " is already active.");

            }

            if (!ModelState.IsValid)
            {
                emergencyContactReactivate.EmergencyContactId = emergencyContact.EmergencyContactId;
                emergencyContactReactivate.CustomerId = emergencyContact.CustomerId;
                emergencyContactReactivate.FullNameDisplay = emergencyContact.FirstName + " " + emergencyContact.LastName;
                emergencyContactReactivate.RelationshipDisplay = emergencyContact.RelationshipType.ToString();
                emergencyContactReactivate.PhoneDisplay = emergencyContact.Phone;
                emergencyContactReactivate.EmailDisplay = emergencyContact.Email;
                emergencyContactReactivate.AddressDisplay = emergencyContact.Address;
                emergencyContactReactivate.CityStateZipDisplay = emergencyContact.City + ", " + emergencyContact.State + " " + emergencyContact.ZipCode;

                emergencyContactReactivate.NotesDisplay = string.IsNullOrWhiteSpace(emergencyContact.Notes)
                    ? "No notes"
                    : emergencyContact.Notes;

                emergencyContactReactivate.InactivationReasonDisplay =
                    emergencyContact.InactivationReason.HasValue
                        ? GetEnumDisplayName(emergencyContact.InactivationReason.Value)
                        : "Not Applicable";

                emergencyContactReactivate.InactivationDateDisplay = emergencyContact.InactivationDate.HasValue
                    ? emergencyContact.InactivationDate.Value.ToShortDateString()
                    : "Not Applicable";

                emergencyContactReactivate.InactivationNotesDisplay = string.IsNullOrWhiteSpace(emergencyContact.InactivationNotes)
                    ? "No inactive notes"
                    : emergencyContact.InactivationNotes;

                return View(emergencyContactReactivate);
            }

            emergencyContact.IsActive = true;
            emergencyContact.ReactivationDate = DateTime.Now;
            emergencyContact.ReactivationNotes = emergencyContactReactivate.ReactivationNotes;
            dbContext.SaveChanges();

            return RedirectToAction("Read", "Customers", new { customerId = emergencyContact.CustomerId });
        }


        private string GetEnumDisplayName(Enum enumValue)
        {
            DisplayAttribute displayAttribute =
                enumValue
                    .GetType()
                    .GetField(enumValue.ToString())
                    .GetCustomAttribute<DisplayAttribute>();

            return displayAttribute != null
                ? displayAttribute.Name
                : enumValue.ToString();
        }

        private EmployeeModel GetCurrentEmployee(ApplicationDbContext dbContext)
        {
            string loggedInEmail = User.Identity.Name;

            return dbContext.Employees
                .FirstOrDefault(x =>
                    x.Email == loggedInEmail &&
                    x.IsActive);

        }

        private string FormatName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            string[] words = name.Trim().ToLower()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                string[] hyphenatedParts = words[i].Split('-');

                for (int j = 0; j < hyphenatedParts.Length; j++)
                {
                    if (!string.IsNullOrWhiteSpace(hyphenatedParts[j]))
                    {
                        hyphenatedParts[j] =
                            char.ToUpper(hyphenatedParts[j][0]) +
                            hyphenatedParts[j].Substring(1);
                    }
                }

                words[i] = string.Join("-", hyphenatedParts);
            }

            return string.Join(" ", words);
        }

    }
}
