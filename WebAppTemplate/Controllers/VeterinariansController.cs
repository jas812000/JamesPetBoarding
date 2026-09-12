using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.BuilderProperties;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.ClientServices.Providers;
using System.Web.Mvc;
using System.Xml.Linq;

namespace WebAppTemplate.Controllers
{
    [Authorize]
    public class VeterinariansController : Controller
    {

        // GET: Veterinarians/Search
        public ActionResult Search()
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            VeterinarianSearchVM veterinarianSearch = new VeterinarianSearchVM();

            return View(veterinarianSearch);

        }


        // POST: Veterinarians/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(VeterinarianSearchVM veterinarianSearch)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            List<VeterinarianModel> veterinarians = dbContext.Veterinarians
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ThenBy(x => x.ClinicName)
                .ToList();

            if (!string.IsNullOrWhiteSpace(veterinarianSearch.FirstName))
            {
                veterinarians = veterinarians
                    .Where(x => x.FirstName.ToLower().Contains(veterinarianSearch.FirstName.ToLower()))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(veterinarianSearch.LastName))
            {
                veterinarians = veterinarians
                    .Where(x => x.LastName.ToLower().Contains(veterinarianSearch.LastName.ToLower()))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(veterinarianSearch.ClinicName))
            {
                veterinarians = veterinarians
                    .Where(x => x.ClinicName.ToLower().Contains(veterinarianSearch.ClinicName.ToLower()))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(veterinarianSearch.City))
            {
                veterinarians = veterinarians
                    .Where(x => x.City.ToLower().Contains(veterinarianSearch.City.ToLower()))
                    .ToList();
            }

            if (veterinarianSearch.State.HasValue)
            {
                veterinarians = veterinarians
                    .Where(x => x.State == veterinarianSearch.State.Value)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(veterinarianSearch.ZipCode))
            {
                veterinarians = veterinarians
                    .Where(x => x.ZipCode.ToLower().Contains(veterinarianSearch.ZipCode.ToLower()))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(veterinarianSearch.Phone))
            {
                veterinarians = veterinarians
                    .Where(x => x.Phone.ToLower().Contains(veterinarianSearch.Phone.ToLower()))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(veterinarianSearch.Email))
            {
                veterinarians = veterinarians
                    .Where(x => x.Email.ToLower().Contains(veterinarianSearch.Email.ToLower()))
                    .ToList();
            }

            foreach (VeterinarianModel veterinarian in veterinarians)
            {
                veterinarianSearch.VeterinarianSummaryResults.Add(new VeterinarianSummaryVM
                {
                    VetId = veterinarian.VetId,

                    ClinicNameDisplay = veterinarian.ClinicName,

                    FullNameCredentialsDisplay =
                        veterinarian.FirstName + " " +
                        veterinarian.LastName + ", " +
                        veterinarian.Credentials,

                    CityStateDisplay =
                        veterinarian.City + ", " +
                        veterinarian.State + " " +
                        veterinarian.ZipCode,

                    PhoneDisplay = veterinarian.Phone,

                    EmailDisplay = veterinarian.Email,

                    ActiveStatusDisplay = veterinarian.IsActive
                        ? "Active"
                        : "Inactive",

                    IsActive = veterinarian.IsActive,

                    NotesDisplay = veterinarian.Notes,

                });

            }

            return View(veterinarianSearch);

        }


        // GET: Veterinarians/Create
        public ActionResult Create()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            VeterinarianFormVM veterinarianForm = new VeterinarianFormVM();

            return View(veterinarianForm);

        }


        // POST: Veterinarians/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VeterinarianFormVM veterinarianForm)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            if (!ModelState.IsValid)
            {
                return View(veterinarianForm);
            }

            VeterinarianModel existingVeterinarian = dbContext.Veterinarians
                .FirstOrDefault(x =>
                x.LastName == veterinarianForm.LastName &&
                x.FirstName == veterinarianForm.FirstName &&
                x.ClinicName == veterinarianForm.ClinicName &&
                x.Email == veterinarianForm.Email);

            if (existingVeterinarian != null)
            {
                ModelState.AddModelError("",
                    "The email address " +
                    veterinarianForm.Email +
                    " for " +
                    veterinarianForm.FirstName +
                    " " +
                    veterinarianForm.LastName +
                    " already exists.");

                return View(veterinarianForm);
            }

            VeterinarianModel veterinarian = new VeterinarianModel();

            veterinarian.LastName = FormatName(veterinarianForm.LastName);
            veterinarian.FirstName = FormatName(veterinarianForm.FirstName);
            veterinarian.Credentials = veterinarianForm.Credentials;
            veterinarian.ClinicName = veterinarianForm.ClinicName;
            veterinarian.Address = veterinarianForm.Address;
            veterinarian.City = veterinarianForm.City;
            veterinarian.State = veterinarianForm.State.Value;
            veterinarian.ZipCode = veterinarianForm.ZipCode;
            veterinarian.Phone = veterinarianForm.Phone;
            veterinarian.Email = veterinarianForm.Email;
            veterinarian.Notes = veterinarianForm.Notes;
            veterinarian.IsActive = true;

            dbContext.Veterinarians.Add(veterinarian);
            dbContext.SaveChanges();

            return RedirectToAction("Read", new { vetId = veterinarian.VetId });

        }


        // GET: Veterinarians/Read
        public ActionResult Read(Guid vetId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            VeterinarianModel veterinarian = dbContext.Veterinarians.FirstOrDefault(x => x.VetId == vetId);

            if (veterinarian == null)
            {
                return Content("Vet ID #" + vetId + " does not exist.");
            }

            VeterinarianDetailsVM veterinarianDetails = new VeterinarianDetailsVM();

            veterinarianDetails.VetId = veterinarian.VetId;

            veterinarianDetails.FullNameCredentialsDisplay =
                veterinarian.FirstName + " " +
                veterinarian.LastName + ", " +
                veterinarian.Credentials;

            veterinarianDetails.ClinicNameDisplay = veterinarian.ClinicName;

            veterinarianDetails.AddressDisplay = veterinarian.Address;

            veterinarianDetails.CityStateZipDisplay =
                veterinarian.City + ", " +
                veterinarian.State + " " +
                veterinarian.ZipCode;

            veterinarianDetails.PhoneDisplay = veterinarian.Phone;

            veterinarianDetails.EmailDisplay = veterinarian.Email;

            veterinarianDetails.IsActive = veterinarian.IsActive;

            veterinarianDetails.ActiveStatusDisplay = veterinarian.IsActive
                ? "Active"
                : "Inactive";

            veterinarianDetails.NotesDisplay = string.IsNullOrWhiteSpace(veterinarian.Notes)
                ? "No notes"
                : veterinarian.Notes;

            List<PetModel> pets = dbContext.Pets
                .Where(x => x.VetId == veterinarian.VetId)
                .OrderBy(x => x.PetName)
                .ToList();

            foreach (PetModel pet in pets)
            {
                PetSummaryVM petSummaries = new PetSummaryVM();

                petSummaries.PetId = pet.PetId;

                petSummaries.PetNameDisplay = pet.PetName;

                petSummaries.SpeciesDisplay = pet.Species.ToString();

                petSummaries.BreedDisplay = pet.Breed;

                petSummaries.SexDisplay = pet.Sex.ToString();

                petSummaries.BirthDateDisplay = $"{pet.BirthDate:MM/dd/yyyy}";

                int age = DateTime.Today.Year - pet.BirthDate.Year;
                if (pet.BirthDate.Date > DateTime.Today.AddYears(-age))
                {
                    age--;
                }

                petSummaries.AgeDisplay = age.ToString();

                petSummaries.WeightDisplay = pet.Weight.ToString();

                petSummaries.IsActive = pet.IsActive;

                petSummaries.ActiveStatusDisplay = pet.IsActive
                    ? "Active"
                    : "Inactive";

                veterinarianDetails.PetSummaries.Add(petSummaries);

            }

            return View(veterinarianDetails);

        }


        // GET: Veterinarians/Update
        public ActionResult Update(Guid vetId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            VeterinarianModel veterinarian = dbContext.Veterinarians.FirstOrDefault(x => x.VetId == vetId);

            if (veterinarian == null)
            {
                return Content("Vet ID #" + vetId + " does not exist.");
            }

            VeterinarianFormVM veterinarianForm = new VeterinarianFormVM();

            veterinarianForm.VetId = veterinarian.VetId;

            veterinarianForm.LastName = veterinarian.LastName;

            veterinarianForm.FirstName = veterinarian.FirstName;

            veterinarianForm.Credentials = veterinarian.Credentials;

            veterinarianForm.ClinicName = veterinarian.ClinicName;

            veterinarianForm.Address = veterinarian.Address;

            veterinarianForm.City = veterinarian.City;

            veterinarianForm.State = veterinarian.State;

            veterinarianForm.ZipCode = veterinarian.ZipCode;

            veterinarianForm.Phone = veterinarian.Phone;

            veterinarianForm.Email = veterinarian.Email;

            veterinarianForm.Notes = veterinarian.Notes;

            return View(veterinarianForm);

        }


        // POST: Veterinarians/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(VeterinarianFormVM veterinarianForm)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            if (!ModelState.IsValid)
            {
                return View(veterinarianForm);
            }

            VeterinarianModel veterinarian = dbContext.Veterinarians.FirstOrDefault(x => x.VetId == veterinarianForm.VetId);

            if (veterinarian == null)
            {
                return Content("Vet ID #" + veterinarianForm.VetId + " does not exist.");
            }

            VeterinarianModel existingVeterinarian = dbContext.Veterinarians
                .FirstOrDefault(x =>
                x.VetId != veterinarianForm.VetId &&
                x.LastName == veterinarianForm.LastName &&
                x.FirstName == veterinarianForm.FirstName &&
                x.ClinicName == veterinarianForm.ClinicName &&
                x.Email == veterinarianForm.Email);

            if (existingVeterinarian != null)
            {
                ModelState.AddModelError("",
                    "The email address " +
                    veterinarianForm.Email +
                    " for " +
                    veterinarianForm.FirstName +
                    " " +
                    veterinarianForm.LastName +
                    " already exists.");

                return View(veterinarianForm);
            }

            veterinarian.LastName = FormatName(veterinarianForm.LastName);
            veterinarian.FirstName = FormatName(veterinarianForm.FirstName);
            veterinarian.Credentials = veterinarianForm.Credentials;
            veterinarian.ClinicName = veterinarianForm.ClinicName;
            veterinarian.Address = veterinarianForm.Address;
            veterinarian.City = veterinarianForm.City;
            veterinarian.State = veterinarianForm.State.Value;
            veterinarian.ZipCode = veterinarianForm.ZipCode;
            veterinarian.Phone = veterinarianForm.Phone;
            veterinarian.Email = veterinarianForm.Email;
            veterinarian.Notes = veterinarianForm.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { vetId = veterinarian.VetId });

        }


        // GET: Veterinarians/Deactivate
        public ActionResult Deactivate(Guid vetId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            VeterinarianModel veterinarian = dbContext.Veterinarians.FirstOrDefault(x => x.VetId == vetId);

            if (veterinarian == null)
            {
                return Content("Vet ID #" + vetId + " does not exist.");
            }

            if (!veterinarian.IsActive)
            {
                return Content("This veterinarian is currently inactive.");
            }

            VeterinarianStatusVM veterinarianStatus = new VeterinarianStatusVM();

            veterinarianStatus.VetId = veterinarian.VetId;

            veterinarianStatus.FullNameCredentialsDisplay =
                veterinarian.FirstName + " " +
                veterinarian.LastName + ", " +
                veterinarian.Credentials;

            veterinarianStatus.ClinicNameDisplay = veterinarian.ClinicName;

            veterinarianStatus.IsActive = veterinarian.IsActive;

            veterinarianStatus.ActiveStatusDisplay = veterinarian.IsActive
                ? "Active"
                : "Inactive";

            return View(veterinarianStatus);

        }


        // POST: Veterinarians/Deactivate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(VeterinarianStatusVM veterinarianStatus)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            VeterinarianModel veterinarian = dbContext.Veterinarians.FirstOrDefault(x => x.VetId == veterinarianStatus.VetId);

            if (veterinarian == null)
            {
                return Content("Vet ID #" + veterinarianStatus.VetId + " does not exist.");
            }

            if (!veterinarian.IsActive)
            {
                return Content("This veterinarian is currently inactive.");
            }

            veterinarian.IsActive = false;

            veterinarian.Notes = veterinarianStatus.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { vetId = veterinarian.VetId });

        }


        // GET: Veterinarians/Reactivate
        public ActionResult Reactivate(Guid vetId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            VeterinarianModel veterinarian = dbContext.Veterinarians.FirstOrDefault(x => x.VetId == vetId);

            if (veterinarian == null)
            {
                return Content("Vet ID #" + vetId + " does not exist.");
            }

            if (veterinarian.IsActive)
            {
                return Content("This veterinarian is currently active.");
            }

            VeterinarianStatusVM veterinarianStatus = new VeterinarianStatusVM();

            veterinarianStatus.VetId = veterinarian.VetId;

            veterinarianStatus.FullNameCredentialsDisplay =
                veterinarian.FirstName + " " +
                veterinarian.LastName + ", " +
                veterinarian.Credentials;

            veterinarianStatus.ClinicNameDisplay = veterinarian.ClinicName;

            veterinarianStatus.IsActive = veterinarian.IsActive;

            veterinarianStatus.ActiveStatusDisplay = veterinarian.IsActive
                ? "Active"
                : "Inactive";

            return View(veterinarianStatus);

        }


        // POST: Veterinarians/Reactivate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reactivate(VeterinarianStatusVM veterinarianStatus)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            VeterinarianModel veterinarian = dbContext.Veterinarians.FirstOrDefault(x => x.VetId == veterinarianStatus.VetId);

            if (veterinarian == null)
            {
                return Content("Vet ID #" + veterinarianStatus.VetId + " does not exist.");
            }

            if (veterinarian.IsActive)
            {
                return Content("This veterinarian is currently active.");
            }

            veterinarian.IsActive = true;

            veterinarian.Notes = null;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { vetId = veterinarian.VetId });

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
