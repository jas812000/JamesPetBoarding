using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace JamesPetBoarding.Controllers
{
    [Authorize]
    public class PetVaccinesController : Controller
    {

        // GET: PetVaccines/Create
        public ActionResult Create(Guid petId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "User");
            }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);
            if (pet == null)
            {
                return Content("Pet ID #" + petId + " does not exist.");
            }

            PetVaccineFormVM petVaccineForm = new PetVaccineFormVM();

            petVaccineForm.PetId = petId;
            petVaccineForm.PetNameDisplay = pet.PetName;

            petVaccineForm.VaccineOptions = BuildVaccineSelectList(petId);

            return View(petVaccineForm);
        }


        // POST: PetVaccines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PetVaccineFormVM petVaccineForm)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "User");
            }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petVaccineForm.PetId);

            if (pet == null)
            {
                return Content("Pet ID #" + petVaccineForm.PetId + " does not exist.");
            }

            if (petVaccineForm.VaccineId == Guid.Empty)
            {
                ModelState.AddModelError("VaccineId", "Please select a vaccine.");
            }

            VaccineModel vaccine = null;

            if (petVaccineForm.VaccineId != Guid.Empty)
            {
                vaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineId == petVaccineForm.VaccineId);

                if (vaccine == null)
                {
                    ModelState.AddModelError("VaccineId", "The selected vaccine does not exist.");
                }
                else if (pet.Species != vaccine.Species)
                {
                    ModelState.AddModelError("VaccineId", "The selected vaccine does not match the pet's species.");
                }
            }

            if (petVaccineForm.ExpirationDate < petVaccineForm.DateGiven)
            {
                ModelState.AddModelError("ExpirationDate", "Expiration date cannot be before date given.");
            }

            if (!ModelState.IsValid)
            {
                petVaccineForm.PetNameDisplay = pet.PetName;
                petVaccineForm.VaccineOptions = BuildVaccineSelectList(petVaccineForm.PetId);

                return View(petVaccineForm);
            }

            PetVaccineModel petVaccine = new PetVaccineModel();

            petVaccine.PetId = petVaccineForm.PetId;
            petVaccine.VaccineId = petVaccineForm.VaccineId;
            petVaccine.DateGiven = petVaccineForm.DateGiven;
            petVaccine.ExpirationDate = petVaccineForm.ExpirationDate;
            petVaccine.DocumentFilePath = petVaccineForm.DocumentFilePath;
            petVaccine.Notes = petVaccineForm.Notes;

            dbContext.PetVaccines.Add(petVaccine);
            dbContext.SaveChanges();

            return RedirectToAction("Read", "Pets", new { petId = petVaccine.PetId });

        }


        // GET: PetVaccines/Read
        public ActionResult Read(Guid petVaccineId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "User");
            }

            PetVaccineModel petVaccine = dbContext.PetVaccines
                .Include(x => x.Pet)
                .Include(x => x.Vaccine)
                .FirstOrDefault(x => x.PetVaccineId == petVaccineId);

            if (petVaccine == null)
            {
                return Content("PetVaccine ID #" + petVaccineId + " does not exist.");
            }

            PetVaccineDetailsVM petVaccineDetails = new PetVaccineDetailsVM();

            petVaccineDetails.PetVaccineId = petVaccine.PetVaccineId;
            petVaccineDetails.PetId = petVaccine.PetId;
            petVaccineDetails.PetNameDisplay = petVaccine.Pet.PetName;
            petVaccineDetails.VaccineNameDisplay = petVaccine.Vaccine.VaccineName;
            petVaccineDetails.DateGivenDisplay = petVaccine.DateGiven.ToString("MM/dd/yyyy");
            petVaccineDetails.ExpirationDateDisplay = petVaccine.ExpirationDate.ToString("MM/dd/yyyy");
            petVaccineDetails.DocumentFilePath = petVaccine.DocumentFilePath;
            petVaccineDetails.Notes = string.IsNullOrWhiteSpace(petVaccine.Notes)
                ? "No notes"
                : petVaccine.Notes;

            return View(petVaccineDetails);
        }


        // GET: PetVaccines/Update
        public ActionResult Update(Guid petVaccineId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "User");
            }

            PetVaccineModel petVaccine = dbContext.PetVaccines
                .Include(x => x.Pet)
                .Include(x => x.Vaccine)
                .FirstOrDefault(x => x.PetVaccineId == petVaccineId);

            if (petVaccine == null)
            {
                return Content("PetVaccine ID #" + petVaccineId + " does not exist.");
            }

            PetVaccineFormVM petVaccineForm = new PetVaccineFormVM();

            petVaccineForm.PetVaccineId = petVaccine.PetVaccineId;
            petVaccineForm.PetId = petVaccine.PetId;
            petVaccineForm.PetNameDisplay = petVaccine.Pet.PetName;
            petVaccineForm.VaccineId = petVaccine.VaccineId;
            petVaccineForm.VaccineOptions = BuildVaccineSelectList(petVaccine.PetId);
            petVaccineForm.DateGiven = petVaccine.DateGiven;
            petVaccineForm.ExpirationDate = petVaccine.ExpirationDate;
            petVaccineForm.DocumentFilePath = petVaccine.DocumentFilePath;
            petVaccineForm.Notes = petVaccine.Notes;

            return View(petVaccineForm);
        }


        // POST: PetVaccines/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(PetVaccineFormVM petVaccineForm)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "User");
            }

            PetVaccineModel petVaccine = dbContext.PetVaccines
                .Include(x => x.Pet)
                .FirstOrDefault(x => x.PetVaccineId == petVaccineForm.PetVaccineId);

            if (petVaccine == null)
            {
                return Content("PetVaccine ID #" + petVaccineForm.PetVaccineId + " does not exist.");

            }

            PetModel pet = petVaccine.Pet;

            if (pet == null)
            {
                return Content("Pet ID #" + petVaccineForm.PetId + " does not exist.");
            }

            if (petVaccineForm.VaccineId == Guid.Empty)
            {
                ModelState.AddModelError("VaccineId", "Please select a vaccine.");
            }

            VaccineModel vaccine = null;

            if (petVaccineForm.VaccineId != Guid.Empty)
            {
                vaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineId == petVaccineForm.VaccineId);

                if (vaccine == null)
                {
                    ModelState.AddModelError("VaccineId", "The selected vaccine does not exist.");
                }
                else if (pet.Species != vaccine.Species)
                {
                    ModelState.AddModelError("VaccineId", "The selected vaccine does not match the pet's species.");
                }
            }

            if (petVaccineForm.ExpirationDate < petVaccineForm.DateGiven)
            {
                ModelState.AddModelError("ExpirationDate", "Expiration date cannot be before date given.");
            }

            if (!ModelState.IsValid)
            {
                petVaccineForm.PetId = petVaccine.PetId;
                petVaccineForm.PetNameDisplay = pet.PetName;
                petVaccineForm.VaccineOptions = BuildVaccineSelectList(petVaccineForm.PetId);

                return View(petVaccineForm);
            }

            petVaccine.VaccineId = petVaccineForm.VaccineId;
            petVaccine.DateGiven = petVaccineForm.DateGiven;
            petVaccine.ExpirationDate = petVaccineForm.ExpirationDate;
            petVaccine.DocumentFilePath = petVaccineForm.DocumentFilePath;
            petVaccine.Notes = petVaccineForm.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", "Pets", new { petId = petVaccine.PetId });

        }


        // GET: PetVaccines/Delete
        public ActionResult Delete(Guid petVaccineId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "User");
            }

            PetVaccineModel petVaccine = dbContext.PetVaccines
                .Include(x => x.Pet)
                .Include(x => x.Vaccine)
                .FirstOrDefault(x => x.PetVaccineId == petVaccineId);

            if (petVaccine == null)
            {
                return Content("PetVaccine ID #" + petVaccineId + " does not exist.");
            }

            PetVaccineDeleteVM petVaccineDelete = new PetVaccineDeleteVM();

            petVaccineDelete.PetVaccineId = petVaccine.PetVaccineId;
            petVaccineDelete.VaccineNameDisplay = petVaccine.Vaccine.VaccineName;
            petVaccineDelete.PetId = petVaccine.PetId;
            petVaccineDelete.PetNameDisplay = petVaccine.Pet.PetName;
            petVaccineDelete.DateGivenDisplay = petVaccine.DateGiven.ToString("MM/dd/yyyy");
            petVaccineDelete.ExpirationDateDisplay = petVaccine.ExpirationDate.ToString("MM/dd/yyyy");
            petVaccineDelete.DocumentFilePath = petVaccine.DocumentFilePath;
            petVaccineDelete.Notes = string.IsNullOrWhiteSpace(petVaccine.Notes)
                ? "No notes"
                : petVaccine.Notes;

            return View(petVaccineDelete);

        }


        // POST: PetVaccines/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PetVaccineDeleteVM petVaccineDelete)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "User");
            }

            PetVaccineModel petVaccine = dbContext.PetVaccines
                .Include(x => x.Pet)
                .Include(x => x.Vaccine)
                .FirstOrDefault(x => x.PetVaccineId == petVaccineDelete.PetVaccineId);

            if (petVaccine == null)
            {
                return Content("PetVaccine ID #" + petVaccineDelete.PetVaccineId + " does not exist.");
            }

            Guid petId = petVaccine.PetId;

            dbContext.PetVaccines.Remove(petVaccine);
            dbContext.SaveChanges();

            return RedirectToAction("Read", "Pets", new { petId = petId });

        }


        private List<SelectListItem> BuildVaccineSelectList(Guid petId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null) 
            { 
                return new List<SelectListItem>(); 
            }

            List<SelectListItem> vaccineOptions = dbContext.Vaccines
                .Where(x => x.Species == pet.Species)
                .OrderBy(x => x.VaccineName)
                .Select(x => new SelectListItem
                {
                    Value = x.VaccineId.ToString(),
                    Text = x.VaccineName + " - " + x.Species 
                })
                .ToList();

            return vaccineOptions;

        }


        private EmployeeModel GetCurrentEmployee(ApplicationDbContext dbContext)
        {
            string loggedInEmail = User.Identity.Name;

            return dbContext.Employees
                .FirstOrDefault(x =>
                    x.Email == loggedInEmail &&
                    x.IsActive);

        }
    }
}
