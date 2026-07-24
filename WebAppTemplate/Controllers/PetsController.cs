using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

namespace JamesPetBoarding.Controllers
{
    public class PetsController : Controller
    {
        // GET: Pets
        public ActionResult Index()
        {
            return View();
        }

        // GET: Pets/Search
        public ActionResult Search()
        {
            PetSearchVM petSearch = new PetSearchVM();

            return View(petSearch);
        }


        // POST: Pets/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(PetSearchVM petSearch)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            List<PetModel> pets = dbContext.Pets.ToList();

            if (!string.IsNullOrWhiteSpace(petSearch.PetName))
            {
                pets = pets
                    .Where(x => x.PetName.ToLower().Contains(petSearch.PetName.ToLower()))
                    .ToList();
            }

            if (petSearch.Species.HasValue) {
                pets = pets
                    .Where(x => x.Species == petSearch.Species.Value)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(petSearch.Breed))
            {
                pets = pets
                    .Where(x => x.Breed != null && x.Breed.ToLower().Contains(petSearch.Breed.ToLower()))
                    .ToList();
            }

            if (petSearch.Sex.HasValue)
            {
                pets = pets
                    .Where(x => x.Sex == petSearch.Sex.Value)
                    .ToList();
            }

            if (petSearch.IsActive.HasValue)
            {
                pets = pets
                    .Where(x => x.IsActive == petSearch.IsActive.Value)
                    .ToList();
            }

            petSearch.Pets.Clear();

            foreach (PetModel pet in pets)
            {
                petSearch.Pets.Add(new PetSummaryVM
                {
                    PetId = pet.PetId,
                    PetNameDisplay = pet.PetName,
                    SpeciesDisplay = pet.Species.ToString(),
                    BreedDisplay = pet.Breed,
                    SexDisplay = pet.Sex.ToString(),
                    ActiveStatusDisplay = pet.IsActive ? "Active" : "Inactive",
                    IsActive = pet.IsActive
                });
            }

            return View(petSearch);
        }


        // GET: Pets/Create
        public ActionResult Create()
        {
            PetFormVM petForm = new PetFormVM();

            petForm.VeterinarianSelectList = BuildVeterinarianSelectList();

            return View(petForm);
        }


        // POST: Pets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PetFormVM petForm)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (!ModelState.IsValid)
            {
                petForm.VeterinarianSelectList = BuildVeterinarianSelectList();
                return View(petForm);
            }

            if (petForm.BirthDate > DateTime.Today)
            {
                ModelState.AddModelError("BirthDate", "Birth date cannot be in the future.");
                petForm.VeterinarianSelectList = BuildVeterinarianSelectList();
                return View(petForm);
            }

            if (petForm.VetId.HasValue)
            {
                bool veterinarianExists = dbContext.Veterinarians.Any(x => x.VetId == petForm.VetId.Value);

                if (!veterinarianExists)
                {
                    ModelState.AddModelError("VetId", "Selected veterinarian does not exist.");
                    petForm.VeterinarianSelectList = BuildVeterinarianSelectList();
                    return View(petForm);
                }
            }

            PetModel pet = new PetModel();

            pet.VetId = petForm.VetId;
            pet.PetName = petForm.PetName;
            pet.Species = petForm.Species;
            pet.Breed = petForm.Breed;
            pet.Sex = petForm.Sex;
            pet.BirthDate = petForm.BirthDate;
            pet.Weight = petForm.Weight;
            pet.Notes = petForm.Notes;

            dbContext.Pets.Add(pet);
            dbContext.SaveChanges();

            return RedirectToAction("Read", new { petId = pet.PetId });

        }


        // GET: Pets/Read
        public ActionResult Read(Guid petId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null)
            {
                return Content("Pet Id #" + petId + " does not exist.");
            }

            int age = DateTime.Today.Year - pet.BirthDate.Year;
            if (pet.BirthDate.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            PetDetailsVM petDetails = new PetDetailsVM();

            petDetails.PetId = pet.PetId;
            petDetails.PetNameDisplay = pet.PetName;
            petDetails.SpeciesDisplay = pet.Species.ToString();
            petDetails.BreedDisplay = pet.Breed;
            petDetails.SexDisplay = pet.Sex.ToString();
            petDetails.BirthDateDisplay = pet.BirthDate.ToString("MM/dd/yyyy");
            petDetails.AgeDisplay = age.ToString();
            petDetails.WeightDisplay = pet.Weight.ToString();
            petDetails.IsActive = pet.IsActive;

            petDetails.ActiveStatusDisplay = pet.IsActive ? "Active" : "Inactive";

            petDetails.NotesDisplay = string.IsNullOrWhiteSpace(pet.Notes)
                ? "No notes"
                : pet.Notes;

            petDetails.InactivationReasonDisplay = pet.InactivationReason.HasValue
                ? pet.InactivationReason.ToString()
                : "Not Applicable";

            petDetails.InactivationDateDisplay = pet.InactivationDate.HasValue
                ? pet.InactivationDate.Value.ToString("MM/dd/yyyy")
                : "Not Applicable";

            petDetails.InactiveNotesDisplay = string.IsNullOrWhiteSpace(pet.InactivationNotes)
                ? "No Inactivation notes"
                : pet.InactivationNotes;

            petDetails.ReactivationDateDisplay = pet.ReactivationDate.HasValue
                ? pet.ReactivationDate.Value.ToString("MM/dd/yyyy")
                : "Not Applicable";

            petDetails.ReactivationNotesDisplay = string.IsNullOrWhiteSpace(pet.ReactivationNotes)
                ? "No reactivation notes"
                : pet.ReactivationNotes;

            if (pet.VetId.HasValue)
            {
                VeterinarianModel veterinarian = dbContext.Veterinarians.FirstOrDefault(x => x.VetId == pet.VetId.Value);

                petDetails.VeterinarianDisplay = veterinarian == null
                    ? "Veterinarian not found"
                    : veterinarian.FirstName + " " + veterinarian.LastName + ", " + veterinarian.Credentials;
            }
            else
            {
                petDetails.VeterinarianDisplay = "No veterinarian assigned";
            }

            List<DietModel> diets = dbContext.Diets
                .Where(x => x.PetId  == petId)
                .ToList();

            foreach (DietModel diet in diets)
            {
                DietSummaryVM dietSummary = new DietSummaryVM();

                dietSummary.DietId = diet.DietId;
                dietSummary.FoodName = diet.FoodName;
                dietSummary.Amount = diet.Amount;

                petDetails.Diets.Add(dietSummary);

            }

            List<MedicationModel> medications = dbContext.Medications
               .Where(x => x.PetId == petId)
               .ToList();

            foreach (MedicationModel medication in medications)
            {
                MedicationSummaryVM medicationSummary = new MedicationSummaryVM();

                medicationSummary.MedicationId = medication.MedicationId;
                medicationSummary.MedicationName = medication.MedicationName;
                medicationSummary.Dosage = medication.Dosage;

                petDetails.Medications.Add(medicationSummary);

            }

            List<PetVaccineModel> petVaccines = dbContext.PetVaccines
                .Include(x => x.Vaccine)
                .Where(x => x.PetId == petId)
                .ToList();

            foreach (PetVaccineModel petVaccine in petVaccines)
            {
                PetVaccineSummaryVM petVaccineSummary = new PetVaccineSummaryVM();

                petVaccineSummary.PetVaccineId = petVaccine.PetVaccineId;
                petVaccineSummary.VaccineNameDisplay = petVaccine.Vaccine.VaccineName;
                petVaccineSummary.DateGivenDisplay = petVaccine.DateGiven.ToString("MM/dd/yyyy");
                petVaccineSummary.ExpirationDateDisplay = petVaccine.ExpirationDate.ToString("MM/dd/yyyy");

                petDetails.PetVaccines.Add(petVaccineSummary);

            }

            return View(petDetails);
        }


        // GET: Pets/Update
        public ActionResult Update(Guid petId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null)
            {
                return Content("Pet ID #" + petId + " does not exist.");
            }

            PetFormVM petForm = new PetFormVM();

            petForm.PetId = pet.PetId;
            petForm.VetId = pet.VetId;
            petForm.PetName = pet.PetName;
            petForm.Species = pet.Species;
            petForm.Breed = pet.Breed;
            petForm.Sex = pet.Sex;
            petForm.BirthDate = pet.BirthDate;
            petForm.Weight = pet.Weight;
            petForm.Notes = pet.Notes;

            petForm.VeterinarianSelectList = BuildVeterinarianSelectList();

            return View(petForm);

        }


        // POST: Pets/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(PetFormVM petForm)
        {
            
            if (!ModelState.IsValid)
            {
                petForm.VeterinarianSelectList = BuildVeterinarianSelectList();
                return View(petForm);
            }

            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (petForm.BirthDate > DateTime.Today)
            {
                ModelState.AddModelError("BirthDate", "Birth date cannot be in the future.");
                petForm.VeterinarianSelectList = BuildVeterinarianSelectList();
                return View(petForm);
            }

            if (petForm.VetId.HasValue)
            {
                bool veterinarianExists = dbContext.Veterinarians.Any(x => x.VetId == petForm.VetId.Value);

                if (!veterinarianExists)
                {
                    ModelState.AddModelError("VetId", "Selected veterinarian does not exist.");
                    petForm.VeterinarianSelectList = BuildVeterinarianSelectList();
                    return View(petForm);
                }
            }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petForm.PetId);

            if (pet == null)
            {
                return Content("Pet ID #" + petForm.PetId + " does not exist.");
            }

            pet.VetId = petForm.VetId;
            pet.PetName = petForm.PetName;
            pet.Species = petForm.Species;
            pet.Breed = petForm.Breed;
            pet.Sex = petForm.Sex;
            pet.BirthDate = petForm.BirthDate;
            pet.Weight = petForm.Weight;
            pet.Notes = petForm.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read",
                new
                {
                    petId = pet.PetId
                }
             );
        }


        // GET: Pets/Delete
        public ActionResult Delete(Guid petId) { 

            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null)
            { 
                return Content("Pet ID #" + petId + " does not exist.");
            }

            int age = DateTime.Today.Year - pet.BirthDate.Year;
            if (pet.BirthDate.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            PetDeactivateVM petDeactivate = new PetDeactivateVM();

            petDeactivate.PetId = pet.PetId;

            petDeactivate.PetNameDisplay = pet.PetName;

            petDeactivate.SpeciesDisplay = pet.Species.ToString();

            petDeactivate.BreedDisplay = pet.Breed;

            petDeactivate.SexDisplay = pet.Sex.ToString();

            petDeactivate.BirthDateDisplay = pet.BirthDate.ToShortDateString();

            petDeactivate.AgeDisplay = age.ToString();

            petDeactivate.ActiveStatusDisplay = pet.IsActive ? "Active" : "Inactive";

            petDeactivate.NotesDisplay = string.IsNullOrWhiteSpace(pet.Notes)
                ? "No notes"
                : pet.Notes;

            return View(petDeactivate);
 
        }


        // POST: Pets/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(PetDeactivateVM petDeactivate)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petDeactivate.PetId);

            if (pet == null)
            {
                return Content("Pet ID #" + petDeactivate.PetId + " does not exist.");

            }

            int age = DateTime.Today.Year - pet.BirthDate.Year;
            if (pet.BirthDate.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            if (!ModelState.IsValid) 
            {

                petDeactivate.PetId = pet.PetId;

                petDeactivate.PetNameDisplay = pet.PetName;

                petDeactivate.SpeciesDisplay = pet.Species.ToString();

                petDeactivate.BreedDisplay = pet.Breed;

                petDeactivate.SexDisplay = pet.Sex.ToString();

                petDeactivate.BirthDateDisplay = pet.BirthDate.ToShortDateString();

                petDeactivate.AgeDisplay = age.ToString();

                petDeactivate.ActiveStatusDisplay = pet.IsActive ? "Active" : "Inactive";

                petDeactivate.NotesDisplay = string.IsNullOrWhiteSpace(pet.Notes)
                    ? "No notes"
                    : pet.Notes;

                return View(petDeactivate); 
            }

            pet.IsActive = false;

            pet.InactivationReason = petDeactivate.InactivationReason;

            pet.InactivationDate = DateTime.Now;

            pet.InactivationNotes = petDeactivate.InactivationNotes;

            pet.ReactivationDate = null;

            pet.ReactivationNotes = null;

          
            dbContext.SaveChanges();

            return RedirectToAction("Read", new { petId = pet.PetId });

        }


        // GET: Pets/Reactivate
        public ActionResult Reactivate(Guid petId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null)
            {
                return Content("Pet ID #" + petId + " does not exist.");
            }

            int age = DateTime.Today.Year - pet.BirthDate.Year;
            if (pet.BirthDate.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            PetReactivateVM petReactivate = new PetReactivateVM();

            petReactivate.PetId = pet.PetId;
            petReactivate.PetNameDisplay = pet.PetName;
            petReactivate.SpeciesDisplay = pet.Species.ToString();
            petReactivate.BreedDisplay = pet.Breed;
            petReactivate.SexDisplay = pet.Sex.ToString();
            petReactivate.BirthDateDisplay = pet.BirthDate.ToShortDateString();
            petReactivate.AgeDisplay = age.ToString();
            petReactivate.ActiveStatusDisplay = pet.IsActive ? "Active" : "Inactive";

            petReactivate.NotesDisplay = string.IsNullOrWhiteSpace(pet.Notes)
                ? "No notes"
                : pet.Notes;

            petReactivate.InactivationReasonDisplay = pet.InactivationReason.HasValue
                ? pet.InactivationReason.ToString()
                : "Not Applicable";

            petReactivate.InactivationDateDisplay = pet.InactivationDate.HasValue
                ? pet.InactivationDate.Value.ToShortDateString()
                : "Not Applicable";

            petReactivate.InactivationNotesDisplay = string.IsNullOrWhiteSpace(pet.InactivationNotes)
                ? "No Inactivation notes"
                : pet.InactivationNotes;

            return View(petReactivate);

        }


        // POST: Pets/Reactivate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reactivate(PetReactivateVM petReactivate)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petReactivate.PetId);

            if (pet == null)
            {
                return Content("Pet ID #" + petReactivate.PetId + " does not exist.");
            }

            int age = DateTime.Today.Year - pet.BirthDate.Year;
            if (pet.BirthDate.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            if (!ModelState.IsValid) 
            {

                petReactivate.PetId = pet.PetId;

                petReactivate.PetNameDisplay = pet.PetName;

                petReactivate.SpeciesDisplay = pet.Species.ToString();

                petReactivate.BreedDisplay = pet.Breed;

                petReactivate.SexDisplay = pet.Sex.ToString();

                petReactivate.BirthDateDisplay = pet.BirthDate.ToShortDateString();

                petReactivate.AgeDisplay = age.ToString();

                petReactivate.ActiveStatusDisplay = pet.IsActive ? "Active" : "Inactive";

                petReactivate.NotesDisplay = string.IsNullOrWhiteSpace(pet.Notes)
                    ? "No notes"
                    : pet.Notes;

                petReactivate.InactivationReasonDisplay = pet.InactivationReason.HasValue
                    ? pet.InactivationReason.ToString()
                    : "Not Applicable";

                petReactivate.InactivationDateDisplay = pet.InactivationDate.HasValue
                    ? pet.InactivationDate.Value.ToShortDateString()
                    : "Not Applicable";

                petReactivate.InactivationNotesDisplay = string.IsNullOrWhiteSpace(pet.InactivationNotes)
                    ? "No Inactivation notes"
                    : pet.InactivationNotes;

                return View(petReactivate);
            }

            pet.IsActive = true;

            pet.ReactivationDate = DateTime.Now;

            pet.ReactivationNotes = petReactivate.ReactivationNotes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { petId = pet.PetId });

        }


        private SelectList BuildVeterinarianSelectList()
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            var vetDropdownItems = dbContext.Veterinarians
                        .Select(x => new
                        {
                            VetId = x.VetId,
                            VetDisplay = x.FirstName + " " + x.LastName + ", " + x.Credentials
                        })
                        .ToList();

            SelectList vetSelectList = new SelectList(vetDropdownItems, "VetId", "VetDisplay");

            return vetSelectList;

        }
    }
}