using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
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

        // GET: Vaccines/Search
        public ActionResult Search()
        { 
            VaccineSearchVM vaccineSearch = new VaccineSearchVM();

            return View(vaccineSearch);    
        }


        // POST: Vaccines/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search (VaccineSearchVM vaccineSearch)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            List<VaccineModel> vaccines = dbContext.Vaccines.ToList();

            if (!string.IsNullOrWhiteSpace(vaccineSearch.VaccineName))
            {
                vaccines = vaccines
                    .Where(x => x.VaccineName.ToLower().Contains(vaccineSearch.VaccineName.ToLower()))
                    .ToList();
            }

            if (vaccineSearch.Species.HasValue)
            {
                vaccines = vaccines
                    .Where(x => x.Species == vaccineSearch.Species.Value)
                    .ToList();
            }

            if (vaccineSearch.RequiredFlag.HasValue)
            {
                vaccines = vaccines
                    .Where(x => x.RequiredFlag == vaccineSearch.RequiredFlag.Value)
                    .ToList();
            }

            vaccineSearch.VaccineSummary.Clear();

            foreach (VaccineModel vaccine in vaccines)
            {
                vaccineSearch.VaccineSummary.Add(new VaccineSummaryVM
                { 
                    VaccineId = vaccine.VaccineId,
                    VaccineName = vaccine.VaccineName,
                    SpeciesDisplay = vaccine.Species.ToString(),
                    RequiredDisplay = vaccine.RequiredFlag
                    ? "Yes" 
                    : "No"
                     
                });
            }

            return View(vaccineSearch);
        }


        // GET: Vaccines/Create
        public ActionResult Create()
        {
            VaccineFormVM vaccineForm = new VaccineFormVM();

            return View(vaccineForm);
            
        }


        // POST: Vaccines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VaccineFormVM vaccineForm)
        {

            if (!ModelState.IsValid)
            {

                return View(vaccineForm);
            }
            ApplicationDbContext dbContext = new ApplicationDbContext();

            VaccineModel existingVaccine = dbContext.Vaccines
                .FirstOrDefault(x => 
                x.VaccineName == vaccineForm.VaccineName && 
                x.Species == vaccineForm.Species.Value);

            if (existingVaccine != null)
            {
                ModelState.AddModelError("", vaccineForm.VaccineName + " for " + vaccineForm.Species + " already exists.");

                return View(vaccineForm);
            }

            VaccineModel vaccine = new VaccineModel();

            vaccine.VaccineName = vaccineForm.VaccineName;
            vaccine.Species = vaccineForm.Species.Value;
            vaccine.RequiredFlag = vaccineForm.RequiredFlag.Value;
            vaccine.Notes = vaccineForm.Notes;

            dbContext.Vaccines.Add(vaccine);
            dbContext.SaveChanges();

            return RedirectToAction("Read", new { vaccineId = vaccine.VaccineId });

        }


        // GET: Vaccines/Read
        public ActionResult Read(Guid vaccineId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            VaccineModel vaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineId == vaccineId);

            if (vaccine == null) 
            { 
                return Content("Vaccine ID #" + vaccineId + " does not exist."); 
            }

            VaccineDetailsVM vaccineDetails = new VaccineDetailsVM();

            vaccineDetails.VaccineId = vaccine.VaccineId;
            vaccineDetails.VaccineName = vaccine.VaccineName;
            vaccineDetails.SpeciesDisplay = vaccine.Species.ToString();
            vaccineDetails.RequiredDisplay = vaccine.RequiredFlag ? "Yes" : "No";
            vaccineDetails.Notes = string.IsNullOrWhiteSpace(vaccine.Notes)
                ? "No notes"
                : vaccine.Notes;


            return View(vaccineDetails);
        }


        // GET: Vaccines/Update
        public ActionResult Update(Guid vaccineId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            VaccineModel vaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineId == vaccineId);

            if (vaccine == null) 
            { 
                return Content("Vaccine ID #" + vaccineId + " does not exist."); 
            }

            VaccineFormVM vaccineForm = new VaccineFormVM();

            vaccineForm.VaccineId = vaccine.VaccineId;
            vaccineForm.VaccineName = vaccine.VaccineName;
            vaccineForm.Species = vaccine.Species;
            vaccineForm.RequiredFlag = vaccine.RequiredFlag;
            vaccineForm.Notes = vaccine.Notes;

          return View(vaccineForm); 
            
        }


        // POST: Vaccines/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(VaccineFormVM vaccineForm)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            VaccineModel vaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineId == vaccineForm.VaccineId);

            if (vaccine == null)
            { 
                return Content("Vaccine ID #" + vaccineForm.VaccineId + " does not exist."); 
            }

            if (!ModelState.IsValid) 
            { 
                return View(vaccineForm); 
            }

            VaccineModel existingVaccine = dbContext.Vaccines
                .FirstOrDefault(x =>
                x.VaccineName == vaccineForm.VaccineName &&
                x.Species == vaccineForm.Species.Value &&
                x.VaccineId != vaccineForm.VaccineId);

            if (existingVaccine != null)
            {
                ModelState.AddModelError("", "A vaccine with this name and species already exists.");

                return View(vaccineForm);
            }

            vaccine.VaccineName = vaccineForm.VaccineName;
            vaccine.Species = vaccineForm.Species.Value;
            vaccine.RequiredFlag = vaccineForm.RequiredFlag.Value;
            vaccine.Notes = vaccineForm.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { vaccineId = vaccine.VaccineId });

        }


        // GET: Vaccines/Delete
        public ActionResult Delete(Guid vaccineId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            VaccineModel vaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineId == vaccineId);

            if (vaccine == null) 
            { 
                return Content("Vaccine ID #" + vaccineId + " does not exist"); 
            }

            VaccineDeleteVM vaccineDelete = new VaccineDeleteVM();

            vaccineDelete.VaccineId = vaccine.VaccineId;
            vaccineDelete.VaccineName = vaccine.VaccineName;
            vaccineDelete.SpeciesDisplay = vaccine.Species.ToString();
            vaccineDelete.RequiredDisplay = vaccine.RequiredFlag ? "Yes" : "No";
            vaccineDelete.Notes = string.IsNullOrWhiteSpace(vaccine.Notes)
                ? "No notes"
                : vaccine.Notes;

            return View(vaccineDelete);
      
        }


        // POST: Vaccines/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(VaccineDeleteVM vaccineDelete)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            VaccineModel vaccine = dbContext.Vaccines.FirstOrDefault(x => x.VaccineId == vaccineDelete.VaccineId);

            if (vaccine == null)
            {
                return Content("Vaccine ID #" + vaccineDelete.VaccineId + " does not exist");
            }

            vaccineDelete.VaccineId = vaccine.VaccineId;
            vaccineDelete.VaccineName = vaccine.VaccineName;
            vaccineDelete.SpeciesDisplay = vaccine.Species.ToString();
            vaccineDelete.RequiredDisplay = vaccine.RequiredFlag ? "Yes" : "No";
            vaccineDelete.Notes = string.IsNullOrWhiteSpace(vaccine.Notes)
                ? "No notes"
                : vaccine.Notes;

            List<PetVaccineModel> petVaccines = dbContext.PetVaccines
                .Where(x => x.VaccineId == vaccineDelete.VaccineId)
                .ToList();

            if (petVaccines.Count > 0) 
            { 
                return Content("This vaccine is assigned to pet vaccine records and cannot be deleted."); 
            }

            dbContext.Vaccines.Remove(vaccine);
            dbContext.SaveChanges();

            return RedirectToAction("Search");

        }
    }
}
