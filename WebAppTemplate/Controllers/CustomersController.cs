using JamesPetBoarding.Enums;
using JamesPetBoarding.Migrations;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using Microsoft.Owin.BuilderProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace JamesPetBoarding.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }


        // GET: Customers/Search
        public ActionResult Search()
        {
            CustomerSearchVM customerSearch = new CustomerSearchVM();

            return View(customerSearch);
        }


        // POST: Customers/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(CustomerSearchVM customerSearch)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            List<CustomerModel> customers = dbContext.Customers.ToList();

            if (!string.IsNullOrWhiteSpace(customerSearch.FirstName)) 
            { 
                customers = customers
                    .Where(x => x.FirstName.ToLower().Contains(customerSearch.FirstName.ToLower()))
                    .ToList(); 
            }

            if (!string.IsNullOrWhiteSpace(customerSearch.LastName)) 
            { customers = customers
                    .Where(x => x.LastName.ToLower().Contains(customerSearch.LastName.ToLower()))
                    .ToList(); 
            }

            if (!string.IsNullOrWhiteSpace(customerSearch.Phone)) 
            { customers = customers
                    .Where(x => x.Phone.Contains(customerSearch.Phone))
                    .ToList(); 
            }

            if (!string.IsNullOrWhiteSpace(customerSearch.Email)) 
            { 
                customers = customers
                    .Where(x => x.Email.Contains(customerSearch.Email))
                    .ToList(); 
            }

            if (customerSearch.IsActive.HasValue) 
            { 
                customers = customers
                    .Where(x => x.IsActive == customerSearch.IsActive.Value)
                    .ToList(); 
            }

            customerSearch.CustomerSearchResults.Clear();

            foreach (CustomerModel customer in customers) 
            { 
                customerSearch.CustomerSearchResults.Add(new CustomerSearchResultVM 
                { 
                    CustomerId = customer.CustomerId,
                    CustomerNameDisplay = customer.LastName + ", " + customer.FirstName,
                    PhoneDisplay = customer.Phone,
                    EmailDisplay = customer.Email,
                    ActiveStatusDisplay = customer.IsActive ? "Active" : "Inactive",
                    IsActive = customer.IsActive    
                }); 
            }

            return View(customerSearch);
        }


        // GET: Customers/Create
        public ActionResult Create()
        {
            CustomerFormVM customerForm = new CustomerFormVM();

            return View(customerForm);
        }


        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerFormVM customerForm)
        {
            if (!ModelState.IsValid) 
            { 

                return View(customerForm); 
            }
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = new CustomerModel();

            customer.LastName = customerForm.LastName;
            customer.FirstName = customerForm.FirstName;
            customer.Address = customerForm.Address;
            customer.City = customerForm.City;
            customer.State = customerForm.State;
            customer.ZipCode = customerForm.ZipCode;
            customer.Phone = customerForm.Phone;
            customer.Email = customerForm.Email;
            customer.Notes = customerForm.Notes;

            dbContext.Customers.Add(customer);
            dbContext.SaveChanges();

            return RedirectToAction("Read", 
                new 
                { 
                    customerId = customer.CustomerId 
                }
             );
        }


        // GET: Customers/Read
        public ActionResult Read(Guid customerId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);

            if (customer == null)
            {
                return Content("Customer ID #" + customerId + " does not exist.");
            }

            string notesDisplay = string.IsNullOrWhiteSpace(customer.Notes)
                ? "No notes"
                : customer.Notes;

            CustomerDetailsVM customerDetails = new CustomerDetailsVM();

            customerDetails.CustomerId = customer.CustomerId;
            customerDetails.CustomerNameDisplay = customer.FirstName + " " + customer.LastName;
            customerDetails.PhoneDisplay = customer.Phone;
            customerDetails.EmailDisplay = customer.Email;
            customerDetails.AddressDisplay = customer.Address;
            customerDetails.CityStateZipDisplay = customer.City + ", " + customer.State + " " + customer.ZipCode;
            customerDetails.NotesDisplay = notesDisplay;
            customerDetails.IsActive = customer.IsActive;

            customerDetails.ActiveStatusDisplay = customer.IsActive
                ? "Active"
                : "Inactive";

            customerDetails.InactiveReasonDisplay = customer.InactiveReason.HasValue
                ? customer.InactiveReason.ToString()
                : "Not Applicable";

            customerDetails.InactivatedDateDisplay = customer.InactivatedDate.HasValue
                ? customer.InactivatedDate.Value.ToShortDateString()
                : "Not Applicable";

            customerDetails.InactiveNotesDisplay = string.IsNullOrWhiteSpace(customer.InactiveNotes)
                ? "No inactive notes"
                : customer.InactiveNotes;

            customerDetails.ReactivatedDateDisplay = customer.ReactivatedDate.HasValue
                ? customer.ReactivatedDate.Value.ToShortDateString()
                : "Not Applicable";

            customerDetails.ReactivatedNotesDisplay = string.IsNullOrWhiteSpace(customer.ReactivatedNotes)
                ? "No reactivation notes"
                : customer.ReactivatedNotes;

            List<EmergencyContactModel> emergencyContacts = dbContext.EmergencyContacts
                .Where(x => x.CustomerId == customerId)
                .ToList();

            foreach (EmergencyContactModel emergencyContact in emergencyContacts)
            {
                EmergencyContactSummaryVM emergencyContactSummary = new EmergencyContactSummaryVM();

                emergencyContactSummary.EmergencyContactId = emergencyContact.EmergencyContactId;
                emergencyContactSummary.FullNameDisplay = emergencyContact.FirstName + " " + emergencyContact.LastName;
                emergencyContactSummary.RelationshipDisplay = emergencyContact.RelationshipType.ToString();
                emergencyContactSummary.IsActive = emergencyContact.IsActive;
                emergencyContactSummary.ActiveStatusDisplay = emergencyContact.IsActive ? "Active" : "Inactive";

                customerDetails.EmergencyContacts.Add(emergencyContactSummary);

            }

            List<CustomerPetModel> customerPets = dbContext.CustomerPets
                .Include(x => x.Pet)
                .Where(x => x.CustomerId == customerId)
                .ToList();

            foreach (CustomerPetModel customerPet in customerPets) 
            { 
                CustomerPetSummaryVM customerPetSummary = new CustomerPetSummaryVM();

                customerPetSummary.CustomerPetId = customerPet.CustomerPetId;
                customerPetSummary.CustomerId = customerPet.CustomerId;
                customerPetSummary.CustomerNameDisplay = customer.FirstName + " " + customer.LastName;
                customerPetSummary.PetId = customerPet.PetId;
                customerPetSummary.PetNameDisplay = customerPet.Pet.PetName;
                customerPetSummary.SpeciesDisplay = customerPet.Pet.Species.ToString();
                customerPetSummary.BreedDisplay = customerPet.Pet.Breed;
                customerPetSummary.RelationshipTypeDisplay = customerPet.RelationshipType.ToString();

                customerDetails.CustomerPets.Add(customerPetSummary);
            
            }

            return View(customerDetails);

        }


        // GET: Customers/Update
        public ActionResult Update(Guid customerId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);

            if (customer == null)
            {
                return Content("Customer ID #" + customerId + " does not exist.");
            }

            CustomerFormVM customerForm = new CustomerFormVM();

            customerForm.CustomerId = customer.CustomerId;
            customerForm.LastName = customer.LastName;
            customerForm.FirstName = customer.FirstName;
            customerForm.Address = customer.Address;
            customerForm.City = customer.City;
            customerForm.State = customer.State;
            customerForm.ZipCode = customer.ZipCode;
            customerForm.Phone = customer.Phone;
            customerForm.Email = customer.Email;
            customerForm.Notes = customer.Notes;

            return View(customerForm);
        }


        // POST: Customers/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CustomerFormVM customerForm)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerForm.CustomerId);

            if (customer == null)
            {

                return Content("Customer ID #" + customerForm.CustomerId + " does not exist.");
            }

            if (!ModelState.IsValid)
            {
                customerForm.CustomerId = customer.CustomerId;
                customerForm.LastName = customer.LastName;
                customerForm.FirstName = customer.FirstName;
                customerForm.Address = customer.Address;
                customerForm.City = customer.City;
                customerForm.State = customer.State;
                customerForm.ZipCode = customer.ZipCode;
                customerForm.Phone = customer.Phone;
                customerForm.Email = customer.Email;
                customerForm.Notes = customer.Notes;
                return View(customerForm);
            }
            
            customer.LastName = customerForm.LastName;
            customer.FirstName = customerForm.FirstName;
            customer.Address = customerForm.Address;
            customer.City = customerForm.City;
            customer.State = customerForm.State;
            customer.ZipCode = customerForm.ZipCode;
            customer.Phone = customerForm.Phone;
            customer.Email = customerForm.Email;
            customer.Notes = customerForm.Notes;


            dbContext.SaveChanges();
            return RedirectToAction("Read",
                new
                {
                    customerId = customer.CustomerId
                }
             );
        }


        // GET: Customers/Delete
        public ActionResult Delete(Guid customerId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);

            if (customer == null)
            {
                return Content("Customer ID #" + customerId + " does not exist.");
            }

            CustomerDeleteVM customerDelete = new CustomerDeleteVM();

            customerDelete.CustomerId = customer.CustomerId;
            customerDelete.CustomerNameDisplay = customer.FirstName + " " + customer.LastName;
            customerDelete.AddressDisplay = customer.Address;
            customerDelete.CityStateZipDisplay = customer.City + ", " + customer.State + " " + customer.ZipCode;
            customerDelete.PhoneDisplay = customer.Phone;
            customerDelete.EmailDisplay = customer.Email;
            customerDelete.NotesDisplay = string.IsNullOrWhiteSpace(customer.Notes)
                ? "No notes"
                : customer.Notes;

            return View(customerDelete);

        }


        // POST: Customers/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CustomerDeleteVM customerDelete)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerDelete.CustomerId);

            if (customer == null)
            {
                return Content("Customer ID #" + customerDelete.CustomerId + " does not exist.");
            }

            if (!ModelState.IsValid)
            {
                customerDelete.CustomerId = customer.CustomerId;
                customerDelete.CustomerNameDisplay = customer.FirstName + " " + customer.LastName;
                customerDelete.AddressDisplay = customer.Address;
                customerDelete.CityStateZipDisplay = customer.City + ", " + customer.State + " " + customer.ZipCode;
                customerDelete.PhoneDisplay = customer.Phone;
                customerDelete.EmailDisplay = customer.Email;
                customerDelete.NotesDisplay = string.IsNullOrWhiteSpace(customer.Notes)
                    ? "No notes"
                    : customer.Notes;

                return View(customerDelete);
            }

            customer.IsActive = false;
            customer.InactiveReason = customerDelete.InactiveReason;
            customer.InactivatedDate = DateTime.Now;
            customer.InactiveNotes = customerDelete.InactiveNotes;
            dbContext.SaveChanges();

            return RedirectToAction("Read", new { customerId = customer.CustomerId });

        }


        // GET: Customers/Reactivate
        public ActionResult Reactivate(Guid customerId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);

            if (customer == null)
            {
                return Content("Customer ID #" + customerId + " does not exist.");
            }

            CustomerReactivateVM customerReactivate = new CustomerReactivateVM();

            customerReactivate.CustomerId = customer.CustomerId;
            customerReactivate.CustomerNameDisplay = customer.FirstName + " " + customer.LastName;
            customerReactivate.AddressDisplay = customer.Address;
            customerReactivate.CityStateZipDisplay = customer.City + ", " + customer.State + " " + customer.ZipCode;
            customerReactivate.PhoneDisplay = customer.Phone;
            customerReactivate.EmailDisplay = customer.Email;

            customerReactivate.NotesDisplay = string.IsNullOrWhiteSpace(customer.Notes)
                ? "No notes"
                : customer.Notes;

            customerReactivate.InactiveReasonDisplay = customer.InactiveReason.HasValue
                ? customer.InactiveReason.ToString()
                : "Not Applicable";

            customerReactivate.InactivatedDateDisplay = customer.InactivatedDate.HasValue
                ? customer.InactivatedDate.Value.ToShortDateString()
                : "Not Applicable";

            customerReactivate.InactiveNotesDisplay = string.IsNullOrWhiteSpace(customer.InactiveNotes)
                ? "No inactive notes"
                : customer.InactiveNotes;

            return View(customerReactivate);

        }


        // POST: Customers/Reactivate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reactivate(CustomerReactivateVM customerReactivate)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerReactivate.CustomerId);

            if (customer == null)
            {
                return Content("Customer ID #" + customerReactivate.CustomerId + " does not exist.");
            }

            if (!ModelState.IsValid)
            {
                customerReactivate.CustomerId = customer.CustomerId;
                customerReactivate.CustomerNameDisplay = customer.FirstName + " " + customer.LastName;
                customerReactivate.AddressDisplay = customer.Address;
                customerReactivate.CityStateZipDisplay = customer.City + ", " + customer.State + " " + customer.ZipCode;
                customerReactivate.PhoneDisplay = customer.Phone;
                customerReactivate.EmailDisplay = customer.Email;

                customerReactivate.NotesDisplay = string.IsNullOrWhiteSpace(customer.Notes)
                    ? "No notes"
                    : customer.Notes;

                customerReactivate.InactiveReasonDisplay = customer.InactiveReason.HasValue
                    ? customer.InactiveReason.ToString()
                    : "Not Applicable";

                customerReactivate.InactivatedDateDisplay = customer.InactivatedDate.HasValue
                    ? customer.InactivatedDate.Value.ToShortDateString()
                    : "Not Applicable";

                customerReactivate.InactiveNotesDisplay = string.IsNullOrWhiteSpace(customer.InactiveNotes)
                    ? "No inactive notes"
                    : customer.InactiveNotes;

                return View(customerReactivate);
            }

            customer.IsActive = true;
            customer.ReactivatedDate = DateTime.Now;
            customer.ReactivatedNotes = customerReactivate.ReactivatedNotes;
            customer.InactiveReason = null;
            customer.InactivatedDate = null;
            customer.InactiveNotes = null;
            dbContext.SaveChanges();

            return RedirectToAction("Read", new { customerId = customer.CustomerId });

        }
    }
}
