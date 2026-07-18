using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JamesPetBoarding.Controllers
{
    public class MedicationsController : Controller
    {
        // GET: Medications
        public ActionResult Index()
        {
            return View();
        }

        // GET: Medications/Create
        public ActionResult Create(Guid petId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null) 
            { 
                return Content("Pet ID #" + petId + " does not exist."); 
            }

            MedicationFormVM medicationForm = new MedicationFormVM();

            medicationForm.PetId = petId;
            medicationForm.PetNameDisplay = pet.PetName;

            return View(medicationForm); 
        }


        //POST: Medications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MedicationFormVM medicationForm)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == medicationForm.PetId);

            if (pet == null)
            {
                return Content("Pet ID #" + medicationForm.PetId + " does not exist.");
            }

            if (medicationForm.EndDate.HasValue && medicationForm.EndDate.Value < medicationForm.StartDate)
            {
                ModelState.AddModelError("EndDate", "End date cannot be before start date.");
            }

            if (!ModelState.IsValid)
            {
                medicationForm.PetNameDisplay = pet.PetName;
                return View(medicationForm);
            }

            MedicationModel medication = new MedicationModel();

            medication.PetId = medicationForm.PetId;
            medication.MedicationName = medicationForm.MedicationName;
            medication.Dosage = medicationForm.Dosage;
            medication.Route = medicationForm.Route;
            medication.Frequency = medicationForm.Frequency;
            medication.StartDate = medicationForm.StartDate;
            medication.EndDate = medicationForm.EndDate;
            medication.Notes = medicationForm.Notes;

            dbContext.Medications.Add(medication);
            dbContext.SaveChanges();

            return RedirectToAction("Read", "Medications", new { medicationId = medication.MedicationId });
        }

        // GET: Medications/Read
        public ActionResult Read(Guid medicationId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            MedicationModel medication = dbContext.Medications
                .Include(x => x.Pet)
                .FirstOrDefault(x => x.MedicationId == medicationId);

            if (medication == null) 
            { 
                return Content("Medication ID #" + medicationId + " does not exist."); 
            }

            MedicationDetailsVM medicationDetails = new MedicationDetailsVM();

            medicationDetails.MedicationId = medication.MedicationId;
            medicationDetails.PetId = medication.PetId;
            medicationDetails.PetNameDisplay = medication.Pet.PetName;
            medicationDetails.MedicationName = medication.MedicationName;
            medicationDetails.Dosage = medication.Dosage;
            medicationDetails.RouteDisplay = medication.Route.ToString();
            medicationDetails.FrequencyDisplay = medication.Frequency.ToString();
            medicationDetails.StartDateDisplay = medication.StartDate.ToString("MM/dd/yyyy");
            medicationDetails.EndDateDisplay = medication.EndDate.HasValue
                ? medication.EndDate.Value.ToString("MM/dd/yyyy")
                : "No end date";

            string notesDisplay = string.IsNullOrWhiteSpace(medication.Notes)
                ? "No notes"
                : medication.Notes;

            medicationDetails.NotesDisplay = notesDisplay;

            return View(medicationDetails);

        }


        // GET: Medications/Update
        public ActionResult Update(Guid medicationId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            MedicationModel medication = dbContext.Medications
                .Include(x => x.Pet)
                .FirstOrDefault(x => x.MedicationId == medicationId);

            if (medication == null) 
            { 
                return Content("Medication ID #" + medicationId + " does not exist."); 
            }

            MedicationFormVM medicationForm = new MedicationFormVM();

            medicationForm.MedicationId = medication.MedicationId;
            medicationForm.PetId = medication.PetId;
            medicationForm.PetNameDisplay = medication.Pet.PetName;
            medicationForm.MedicationName = medication.MedicationName;
            medicationForm.Dosage = medication.Dosage;
            medicationForm.Route = medication.Route;
            medicationForm.Frequency = medication.Frequency;
            medicationForm.StartDate = medication.StartDate;
            medicationForm.EndDate = medication.EndDate;
            medicationForm.Notes = medication.Notes;

            return View(medicationForm);
        }


        //POST: Medications/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(MedicationFormVM medicationForm)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            MedicationModel medication = dbContext.Medications
                .Include(x => x.Pet)
                .FirstOrDefault(x => x.MedicationId == medicationForm.MedicationId);

            if (medication == null)
            {
                return Content("Medication ID #" + medicationForm.MedicationId + " does not exist.");
            }

            if (medicationForm.EndDate.HasValue && medicationForm.EndDate.Value < medicationForm.StartDate)
            {
                ModelState.AddModelError("EndDate", "End date cannot be before start date.");
            }

            if (!ModelState.IsValid)
            {
                medicationForm.PetNameDisplay = medication.Pet.PetName;
                return View(medicationForm);
            }

            medication.MedicationName = medicationForm.MedicationName;
            medication.Dosage = medicationForm.Dosage;
            medication.Route = medicationForm.Route;
            medication.Frequency = medicationForm.Frequency;
            medication.StartDate = medicationForm.StartDate;
            medication.EndDate = medicationForm.EndDate;
            medication.Notes = medicationForm.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", "Medications", new { medicationId = medication.MedicationId });
        }


        // GET: Medications/Delete
        public ActionResult Delete(Guid medicationId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            MedicationModel medication = dbContext.Medications
                .Include(x => x.Pet)
                .FirstOrDefault(x => x.MedicationId == medicationId);

            if (medication == null) 
            { 
                return Content("Medication ID #" + medicationId + " does not exist."); 
            }

            MedicationDeleteVM medicationDelete = new MedicationDeleteVM();

            medicationDelete.MedicationId = medication.MedicationId;
            medicationDelete.PetId = medication.PetId;
            medicationDelete.PetNameDisplay = medication.Pet.PetName;
            medicationDelete.MedicationName = medication.MedicationName;
            medicationDelete.Dosage = medication.Dosage;
            medicationDelete.RouteDisplay = medication.Route.ToString();
            medicationDelete.FrequencyDisplay = medication.Frequency.ToString();
            medicationDelete.StartDateDisplay = medication.StartDate.ToString("MM/dd/yyyy");
            medicationDelete.EndDateDisplay = medication.EndDate.HasValue
                ? medication.EndDate.Value.ToString("MM/dd/yyyy")
                : "No end date";

            string notesDisplay = string.IsNullOrWhiteSpace(medication.Notes)
                ? "No notes"
                : medication.Notes;

            medicationDelete.NotesDisplay = notesDisplay;

            return View(medicationDelete);

        }


        // POST: Medications/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(MedicationDeleteVM medicationDelete)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            MedicationModel medication = dbContext.Medications
                .FirstOrDefault(x => x.MedicationId == medicationDelete.MedicationId);

            if (medication == null)
            {
                return Content("Medication ID #" + medicationDelete.MedicationId + " does not exist.");
            }
            Guid petId = medication.PetId;

            dbContext.Medications.Remove(medication);
            dbContext.SaveChanges();

            return RedirectToAction("Read", "Pets", new { petId });

        }
    }
}
