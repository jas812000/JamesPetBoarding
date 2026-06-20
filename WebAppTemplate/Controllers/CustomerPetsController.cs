using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        // /CustomerPets/Create?petId=USE_EXISTING_PET_ID&customerId=USE_EXISTING_CUSTOMER_ID&relationshipType=owner
        public ActionResult Create(
            Guid petId,
            Guid customerId,
            string relationshipType
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (string.IsNullOrWhiteSpace(relationshipType)) { return Content("Relationship type is required."); }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);
            if (pet == null) { return Content("Pet ID #" + petId + " does not exist."); }

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer == null) { return Content("Customer ID #" + customerId + " does not exist."); }

            CustomerPetModel customerPet = new CustomerPetModel();

            customerPet.PetId = petId;
            customerPet.CustomerId = customerId;
            customerPet.RelationshipType = relationshipType;

            try
            {
                dbContext.CustomerPets.Add( customerPet );
                dbContext.SaveChanges();

                return Content("The " + customer.FirstName + " and " + pet.Name + " relationship was successfully created.");
            }
            catch (Exception ex) 
            {
                return Content(ex.Message);
            }
        }


        // GET: CustomerPets/Read
        // /CustomerPets/Read?customerPetId=USE_EXISTING_CUSTOMERPET_ID
        public ActionResult Read(Guid customerPetId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerPetModel customerPet = dbContext.CustomerPets.FirstOrDefault(x => x.CustomerPetId == customerPetId);

            if (customerPet == null) { return Content("CustomerPet ID #" + customerPetId + " does not exist."); }

            return Content(
                "CustomerPet ID #" + customerPet.CustomerPetId +
                "<br />Customer ID #" + customerPet.CustomerId +
                "<br />Pet ID #" + customerPet.PetId +
                "<br />Relationship Type: " + customerPet.RelationshipType
            );
        }


        // GET: CustomerPets/Update
        // /CustomerPets/Update?customerPetId=USE_EXISTING_CUSTOMERPET_ID&petId=USE_EXISTING_PET_ID&customerId=USE_EXISTING_CUSTOMER_ID&relationshipType=owner
        public ActionResult Update(
            Guid customerPetId,
            Guid petId,
            Guid customerId,
            string relationshipType
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerPetModel customerPet = dbContext.CustomerPets.FirstOrDefault(x => x.CustomerPetId == customerPetId);
            if (customerPet == null) { return Content("CustomerPet ID #" + customerPetId + "does not exist"); }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);
            if (pet == null) { return Content("Pet ID #" + petId + "does not exist"); }

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer == null) { return Content("Customer ID #" + customerId + "does not exist"); }

            if (string.IsNullOrWhiteSpace(relationshipType)) { return Content("Relationship type is required."); }

            customerPet.PetId = petId;
            customerPet.CustomerId = customerId;
            customerPet.RelationshipType = relationshipType;

            try
            {
                dbContext.SaveChanges();

                return Content("CustomerPet ID #" + customerPet.CustomerPetId + " was successfully updated.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        // GET: CustomerPets/Delete
        // /CustomerPets/Delete?customerPetId=USE_EXISTING_CUSTOMERPET_ID
        public ActionResult Delete(Guid customerPetId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerPetModel customerPet = dbContext.CustomerPets.FirstOrDefault(x => x.CustomerPetId == customerPetId);
            if (customerPet == null) { return Content("CustomerPet ID #" + customerPetId + " does not exist"); }


            try
            {
                dbContext.CustomerPets.Remove(customerPet);

                dbContext.SaveChanges();

                return Content("CustomerPet ID #" + customerPet.CustomerPetId + " was successfully deleted.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
