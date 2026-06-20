using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace JamesPetBoarding.Controllers
{
    public class PetVaccinesController : Controller
    {
        // GET: PetVaccines
        public ActionResult Index()
        {
            return View();
        }


        // GET: PetVaccines/Create
        // /PetVaccines/Create?petId=USE_EXISTING_PET_ID&vaccineId=USE_EXISTING_VACCINE_ID&dateGiven=2025-06-01&expirationDate=2026-06-01&documentFilePath=/documents/vaccines/rabies.pdf&notes=will%20get%20updated%20vaccine
        public ActionResult Create(
            Guid petId,
            Guid vaccineId,
            DateTime dateGiven,
            DateTime expirationDate,
            string documentFilePath,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);
            if (pet == null) { return Content("Pet ID #" + petId + " does not exist."); }

            VaccineModel vaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineId == vaccineId);
            if (vaccine == null) { return Content("Vaccine ID #" + vaccineId + " does not exist."); }

            if (expirationDate < dateGiven) { return Content("Expiration date cannot be before date given."); }
            
            if (string.IsNullOrWhiteSpace(documentFilePath)) { return Content("A file path for documents is required."); }

            PetVaccineModel petVaccine = new PetVaccineModel();

            petVaccine.PetId = petId;
            petVaccine.VaccineId = vaccineId;
            petVaccine.DateGiven = dateGiven;
            petVaccine.ExpirationDate = expirationDate;
            petVaccine.DocumentFilePath = documentFilePath;
            petVaccine.Notes = notes;


            try
            {
                dbContext.PetVaccines.Add( petVaccine );
                dbContext.SaveChanges();

                return Content(pet.Name + " and " + vaccine.VaccineName + " relationship was successfully created.");
            }
            catch (Exception ex) 
            {
                return Content(ex.Message);
            }
        }


        // GET: PetVaccines/Read
        // /PetVaccines/Read?petVaccineId=USE_EXISTING_PETVACCINE_ID
        public ActionResult Read(Guid petVaccineId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetVaccineModel petVaccine = dbContext.PetVaccines.FirstOrDefault(x => x.PetVaccineId == petVaccineId);
            if (petVaccine == null) { return Content("PetVaccine ID #" + petVaccineId + " does not exist."); }

            string notesDisplay = string.IsNullOrWhiteSpace(petVaccine.Notes)
                ? "No notes"
                : petVaccine.Notes;

            return Content(
                "PetVaccine ID #" + petVaccine.PetVaccineId +
                "<br />Pet ID #" + petVaccine.PetId +
                "<br />Vaccine ID #" + petVaccine.VaccineId +
                "<br />Date Administered: " + petVaccine.DateGiven.ToString("MM/dd/yyyy") +
                "<br />Expiration Date: " + petVaccine.ExpirationDate.ToString("MM/dd/yyyy") +
                "<br />Document File Path: " + petVaccine.DocumentFilePath +
                "<br />Notes: " + notesDisplay
            );
        }


        // GET: PetVaccines/Update
        // /PetVaccines/Update?petVaccineId=USE_EXISTING_PETVACCINE_ID&petId=USE_EXISTING_PET_ID&vaccineId=USE_EXISTING_VACCINE_ID&dateGiven=2026-05-29&expirationDate=2027-05-29&documentFilePath=/documents/vaccines/rabies.pdf&notes=
        public ActionResult Update(
            Guid petVaccineId,
            Guid petId,
            Guid vaccineId,
            DateTime dateGiven,
            DateTime expirationDate,
            string documentFilePath,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetVaccineModel petVaccine = dbContext.PetVaccines.FirstOrDefault(x => x.PetVaccineId == petVaccineId);
            if (petVaccine == null) { return Content("PetVaccine ID #" + petVaccineId + " does not exist."); }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);
            if (pet == null) { return Content("Pet ID #" + petId + " does not exist."); }

            VaccineModel vaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineId == vaccineId);
            if (vaccine == null) { return Content("Vaccine ID #" + vaccineId + " does not exist."); }

            if (expirationDate < dateGiven) { return Content("Expiration date cannot be before date given."); }

            if (string.IsNullOrWhiteSpace(documentFilePath)) { return Content("A file path for documents is required."); }

            petVaccine.PetId = petId;
            petVaccine.VaccineId = vaccineId;
            petVaccine.DateGiven = dateGiven;
            petVaccine.ExpirationDate = expirationDate;
            petVaccine.DocumentFilePath = documentFilePath;
            petVaccine.Notes = notes;

            try
            {
                dbContext.SaveChanges();

                return Content("PetVaccine ID #" + petVaccine.PetVaccineId + " was successfully updated.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        // GET: PetVaccines/Delete
        // /PetVaccines/Delete?petVaccineId=USE_EXISTING_PETVACCINE_ID
        public ActionResult Delete(Guid petVaccineId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetVaccineModel petVaccine = dbContext.PetVaccines.FirstOrDefault(x => x.PetVaccineId == petVaccineId);
            if (petVaccine == null) { return Content("PetVaccine ID #" + petVaccineId + " does not exist."); }

            try
            {
                dbContext.PetVaccines.Remove(petVaccine);
                dbContext.SaveChanges();
                return Content("PetVaccine ID #" + petVaccine.PetVaccineId + " was successfully deleted.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
