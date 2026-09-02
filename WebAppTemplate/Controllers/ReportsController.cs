using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {

        // GET: Reports
        public ActionResult Index()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewReports(currentEmployee))
            { 
                return RedirectToAction("Index", "User");
            }

            ViewBag.CanViewFinancialReports = 
                currentEmployee.Role == EmployeeRoleEnum.Admin || 
                currentEmployee.Role == EmployeeRoleEnum.Manager;

            return View();

        }

        // GET: Reports/CustomerActivityReport
        public ActionResult CustomerActivityReport() 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerActivityReportVM customerActivityReport = new CustomerActivityReportVM();

            customerActivityReport.CustomerActivityReportFilter = new CustomerActivityReportFilterVM();

            customerActivityReport.CustomerActivityReportRows = new List<CustomerActivityReportRowVM>();

            customerActivityReport.CustomerSelectList = BuildCustomerSelectList(dbContext);

            return View (customerActivityReport);
      
        }


        // POST: Reports/CustomerActivityReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerActivityReport(CustomerActivityReportVM customerActivityReport) 
        { 
            ApplicationDbContext dbContext = new ApplicationDbContext();

            customerActivityReport.CustomerSelectList = BuildCustomerSelectList(
                dbContext,
                customerActivityReport.CustomerActivityReportFilter.CustomerId
            );

            if (!ModelState.IsValid)
            {
                return View(customerActivityReport);
            }

            List<CustomerModel> customers = dbContext.Customers
                .Include(x => x.EmergencyContacts)
                .Include(x => x.CustomerPets)
                .Include(x => x.Boardings)
                .Include(x => x.Invoices.Select(w => w.Payments))
                .ToList();

            if (customerActivityReport.CustomerActivityReportFilter.CustomerId.HasValue)
            {
                customers = customers
                    .Where(x => x.CustomerId == customerActivityReport.CustomerActivityReportFilter.CustomerId.Value)
                    .ToList();

            }

            if (customerActivityReport.CustomerActivityReportFilter.ActiveStatus.HasValue)
            {
                if (customerActivityReport.CustomerActivityReportFilter.ActiveStatus.Value == ActiveStatusEnum.Active)
                {
                    customers = customers
                        .Where(x => x.IsActive)
                        .ToList();
                }

                if (customerActivityReport.CustomerActivityReportFilter.ActiveStatus.Value == ActiveStatusEnum.Inactive)
                {
                    customers = customers
                        .Where(x => !x.IsActive)
                        .ToList();
                }
            }
            
            if (!string.IsNullOrWhiteSpace(customerActivityReport.CustomerActivityReportFilter.LastName))
            {
                customers = customers
                    .Where(x => x.LastName == customerActivityReport.CustomerActivityReportFilter.LastName)
                    .ToList();
            }
            
            if (!string.IsNullOrWhiteSpace(customerActivityReport.CustomerActivityReportFilter.FirstName))
            {
                customers = customers
                    .Where(x => x.FirstName == customerActivityReport.CustomerActivityReportFilter.FirstName)
                    .ToList();
            }
            
            if (!string.IsNullOrWhiteSpace(customerActivityReport.CustomerActivityReportFilter.City))
            {
                customers = customers
                    .Where(x => x.City == customerActivityReport.CustomerActivityReportFilter.City)
                    .ToList();
            }
            
            if (customerActivityReport.CustomerActivityReportFilter.State.HasValue)
            {
                customers = customers
                    .Where(x => x.State == customerActivityReport.CustomerActivityReportFilter.State.Value)
                    .ToList();
            }
            
            if (!string.IsNullOrWhiteSpace(customerActivityReport.CustomerActivityReportFilter.ZipCode))
            {
                customers = customers
                    .Where(x => x.ZipCode == customerActivityReport.CustomerActivityReportFilter.ZipCode)
                    .ToList();
            }
            
            if (!string.IsNullOrWhiteSpace(customerActivityReport.CustomerActivityReportFilter.Phone))
            {
                customers = customers
                    .Where(x => x.Phone == customerActivityReport.CustomerActivityReportFilter.Phone)
                    .ToList();
            }
            
            if (!string.IsNullOrWhiteSpace(customerActivityReport.CustomerActivityReportFilter.Email))
            {
                customers = customers
                    .Where(x => x.Email == customerActivityReport.CustomerActivityReportFilter.Email)
                    .ToList();
            }

            customerActivityReport.CustomerActivityReportRows = new List<CustomerActivityReportRowVM>();

            foreach (CustomerModel customer in customers) 
            { 
                CustomerActivityReportRowVM customerRow = new CustomerActivityReportRowVM();

                customerRow.CustomerId = customer.CustomerId;

                customerRow.CustomerNameDisplay = customer.FirstName + " " + customer.LastName;

                customerRow.ActiveStatusDisplay = customer.IsActive 
                    ? "Active" 
                    : "Inactive";

                customerRow.AddressDisplay = customer.Address;

                customerRow.CityDisplay = customer.City;

                customerRow.StateDisplay = customer.State.ToString();

                customerRow.ZipCodeDisplay = customer.ZipCode;

                customerRow.EmailDisplay = customer.Email;

                customerRow.PhoneDisplay = customer.Phone;

                customerRow.NotesDisplay = customer.Notes;

                customerRow.EmergencyContactCount = customer.EmergencyContacts.Count;

                customerRow.PetCount = customer.CustomerPets.Count;

                List<BoardingModel> actualBoardings = customer.Boardings
                    .Where(x => x.Status == BoardingStatusEnum.CheckedIn || x.Status == BoardingStatusEnum.CheckedOut)
                    .ToList();

                customerRow.BoardingCount = actualBoardings.Count;

                List<InvoiceModel> nonVoidedInvoices = customer.Invoices
                    .Where(x => x.InvoiceStatus != InvoiceStatusEnum.Void)
                    .ToList();

                customerRow.TotalInvoiceAmountDisplay = nonVoidedInvoices
                    .Sum(x => x.TotalAmount)
                    .ToString("C");

                customerRow.OutstandingBalanceDisplay = nonVoidedInvoices
                    .Sum(x => x.Balance)
                    .ToString("C");

                decimal totalPayments = nonVoidedInvoices
                    .SelectMany(x => x.Payments)
                    .Where(x => !x.IsVoided)
                    .Sum(x => x.Amount);

                customerRow.TotalPaymentAmountDisplay = totalPayments.ToString("C");

                customerRow.InvoiceCount = nonVoidedInvoices.Count;

                List<DateTime> activityDates = new List<DateTime>();

                activityDates.AddRange(actualBoardings
                    .Select(x =>
                        x.ActualCheckOutDateTime
                        ?? x.ActualCheckInDateTime
                        ?? x.StartDateTime)
                );

                activityDates.AddRange(nonVoidedInvoices
                    .Select(x => x.InvoiceDateTime)
                );

                activityDates.AddRange(nonVoidedInvoices
                    .SelectMany(x => x.Payments)
                    .Where(x => !x.IsVoided)
                    .Select(x => x.PaymentDateTime)
                );

                customerRow.LastActivityDateDisplay = activityDates.Any() 
                    ? activityDates.Max().ToString("MM/dd/yyyy") 
                    : "No Activity";

                DateTime frequentCustomerStartDate = DateTime.Today.AddYears(-1);

                int recentBoardingCount = actualBoardings.Count(x => x.StartDateTime >= frequentCustomerStartDate);

                customerRow.IsFrequentCustomerDisplay = recentBoardingCount >= 5
                    ? "Yes"
                    : "No";

                customerRow.LastBoardingDateDisplay = actualBoardings.Any() 
                    ? actualBoardings
                        .Max(x => x.StartDateTime)
                        .ToString("MM/dd/yyyy") 
                    : "No Boardings";

                customerActivityReport.CustomerActivityReportRows.Add(customerRow);
            
            }

            customerActivityReport.TotalCount = customers.Count;

            customerActivityReport.ActiveCount = customers.Count(x => x.IsActive);

            customerActivityReport.InactiveCount = customers.Count(x => !x.IsActive);

            return View(customerActivityReport);

        }


        // GET: Reports/PetReport
        public ActionResult PetReport() 
        { 
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetReportVM petReport = new PetReportVM();

            petReport.PetReportFilter = new PetReportFilterVM();

            petReport.PetReportRows = new List<PetReportRowVM>();

            petReport.PetSelectList = BuildPetSelectList(dbContext);

            petReport.VeterinarianSelectList = BuildVeterinarianSelectList(dbContext);

            petReport.CustomerSelectList = BuildCustomerSelectList(dbContext);

            return View(petReport);

        }


        // POST: Reports/PetReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PetReport(PetReportVM petReport) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            petReport.PetSelectList = BuildPetSelectList(dbContext, petReport.PetReportFilter.PetId);

            petReport.VeterinarianSelectList = BuildVeterinarianSelectList(dbContext, petReport.PetReportFilter.VetId);

            petReport.CustomerSelectList = BuildCustomerSelectList(dbContext, petReport.PetReportFilter.CustomerId);

            if (!ModelState.IsValid)
            {
                return View(petReport);
            }

            List<PetModel> pets = dbContext.Pets
                .Include(x => x.Veterinarian)
                .Include(x => x.CustomerPets.Select(w => w.Customer))
                .ToList();

            if (petReport.PetReportFilter.PetId.HasValue)
            {
                pets = pets
                    .Where(x => x.PetId == petReport.PetReportFilter.PetId.Value)
                    .ToList();

            }

            if (petReport.PetReportFilter.ActiveStatus.HasValue)
            {
                if (petReport.PetReportFilter.ActiveStatus.Value == ActiveStatusEnum.Active)
                {
                    pets = pets
                        .Where(x => x.IsActive)
                        .ToList();
                }

                if (petReport.PetReportFilter.ActiveStatus.Value == ActiveStatusEnum.Inactive)
                {
                    pets = pets
                        .Where(x => !x.IsActive)
                        .ToList();
                }
            }

            if (petReport.PetReportFilter.VetId.HasValue)
            {
                pets = pets
                    .Where(x => x.VetId == petReport.PetReportFilter.VetId.Value)
                    .ToList();

            }

            if (petReport.PetReportFilter.CustomerId.HasValue)
            {
                pets = pets
                    .Where(x => x.CustomerPets.Any(
                        w => w.CustomerId == petReport.PetReportFilter.CustomerId.Value))
                    .ToList();

            }

            if (!string.IsNullOrWhiteSpace(petReport.PetReportFilter.PetName))
            {
                pets = pets
                    .Where(x => x.PetName.Contains(petReport.PetReportFilter.PetName))
                    .ToList();
            }

            if (petReport.PetReportFilter.Species.HasValue)
            {
                pets = pets
                    .Where(x => x.Species == petReport.PetReportFilter.Species.Value)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(petReport.PetReportFilter.Breed))
            {
                pets = pets
                    .Where(x => x.Breed.Contains(petReport.PetReportFilter.Breed))
                    .ToList();
            }

            if (petReport.PetReportFilter.Sex.HasValue)
            {
                pets = pets
                    .Where(x => x.Sex == petReport.PetReportFilter.Sex.Value)
                    .ToList();
            }

            if (petReport.PetReportFilter.BirthDate.HasValue)
            {
                pets = pets
                    .Where(x => x.BirthDate.Date == petReport.PetReportFilter.BirthDate.Value.Date)
                    .ToList();
            }

            petReport.PetReportRows = new List<PetReportRowVM>();

            pets = pets
                .OrderBy(x => x.PetName)
                .ThenBy(x => x.Species)
                .ToList();

            foreach (PetModel pet in pets)
            {
                PetReportRowVM petRow = new PetReportRowVM();

                petRow.PetId = pet.PetId;

                petRow.PetNameDisplay = pet.PetName;

                petRow.VetId = pet.VetId;

                petRow.VeterinarianNameDisplay = pet.Veterinarian != null
                    ? "Dr. " + pet.Veterinarian.FirstName + " " + pet.Veterinarian.LastName + ", " + pet.Veterinarian.Credentials
                    : "No Veterinarian";

                petRow.CustomerNameDisplay = string.Join(
                    ", ",
                    pet.CustomerPets
                    .OrderBy(x => x.Customer.LastName)
                    .ThenBy(x => x.Customer.FirstName)
                    .Select(x => x.Customer.FirstName + " " 
                    + x.Customer.LastName
                    + " (" + GetEnumDisplayName(x.RelationshipType) + ")")
                );

                if (string.IsNullOrWhiteSpace(petRow.CustomerNameDisplay)) 
                {
                    petRow.CustomerNameDisplay = "No Customer";
                }

                petRow.SpeciesDisplay = GetEnumDisplayName(pet.Species);

                petRow.BreedDisplay = pet.Breed;

                petRow.SexDisplay = GetEnumDisplayName(pet.Sex);

                petRow.BirthDateDisplay = pet.BirthDate.ToString("MM/dd/yyyy");

                int age = DateTime.Today.Year - pet.BirthDate.Year;

                if (pet.BirthDate.Date > DateTime.Today.AddYears(-age))
                { 
                    age--; 
                }

                petRow.AgeDisplay = age + " years";

                if (petReport.PetReportFilter.WeightUnit == WeightUnitEnum.Kilograms)
                {
                    decimal kilograms = pet.Weight * 0.45359237m;

                    petRow.WeightDisplay = kilograms.ToString("0.00") + " kg";
                }
                else
                {
                    petRow.WeightDisplay = pet.Weight.ToString("0.00") + " Lbs";
                }

                petRow.ActiveStatusDisplay = pet.IsActive 
                    ? "Active"
                    :"Inactive";

                petRow.NotesDisplay = string.IsNullOrWhiteSpace(pet.Notes)
                    ? "None"
                    : pet.Notes;

                petReport.PetReportRows.Add(petRow);

            }

            petReport.TotalCount = pets.Count;

            petReport.ActiveCount = pets.Count(x => x.IsActive);

            petReport.InactiveCount = pets.Count(x => !x.IsActive);

            return View(petReport);

        }


        // GET: Reports/PetCareReport
        public ActionResult PetCareReport()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetCareReportVM petCareReport = new PetCareReportVM();

            petCareReport.PetCareReportFilter = new PetCareReportFilterVM();

            petCareReport.PetCareReportRows = new List<PetCareReportRowVM>();

            petCareReport.PetSelectList = BuildPetSelectList(dbContext);

            petCareReport.CustomerSelectList = BuildCustomerSelectList(dbContext);

            return View(petCareReport);
        }


        // POST: Reports/PetCareReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PetCareReport(PetCareReportVM petCareReport)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            petCareReport.PetSelectList = BuildPetSelectList(dbContext, petCareReport.PetCareReportFilter.PetId);

            petCareReport.CustomerSelectList = BuildCustomerSelectList(dbContext, petCareReport.PetCareReportFilter.CustomerId);

            if (!ModelState.IsValid)
            {
                return View(petCareReport);
            }

            List<PetModel> pets = dbContext.Pets
                .Include(x => x.CustomerPets.Select(w => w.Customer))
                .Include(x => x.Diets)
                .Include(x => x.Medications)
                .ToList();

            if (petCareReport.PetCareReportFilter.PetId.HasValue)
            {
                pets = pets
                    .Where(x => x.PetId == petCareReport.PetCareReportFilter.PetId.Value)
                    .ToList();

            }

            if (petCareReport.PetCareReportFilter.CustomerId.HasValue)
            {
                pets = pets
                    .Where(x => x.CustomerPets.Any(
                        w => w.CustomerId == petCareReport.PetCareReportFilter.CustomerId.Value))
                    .ToList();

            }

            if (!string.IsNullOrWhiteSpace(petCareReport.PetCareReportFilter.PetName))
            {
                pets = pets
                    .Where(x => x.PetName.Contains(petCareReport.PetCareReportFilter.PetName))
                    .ToList();
            }

            if (petCareReport.PetCareReportFilter.Species.HasValue)
            {
                pets = pets
                    .Where(x => x.Species == petCareReport.PetCareReportFilter.Species.Value)
                    .ToList();
            }

            if (petCareReport.PetCareReportFilter.HasDiet.HasValue)
            {
                if (petCareReport.PetCareReportFilter.HasDiet.Value)
                {
                    pets = pets
                    .Where(x => x.Diets.Any())
                    .ToList();
                }
                else
                {
                    pets = pets
                    .Where(x => !x.Diets.Any())
                    .ToList();
                }
            }

            if (petCareReport.PetCareReportFilter.HasMedication.HasValue)
            {
                if (petCareReport.PetCareReportFilter.HasMedication.Value)
                {
                    pets = pets
                    .Where(x => x.Medications.Any())
                    .ToList();
                }
                else
                {
                    pets = pets
                    .Where(x => !x.Medications.Any())
                    .ToList();
                }
            }

            petCareReport.PetCareReportRows = new List<PetCareReportRowVM>();

            pets = pets
                .OrderBy(x => x.PetName)
                .ThenBy(x => x.Species)
                .ToList();

            foreach (PetModel pet in pets)
            {
                PetCareReportRowVM petCareRow = new PetCareReportRowVM();

                petCareRow.PetId = pet.PetId;

                petCareRow.PetNameDisplay = pet.PetName;


                petCareRow.CustomerNameDisplay = string.Join(
                    ", ",
                    pet.CustomerPets
                    .OrderBy(x => x.Customer.LastName)
                    .ThenBy(x => x.Customer.FirstName)
                    .Select(x => x.Customer.FirstName + " "
                    + x.Customer.LastName
                    + " (" + GetEnumDisplayName(x.RelationshipType) + ")")
                );

                if (string.IsNullOrWhiteSpace(petCareRow.CustomerNameDisplay))
                {
                    petCareRow.CustomerNameDisplay = "No Customer";
                }

                petCareRow.SpeciesDisplay = GetEnumDisplayName(pet.Species);

                petCareRow.DietNameDisplay = pet.Diets.Any()
                    ? string.Join(
                        ", ",
                        pet.Diets
                            .OrderBy(x => x.FoodName)
                            .Select(x => x.FoodName))
                    : "None";

                petCareRow.FeedingAmountDisplay = pet.Diets.Any()
                    ? string.Join(
                        ", ",
                        pet.Diets
                            .OrderBy(x => x.FoodName)
                            .Select(x => x.Amount))
                    : "None";

                petCareRow.FeedingFrequencyDisplay = pet.Diets.Any()
                    ? string.Join(
                        ", ",
                        pet.Diets
                            .OrderBy(x => x.FoodName)
                            .Select(x => GetEnumDisplayName(x.Frequency)))
                    : "None";

                petCareRow.DietNotesDisplay = pet.Diets.Any()
                    ? string.Join(
                        ", ",
                        pet.Diets
                            .OrderBy(x => x.FoodName)
                            .Select(x => string.IsNullOrWhiteSpace(x.Notes)
                                ? "No Notes" :
                                x.Notes))
                    : "None";

                petCareRow.MedicationNameDisplay = pet.Medications.Any()
                    ? string.Join(
                        ", ",
                        pet.Medications
                            .OrderBy(x => x.MedicationName)
                            .Select(x => x.MedicationName))
                    : "None";

                petCareRow.DosageDisplay = pet.Medications.Any()
                    ? string.Join(
                        ", ",
                        pet.Medications
                            .OrderBy(x => x.MedicationName)
                            .Select(x => x.Dosage))
                    : "None";

                petCareRow.MedicationRouteDisplay = pet.Medications.Any()
                    ? string.Join(
                        ", ",
                        pet.Medications
                            .OrderBy(x => x.MedicationName)
                            .Select(x => GetEnumDisplayName(x.Route)))
                    : "None";

                petCareRow.MedicationFrequencyDisplay = pet.Medications.Any()
                    ? string.Join(
                        ", ",
                        pet.Medications
                            .OrderBy(x => x.MedicationName)
                            .Select(x => GetEnumDisplayName(x.Frequency)))
                    : "None";

                petCareRow.MedicationStartDateDisplay = pet.Medications.Any()
                    ? string.Join(
                        ", ",
                        pet.Medications
                            .OrderBy(x => x.MedicationName)
                            .Select(x => x.StartDate.ToString("MM/dd/yyyy")))
                    : "None";

                petCareRow.MedicationEndDateDisplay = pet.Medications.Any()
                    ? string.Join(
                        ", ",
                        pet.Medications
                            .OrderBy(x => x.MedicationName)
                            .Select(x => x.EndDate.HasValue 
                                ? x.EndDate.Value.ToString("MM/dd/yyyy")
                                : "Ongoing"))
                    : "None";

                petCareRow.MedicationNotesDisplay = pet.Medications.Any()
                    ? string.Join(
                        ", ",
                        pet.Medications
                            .OrderBy(x => x.MedicationName)
                            .Select(x => string.IsNullOrWhiteSpace(x.Notes)
                                ? "No Notes" :
                                x.Notes))
                    : "None";

                petCareReport.PetCareReportRows.Add(petCareRow);

            }

            petCareReport.TotalCount = pets.Count;

            petCareReport.DietCount = pets.Sum(x => x.Diets.Count);

            petCareReport.MedicationCount = pets.Sum(x => x.Medications.Count);

            return View(petCareReport);

        }


        // GET: Reports/SpeciesReport
        public ActionResult SpeciesReport() 
        { 

            SpeciesReportVM speciesReport = new SpeciesReportVM();

            speciesReport.SpeciesReportFilter = new SpeciesReportFilterVM();

            speciesReport.SpeciesReportRows = new List<SpeciesReportRowVM>();

            return View(speciesReport);        
        
        }


        // POST: Reports/SpeciesReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SpeciesReport(SpeciesReportVM speciesReport) 
        { 
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (!ModelState.IsValid) 
            { 
                return View(speciesReport); 
            }

            List<PetModel> pets = dbContext.Pets
                .Include(x => x.Boardings)
                .ToList();

            if (speciesReport.SpeciesReportFilter.Species.HasValue)
            {
                pets = pets
                    .Where(x => x.Species == speciesReport.SpeciesReportFilter.Species.Value)
                    .ToList();
            }

            if (speciesReport.SpeciesReportFilter.ActiveStatus.HasValue)
            {
                if (speciesReport.SpeciesReportFilter.ActiveStatus.Value == ActiveStatusEnum.Active)
                {
                    pets = pets
                        .Where(x => x.IsActive)
                        .ToList();
                }

                if (speciesReport.SpeciesReportFilter.ActiveStatus.Value == ActiveStatusEnum.Inactive)
                {
                    pets = pets
                        .Where(x => !x.IsActive)
                        .ToList();
                }
            }

            speciesReport.SpeciesReportRows = new List<SpeciesReportRowVM>();

            List<SpeciesEnum> speciesList = pets
                .Select(x => x.Species)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            foreach (SpeciesEnum species in speciesList)
            {
                List<PetModel> speciesPet = pets
                    .Where(x => x.Species == species)
                    .ToList();


                SpeciesReportRowVM speciesRow = new SpeciesReportRowVM();

                speciesRow.Species = species;

                speciesRow.SpeciesDisplay = GetEnumDisplayName(species);

                speciesRow.TotalPetCount = speciesPet.Count;

                speciesRow.ActivePetCount = speciesPet.Count(x => x.IsActive);

                speciesRow.InactivePetCount = speciesPet.Count(x => !x.IsActive);

                speciesRow.CurrentBoarderCount = speciesPet
                    .Count(x => x.Boardings
                    .Any(w => w.Status == BoardingStatusEnum.CheckedIn));

                speciesRow.TotalBoardingCount = speciesPet
                    .SelectMany(x => x.Boardings)
                    .Count(x => x.Status == BoardingStatusEnum.CheckedIn || x.Status == BoardingStatusEnum.CheckedOut);

                speciesReport.SpeciesReportRows.Add(speciesRow);

            }

            speciesReport.TotalPetCount = pets.Count;

            speciesReport.ActivePetCount = pets.Count(x => x.IsActive);

            speciesReport.InactivePetCount = pets.Count(x => !x.IsActive);

            speciesReport.CurrentBoarderCount = pets
                .Count(x => x.Boardings
                .Any(w => w.Status == BoardingStatusEnum.CheckedIn));

            return View(speciesReport);

        }


        // GET: Reports/PaymentReport
        public ActionResult PaymentReport() 
        { 
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewFinancialReports(currentEmployee))
            {
                return RedirectToAction("Index", "Reports");
            }

            PaymentReportVM paymentReport = new PaymentReportVM();

            paymentReport.PaymentReportFilter = new PaymentReportFilterVM();

            paymentReport.PaymentReportRows = new List<PaymentReportRowVM>();

            paymentReport.PaymentMethodSummaryRows = new List<PaymentMethodSummaryRowVM>();

            paymentReport.CustomerSelectList = BuildCustomerSelectList(dbContext);

            paymentReport.PetSelectList = BuildPetSelectList(dbContext);

            paymentReport.EmployeeSelectList = BuildEmployeeSelectList(dbContext);

            return View(paymentReport);
        
        }


        // POST: Reports/PaymentReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentReport(PaymentReportVM paymentReport) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewFinancialReports(currentEmployee))
            {
                return RedirectToAction("Index", "Reports");
            }

            paymentReport.CustomerSelectList = BuildCustomerSelectList(
                dbContext,
                paymentReport.PaymentReportFilter.CustomerId);

            paymentReport.PetSelectList = BuildPetSelectList(
                dbContext,
                paymentReport.PaymentReportFilter.PetId);

            paymentReport.EmployeeSelectList = BuildEmployeeSelectList(
                dbContext, 
                paymentReport.PaymentReportFilter.ProcessedByEmployeeId);

            if (paymentReport.PaymentReportFilter.PaymentStartDate.HasValue &&
                  paymentReport.PaymentReportFilter.PaymentEndDate.HasValue &&
                  paymentReport.PaymentReportFilter.PaymentEndDate.Value.Date <
                  paymentReport.PaymentReportFilter.PaymentStartDate.Value.Date)
            {
                ModelState.AddModelError(
                    "PaymentReportFilter.PaymentEndDate",
                    "Payment end date cannot be before payment start date."
                );

                return View(paymentReport);

            }

            if (!ModelState.IsValid) 
            { 
                return View(paymentReport); 
            }

            List<PaymentModel> allPayments = dbContext.Payments
                .Include(x => x.Invoice)
                .Include(x => x.Invoice.Customer)
                .Include (x => x.Invoice.Pet)
                .Include(x => x.ProcessedByEmployee)
                .ToList();

            if (paymentReport.PaymentReportFilter.PaymentStartDate.HasValue)
            {
                allPayments = allPayments
                    .Where(x => x.PaymentDateTime.Date >= paymentReport.PaymentReportFilter.PaymentStartDate.Value.Date)
                    .ToList();
            }

            if (paymentReport.PaymentReportFilter.PaymentEndDate.HasValue)
            {
                allPayments = allPayments
                    .Where(x => x.PaymentDateTime.Date <= paymentReport.PaymentReportFilter.PaymentEndDate.Value.Date)
                    .ToList();
            }

            if (paymentReport.PaymentReportFilter.PaymentMethod.HasValue)
            {
                allPayments = allPayments
                    .Where(x => x.PaymentMethod == paymentReport.PaymentReportFilter.PaymentMethod.Value)
                    .ToList();
            }

            if (paymentReport.PaymentReportFilter.InvoiceType.HasValue)
            {
                allPayments = allPayments
                .Where(x => x.Invoice.InvoiceType == paymentReport.PaymentReportFilter.InvoiceType.Value)
                .ToList();
            }

            if (paymentReport.PaymentReportFilter.CustomerId.HasValue)
            {
                allPayments = allPayments
                    .Where(x => x.Invoice.CustomerId == paymentReport.PaymentReportFilter.CustomerId.Value)
                    .ToList();
            }

            if (paymentReport.PaymentReportFilter.PetId.HasValue)
            {
                allPayments = allPayments
                    .Where(x => x.Invoice.PetId == paymentReport.PaymentReportFilter.PetId.Value)
                    .ToList();
            }

            if (paymentReport.PaymentReportFilter.ProcessedByEmployeeId.HasValue)
            {
                allPayments = allPayments
                    .Where(x => x.ProcessedByEmployeeId == paymentReport.PaymentReportFilter.ProcessedByEmployeeId.Value)
                    .ToList();
            }

            List<PaymentModel> payments = allPayments
                .Where(x => !x.IsVoided)
                .ToList();

            List<PaymentMethodEnum> paymentMethodList = allPayments
                .Select(x => x.PaymentMethod)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            paymentReport.PaymentReportRows = new List<PaymentReportRowVM>();

            paymentReport.PaymentMethodSummaryRows = new List<PaymentMethodSummaryRowVM>();


            foreach (PaymentModel payment in payments)
            {

                PaymentReportRowVM paymentReportRow = new PaymentReportRowVM();

                paymentReportRow.PaymentId = payment.PaymentId;

                paymentReportRow.InvoiceId = payment.InvoiceId;

                paymentReportRow.InvoiceTypeDisplay = GetEnumDisplayName(payment.Invoice.InvoiceType);

                paymentReportRow.CustomerId = payment.Invoice.CustomerId;

                paymentReportRow.CustomerNameDisplay = payment.Invoice.Customer.LastName + ", " + payment.Invoice.Customer.FirstName;

                paymentReportRow.PetId = payment.Invoice.PetId;

                paymentReportRow.PetNameDisplay = payment.Invoice.Pet.PetName;

                paymentReportRow.PaymentDateDisplay = payment.PaymentDateTime.ToString("MM/dd/yyyy");

                paymentReportRow.AmountPaidDisplay = payment.Amount.ToString("C");

                paymentReportRow.PaymentMethodDisplay = GetEnumDisplayName(payment.PaymentMethod);

                paymentReportRow.TransactionReferenceDisplay = payment.TransactionReference;

                paymentReportRow.ProcessedByEmployeeId = payment.ProcessedByEmployeeId;

                paymentReportRow.ProcessedByEmployeeNameDisplay = payment.ProcessedByEmployee.FirstName + " " + payment.ProcessedByEmployee.LastName;

                paymentReport.PaymentReportRows.Add(paymentReportRow);

            }

            foreach (PaymentMethodEnum paymentMethodType in paymentMethodList)
            {

                List<PaymentModel> paymentsByMethod = allPayments
                    .Where(x => x.PaymentMethod == paymentMethodType)
                    .ToList();

                List<PaymentModel> nonVoidedPaymentsByMethod = paymentsByMethod
                    .Where(x => !x.IsVoided)
                    .ToList();

                PaymentMethodSummaryRowVM paymentMethodReportRow = new PaymentMethodSummaryRowVM();

                paymentMethodReportRow.PaymentMethod = paymentMethodType;

                paymentMethodReportRow.PaymentMethodDisplay = GetEnumDisplayName(paymentMethodType);

                paymentMethodReportRow.PaymentCount = nonVoidedPaymentsByMethod.Count;

                decimal totalAmountForMethod = paymentsByMethod
                    .Sum(x => x.Amount);

                paymentMethodReportRow.TotalPaymentAmountDisplay = totalAmountForMethod.ToString("C");

                paymentMethodReportRow.VoidedPaymentCount = paymentsByMethod
                    .Count(x => x.IsVoided);

                decimal voidedAmountForMethod = paymentsByMethod
                    .Where(x => x.IsVoided)
                    .Sum(x => x.Amount);

                paymentMethodReportRow.VoidedPaymentAmountDisplay = voidedAmountForMethod.ToString("C");

                decimal netAmountForMethod = totalAmountForMethod - voidedAmountForMethod;

                paymentMethodReportRow.NetPaymentAmountDisplay = netAmountForMethod.ToString("C");

                paymentReport.PaymentMethodSummaryRows.Add(paymentMethodReportRow);

            }

            paymentReport.PaymentCount = payments.Count;

            decimal totalAmountPaid = payments.Sum(x => x.Amount);

            paymentReport.TotalAmountPaidDisplay = totalAmountPaid.ToString("C");

            decimal averagePaymentAmount = payments.Any() 
                ? payments.Average(x => x.Amount) 
                : 0;

            paymentReport.AveragePaymentAmountDisplay = averagePaymentAmount.ToString("C");

            paymentReport.HasSearched = true;

            return View(paymentReport);

        }


        // GET: Reports/RevenueReport
        public ActionResult RevenueReport() 
        { 
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewFinancialReports(currentEmployee))
            { 
                return RedirectToAction("Index", "Reports"); 
            }

            RevenueReportVM revenueReport = new RevenueReportVM();

            revenueReport.RevenueReportFilter = new RevenueReportFilterVM();

            revenueReport.RevenueReportRows = new List<RevenueReportRowVM>();

            revenueReport.RevenueReportFilter.InvoiceStartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            revenueReport.RevenueReportFilter.InvoiceEndDate = DateTime.Today;

            return View(revenueReport);
        
        }


        // POST: Reports/RevenueReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RevenueReport(RevenueReportVM revenueReport) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewFinancialReports(currentEmployee))
            {
                return RedirectToAction("Index", "Reports");
            }

            if (revenueReport.RevenueReportFilter.InvoiceEndDate < revenueReport.RevenueReportFilter.InvoiceStartDate)
            {
                ModelState.AddModelError(
                    "RevenueReportFilter.InvoiceEndDate",
                    "Invoice end date cannot be before invoice start date.");        
            }

            if (!ModelState.IsValid)
            {
                revenueReport.RevenueReportRows = new List<RevenueReportRowVM>();

                return View(revenueReport);
            }

            revenueReport.HasSearched = true;

            DateTime startDate = revenueReport.RevenueReportFilter.InvoiceStartDate.Date;

            DateTime endDate = revenueReport.RevenueReportFilter.InvoiceEndDate.Date.AddDays(1);

            List<InvoiceModel> invoices = dbContext.Invoices
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.Payments)
                .ToList();

            invoices = invoices
                .Where(x => x.InvoiceDateTime >= startDate && x.InvoiceDateTime < endDate)
                .ToList();

            if (revenueReport.RevenueReportFilter.InvoiceType.HasValue)
            {
                invoices = invoices
                .Where(x => x.InvoiceType == revenueReport.RevenueReportFilter.InvoiceType.Value)
                .ToList();
            }

            if (revenueReport.RevenueReportFilter.InvoiceStatus.HasValue)
            {
                invoices = invoices
                .Where(x => x.InvoiceStatus == revenueReport.RevenueReportFilter.InvoiceStatus.Value)
                .ToList();
            }

            List<InvoiceModel> voidedInvoices = invoices
                .Where(x => x.InvoiceStatus == InvoiceStatusEnum.Void)
                .ToList();

            List<InvoiceModel> validInvoices = invoices
                .Where(x => x.InvoiceStatus != InvoiceStatusEnum.Void)
                .ToList();

            revenueReport.RevenueReportRows = new List<RevenueReportRowVM>();

            foreach (InvoiceModel invoice in invoices)
            { 
                RevenueReportRowVM revenueReportRow = new RevenueReportRowVM(); 

                revenueReportRow.InvoiceId = invoice.InvoiceId;

                revenueReportRow.InvoiceTypeDisplay = GetEnumDisplayName(invoice.InvoiceType);

                revenueReportRow.InvoiceDateTimeDisplay = invoice.InvoiceDateTime.ToString("MM/dd/yyyy");

                revenueReportRow.CustomerId = invoice.CustomerId;

                revenueReportRow.CustomerNameDisplay = invoice.Customer.LastName + ", " + invoice.Customer.FirstName;

                revenueReportRow.PetId = invoice.PetId;

                revenueReportRow.PetNameDisplay = invoice.Pet.PetName;

                revenueReportRow.StatusDisplay = GetEnumDisplayName(invoice.InvoiceStatus);

                decimal actualAmountPaid = invoice.Payments
                    .Where(x => !x.IsVoided)
                    .Sum(x => (decimal?)x.Amount) ?? 0;

                if (invoice.InvoiceStatus == InvoiceStatusEnum.Void)
                {

                    revenueReportRow.SubtotalDisplay = invoice.Subtotal.ToString("C");

                    revenueReportRow.TaxAmountDisplay = invoice.TaxAmount.ToString("C");

                    revenueReportRow.DiscountAmountDisplay = invoice.DiscountAmount.ToString("C");

                    revenueReportRow.TotalAmountDisplay = invoice.TotalAmount.ToString("C");

                    revenueReportRow.AmountPaidDisplay = "$0.00";

                    revenueReportRow.BalanceDisplay = "$0.00";

                }
                else 
                {

                    decimal actualBalance = invoice.TotalAmount - actualAmountPaid;

                    revenueReportRow.SubtotalDisplay = invoice.Subtotal.ToString("C");

                    revenueReportRow.TaxAmountDisplay = invoice.TaxAmount.ToString("C");

                    revenueReportRow.DiscountAmountDisplay = invoice.DiscountAmount.ToString("C");

                    revenueReportRow.TotalAmountDisplay = invoice.TotalAmount.ToString("C");

                    revenueReportRow.AmountPaidDisplay = actualAmountPaid.ToString("C");

                    revenueReportRow.BalanceDisplay = actualBalance.ToString("C");

                }

                revenueReport.RevenueReportRows.Add(revenueReportRow);

            }

            revenueReport.TotalInvoiceCount = validInvoices.Count;

            decimal invoicedTotal = validInvoices.Sum(x => x.TotalAmount);

            revenueReport.TotalInvoicedDisplay = invoicedTotal.ToString("C");

            decimal receivedTotal = validInvoices
                .SelectMany(x => x.Payments)
                .Where(x => !x.IsVoided)
                .Sum(x => (decimal?)x.Amount) ?? 0;

            revenueReport.TotalReceivedDisplay = receivedTotal.ToString("C");


            decimal outstandingTotal = invoicedTotal - receivedTotal;

            revenueReport.TotalOutstandingDisplay = outstandingTotal.ToString("C");

            if (validInvoices.Count > 0)
            {
                revenueReport.AverageInvoiceValueDisplay = (invoicedTotal / validInvoices.Count).ToString("C");
            }
            else
            { 
                revenueReport.AverageInvoiceValueDisplay = "$0.00"; 
            }

            int paidInvoiceCount = validInvoices
                .Count(x => x.Payments.Any(w => !w.IsVoided));

            if (paidInvoiceCount > 0)
            { 
                revenueReport.AverageAmountReceivedDisplay = (receivedTotal / paidInvoiceCount).ToString("C"); 
            }
            else 
            { 
                revenueReport.AverageAmountReceivedDisplay = "$0.00"; 
            }

            revenueReport.TotalVoidedInvoiceCount = voidedInvoices.Count;

            decimal voidedTotal = voidedInvoices
                .Sum(x => x.TotalAmount);

            revenueReport.TotalVoidedAmountDisplay = voidedTotal.ToString("C");

            return View(revenueReport);

        }


        // GET: Reports/VoidedTransactionsReport
        public ActionResult VoidedTransactionsReport() 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewFinancialReports(currentEmployee))
            { 
                return RedirectToAction("Index", "Reports"); 
            }

            VoidedTransactionsReportVM voidedTransactionsReport = new VoidedTransactionsReportVM();

            voidedTransactionsReport.VoidedTransactionsReportFilter = new VoidedTransactionsReportFilterVM();

            voidedTransactionsReport.VoidedTransactionsReportRows = new List<VoidedTransactionsReportRowVM>();

            voidedTransactionsReport.VoidedTransactionsReportFilter.VoidedStartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            voidedTransactionsReport.VoidedTransactionsReportFilter.VoidedEndDate = DateTime.Today;

            voidedTransactionsReport.EmployeeSelectList = BuildEmployeeSelectList(dbContext);

            return View(voidedTransactionsReport);

        }



        // POST: Reports/VoidedTransactionsReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VoidedTransactionsReport(VoidedTransactionsReportVM voidedTransactionsReport) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewFinancialReports(currentEmployee))
            {
                return RedirectToAction("Index", "Reports");
            }

            voidedTransactionsReport.EmployeeSelectList = BuildEmployeeSelectList(dbContext);

            if (voidedTransactionsReport.VoidedTransactionsReportFilter.VoidedEndDate < voidedTransactionsReport.VoidedTransactionsReportFilter.VoidedStartDate)
            {
                ModelState.AddModelError(
                    "VoidedTransactionsReportFilter.VoidedEndDate",
                    "Voided end date cannot be before voided start date.");
            }

            if (!ModelState.IsValid)
            {
                voidedTransactionsReport.VoidedTransactionsReportRows = new List<VoidedTransactionsReportRowVM>();

                return View(voidedTransactionsReport);
            }

            voidedTransactionsReport.HasSearched = true;

            DateTime startDate = voidedTransactionsReport.VoidedTransactionsReportFilter.VoidedStartDate.Date;

            DateTime endDate = voidedTransactionsReport.VoidedTransactionsReportFilter.VoidedEndDate.Date.AddDays(1);

            List<InvoiceModel> voidedInvoices = dbContext.Invoices
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.VoidedByEmployee)
                .Where(x => x.InvoiceStatus == InvoiceStatusEnum.Void)
                .Where(x => x.VoidDateTime >= startDate && x.VoidDateTime < endDate)
                .ToList();

            List<PaymentModel> voidedPayments = dbContext.Payments
                .Include(x => x.Invoice)
                .Include(x => x.Invoice.Customer)
                .Include(x => x.Invoice.Pet)
                .Include(x => x.ProcessedByEmployee)
                .Include(x => x.VoidedByEmployee)
                .Where(x => x.IsVoided)
                .Where(x => x.VoidedDateTime >= startDate && x.VoidedDateTime < endDate)
                .ToList();

            if (voidedTransactionsReport.VoidedTransactionsReportFilter.InvoiceType.HasValue)
            {
                voidedInvoices = voidedInvoices
                    .Where(x => x.InvoiceType == voidedTransactionsReport.VoidedTransactionsReportFilter.InvoiceType.Value)
                    .ToList();

                voidedPayments = voidedPayments
                    .Where(x => x.Invoice.InvoiceType == voidedTransactionsReport.VoidedTransactionsReportFilter.InvoiceType.Value)
                    .ToList();
            }

            if (voidedTransactionsReport.VoidedTransactionsReportFilter.PaymentMethod.HasValue) 
            {
                voidedInvoices = new List<InvoiceModel>();

                voidedPayments = voidedPayments
                    .Where(x => x.PaymentMethod == voidedTransactionsReport.VoidedTransactionsReportFilter.PaymentMethod.Value)
                    .ToList();
            }

            if (voidedTransactionsReport.VoidedTransactionsReportFilter.VoidedByEmployeeId.HasValue)
            {
                voidedInvoices = voidedInvoices
                    .Where(x => x.VoidedByEmployeeId == voidedTransactionsReport.VoidedTransactionsReportFilter.VoidedByEmployeeId.Value)
                    .ToList();

                voidedPayments = voidedPayments
                    .Where(x => x.VoidedByEmployeeId == voidedTransactionsReport.VoidedTransactionsReportFilter.VoidedByEmployeeId.Value)
                    .ToList();
            }

            voidedTransactionsReport.VoidedTransactionsReportRows = new List<VoidedTransactionsReportRowVM>();

            foreach (InvoiceModel invoice in voidedInvoices)
            { 
                VoidedTransactionsReportRowVM voidedTransactionsRow = new VoidedTransactionsReportRowVM();

                voidedTransactionsRow.TransactionTypeDisplay = "Invoice";

                voidedTransactionsRow.PaymentId = null;

                voidedTransactionsRow.InvoiceId = invoice.InvoiceId;

                voidedTransactionsRow.InvoiceTypeDisplay = GetEnumDisplayName(invoice.InvoiceType);

                voidedTransactionsRow.CustomerId = invoice.CustomerId;

                voidedTransactionsRow.CustomerNameDisplay = invoice.Customer.LastName + ", " + invoice.Customer.FirstName;

                voidedTransactionsRow.PetId = invoice.PetId;

                voidedTransactionsRow.PetNameDisplay = invoice.Pet.PetName;

                voidedTransactionsRow.OriginalTransactionDateDisplay = invoice.InvoiceDateTime.ToString("MM/dd/yyyy");

                voidedTransactionsRow.AmountVoidedDisplay = invoice.TotalAmount.ToString("C");

                voidedTransactionsRow.PaymentMethodDisplay = "N/A";

                voidedTransactionsRow.TransactionReferenceDisplay = "N/A";

                voidedTransactionsRow.ProcessedByEmployeeId = null;

                voidedTransactionsRow.ProcessedByEmployeeNameDisplay = "N/A";

                voidedTransactionsRow.VoidedDate = invoice.VoidDateTime;

                voidedTransactionsRow.VoidedDateDisplay = invoice.VoidDateTime.HasValue 
                    ? invoice.VoidDateTime.Value.ToString("MM/dd/yyyy")
                    : "N/A";

                voidedTransactionsRow.VoidedByEmployeeId = invoice.VoidedByEmployeeId;

                voidedTransactionsRow.VoidedByEmployeeNameDisplay = invoice.VoidedByEmployee != null
                    ? invoice.VoidedByEmployee.FirstName + " " + invoice.VoidedByEmployee.LastName
                    : "N/A";

                voidedTransactionsRow.VoidedReasonDisplay = invoice.VoidReason.HasValue 
                    ? GetEnumDisplayName(invoice.VoidReason.Value)
                    : "N/A";

                voidedTransactionsReport.VoidedTransactionsReportRows.Add(voidedTransactionsRow);

            }

            foreach (PaymentModel payment in voidedPayments)
            {
                VoidedTransactionsReportRowVM voidedTransactionsRow = new VoidedTransactionsReportRowVM();

                voidedTransactionsRow.TransactionTypeDisplay = "Payment";

                voidedTransactionsRow.PaymentId = payment.PaymentId;

                voidedTransactionsRow.InvoiceId = payment.InvoiceId;

                voidedTransactionsRow.InvoiceTypeDisplay = GetEnumDisplayName(payment.Invoice.InvoiceType);

                voidedTransactionsRow.CustomerId = payment.Invoice.CustomerId;

                voidedTransactionsRow.CustomerNameDisplay = payment.Invoice.Customer.LastName + ", " + payment.Invoice.Customer.FirstName;

                voidedTransactionsRow.PetId = payment.Invoice.PetId;

                voidedTransactionsRow.PetNameDisplay = payment.Invoice.Pet.PetName;

                voidedTransactionsRow.OriginalTransactionDateDisplay = payment.PaymentDateTime.ToString("MM/dd/yyyy");

                voidedTransactionsRow.AmountVoidedDisplay = payment.Amount.ToString("C");

                voidedTransactionsRow.PaymentMethodDisplay = GetEnumDisplayName(payment.PaymentMethod);

                voidedTransactionsRow.TransactionReferenceDisplay = payment.TransactionReference;

                voidedTransactionsRow.ProcessedByEmployeeId = payment.ProcessedByEmployeeId;

                voidedTransactionsRow.ProcessedByEmployeeNameDisplay = payment.ProcessedByEmployee.FirstName + " " + payment.ProcessedByEmployee.LastName;

                voidedTransactionsRow.VoidedDate = payment.VoidedDateTime;

                voidedTransactionsRow.VoidedDateDisplay = payment.VoidedDateTime.HasValue 
                    ? payment.VoidedDateTime.Value.ToString("MM/dd/yyyy") 
                    : "N/A";

                voidedTransactionsRow.VoidedByEmployeeId = payment.VoidedByEmployeeId;

                voidedTransactionsRow.VoidedByEmployeeNameDisplay = payment.VoidedByEmployee != null
                    ? payment.VoidedByEmployee.FirstName + " " + payment.VoidedByEmployee.LastName
                    : "N/A";

                voidedTransactionsRow.VoidedReasonDisplay = payment.VoidReason.HasValue
                    ? GetEnumDisplayName(payment.VoidReason.Value) 
                    : "N/A";

                voidedTransactionsReport.VoidedTransactionsReportRows.Add(voidedTransactionsRow);

            }

            voidedTransactionsReport.VoidedTransactionsReportRows = voidedTransactionsReport.VoidedTransactionsReportRows
                .OrderByDescending(x => x.VoidedDate)
                .ToList();

            voidedTransactionsReport.VoidedInvoiceCount = voidedInvoices.Count;

            voidedTransactionsReport.VoidedPaymentCount = voidedPayments.Count;

            decimal voidedInvoiceTotal = voidedInvoices
                .Sum(x => x.TotalAmount);

            decimal voidedPaymentTotal = voidedPayments
                .Sum(x => x.Amount);

            voidedTransactionsReport.TotalInvoiceAmountVoidedDisplay = voidedInvoiceTotal.ToString("C");

            voidedTransactionsReport.TotalPaymentAmountVoidedDisplay = voidedPaymentTotal.ToString("C");

            return View(voidedTransactionsReport); 
        
        }


        // GET: Reports/CurrentBoardersReport
        public ActionResult CurrentBoardersReport() 
        { 
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CurrentBoardersReportVM currentBoardersReport = new CurrentBoardersReportVM();

            currentBoardersReport.CurrentBoardersReportFilter = new CurrentBoardersReportFilterVM();

            currentBoardersReport.CurrentBoardersReportRows = new List<CurrentBoardersReportRowVM>();

            currentBoardersReport.CustomerSelectList = BuildCustomerSelectList(dbContext);

            currentBoardersReport.PetSelectList = BuildPetSelectList(dbContext);

            currentBoardersReport.BoardingUnitSelectList = BuildBoardingUnitSelectList(dbContext);

            return View(currentBoardersReport); 
        }


        // POST: Reports/CurrentBoardersReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CurrentBoardersReport(CurrentBoardersReportVM currentBoardersReport) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            currentBoardersReport.CustomerSelectList = BuildCustomerSelectList(dbContext);

            currentBoardersReport.PetSelectList = BuildPetSelectList(dbContext);

            currentBoardersReport.BoardingUnitSelectList = BuildBoardingUnitSelectList(dbContext);

            if (!ModelState.IsValid)
            {
                currentBoardersReport.CurrentBoardersReportRows = new List<CurrentBoardersReportRowVM>();

                return View(currentBoardersReport);
            }

            currentBoardersReport.HasSearched = true;

            List<BoardingModel> boardings = dbContext.Boardings
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.BoardingUnit)
                .Include(x => x.Pet.Diets)
                .Include(x => x.Pet.Medications)
                .Include(x => x.Pet.PetVaccines.Select(w => w.Vaccine))
                .Where(x => x.Status == BoardingStatusEnum.CheckedIn)
                .ToList();

            if (currentBoardersReport.CurrentBoardersReportFilter.CustomerId.HasValue)
            {
                boardings = boardings
                    .Where(x => x.Customer.CustomerId == currentBoardersReport.CurrentBoardersReportFilter.CustomerId.Value)
                    .ToList();

            }

            if (currentBoardersReport.CurrentBoardersReportFilter.PetId.HasValue)
            {
                boardings = boardings
                    .Where(x => x.Pet.PetId == currentBoardersReport.CurrentBoardersReportFilter.PetId.Value)
                    .ToList();

            }

            if (currentBoardersReport.CurrentBoardersReportFilter.BoardingUnitId.HasValue)
            {
                boardings = boardings
                    .Where(x => x.BoardingUnitId == currentBoardersReport.CurrentBoardersReportFilter.BoardingUnitId.Value)
                    .ToList();

            }

            if (currentBoardersReport.CurrentBoardersReportFilter.Species.HasValue)
            {
                boardings = boardings
                    .Where(x => x.Pet.Species == currentBoardersReport.CurrentBoardersReportFilter.Species.Value)
                    .ToList();
            }

            currentBoardersReport.CurrentBoardersReportRows = new List<CurrentBoardersReportRowVM>();

            List<VaccineModel> requiredVaccines = dbContext.Vaccines
                .Where(x => x.RequiredFlag)
                .ToList();

            foreach (BoardingModel boarding in boardings)
            { 
                CurrentBoardersReportRowVM boardersReportRow = new CurrentBoardersReportRowVM();

                boardersReportRow.BoardingId = boarding.BoardingId;

                boardersReportRow.PetId = boarding.PetId;

                boardersReportRow.PetNameDisplay = boarding.Pet.PetName;

                boardersReportRow.CustomerId = boarding.CustomerId;

                boardersReportRow.CustomerNameDisplay = boarding.Customer.LastName + ", " + boarding.Customer.FirstName;

                boardersReportRow.BoardingUnitId = boarding.BoardingUnitId;


                if (boarding.BoardingUnit != null)
                { 
                    boardersReportRow.BoardingUnitDisplay = boarding.BoardingUnit.UnitName + " - " + boarding.BoardingUnit.UnitNumber; 
                }
                else 
                { 
                    boardersReportRow.BoardingUnitDisplay = "No Boarding Unit"; 
                }

                boardersReportRow.SpeciesDisplay = GetEnumDisplayName(boarding.Pet.Species);

                boardersReportRow.CheckInDateTime = boarding.ActualCheckInDateTime;

                boardersReportRow.CheckInDateTimeDisplay = boarding.ActualCheckInDateTime.HasValue 
                    ? boarding.ActualCheckInDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") 
                    : "N/A";

                boardersReportRow.ScheduledCheckOutDateTimeDisplay = boarding.EndDateTime.ToString("MM/dd/yyyy hh:mm tt");

                String lengthOfStay;

                if (boarding.ActualCheckInDateTime.HasValue)
                { 
                    TimeSpan stayLength = DateTime.Now - boarding.ActualCheckInDateTime.Value;

                    lengthOfStay = stayLength.Days + " day(s), " + stayLength.Hours + " hour(s)";

                }
                else 
                { 
                    lengthOfStay = "N/A"; 
                }

                boardersReportRow.LengthOfStayDisplay = lengthOfStay;

                boardersReportRow.DietDisplay = boarding.Pet.Diets.Any()
                    ? string.Join(" / ", boarding.Pet.Diets.Select(x => x.FoodName)) 
                    : "No Diet";

                boardersReportRow.MedicationStatusDisplay = boarding.Pet.Medications.Any()
                    ? string.Join(" / ", boarding.Pet.Medications.Select(x => x.MedicationName)) 
                    : "No Medication";

                List<VaccineModel> requiredVaccinesForPet = requiredVaccines
                    .Where(x => x.Species == boarding.Pet.Species)
                    .ToList();

                bool vaccineCompliant = requiredVaccinesForPet
                    .All(requiredVaccine => boarding.Pet.PetVaccines
                    .Any(petVaccine => 
                        petVaccine.VaccineId == requiredVaccine.VaccineId && 
                        petVaccine.ExpirationDate >= DateTime.Today));
                
                boardersReportRow.VaccineComplianceDisplay = vaccineCompliant 
                    ? "Compliant"
                    : "Not Compliant";

                boardersReportRow.NotesDisplay = boarding.Notes;

                currentBoardersReport.CurrentBoardersReportRows.Add(boardersReportRow);

            }

            currentBoardersReport.CurrentBoardersReportRows = currentBoardersReport.CurrentBoardersReportRows
                .OrderBy(x => x.CheckInDateTime)
                .ToList();

            currentBoardersReport.TotalCurrentBoarderCount = boardings.Count;

            return View(currentBoardersReport); 
        
        }



        // GET: Reports/OutstandingBalanceReport
        public ActionResult OutstandingBalanceReport() 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewFinancialReports(currentEmployee))
            { 
                return RedirectToAction("Index", "Reports"); 
            }

            OutstandingBalanceReportVM outstandingBalanceReport = new OutstandingBalanceReportVM();

            outstandingBalanceReport.OutstandingBalanceReportFilter = new OutstandingBalanceReportFilterVM();

            outstandingBalanceReport.OutstandingBalanceReportRows = new List<OutstandingBalanceReportRowVM>();

            outstandingBalanceReport.CustomerSelectList = BuildCustomerSelectList(dbContext);

            outstandingBalanceReport.PetSelectList = BuildPetSelectList(dbContext);

            return View(outstandingBalanceReport); 

        }



        // POST: Reports/OutstandingBalanceReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OutstandingBalanceReport(OutstandingBalanceReportVM outstandingBalanceReport)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewFinancialReports(currentEmployee))
            {
                return RedirectToAction("Index", "Reports");
            }

            outstandingBalanceReport.CustomerSelectList = BuildCustomerSelectList(dbContext);

            outstandingBalanceReport.PetSelectList = BuildPetSelectList(dbContext);

            if (!ModelState.IsValid)
            {
                outstandingBalanceReport.OutstandingBalanceReportRows = new List<OutstandingBalanceReportRowVM>();

                return View(outstandingBalanceReport);
            }

            List<InvoiceModel> invoices = dbContext.Invoices
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.Payments)
                .Where(x => x.InvoiceStatus != InvoiceStatusEnum.Void)
                .Where(x => x.Balance > 0)
                .ToList();

            if (outstandingBalanceReport.OutstandingBalanceReportFilter.InvoiceStartDate.HasValue)
            {
                invoices = invoices
                    .Where(x => x.InvoiceDateTime.Date >= outstandingBalanceReport.OutstandingBalanceReportFilter.InvoiceStartDate.Value.Date)
                    .ToList();
            }

            if (outstandingBalanceReport.OutstandingBalanceReportFilter.InvoiceEndDate.HasValue)
            {
                invoices = invoices
                    .Where(x => x.InvoiceDateTime.Date <= outstandingBalanceReport.OutstandingBalanceReportFilter.InvoiceEndDate.Value.Date)
                    .ToList();
            }

            if (outstandingBalanceReport.OutstandingBalanceReportFilter.CustomerId.HasValue)
            {
                invoices = invoices
                    .Where(x => x.Customer.CustomerId == outstandingBalanceReport.OutstandingBalanceReportFilter.CustomerId.Value)
                    .ToList();

            }

            if (outstandingBalanceReport.OutstandingBalanceReportFilter.PetId.HasValue)
            {
                invoices = invoices
                    .Where(x => x.Pet.PetId == outstandingBalanceReport.OutstandingBalanceReportFilter.PetId.Value)
                    .ToList();

            }

            if (outstandingBalanceReport.OutstandingBalanceReportFilter.MinimumBalance.HasValue)
            {
                invoices = invoices
                    .Where(x => x.Balance >= outstandingBalanceReport.OutstandingBalanceReportFilter.MinimumBalance.Value)
                    .ToList();
            }

            if (outstandingBalanceReport.OutstandingBalanceReportFilter.InvoiceStatus.HasValue)
            {
                invoices = invoices
                .Where(x => x.InvoiceStatus == outstandingBalanceReport.OutstandingBalanceReportFilter.InvoiceStatus.Value)
                .ToList();
            }

            outstandingBalanceReport.HasSearched = true;

            outstandingBalanceReport.OutstandingBalanceReportRows = new List<OutstandingBalanceReportRowVM>();

            foreach (InvoiceModel invoice in invoices)
            { 
                OutstandingBalanceReportRowVM outstandingBalanceRow = new OutstandingBalanceReportRowVM();

                outstandingBalanceRow.InvoiceId = invoice.InvoiceId;

                outstandingBalanceRow.CustomerId = invoice.CustomerId;

                outstandingBalanceRow.CustomerNameDisplay = invoice.Customer.LastName + ", " + invoice.Customer.FirstName;

                outstandingBalanceRow.PetId = invoice.PetId;

                outstandingBalanceRow.PetNameDisplay = invoice.Pet.PetName;

                outstandingBalanceRow.InvoiceDateTimeDisplay = invoice.InvoiceDateTime.ToString("MM/dd/yyyy");

                outstandingBalanceRow.InvoiceStatusDisplay = GetEnumDisplayName(invoice.InvoiceStatus);

                outstandingBalanceRow.TotalAmountDisplay = invoice.TotalAmount.ToString("C");

                outstandingBalanceRow.AmountPaidDisplay = invoice.AmountPaid.ToString("C");

                List<DateTime> paymentDates = new List<DateTime>();

                paymentDates.AddRange(invoice.Payments
                    .Where(x => !x.IsVoided)
                    .Select(x => x.PaymentDateTime)
                );

                outstandingBalanceRow.LastPaymentDateTimeDisplay = paymentDates.Any()
                    ? paymentDates.Max().ToString("MM/dd/yyyy")
                    : "No Payments Made";

                outstandingBalanceRow.OutstandingBalanceDisplay = invoice.Balance.ToString("C");

                outstandingBalanceReport.OutstandingBalanceReportRows.Add(outstandingBalanceRow);
            }

            outstandingBalanceReport.InvoiceCount = invoices.Count;

            decimal totalBalance = invoices.Sum(x  => x.Balance);

            outstandingBalanceReport.TotalOutstandingBalanceDisplay = totalBalance.ToString("C");

            return View(outstandingBalanceReport);

        }


        // GET: Reports/InvoiceReport
        public ActionResult InvoiceReport() 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewFinancialReports(currentEmployee)) 
            { 
                return RedirectToAction("Index", "Reports"); 
            }

            InvoiceReportVM invoiceReport = new InvoiceReportVM();

            invoiceReport.InvoiceReportFilter = new InvoiceReportFilterVM();

            invoiceReport.InvoiceReportRows = new List<InvoiceReportRowVM>();

            invoiceReport.CustomerSelectList = BuildCustomerSelectList(dbContext);

            invoiceReport.PetSelectList = BuildPetSelectList(dbContext);

            return View(invoiceReport); 
        
        }


        // POST: Reports/InvoiceReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InvoiceReport(InvoiceReportVM invoiceReport) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewFinancialReports(currentEmployee))
            {
                return RedirectToAction("Index", "Reports");
            }

            invoiceReport.CustomerSelectList = BuildCustomerSelectList(dbContext);

            invoiceReport.PetSelectList = BuildPetSelectList(dbContext);

            if (!ModelState.IsValid)
            {
                invoiceReport.InvoiceReportRows = new List<InvoiceReportRowVM>();

                return View(invoiceReport);
            }

            List<InvoiceModel> invoices = dbContext.Invoices
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.Payments)
                .Include(x => x.Boarding)
                .Include(x => x.Boarding.BoardingUnit)
                .Include(x => x.InvoiceItems)
                .ToList();

            if (invoiceReport.InvoiceReportFilter.InvoiceStartDate.HasValue)
            {
                invoices = invoices
                    .Where(x => x.InvoiceDateTime.Date >= invoiceReport.InvoiceReportFilter.InvoiceStartDate.Value.Date)
                    .ToList();
            }

            if (invoiceReport.InvoiceReportFilter.InvoiceEndDate.HasValue)
            {
                invoices = invoices
                    .Where(x => x.InvoiceDateTime.Date <= invoiceReport.InvoiceReportFilter.InvoiceEndDate.Value.Date)
                    .ToList();
            }

            if (invoiceReport.InvoiceReportFilter.CustomerId.HasValue)
            {
                invoices = invoices
                    .Where(x => x.Customer.CustomerId == invoiceReport.InvoiceReportFilter.CustomerId.Value)
                    .ToList();
            }

            if (invoiceReport.InvoiceReportFilter.PetId.HasValue)
            {
                invoices = invoices
                    .Where(x => x.Pet.PetId == invoiceReport.InvoiceReportFilter.PetId.Value)
                    .ToList();
            }

            if (invoiceReport.InvoiceReportFilter.InvoiceType.HasValue)
            {
                invoices = invoices
                .Where(x => x.InvoiceType == invoiceReport.InvoiceReportFilter.InvoiceType.Value)
                .ToList();
            }

            if (invoiceReport.InvoiceReportFilter.InvoiceStatus.HasValue)
            {
                invoices = invoices
                .Where(x => x.InvoiceStatus == invoiceReport.InvoiceReportFilter.InvoiceStatus.Value)
                .ToList();
            }

            invoiceReport.HasSearched = true;

            invoiceReport.InvoiceReportRows = new List<InvoiceReportRowVM>();

            foreach (InvoiceModel invoice in invoices)
            {
                InvoiceReportRowVM invoiceReportRow = new InvoiceReportRowVM();

                invoiceReportRow.InvoiceId = invoice.InvoiceId;

                invoiceReportRow.InvoiceTypeDisplay = GetEnumDisplayName(invoice.InvoiceType);

                invoiceReportRow.InvoiceItemCount = invoice.InvoiceItems.Count;

                invoiceReportRow.CustomerId = invoice.CustomerId;

                invoiceReportRow.CustomerNameDisplay = invoice.Customer.LastName + ", " + invoice.Customer.FirstName;

                invoiceReportRow.PetId = invoice.PetId;

                invoiceReportRow.PetNameDisplay = invoice.Pet.PetName;

                invoiceReportRow.BoardingId = invoice.BoardingId;

                if (invoice.Boarding != null && invoice.Boarding.BoardingUnit != null) 
                {
                    invoiceReportRow.BoardingDisplay = invoice.Boarding.BoardingUnit.UnitName + " - "
                    + invoice.Boarding.BoardingUnit.UnitNumber;
                } 
                else 
                {
                    invoiceReportRow.BoardingDisplay = "No Boarding"; 
                }

                invoiceReportRow.InvoiceDateTimeDisplay = invoice.InvoiceDateTime.ToString("MM/dd/yyyy hh:mm tt");

                invoiceReportRow.StatusDisplay = GetEnumDisplayName(invoice.InvoiceStatus);

                invoiceReportRow.SubtotalDisplay = invoice.Subtotal.ToString("C");

                invoiceReportRow.TaxAmountDisplay = invoice.TaxAmount.ToString("C");

                invoiceReportRow.DiscountAmountDisplay = invoice.DiscountAmount.ToString("C");

                invoiceReportRow.TotalAmountDisplay = invoice.TotalAmount.ToString("C");

                invoiceReportRow.AmountPaidDisplay = invoice.AmountPaid.ToString("C");

                invoiceReportRow.BalanceDisplay = invoice.Balance.ToString("C");

                invoiceReport.InvoiceReportRows.Add(invoiceReportRow);
                                 
            }

            invoiceReport.InvoiceCount = invoices.Count;

            decimal subtotalTotal = invoices.Sum(x => x.Subtotal);

            invoiceReport.TotalSubtotalDisplay = subtotalTotal.ToString("C");

            decimal taxTotal = invoices.Sum(x => x.TaxAmount);

            invoiceReport.TotalTaxDisplay = taxTotal.ToString("C");

            decimal discountTotal = invoices.Sum(x => x.DiscountAmount);

            invoiceReport.TotalDiscountDisplay = discountTotal.ToString("C");

            decimal amountTotal = invoices.Sum(x => x.TotalAmount);

            invoiceReport.TotalAmountDisplay = amountTotal.ToString("C");

            decimal amountPaidTotal = invoices.Sum(x => x.AmountPaid);

            invoiceReport.TotalAmountPaidDisplay = amountPaidTotal.ToString("C");

            decimal balanceTotal = invoices.Sum(x => x.Balance);

            invoiceReport.TotalBalanceDisplay = balanceTotal.ToString("C");
            
            return View(invoiceReport); 
        
        }


        // GET: Reports/VaccineComplianceReport
        public ActionResult VaccineComplianceReport() 
        { 
            ApplicationDbContext dbContext = new ApplicationDbContext();

            VaccineComplianceReportVM vaccineComplianceReport = new VaccineComplianceReportVM();

            vaccineComplianceReport.VaccineComplianceReportFilter = new VaccineComplianceReportFilterVM();

            vaccineComplianceReport.VaccineComplianceReportRows = new List<VaccineComplianceReportRowVM>();

            vaccineComplianceReport.CustomerSelectList = BuildCustomerSelectList(dbContext);

            vaccineComplianceReport.PetSelectList = BuildPetSelectList(dbContext);

            return View(vaccineComplianceReport); 
        
        }


        // POST: Reports/VaccineComplianceReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VaccineComplianceReport(VaccineComplianceReportVM vaccineComplianceReport)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            vaccineComplianceReport.CustomerSelectList = BuildCustomerSelectList(dbContext);

            vaccineComplianceReport.PetSelectList = BuildPetSelectList(dbContext);

            if (!ModelState.IsValid)
            {
                vaccineComplianceReport.VaccineComplianceReportRows = new List<VaccineComplianceReportRowVM>();

                return View(vaccineComplianceReport);
            }

            List<PetModel> pets = dbContext.Pets
                .Include(x => x.CustomerPets.Select(w => w.Customer))
                .Include(x => x.PetVaccines.Select(w => w.Vaccine))
                .ToList();

            List<VaccineModel> requiredVaccines = dbContext.Vaccines
                .Where(x => x.RequiredFlag)
                .ToList();

            if (vaccineComplianceReport.VaccineComplianceReportFilter.CustomerId.HasValue)
            {
                pets = pets
                    .Where(x => x.CustomerPets
                    .Any(w => w.CustomerId == vaccineComplianceReport.VaccineComplianceReportFilter.CustomerId.Value))
                    .ToList();
            }

            if (vaccineComplianceReport.VaccineComplianceReportFilter.PetId.HasValue)
            {
                pets = pets
                    .Where(x => x.PetId == vaccineComplianceReport.VaccineComplianceReportFilter.PetId.Value)
                    .ToList();
            }

            if (vaccineComplianceReport.VaccineComplianceReportFilter.Species.HasValue)
            {
                pets = pets
                .Where(x => x.Species == vaccineComplianceReport.VaccineComplianceReportFilter.Species.Value)
                .ToList();
            }

            vaccineComplianceReport.VaccineComplianceReportRows = new List<VaccineComplianceReportRowVM>();

            foreach (PetModel pet in pets)
            {
                List<VaccineModel> vaccinesForPet = requiredVaccines
                    .Where(x => x.Species == pet.Species)
                    .ToList();

                foreach (VaccineModel vaccine in vaccinesForPet)
                {
                    VaccineComplianceReportRowVM vaccineComplianceReportRow = new VaccineComplianceReportRowVM();

                    PetVaccineModel petVaccine = pet.PetVaccines
                        .Where(x => x.VaccineId == vaccine.VaccineId)
                        .OrderByDescending(x => x.ExpirationDate)
                        .FirstOrDefault();

                    vaccineComplianceReportRow.PetVaccineId = petVaccine != null
                        ? (Guid?)petVaccine.PetVaccineId
                        : null; 

                    vaccineComplianceReportRow.VaccineId = vaccine.VaccineId;

                    vaccineComplianceReportRow.VaccineNameDisplay = vaccine.VaccineName;

                    vaccineComplianceReportRow.PetId = pet.PetId;

                    vaccineComplianceReportRow.PetNameDisplay = pet.PetName;

                    vaccineComplianceReportRow.CustomerNameDisplay = string.Join(
                        ", ",
                        pet.CustomerPets
                            .OrderBy(x => x.Customer.LastName)
                            .ThenBy(x => x.Customer.FirstName)
                            .Select(x =>
                                x.Customer.LastName + ", " +
                                x.Customer.FirstName + " (" +
                                GetEnumDisplayName(x.RelationshipType) + ")")
                    );

                    if (string.IsNullOrWhiteSpace(vaccineComplianceReportRow.CustomerNameDisplay))
                    {
                        vaccineComplianceReportRow.CustomerNameDisplay = "No Customer";
                    }

                    vaccineComplianceReportRow.SpeciesDisplay = GetEnumDisplayName(pet.Species);

                    if (petVaccine == null)
                    {
                        vaccineComplianceReportRow.DateGivenDisplay = "N/A";

                        vaccineComplianceReportRow.ExpirationDate = null;

                        vaccineComplianceReportRow.ExpirationDateDisplay = "N/A";

                        vaccineComplianceReportRow.VaccineComplianceStatus = VaccineComplianceStatusEnum.Missing;

                        vaccineComplianceReportRow.VaccineComplianceStatusDisplay = GetEnumDisplayName(vaccineComplianceReportRow.VaccineComplianceStatus);

                        vaccineComplianceReportRow.DaysUntilExpiration = null;

                        vaccineComplianceReportRow.DocumentFilePathDisplay = "N/A";

                        vaccineComplianceReportRow.NotesDisplay = "N/A";
                    }
                    else
                    {
                        vaccineComplianceReportRow.DateGivenDisplay = petVaccine.DateGiven.ToString("MM/dd/yyyy");

                        vaccineComplianceReportRow.ExpirationDate = petVaccine.ExpirationDate;

                        vaccineComplianceReportRow.ExpirationDateDisplay = petVaccine.ExpirationDate.ToString("MM/dd/yyyy");

                        int daysUntilExpiration = (petVaccine.ExpirationDate.Date - DateTime.Today).Days;

                        vaccineComplianceReportRow.DaysUntilExpiration = daysUntilExpiration;

                        if (petVaccine.ExpirationDate.Date < DateTime.Today)
                        {
                            vaccineComplianceReportRow.VaccineComplianceStatus = VaccineComplianceStatusEnum.Expired;
                        }
                        else if (petVaccine.ExpirationDate.Date == DateTime.Today)
                        {
                            vaccineComplianceReportRow.VaccineComplianceStatus = VaccineComplianceStatusEnum.ExpiringToday;
                        }
                        else if (petVaccine.ExpirationDate.Date <= DateTime.Today.AddDays(1))
                        {
                            vaccineComplianceReportRow.VaccineComplianceStatus = VaccineComplianceStatusEnum.ExpiringTomorrow;
                        }
                        else if (petVaccine.ExpirationDate.Date <= DateTime.Today.AddDays(15))
                        {
                            vaccineComplianceReportRow.VaccineComplianceStatus = VaccineComplianceStatusEnum.ExpiringWithin15Days;
                        }
                        else if (petVaccine.ExpirationDate.Date <= DateTime.Today.AddDays(30))
                        {
                            vaccineComplianceReportRow.VaccineComplianceStatus = VaccineComplianceStatusEnum.ExpiringWithin30Days;
                        }
                        else
                        {
                            vaccineComplianceReportRow.VaccineComplianceStatus = VaccineComplianceStatusEnum.Current;
                        }

                        vaccineComplianceReportRow.VaccineComplianceStatusDisplay = GetEnumDisplayName(vaccineComplianceReportRow.VaccineComplianceStatus);

                        vaccineComplianceReportRow.DocumentFilePathDisplay = petVaccine.DocumentFilePath;

                        vaccineComplianceReportRow.NotesDisplay = string.IsNullOrWhiteSpace(petVaccine.Notes)
                                ? "None"
                                : petVaccine.Notes;

                    }

                    vaccineComplianceReport.VaccineComplianceReportRows.Add(vaccineComplianceReportRow);

                }
            }

            if (vaccineComplianceReport.VaccineComplianceReportFilter.ExpirationStartDate.HasValue) 
            {
                vaccineComplianceReport.VaccineComplianceReportRows = vaccineComplianceReport.VaccineComplianceReportRows
                    .Where(x => x.ExpirationDate.HasValue && 
                                x.ExpirationDate.Value.Date >= 
                                vaccineComplianceReport.VaccineComplianceReportFilter.ExpirationStartDate.Value.Date)
                    .ToList();
            }

            if (vaccineComplianceReport.VaccineComplianceReportFilter.ExpirationEndDate.HasValue)
            {
                vaccineComplianceReport.VaccineComplianceReportRows = vaccineComplianceReport.VaccineComplianceReportRows
                    .Where(x => x.ExpirationDate.HasValue &&
                                x.ExpirationDate.Value.Date <=
                                vaccineComplianceReport.VaccineComplianceReportFilter.ExpirationEndDate.Value.Date)
                    .ToList();
            }

            if (vaccineComplianceReport.VaccineComplianceReportFilter.ComplianceStatus.HasValue)
            {
                vaccineComplianceReport.VaccineComplianceReportRows = 
                    vaccineComplianceReport.VaccineComplianceReportRows
                        .Where(x => x.VaccineComplianceStatus == 
                            vaccineComplianceReport.VaccineComplianceReportFilter.ComplianceStatus.Value)
                        .ToList();
            }

            vaccineComplianceReport.HasSearched = true;

            vaccineComplianceReport.TotalCount = vaccineComplianceReport.VaccineComplianceReportRows.Count;

            vaccineComplianceReport.ExpiredCount = vaccineComplianceReport.VaccineComplianceReportRows
                .Count(x => x.VaccineComplianceStatus == VaccineComplianceStatusEnum.Expired);


            vaccineComplianceReport.ExpiringTodayCount = vaccineComplianceReport.VaccineComplianceReportRows
                .Count(x => x.VaccineComplianceStatus == VaccineComplianceStatusEnum.ExpiringToday);


            vaccineComplianceReport.ExpiringTomorrowCount = vaccineComplianceReport.VaccineComplianceReportRows
                .Count(x => x.VaccineComplianceStatus == VaccineComplianceStatusEnum.ExpiringTomorrow);


            vaccineComplianceReport.ExpiringWithin15DaysCount = vaccineComplianceReport.VaccineComplianceReportRows
                .Count(x => x.VaccineComplianceStatus == VaccineComplianceStatusEnum.ExpiringWithin15Days);


            vaccineComplianceReport.ExpiringWithin30DaysCount = vaccineComplianceReport.VaccineComplianceReportRows
                .Count(x => x.VaccineComplianceStatus == VaccineComplianceStatusEnum.ExpiringWithin30Days);


            vaccineComplianceReport.CurrentCount = vaccineComplianceReport.VaccineComplianceReportRows
                .Count(x => x.VaccineComplianceStatus == VaccineComplianceStatusEnum.Current);


            vaccineComplianceReport.MissingCount = vaccineComplianceReport.VaccineComplianceReportRows
                .Count(x => x.VaccineComplianceStatus == VaccineComplianceStatusEnum.Missing);

            return View(vaccineComplianceReport);

        }

        // GET: Reports/BoardingOccupancyReport
        public ActionResult BoardingOccupancyReport() 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingOccupancyReportVM boardingOccupancyReport = new BoardingOccupancyReportVM();

            boardingOccupancyReport.BoardingOccupancyReportFilter = new BoardingOccupancyReportFilterVM();

            boardingOccupancyReport.BoardingOccupancyReportRows = new List<BoardingOccupancyReportRowVM>();
  
            return View(boardingOccupancyReport); 

        }


        // POST: Reports/BoardingOccupancyReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BoardingOccupancyReport(BoardingOccupancyReportVM boardingOccupancyReport) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (!ModelState.IsValid)
            {
                boardingOccupancyReport.BoardingOccupancyReportRows = new List<BoardingOccupancyReportRowVM>();

                return View(boardingOccupancyReport);
            }

            List<BoardingUnitModel> boardingUnits = dbContext.BoardingUnits
                .Include(x => x.Boardings)
                .Include(x => x.Boardings.Select(w => w.Pet))
                .Include(x => x.Boardings.Select(y => y.Customer))
                .ToList();

            if (boardingOccupancyReport.BoardingOccupancyReportFilter.UnitType.HasValue) 
            {
                boardingUnits = boardingUnits
                    .Where(x => x.UnitType == boardingOccupancyReport.BoardingOccupancyReportFilter.UnitType.Value)
                    .ToList();
            }

            if (boardingOccupancyReport.BoardingOccupancyReportFilter.SpeciesAllowed.HasValue)
            {
                boardingUnits = boardingUnits
                    .Where(x => x.SpeciesAllowed == boardingOccupancyReport.BoardingOccupancyReportFilter.SpeciesAllowed.Value)
                    .ToList();
            }

            if (boardingOccupancyReport.BoardingOccupancyReportFilter.SizeCategory.HasValue)
            {
                boardingUnits = boardingUnits
                    .Where(x => x.SizeCategory == boardingOccupancyReport.BoardingOccupancyReportFilter.SizeCategory.Value)
                    .ToList();
            }

            if (boardingOccupancyReport.BoardingOccupancyReportFilter.ReportDate == null)
            {
                boardingOccupancyReport.BoardingOccupancyReportFilter.ReportDate = DateTime.Today;
            }

            boardingOccupancyReport.BoardingOccupancyReportRows = new List<BoardingOccupancyReportRowVM>();

            foreach (BoardingUnitModel boardingUnit in boardingUnits) 
            { 
                BoardingOccupancyReportRowVM boardingOccupancyReportRow = new BoardingOccupancyReportRowVM();

                boardingOccupancyReportRow.BoardingUnitId = boardingUnit.BoardingUnitId;

                boardingOccupancyReportRow.UnitNameDisplay = GetEnumDisplayName(boardingUnit.UnitName);

                boardingOccupancyReportRow.UnitNumberDisplay = boardingUnit.UnitNumber.ToString();

                boardingOccupancyReportRow.UnitTypeDisplay = GetEnumDisplayName(boardingUnit.UnitType);

                boardingOccupancyReportRow.SpeciesAllowedDisplay = GetEnumDisplayName(boardingUnit.SpeciesAllowed);

                boardingOccupancyReportRow.SizeCategoryDisplay = GetEnumDisplayName(boardingUnit.SizeCategory);

                BoardingModel occupiedBoarding = boardingUnit.Boardings
                    .Where(x =>
                        x.StartDateTime.Date <= boardingOccupancyReport.BoardingOccupancyReportFilter.ReportDate.Value.Date &&
                        x.EndDateTime.Date >= boardingOccupancyReport.BoardingOccupancyReportFilter.ReportDate.Value.Date)
                    .Where(x => x.Status != BoardingStatusEnum.Cancelled && x.Status != BoardingStatusEnum.NoShow)
                    .FirstOrDefault();         
                 
                if (occupiedBoarding == null)
                {
                    boardingOccupancyReportRow.OccupancyStatusDisplay = "Available";
                    boardingOccupancyReportRow.BoardingId = null;
                    boardingOccupancyReportRow.PetId = null;
                    boardingOccupancyReportRow.PetNameDisplay = null;
                    boardingOccupancyReportRow.CustomerId = null;
                    boardingOccupancyReportRow.CustomerNameDisplay = null;
                    boardingOccupancyReportRow.CheckInDateDisplay = "N/A";
                    boardingOccupancyReportRow.CheckOutDateDisplay = "N/A";
                }
                else 
                {
                    boardingOccupancyReportRow.OccupancyStatusDisplay = "Occupied";
                    boardingOccupancyReportRow.BoardingId = occupiedBoarding.BoardingId;
                    boardingOccupancyReportRow.PetId = occupiedBoarding.PetId;
                    boardingOccupancyReportRow.PetNameDisplay = occupiedBoarding.Pet.PetName;
                    boardingOccupancyReportRow.CustomerId = occupiedBoarding.CustomerId;
                    boardingOccupancyReportRow.CustomerNameDisplay = occupiedBoarding.Customer.LastName + ", " + 
                        occupiedBoarding.Customer.FirstName;

                    if (occupiedBoarding.ActualCheckInDateTime.HasValue)
                    {
                        boardingOccupancyReportRow.CheckInDateDisplay =
                            occupiedBoarding.ActualCheckInDateTime.Value.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        boardingOccupancyReportRow.CheckInDateDisplay = "Not Checked In";
                    }

                    if (occupiedBoarding.ActualCheckOutDateTime.HasValue)
                    {
                        boardingOccupancyReportRow.CheckOutDateDisplay =
                            occupiedBoarding.ActualCheckOutDateTime.Value.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        boardingOccupancyReportRow.CheckOutDateDisplay = "Not Checked Out";
                    }
                }

                boardingOccupancyReport.BoardingOccupancyReportRows.Add(boardingOccupancyReportRow);
            }

            boardingOccupancyReport.HasSearched = true;

            boardingOccupancyReport.TotalUnitCount = boardingUnits.Count;

            boardingOccupancyReport.OccupiedUnitCount = 
                boardingOccupancyReport.BoardingOccupancyReportRows
                    .Count(x => x.OccupancyStatusDisplay == "Occupied");

            boardingOccupancyReport.AvailableUnitCount =
                boardingOccupancyReport.BoardingOccupancyReportRows
                    .Count(x => x.OccupancyStatusDisplay == "Available");

            decimal percentageOccupancy;

            if (boardingOccupancyReport.TotalUnitCount != 0)
            {
                percentageOccupancy = ((decimal)boardingOccupancyReport.OccupiedUnitCount / boardingOccupancyReport.TotalUnitCount) * 100;  
            }
            else 
            {
                percentageOccupancy = 0;

            }

            boardingOccupancyReport.OccupancyPercentage = percentageOccupancy;

            return View(boardingOccupancyReport); 
        
        }


        // GET: Reports/DailyBoardingReport
        public ActionResult DailyBoardingReport() 
        { 
            ApplicationDbContext dbContext = new ApplicationDbContext();

            DailyBoardingReportVM dailyBoardingReport = new DailyBoardingReportVM();

            dailyBoardingReport.DailyBoardingReportFilter = new DailyBoardingReportFilterVM();

            dailyBoardingReport.DailyBoardingReportRows = new List<DailyBoardingReportRowVM>();

            dailyBoardingReport.CustomerSelectList = BuildCustomerSelectList(dbContext);

            dailyBoardingReport.PetSelectList = BuildPetSelectList(dbContext);

            return View(dailyBoardingReport); 
        
        }


        // POST: Reports/DailyBoardingReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DailyBoardingReport(DailyBoardingReportVM dailyBoardingReport)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            dailyBoardingReport.CustomerSelectList = BuildCustomerSelectList(
                dbContext,
                dailyBoardingReport.DailyBoardingReportFilter.CustomerId);

            dailyBoardingReport.PetSelectList = BuildPetSelectList(
                dbContext,
                dailyBoardingReport.DailyBoardingReportFilter.PetId);

            if (dailyBoardingReport.DailyBoardingReportFilter.StartDate.HasValue &&
                dailyBoardingReport.DailyBoardingReportFilter.EndDate.HasValue &&
                dailyBoardingReport.DailyBoardingReportFilter.EndDate.Value.Date <
                dailyBoardingReport.DailyBoardingReportFilter.StartDate.Value.Date)
            {
                ModelState.AddModelError(
                    "DailyBoardingReportFilter.EndDate",
                    "End Date cannot be before Start Date.");
            }

            if (!ModelState.IsValid)
            {
                dailyBoardingReport.DailyBoardingReportRows = new List<DailyBoardingReportRowVM>();

                return View(dailyBoardingReport);
            }

            List<BoardingModel> boardings = dbContext.Boardings
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.Pet.PetVaccines)
                .Include(x => x.Pet.PetVaccines.Select(w => w.Vaccine))
                .Include(x => x.BoardingUnit)
                .ToList();

            DateTime? startDate = dailyBoardingReport.DailyBoardingReportFilter.StartDate?.Date;
            DateTime? endDate = dailyBoardingReport.DailyBoardingReportFilter.EndDate?.Date;

            if (startDate.HasValue && endDate.HasValue)
            {
                boardings = boardings
                    .Where(x =>
                        x.StartDateTime.Date <= endDate.Value && 
                        x.EndDateTime.Date >= startDate.Value)
                    .ToList();
            }
            else if (startDate.HasValue)
            {
                boardings = boardings
                    .Where(x => x.EndDateTime.Date >= startDate.Value)
                    .ToList();
            }
            else if (endDate.HasValue)
            {
                boardings = boardings
                    .Where(x =>
                        x.StartDateTime.Date <= endDate.Value)
                    .ToList();
            }

            if (dailyBoardingReport.DailyBoardingReportFilter.CustomerId.HasValue)
            {
                boardings = boardings
                    .Where(x => x.Customer.CustomerId == dailyBoardingReport.DailyBoardingReportFilter.CustomerId.Value)
                    .ToList();
            }

            if (dailyBoardingReport.DailyBoardingReportFilter.PetId.HasValue)
            {
                boardings = boardings
                    .Where(x => x.Pet.PetId == dailyBoardingReport.DailyBoardingReportFilter.PetId.Value)
                    .ToList();
            }

            if (dailyBoardingReport.DailyBoardingReportFilter.BoardingStatus.HasValue)
            {
                boardings = boardings
                .Where(x => x.Status == dailyBoardingReport.DailyBoardingReportFilter.BoardingStatus.Value)
                .ToList();
            }

            dailyBoardingReport.HasSearched = true;

            dailyBoardingReport.DailyBoardingReportRows = new List<DailyBoardingReportRowVM>();

            foreach (BoardingModel boarding in boardings)
            {
                DailyBoardingReportRowVM dailyBoardingReportRow = new DailyBoardingReportRowVM();

                dailyBoardingReportRow.BoardingId = boarding.BoardingId;

                dailyBoardingReportRow.PetId = boarding.Pet.PetId;

                dailyBoardingReportRow.PetNameDisplay = boarding.Pet.PetName;

                dailyBoardingReportRow.CustomerId = boarding.Customer.CustomerId;

                dailyBoardingReportRow.CustomerNameDisplay = boarding.Customer.LastName + ", " + boarding.Customer.FirstName;

                dailyBoardingReportRow.BoardingUnitId = boarding.BoardingUnit.BoardingUnitId;

                dailyBoardingReportRow.BoardingUnitDisplay = GetEnumDisplayName(boarding.BoardingUnit.UnitName) + " - " +
                    boarding.BoardingUnit.UnitNumber;

                dailyBoardingReportRow.StartDateDisplay = boarding.StartDateTime.ToString("MM/dd/yyyy");

                dailyBoardingReportRow.EndDateDisplay = boarding.EndDateTime.ToString("MM/dd/yyyy");

                if (boarding.ActualCheckInDateTime.HasValue)
                {
                    dailyBoardingReportRow.CheckInDateTimeDisplay = boarding.ActualCheckInDateTime.Value.ToString("MM/dd/yyyy");
                }
                else
                {
                    dailyBoardingReportRow.CheckInDateTimeDisplay = "Not Checked In";
                }

                if (boarding.ActualCheckOutDateTime.HasValue)
                {
                    dailyBoardingReportRow.CheckOutDateTimeDisplay = boarding.ActualCheckOutDateTime.Value.ToString("MM/dd/yyyy");
                }
                else
                {
                    dailyBoardingReportRow.CheckOutDateTimeDisplay = "Not Checked Out";
                }

                dailyBoardingReportRow.BoardingStatusDisplay = GetEnumDisplayName(boarding.Status);

                List<VaccineModel> requiredVaccinesForPet = dbContext.Vaccines
                    .Where(x => x.RequiredFlag && x.Species == boarding.Pet.Species)
                    .ToList();

                bool hasNonCompliantRequiredVaccine = requiredVaccinesForPet
                    .Any(requiredVaccine =>
                        !boarding.Pet.PetVaccines.Any(petVaccine =>
                            petVaccine.VaccineId == requiredVaccine.VaccineId &&
                            petVaccine.ExpirationDate.Date >= DateTime.Today));

                if (hasNonCompliantRequiredVaccine)
                {
                    dailyBoardingReportRow.VaccineComplianceDisplay = "Not Compliant";
                }
                else
                {
                    dailyBoardingReportRow.VaccineComplianceDisplay = "Compliant";
                }

                List<InvoiceModel> boardingInvoices = dbContext.Invoices
                    .Where(x => x.BoardingId == boarding.BoardingId)
                    .ToList();

                if (boardingInvoices.Any())
                {
                    decimal totalBalance = boardingInvoices.Sum(x => x.Balance);
                    dailyBoardingReportRow.BalanceDisplay = totalBalance.ToString("C");
                }
                else
                {
                    dailyBoardingReportRow.BalanceDisplay = "No Invoice";
                }

                dailyBoardingReportRow.NotesDisplay = boarding.Notes;

                dailyBoardingReport.DailyBoardingReportRows.Add(dailyBoardingReportRow);

            }

            if (startDate.HasValue && endDate.HasValue)
            {
                dailyBoardingReport.ScheduledArrivalCount = boardings
                    .Count(x => x.StartDateTime.Date >= startDate.Value &&
                                x.StartDateTime.Date <= endDate.Value);

                dailyBoardingReport.ScheduledDepartureCount = boardings
                    .Count(x => x.EndDateTime.Date >= startDate.Value &&
                                x.EndDateTime.Date <= endDate.Value);

                dailyBoardingReport.ActualArrivalCount = boardings
                    .Count(x => x.ActualCheckInDateTime.HasValue &&
                                x.ActualCheckInDateTime.Value.Date >= startDate.Value &&
                                x.ActualCheckInDateTime.Value.Date <= endDate.Value);

                dailyBoardingReport.ActualDepartureCount = boardings
                    .Count(x => x.ActualCheckOutDateTime.HasValue &&
                                x.ActualCheckOutDateTime.Value.Date >= startDate.Value &&
                                x.ActualCheckOutDateTime.Value.Date <= endDate.Value);
            }
            else if (startDate.HasValue)
            {
                dailyBoardingReport.ScheduledArrivalCount = boardings
                    .Count(x => x.StartDateTime.Date >= startDate.Value);

                dailyBoardingReport.ScheduledDepartureCount = boardings
                    .Count(x => x.EndDateTime.Date >= startDate.Value);

                dailyBoardingReport.ActualArrivalCount = boardings
                    .Count(x => x.ActualCheckInDateTime.HasValue &&
                                x.ActualCheckInDateTime.Value.Date >= startDate.Value);

                dailyBoardingReport.ActualDepartureCount = boardings
                    .Count(x => x.ActualCheckOutDateTime.HasValue &&
                                x.ActualCheckOutDateTime.Value.Date >= startDate.Value);
            }
            else if (endDate.HasValue)
            {
                dailyBoardingReport.ScheduledArrivalCount = boardings
                    .Count(x => x.StartDateTime.Date <= endDate.Value);

                dailyBoardingReport.ScheduledDepartureCount = boardings
                    .Count(x => x.EndDateTime.Date <= endDate.Value);

                dailyBoardingReport.ActualArrivalCount = boardings
                    .Count(x => x.ActualCheckInDateTime.HasValue &&
                                x.ActualCheckInDateTime.Value.Date <= endDate.Value);

                dailyBoardingReport.ActualDepartureCount = boardings
                    .Count(x => x.ActualCheckOutDateTime.HasValue &&
                                x.ActualCheckOutDateTime.Value.Date <= endDate.Value);
            }
            else 
            {
                dailyBoardingReport.ScheduledArrivalCount = boardings.Count;

                dailyBoardingReport.ScheduledDepartureCount = boardings.Count;

                dailyBoardingReport.ActualArrivalCount = boardings
                    .Count(x => x.ActualCheckInDateTime.HasValue);

                dailyBoardingReport.ActualDepartureCount = boardings
                    .Count(x => x.ActualCheckOutDateTime.HasValue);
            }

            return View(dailyBoardingReport); 
        
        }


        private SelectList BuildCustomerSelectList(ApplicationDbContext dbContext, Guid? selectedCustomerId = null)
        {
            List<SelectListItem> customerSelectListItems = dbContext.Customers
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new SelectListItem
                {
                    Value = x.CustomerId.ToString(),
                    Text = x.LastName + ", " + x.FirstName
                })
                .ToList();

            return new SelectList(
                customerSelectListItems,
                "Value",
                "Text",
                selectedCustomerId.HasValue 
                    ? selectedCustomerId.Value.ToString() 
                    : null
            );
        }


        private SelectList BuildPetSelectList(ApplicationDbContext dbContext, Guid? selectedPetId = null)
        {

            List<PetModel> pets = dbContext.Pets
                .Include(x => x.CustomerPets.Select(cu => cu.Customer))
                .OrderBy(x => x.PetName)
                .ThenBy(x => x.Species)
                .ToList();

            List<SelectListItem> petSelectListItems = pets
                .Select(x => new SelectListItem
                {
                    Value = x.PetId.ToString(),
                    Text = BuildPetDisplay(x)

                })
                .ToList();

            return new SelectList(
                petSelectListItems,
                "Value",
                "Text",
                selectedPetId.HasValue
                    ? selectedPetId.Value.ToString()
                    : null
            );
        }


        private SelectList BuildVeterinarianSelectList(ApplicationDbContext dbContext, Guid? selectedVetId = null)
        {
            List<SelectListItem> veterinarianSelectListItems = dbContext.Veterinarians
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new SelectListItem
                {
                    Value = x.VetId.ToString(),
                    Text = x.LastName + ", " + x.FirstName
                })
                .ToList();

            return new SelectList(
                veterinarianSelectListItems,
                "Value",
                "Text",
                selectedVetId.HasValue
                    ? selectedVetId.Value.ToString()
                    : null
            );
        }


        private SelectList BuildEmployeeSelectList(ApplicationDbContext dbContext, Guid? selectedEmployeeId = null)
        {
            List<SelectListItem> employeeSelectListItems = dbContext.Employees
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new SelectListItem
                {
                    Value = x.EmployeeId.ToString(),
                    Text = x.LastName + ", " + x.FirstName
                })
                .ToList();

            return new SelectList(
                employeeSelectListItems,
                "Value",
                "Text",
                selectedEmployeeId.HasValue
                    ? selectedEmployeeId.Value.ToString()
                    : null
            );
        }


        private SelectList BuildBoardingUnitSelectList(ApplicationDbContext dbContext, Guid? selectedBoardingUnitId = null)
        {
            List<SelectListItem> boardingUnitSelectListItems = dbContext.BoardingUnits
                .OrderBy(x => x.UnitName)
                .ThenBy(x => x.UnitNumber)
                .Select(x => new SelectListItem
                {
                    Value = x.BoardingUnitId.ToString(),
                    Text = x.UnitName + " - " + x.UnitNumber
                })
                .ToList();

            return new SelectList(
                boardingUnitSelectListItems,
                "Value",
                "Text",
                selectedBoardingUnitId.HasValue
                    ? selectedBoardingUnitId.Value.ToString()
                    : null
            );
        }


        public JsonResult GetPetsByCustomer(Guid customerId) 
        { 
            ApplicationDbContext dbContext = new ApplicationDbContext();

            List<PetModel> pets = dbContext.CustomerPets
                .Where(x => x.CustomerId == customerId)
                .Select(x => x.Pet)
                .Include(x => x.CustomerPets.Select(cu => cu.Customer))
                .OrderBy(x => x.PetName)
                .ThenBy(x => x.Species)
                .ToList();

            List<SelectListItem> petSelectListItems = pets
                .Select(x => new SelectListItem
                {
                    Value = x.PetId.ToString(),
                    Text = BuildPetDisplay(x)

                })
                .ToList();

            return Json(petSelectListItems, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCustomersByPet(Guid petId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            List<SelectListItem> customers = dbContext.CustomerPets
                .Where(x => x.PetId == petId)
                .OrderBy(x => x.Customer.LastName)
                .ThenBy(x => x.Customer.FirstName)
                .Select(x => new SelectListItem
                {
                    Value = x.CustomerId.ToString(),
                    Text = x.Customer.LastName + ", " + x.Customer.FirstName

                })
                .ToList();

            return Json(customers, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllPets()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            List<PetModel> pets = dbContext.Pets
                .Include(x => x.CustomerPets.Select(cu => cu.Customer))
                .OrderBy(x => x.PetName)
                .ThenBy(x => x.Species)
                .ToList();

            List<SelectListItem> petSelectListItems = pets
                .Select(x => new SelectListItem
                {
                    Value = x.PetId.ToString(),
                    Text = BuildPetDisplay(x)

                })
                .ToList();
                
            return Json(petSelectListItems, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllCustomers()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            List<SelectListItem> customers = dbContext.Customers
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new SelectListItem
                {
                    Value = x.CustomerId.ToString(),
                    Text = x.LastName + ", " + x.FirstName
                })
                .ToList();

            return Json(customers, JsonRequestBehavior.AllowGet);
        }


        private string BuildPetDisplay(PetModel pet)
        {
            if (pet == null) 
            {
                return "No Pet";
            }

            string customerNames = string.Join(
                " / ",
                pet.CustomerPets
                    .OrderBy(x => x.Customer.LastName)
                    .ThenBy(x => x.Customer.FirstName)
                    .Select(x => x.Customer.LastName + ", " + x.Customer.FirstName)
            );

            if (string.IsNullOrWhiteSpace(customerNames)) 
            { 
                customerNames = "No Customer"; 
            }

            return pet.PetName 
                + " (" + pet.Species + ") - " 
                + customerNames;
        }


        private string GetEnumDisplayName(Enum enumValue)
        {
            DisplayAttribute displayAttribute = enumValue
                .GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            return displayAttribute != null 
                ? displayAttribute.GetName() 
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

        private bool CanViewReports(EmployeeModel employee)
        {
            return employee != null &&
                (employee.Role == EmployeeRoleEnum.Admin ||
                 employee.Role == EmployeeRoleEnum.Manager ||
                 employee.Role == EmployeeRoleEnum.Supervisor);

        }


        private bool CanViewFinancialReports(EmployeeModel employee)
        {
            return employee != null &&
                (employee.Role == EmployeeRoleEnum.Admin ||
                 employee.Role == EmployeeRoleEnum.Manager);

        }
    }
}