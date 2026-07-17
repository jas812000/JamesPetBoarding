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
using System.Web.Services.Description;
using System.Web.UI;

namespace JamesPetBoarding.Controllers
{
    public class CustomerPetsController : Controller
    {
        // GET: CustomerPets
        public ActionResult Index()
        {
            return View();
        }


        // GET: CustomerPets/Create
        public ActionResult Create(Guid customerId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);

            if (customer == null)
            {
                return Content("Customer ID #" + customerId + " does not exist.");
            }

            CustomerPetFormVM customerPetForm = new CustomerPetFormVM();

            customerPetForm.CustomerId = customer.CustomerId;

            customerPetForm.CustomerNameDisplay = customer.FirstName + " " + customer.LastName;

            customerPetForm.CustomerPetId = Guid.NewGuid();

            customerPetForm.PetSelectList = BuildPetSelectList();

            return View(customerPetForm);
        }


        // POST: CustomerPets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerPetFormVM customerPetForm)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerPetForm.CustomerId);

            if (customer == null)
            {
                return Content("Customer ID #" + customerPetForm.CustomerId + " does not exist.");
            }

            if (!ModelState.IsValid)
            {

                customerPetForm.PetSelectList = BuildPetSelectList();

                customerPetForm.CustomerNameDisplay = customer.FirstName + " " + customer.LastName;

                return View(customerPetForm);
            }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == customerPetForm.PetId);
            if (pet == null)
            {
                return Content("Pet ID #" + customerPetForm.PetId + " does not exist.");
            }

            CustomerPetModel existingRelationship = dbContext.CustomerPets.FirstOrDefault(x => x.CustomerId == customerPetForm.CustomerId && x.PetId == customerPetForm.PetId);

            if (existingRelationship != null)
            {
                ModelState.AddModelError("", "This customer is already associated with this pet.");

                customerPetForm.PetSelectList = BuildPetSelectList();

                customerPetForm.CustomerNameDisplay = customer.FirstName + " " + customer.LastName;

                return View(customerPetForm);

            }

            CustomerPetModel customerPet = new CustomerPetModel();

            customerPet.CustomerPetId = Guid.NewGuid();
            customerPet.PetId = customerPetForm.PetId;
            customerPet.CustomerId = customerPetForm.CustomerId;
            customerPet.RelationshipType = customerPetForm.RelationshipType;

            dbContext.CustomerPets.Add(customerPet);
            dbContext.SaveChanges();

            return RedirectToAction("Read", "Customers", new { customerId = customerPetForm.CustomerId });
        }


        // GET: CustomerPets/Read
        public ActionResult Read(Guid customerPetId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerPetModel customerPet = dbContext.CustomerPets
                .Include("Customer")
                .Include("Pet")
                .FirstOrDefault(x => x.CustomerPetId == customerPetId);

            if (customerPet == null)
            {
                return Content("CustomerPet ID #" + customerPetId + " does not exist.");
            }

            CustomerPetSummaryVM customerPetSummary = new CustomerPetSummaryVM();

            customerPetSummary.CustomerPetId = customerPet.CustomerPetId;
            customerPetSummary.CustomerId = customerPet.CustomerId;
            customerPetSummary.CustomerNameDisplay = customerPet.Customer.FirstName + " " + customerPet.Customer.LastName;
            customerPetSummary.PetId = customerPet.PetId;
            customerPetSummary.PetNameDisplay = customerPet.Pet.PetName;
            customerPetSummary.SpeciesDisplay = customerPet.Pet.Species.ToString();
            customerPetSummary.BreedDisplay = customerPet.Pet.Breed;
            customerPetSummary.RelationshipTypeDisplay = customerPet.RelationshipType.ToString();

            return View(customerPetSummary);
        }


        // GET: CustomerPets/Update
        public ActionResult Update(Guid customerPetId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerPetModel customerPet = dbContext.CustomerPets.Include("Customer").Include("Pet").FirstOrDefault(x => x.CustomerPetId == customerPetId);

            if (customerPet == null)
            {
                return Content("CustomerPet ID #" + customerPetId + "does not exist");
            }

            CustomerPetFormVM customerPetForm = new CustomerPetFormVM();

            customerPetForm.CustomerPetId = customerPet.CustomerPetId;

            customerPetForm.CustomerId = customerPet.CustomerId;

            customerPetForm.PetId = customerPet.PetId;

            customerPetForm.RelationshipType = customerPet.RelationshipType;                
            
            customerPetForm.CustomerNameDisplay = customerPet.Customer.FirstName + " " + customerPet.Customer.LastName;

            customerPetForm.PetSelectList = BuildPetSelectList();

            return View(customerPetForm);
        }


        // POST: CustomerPets/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CustomerPetFormVM customerPetForm)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerPetForm.CustomerId);

            if (customer == null)
            {
                return Content("Customer ID #" + customerPetForm.CustomerId + " does not exist");
            }

            if (!ModelState.IsValid)
            {
                customerPetForm.PetSelectList = BuildPetSelectList();

                customerPetForm.CustomerNameDisplay = customer.FirstName + " " + customer.LastName;

                return View(customerPetForm);
            }
            
            CustomerPetModel customerPet = dbContext.CustomerPets.FirstOrDefault(x => x.CustomerPetId == customerPetForm.CustomerPetId);

            if (customerPet == null)
            {
                return Content("CustomerPet ID #" + customerPetForm.CustomerPetId + " does not exist");
            }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == customerPetForm.PetId);
            if (pet == null)
            {
                return Content("Pet ID #" + customerPetForm.PetId + " does not exist");
            }

            CustomerPetModel existingRelationship = dbContext.CustomerPets.FirstOrDefault(x => x.CustomerPetId != customerPetForm.CustomerPetId && x.CustomerId == customerPetForm.CustomerId && x.PetId == customerPetForm.PetId);

            if (existingRelationship != null)
            {
                ModelState.AddModelError("", "This customer is already associated with this pet.");

                customerPetForm.PetSelectList = BuildPetSelectList();

                customerPetForm.CustomerNameDisplay = customer.FirstName + " " + customer.LastName;

                return View(customerPetForm);

            }

            customerPet.RelationshipType = customerPetForm.RelationshipType;

            dbContext.SaveChanges();

            return RedirectToAction("Read", "Customers", new { customerId = customerPetForm.CustomerId });

        }


        // GET: CustomerPets/Delete
        public ActionResult Delete(Guid customerPetId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerPetModel customerPet = dbContext.CustomerPets.Include("Customer").Include("Pet").FirstOrDefault(x => x.CustomerPetId == customerPetId);

            if (customerPet == null)
            {
                return Content("CustomerPet ID #" + customerPetId + "does not exist");
            }

            CustomerPetFormVM customerPetForm = new CustomerPetFormVM();

            customerPetForm.CustomerPetId = customerPet.CustomerPetId;
            customerPetForm.CustomerId = customerPet.CustomerId;
            customerPetForm.CustomerNameDisplay = customerPet.Customer.FirstName + " " + customerPet.Customer.LastName;
            customerPetForm.PetId = customerPet.PetId;
            customerPetForm.PetNameDisplay = customerPet.Pet.PetName;
            customerPetForm.RelationshipType = customerPet.RelationshipType;

            return View(customerPetForm);
        }


        // POST: CustomerPets/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CustomerPetFormVM customerPetForm)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerPetModel customerPet = dbContext.CustomerPets
                .Include("Customer")
                .Include("Pet")
                .FirstOrDefault(x => x.CustomerPetId == customerPetForm.CustomerPetId);

            if (customerPet == null)
            {
                return Content("CustomerPet ID #" + customerPetForm.CustomerPetId + " does not exist");
            }

            if (!ModelState.IsValid) 
            {
                customerPetForm.CustomerPetId = customerPet.CustomerPetId;
                customerPetForm.CustomerId = customerPet.CustomerId;
                customerPetForm.CustomerNameDisplay = customerPet.Customer.FirstName + " " + customerPet.Customer.LastName;
                customerPetForm.PetId = customerPet.PetId;
                customerPetForm.PetNameDisplay = customerPet.Pet.PetName;
                customerPetForm.RelationshipType = customerPet.RelationshipType;
                return View(customerPetForm);

            }

            dbContext.CustomerPets.Remove(customerPet);
            dbContext.SaveChanges();

            return RedirectToAction("Read", "Customers", new { customerId = customerPetForm.CustomerId });
        }


        private SelectList BuildPetSelectList() 
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            var petDropdownItems = dbContext.Pets
                        .Where(x => x.IsActive)
                        .Select(x => new
                        {
                            PetId = x.PetId,
                            PetDisplay = x.PetName + " - " + x.Species + " - " + x.Breed
                        })
                        .ToList();

            SelectList petSelectList = new SelectList(petDropdownItems, "PetId", "PetDisplay");

            return petSelectList;

        }

    }
}
