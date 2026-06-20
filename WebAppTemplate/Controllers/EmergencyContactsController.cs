using JamesPetBoarding.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace JamesPetBoarding.Controllers
{
    public class EmergencyContactsController : Controller
    {
        // GET: EmergencyContacts
        public ActionResult Index()
        {
            return View();
        }

        // GET: EmergencyContacts/Create
        // /EmergencyContacts/Create?customerId=1db53653-7cd8-4737-bba2-f8ef018ec35b&lastName=Rivera&firstName=Maria&address=4522%20BeltLine%20Road&city=Dallas&state=Texas&zipCode=75150&phone=6825558888&email=maria.rivera@anymail.com&relationshipType=Sister&notes=Send%20email%20first
        public ActionResult Create(
            Guid customerId,
            string lastName,
            string firstName,
            string address,
            string city,
            string state,
            string zipCode,
            string phone,
            string email,
            string relationshipType,
            string notes
            )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (string.IsNullOrWhiteSpace(lastName)) { return Content("A last name is required."); }
            if (string.IsNullOrWhiteSpace(firstName)) { return Content("A first name is required."); }
            if (string.IsNullOrWhiteSpace(address)) { return Content("A street address is required."); }
            if (string.IsNullOrWhiteSpace(city)) { return Content("A city is required."); }
            if (string.IsNullOrWhiteSpace(state)) { return Content("A state is required."); }
            if (string.IsNullOrWhiteSpace(zipCode)) { return Content("A zip code is required."); }
            if (string.IsNullOrWhiteSpace(phone)) { return Content("A phone number is required."); }
            if (string.IsNullOrWhiteSpace(email)) { return Content("An email address is required."); }
            if (string.IsNullOrWhiteSpace(relationshipType)) { return Content("A relationship to the owner is required."); }

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);

            // Remove once validation is complete
            if (customer == null)
            {
                // Test case added to database
                // Remove once validation is complete
                customer = new CustomerModel
                {
                    CustomerId = customerId,
                    LastName = "Mendoza",
                    FirstName = "Antonio",
                    Address = "1900 Sam Houston Pkwy",
                    City = "Dallas",
                    State = "Texas",
                    ZipCode = "75214",
                    Phone = "6825551235",
                    Email = "amendoza@anymail.com",
                    IsActive = true,
                    Notes = "Temporary test customer."
                };
                dbContext.Customers.Add(customer);
                dbContext.SaveChanges();
                return Content("Temporary customer created.");

                // Uncomment below after validation
                //return Content("Customer ID #" + customerId + " does not exist.");

            }

            EmergencyContactModel emergencyContact = new EmergencyContactModel();

            emergencyContact.CustomerId = customerId;
            emergencyContact.LastName = lastName;
            emergencyContact.FirstName = firstName;
            emergencyContact.Address = address;
            emergencyContact.City = city;
            emergencyContact.State = state;
            emergencyContact.ZipCode = zipCode;
            emergencyContact.Phone = phone;
            emergencyContact.Email = email;
            emergencyContact.RelationshipType = relationshipType;
            emergencyContact.IsActive = true;
            emergencyContact.Notes = notes;

            try 
            { 
                dbContext.EmergencyContacts.Add(emergencyContact);
                dbContext.SaveChanges();
                return Content(
                    "Successfully added " +
                    emergencyContact.FirstName + " " +
                    emergencyContact.LastName +
                    " to the database."
                );
            
            } 
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }

        }

        // GET: EmergencyContacts/Read
        // /EmergencyContacts/Read?emergencyContactId=
        public ActionResult Read(Guid emergencyContactId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmergencyContactModel emergencyContact = dbContext.EmergencyContacts.FirstOrDefault(x => x.EmergencyContactId == emergencyContactId);

            if (emergencyContact == null) 
            { 
                return Content("Emergency Contact ID #" + emergencyContactId + " does not exist.");
            }

            string emergencyContactStatus = emergencyContact.IsActive 
                ? "Active" 
                : "Inactive";

            string notesDisplay = string.IsNullOrWhiteSpace(emergencyContact.Notes)
                ? "No notes"
                : emergencyContact.Notes;

            return Content(
                "Emergency Contact ID #" + emergencyContact.EmergencyContactId +
                "<br />Customer ID #" + emergencyContact.CustomerId +
                "<br />Last Name: " + emergencyContact.LastName +
                "<br />First Name: " + emergencyContact.FirstName +
                "<br />Address: " + emergencyContact.Address +
                "<br />City: " + emergencyContact.City +
                "<br />State: " + emergencyContact.State +
                "<br />ZipCode: " + emergencyContact.ZipCode +
                "<br />Phone Number: " + emergencyContact.Phone +
                "<br />Email Address: " + emergencyContact.Email +
                "<br />Relationship Type: " + emergencyContact.RelationshipType +
                "<br />Emergency Contact active? " + emergencyContactStatus +
                "<br />Notes: " + notesDisplay
            );

        }

        // GET: EmergencyContacts/Update
        // /EmergencyContacts/Update?emergencyContactId=ENTER_EXISTING_EMERGENCY_CONTACT_ID&customerId=ENTER_EXISTING_CUSTOMER_ID&lastName=Fraizer&firstName=Joe&address=1003%20Montclair%20Road&city=Birmingham&state=Alabama&zipCode=35213&phone=2055554512&email=j.fraizer@anymail.com&relationshipType=Father&isActive=true&notes=Updated%20name,%20phone%20number%20and%20email%20address
        public ActionResult Update(
            Guid emergencyContactId,
            Guid customerId,
            string lastName, 
            string firstName, 
            string address, 
            string city, 
            string state, 
            string zipCode, 
            string phone, 
            string email,
            string relationshipType,
            bool isActive,
            string notes
        )
        {

            ApplicationDbContext dbContext= new ApplicationDbContext();

            EmergencyContactModel emergencyContact = dbContext.EmergencyContacts.FirstOrDefault(x => x.EmergencyContactId == emergencyContactId);

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);

            if (customer == null)
            {
                return Content("Customer ID #" + customerId + " does not exist.");
            }

            if (emergencyContact == null) 
            {

                return Content("Emergency Contact ID #" + emergencyContactId + " does not exist.");

            }

            if (string.IsNullOrWhiteSpace(lastName)) { return Content("A last name is required."); }
            if (string.IsNullOrWhiteSpace(firstName)) { return Content("A first name is required."); }
            if (string.IsNullOrWhiteSpace(address)) { return Content("A street address is required."); }
            if (string.IsNullOrWhiteSpace(city)) { return Content("A city is required."); }
            if (string.IsNullOrWhiteSpace(state)) { return Content("A state is required."); }
            if (string.IsNullOrWhiteSpace(zipCode)) { return Content("A zip code is required."); }
            if (string.IsNullOrWhiteSpace(phone)) { return Content("A phone number is required."); }
            if (string.IsNullOrWhiteSpace(email)) { return Content("An email address is required."); }
            if (string.IsNullOrWhiteSpace(relationshipType)) { return Content("A relationship to the owner is required."); }

            emergencyContact.CustomerId = customerId;
            emergencyContact.LastName = lastName;
            emergencyContact.FirstName = firstName;
            emergencyContact.Address = address;
            emergencyContact.City = city;
            emergencyContact.State = state;
            emergencyContact.ZipCode = zipCode;
            emergencyContact.Phone = phone;
            emergencyContact.Email = email;
            emergencyContact.RelationshipType = relationshipType;
            emergencyContact.IsActive = isActive;
            emergencyContact.Notes = notes;

            try 
            {
                dbContext.SaveChanges();
                return Content("Emergency Contact ID #" + emergencyContact.EmergencyContactId + " successfully updated.");
            } 
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }

        }

        // GET: EmergencyContacts/Delete
        // /EmergencyContacts/Delete?emergencyContactId=USE_THE_ONE_FROM_CREATE
        public ActionResult Delete(Guid emergencyContactId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmergencyContactModel emergencyContact = dbContext.EmergencyContacts.FirstOrDefault(x => x.EmergencyContactId == emergencyContactId);

            if (emergencyContact == null)
            {

                return Content("Emergency Contact ID #" + emergencyContactId + " does not exist.");

            }
            
            try
            {
                dbContext.EmergencyContacts.Remove(emergencyContact);
                dbContext.SaveChanges();

                return Content(
                    "Emergency Contact ID #" + emergencyContactId +
                    " was successfully deleted.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
           

        }

    }
}
