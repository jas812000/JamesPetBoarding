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
    public class VaccinesController : Controller
    {
        // GET: Vaccines
        public ActionResult Index()
        {
            return View();
        }



        // GET: Vaccines/Create
        // /Vaccines/Create?vaccineName=Rabies&species=Dog&requiredFlag=false&notes=
        public ActionResult Create(
            string vaccineName,
            SpeciesEnum species,
            bool requiredFlag,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (string.IsNullOrWhiteSpace(vaccineName)) { return Content("Name of the vaccine is required."); }

            VaccineModel existingVaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineName == vaccineName && x.Species == species);
            if (existingVaccine != null) 
            { 
                return Content(vaccineName + " for " + species + " already exists.");
            }
        

            VaccineModel vaccine = new VaccineModel();

            vaccine.VaccineName = vaccineName;
            vaccine.Species = species;
            vaccine.RequiredFlag = requiredFlag;
            vaccine.Notes = notes;

            try
            {
                dbContext.Vaccines.Add( vaccine );
                dbContext.SaveChanges();
                return Content(vaccine.VaccineName + " successfully added to the database.");
            }
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }
        }


        // GET: Vaccines/Read
        // /Vaccines/Read?vaccineId=USE_EXISTING_VACCINE_ID
        public ActionResult Read(Guid vaccineId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            VaccineModel vaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineId == vaccineId);

            if (vaccine == null) { return Content("Vaccine ID #" + vaccineId + " does not exist."); }

            string vaccineRequired = vaccine.RequiredFlag
                ? "Yes" 
                : "No";

            string notesDisplay = string.IsNullOrWhiteSpace(vaccine.Notes)
                ? "No notes"
                : vaccine.Notes;

            string speciesDisplay = vaccine.Species.ToString();

            return Content(
                "Vaccine ID #" + vaccine.VaccineId +
                "<br />Vaccine Name: " + vaccine.VaccineName +
                "<br />Species: " + speciesDisplay +
                "<br />Vaccine Required: " + vaccineRequired +
                "<br />Notes: " + notesDisplay
            );
        }


        // GET: Vaccines/Update
        // /Vaccines/Update?vaccineId=USE_EXISTING_VACCINE_ID&vaccineName=Rabies&species=Dog&requiredFlag=true&notes=annual%20by%20law
        public ActionResult Update(
            Guid vaccineId,
            string vaccineName,
            SpeciesEnum species,
            bool requiredFlag,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            VaccineModel vaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineId == vaccineId);

            if (vaccine == null) { return Content("Vaccine ID #" + vaccineId + " does not exist."); }

            if (string.IsNullOrWhiteSpace(vaccineName)) { return Content("Name of the vaccine is required."); }

            VaccineModel existingVaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineId != vaccineId && x.VaccineName == vaccineName && x.Species == species);
            if (existingVaccine != null)
            {
                return Content(vaccineName + " for a " + species + " already exists.");
            }

            vaccine.VaccineName = vaccineName;
            vaccine.Species = species;
            vaccine.RequiredFlag = requiredFlag;
            vaccine.Notes = notes;

            try 
            {

                dbContext.SaveChanges();

                return Content("Vaccine ID #" + vaccine.VaccineId + " successfully updated.");

            } 
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }

        }

        // GET: Vaccines/Delete
        // /Vaccines/Delete?vaccineId=USE_EXISTING_VACCINE_ID
        public ActionResult Delete(Guid vaccineId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            VaccineModel vaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineId == vaccineId);

            if (vaccine == null) { return Content("Vaccine ID #" + vaccineId + " does not exist"); }

            List<PetVaccineModel> petVaccines = dbContext.PetVaccines.Where(x => x.VaccineId == vaccineId).ToList();
            
            if (petVaccines.Count > 0) 
            { 
                return Content("This vaccine is assigned to pet vaccine records and cannot be deleted."); 
            }

            try 
            {
                dbContext.Vaccines.Remove(vaccine);
                dbContext.SaveChanges();

                return Content("Vaccine ID #" + vaccine.VaccineId + " successfully deleted.");
            }
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }
        }
    }
}
