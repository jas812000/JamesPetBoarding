using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
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
        // Medications/Create?petId=USE_EXISTING_PET_ID&medicationName=Carprofen&dosage=25mg&route=Oral&frequency=OnceDaily&startDate=2026-06-10&endDate=&notes=give%20with%20food
        public ActionResult Create(
            Guid petId, 
            string medicationName, 
            string dosage, 
            MedicationRouteEnum route, 
            FrequencyEnum frequency,
            DateTime startDate,
            DateTime? endDate,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (string.IsNullOrWhiteSpace(medicationName)) { return Content("Name of the medication is required."); }
            if (string.IsNullOrWhiteSpace(dosage)) { return Content("Medication dosage is required."); }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null) { return Content("Pet ID #" + petId + " does not exist."); }

            MedicationModel medication = new MedicationModel();

            medication.PetId = petId;
            medication.MedicationName = medicationName;
            medication.Dosage = dosage;
            medication.Route = route;
            medication.Frequency = frequency;
            medication.StartDate = startDate;
            medication.EndDate = endDate;
            medication.Notes = notes;

            try
            {

                dbContext.Medications.Add(medication);
                dbContext.SaveChanges();

                return Content("Successfully added " + medication.MedicationName + " to the database.");
            } 
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }
        }


        // GET: Medications/Read
        // Medications/Read?medicationId=USE_EXISTING_MEDICATION_ID
        public ActionResult Read(Guid medicationId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            MedicationModel medication = dbContext.Medications.FirstOrDefault(x => x.MedicationId == medicationId);

            if (medication == null) { return Content("Medication ID #" + medicationId + " does not exist."); }

            string endDateDisplay = medication.EndDate == null
                ? "No end date"
                : medication.EndDate.Value.ToString("MM/dd/yyyy");

            string notesDisplay = string.IsNullOrWhiteSpace(medication.Notes)
                ? "No notes"
                : medication.Notes;

            string routeDisplay = medication.Route.ToString();

            switch (medication.Route)
            {
                case MedicationRouteEnum.Otic:
                    routeDisplay = "Otic (Ear)";
                    break;

                case MedicationRouteEnum.Ophthalmic:
                    routeDisplay = "Ophthalmic (Eye)";
                    break;
            }

            string frequencyDisplay = medication.Frequency.ToString();

            switch (medication.Frequency)
            {
                case FrequencyEnum.OnceDaily:
                    frequencyDisplay = "Once Daily";
                    break;

                case FrequencyEnum.TwiceDaily:
                    frequencyDisplay = "Twice Daily";
                    break;

                case FrequencyEnum.ThreeTimesDaily:
                    frequencyDisplay = "Three Times Daily";
                    break;

                case FrequencyEnum.FourTimesDaily:
                    frequencyDisplay = "Four Times Daily";
                    break;

                case FrequencyEnum.EveryOtherDay:
                    frequencyDisplay = "Every Other Day";
                    break;

                case FrequencyEnum.OnceWeekly:
                    frequencyDisplay = "Once Weekly";
                    break;

                case FrequencyEnum.OnceMonthly:
                    frequencyDisplay = "Once Monthly";
                    break;

                case FrequencyEnum.AsNeeded:
                    frequencyDisplay = "As Needed";
                    break;
            }

            return Content(
                "Medication ID #" + medication.MedicationId +
                "<br />Pet ID #" + medication.PetId +
                "<br />Medication name: " + medication.MedicationName +
                "<br />Dosage: " + medication.Dosage +
                "<br />Route: " + routeDisplay +
                "<br />Frequency: " + frequencyDisplay +
                "<br />Start Date: " + medication.StartDate.ToString("MM/dd/yyyy") +
                "<br />End Date: " + endDateDisplay +
                "<br />Notes: " + notesDisplay
            );
        }


        // GET: Medications/Update
        // Medications/Update?medicationId=USE_EXISTING_MEDICATION_ID&petId=USE_EXISTING_PET_ID&medicationName=Carprofen&dosage=25mg&route=Oral&frequency=TwiceDaily&startDate=2026-06-08&endDate=2026-06-20&notes=give%20with%20food
        public ActionResult Update(
            Guid medicationId,
            Guid petId,
            string medicationName,
            string dosage,
            MedicationRouteEnum route,
            FrequencyEnum frequency,
            DateTime startDate,
            DateTime? endDate,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            MedicationModel medication = dbContext.Medications.FirstOrDefault(x => x.MedicationId == medicationId);

            if (medication == null) { return Content("Medication ID #" + medicationId + " does not exist."); }

            if (string.IsNullOrWhiteSpace(medicationName)) { return Content("Name of the medication is required."); }
            if (string.IsNullOrWhiteSpace(dosage)) { return Content("Medication dosage is required."); }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);
            if (pet == null) { return Content("Pet ID #" + petId + " does not exist."); }

            medication.PetId = petId;
            medication.MedicationName = medicationName;
            medication.Dosage = dosage;
            medication.Route = route;
            medication.Frequency = frequency;
            medication.StartDate = startDate;
            medication.EndDate = endDate;
            medication.Notes = notes;

            try 
            {
                dbContext.SaveChanges();
                return Content("Medication ID #" + medication.MedicationId + " successfully updated.");
            } 
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }
        }



        // GET: Medications/Delete
        // Medications/Delete?medicationId=USE_EXISTING_MEDICATION_ID
        public ActionResult Delete(Guid medicationId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            MedicationModel medication = dbContext.Medications.FirstOrDefault(x => x.MedicationId == medicationId);

            if (medication == null) { return Content("Medication ID #" + medicationId + " does not exist."); }

            try 
            {
                dbContext.Medications.Remove(medication);
                dbContext.SaveChanges();
                return Content("Medication ID #" + medication.MedicationId + " has been successfully deleted.");
            } 
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }
        }
    }
}
