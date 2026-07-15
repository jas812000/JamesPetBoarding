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
            petDetails.BirthDateDisplay = pet.BirthDate.ToShortDateString();
            petDetails.AgeDisplay = age.ToString();
            petDetails.WeightDisplay = pet.Weight.ToString();
            petDetails.IsActive = pet.IsActive;

            petDetails.ActiveStatusDisplay = pet.IsActive ? "Active" : "Inactive";

            petDetails.NotesDisplay = string.IsNullOrWhiteSpace(pet.Notes)
                ? "No notes"
                : pet.Notes;

            petDetails.InactiveReasonDisplay = pet.InactiveReason.HasValue
                ? pet.InactiveReason.ToString()
                : "Not Applicable";

            petDetails.InactivatedDateDisplay = pet.InactivatedDate.HasValue
                ? pet.InactivatedDate.Value.ToShortDateString()
                : "Not Applicable";

            petDetails.InactiveNotesDisplay = string.IsNullOrWhiteSpace(pet.InactiveNotes)
                ? "No inactive notes"
                : pet.InactiveNotes;

            petDetails.ReactivatedDateDisplay = pet.ReactivatedDate.HasValue
                ? pet.ReactivatedDate.Value.ToShortDateString()
                : "Not Applicable";

            petDetails.ReactivatedNotesDisplay = string.IsNullOrWhiteSpace(pet.ReactivatedNotes)
                ? "No reactivation notes"
                : pet.ReactivatedNotes;

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

            PetDeleteVM petDelete = new PetDeleteVM();



            petDelete.PetId = pet.PetId;
            petDelete.PetNameDisplay = pet.PetName;
            petDelete.SpeciesDisplay = pet.Species.ToString();
            petDelete.BreedDisplay = pet.Breed;
            petDelete.SexDisplay = pet.Sex.ToString();
            petDelete.BirthDateDisplay = pet.BirthDate.ToShortDateString();
            petDelete.AgeDisplay = age.ToString();
            petDelete.ActiveStatusDisplay = pet.IsActive ? "Active" : "Inactive";

            petDelete.NotesDisplay = string.IsNullOrWhiteSpace(pet.Notes)
                ? "No notes"
                : pet.Notes;

            return View(petDelete);
 
        }


        // POST: Pets/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PetDeleteVM petDelete)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petDelete.PetId);

            if (pet == null)
            {
                return Content("Pet ID #" + petDelete.PetId + " does not exist.");
            }

            int age = DateTime.Today.Year - pet.BirthDate.Year;
            if (pet.BirthDate.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            if (!ModelState.IsValid) 
            {

                petDelete.PetId = pet.PetId;
                petDelete.PetNameDisplay = pet.PetName;
                petDelete.SpeciesDisplay = pet.Species.ToString();
                petDelete.BreedDisplay = pet.Breed;
                petDelete.SexDisplay = pet.Sex.ToString();
                petDelete.BirthDateDisplay = pet.BirthDate.ToShortDateString();
                petDelete.AgeDisplay = age.ToString();
                petDelete.ActiveStatusDisplay = pet.IsActive ? "Active" : "Inactive";
                petDelete.NotesDisplay = string.IsNullOrWhiteSpace(pet.Notes)
                    ? "No notes"
                    : pet.Notes;

                return View(petDelete); 
            }

            pet.IsActive = false;
            pet.InactiveReason = petDelete.InactiveReason;
            pet.InactivatedDate = DateTime.Now;
            pet.InactiveNotes = petDelete.InactiveNotes;
            dbContext.SaveChanges();

            return RedirectToAction("Read",
                new
                {
                    petId = pet.PetId
                }
            );

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

            petReactivate.InactiveReasonDisplay = pet.InactiveReason.HasValue
                ? pet.InactiveReason.ToString()
                : "Not Applicable";

            petReactivate.InactivatedDateDisplay = pet.InactivatedDate.HasValue
                ? pet.InactivatedDate.Value.ToShortDateString()
                : "Not Applicable";

            petReactivate.InactiveNotesDisplay = string.IsNullOrWhiteSpace(pet.InactiveNotes)
                ? "No inactive notes"
                : pet.InactiveNotes;

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

                petReactivate.InactiveReasonDisplay = pet.InactiveReason.HasValue
                    ? pet.InactiveReason.ToString()
                    : "Not Applicable";

                petReactivate.InactivatedDateDisplay = pet.InactivatedDate.HasValue
                    ? pet.InactivatedDate.Value.ToShortDateString()
                    : "Not Applicable";

                petReactivate.InactiveNotesDisplay = string.IsNullOrWhiteSpace(pet.InactiveNotes)
                    ? "No inactive notes"
                    : pet.InactiveNotes;

                return View(petReactivate);
            }

            pet.IsActive = true;
            pet.ReactivatedDate = DateTime.Now;
            pet.ReactivatedNotes = petReactivate.ReactivatedNotes;

            pet.InactiveReason = null;
            pet.InactivatedDate = null;
            pet.InactiveNotes = null;
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