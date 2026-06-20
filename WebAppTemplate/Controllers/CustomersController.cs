using JamesPetBoarding.Migrations;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace JamesPetBoarding.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        // GET: Customers/Create
        // /Customers/Create?lastName=Chan&firstName=Nancy&address=4522BeltLine%20Road&city=Dallas&state=Texas&zipCode=75150&phone=6825559991&email=nancy.chan@anymail.com&notes=10%20year%20customer
        public ActionResult Create(
            string lastName, 
            string firstName, 
            string address, 
            string city, 
            string state, 
            string zipCode,
            string phone,
            string email,
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

            CustomerModel customer = new CustomerModel();

            try
            {
                customer.CustomerId = Guid.NewGuid();
                customer.LastName = lastName;
                customer.FirstName = firstName;
                customer.Address = address;
                customer.City = city;
                customer.State = state;
                customer.ZipCode = zipCode;
                customer.Phone = phone;
                customer.Email = email;
                customer.IsActive = true;
                customer.Notes = notes;

                dbContext.Customers.Add(customer);
                dbContext.SaveChanges();

                return Content("Successfully added " + firstName + " " + lastName + " to the database.");
            }
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }
        }

        // GET: Customers/Read
        // Customers/Read?customerId=USE_EXISITNG_CUSTOMER_ID
        public ActionResult Read(Guid customerId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);

            if (customer == null) 
            { 
                return Content("Customer ID #" + customerId + " does not exist."); 
            }

            string customerStatus = customer.IsActive 
                ? "Active" 
                : "Inactive";

            string notesDisplay = string.IsNullOrWhiteSpace(customer.Notes)
                ? "No notes"
                : customer.Notes;

            //return View();
            return Content(
                "Customer ID #" + customerId + 
                "<br />Last Name: " + customer.LastName + 
                "<br />First Name: " + customer.FirstName + 
                "<br />Address: " + customer.Address + 
                "<br />City: " + customer.City + 
                "<br />State: " + customer.State + 
                "<br />Zip Code: " + customer.ZipCode + 
                "<br />Phone Number: " + customer.Phone + 
                "<br />Email Address: " + customer.Email + 
                "<br />Customer active? " + customerStatus + 
                "<br />Notes: " + notesDisplay
            );
        }

        // GET: Customers/Update
        // /Customers/Update?customerId=USE_EXISITNG_CUSTOMER_ID&lastName=Chan&firstName=Nancy&address=4522%20BeltLine%20Road&city=Dallas&state=Texas&zipCode=75150&phone=6825559991&email=nancy.chan@anymail.com&isActive=true&notes=10%20year%20customer
        public ActionResult Update(
            Guid customerId,
            string lastName,
            string firstName,
            string address,
            string city,
            string state,
            string zipCode,
            string phone,
            string email,
            bool isActive,
            string notes
            )
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);

            if (customer == null)
            {

                return Content("Customer ID #" + customerId + " does not exist.");
            }

            if (string.IsNullOrWhiteSpace(lastName)) { return Content("A last name is required."); }
            if (string.IsNullOrWhiteSpace(firstName)) { return Content("A first name is required."); }
            if (string.IsNullOrWhiteSpace(address)) { return Content("A street address is required."); }
            if (string.IsNullOrWhiteSpace(city)) { return Content("A city is required."); }
            if (string.IsNullOrWhiteSpace(state)) { return Content("A state is required."); }
            if (string.IsNullOrWhiteSpace(zipCode)) { return Content("A zip code is required."); }
            if (string.IsNullOrWhiteSpace(phone)) { return Content("A phone number is required."); }
            if (string.IsNullOrWhiteSpace(email)) { return Content("An email address is required."); }

            customer.LastName = lastName;
            customer.FirstName = firstName;
            customer.Address = address;
            customer.City = city;
            customer.State = state;
            customer.ZipCode = zipCode;
            customer.Phone = phone;
            customer.Email = email;
            customer.IsActive = isActive;
            customer.Notes = notes;

            try 
            {
                dbContext.SaveChanges();
                return Content("Successfully updated customer ID #" + customer.CustomerId + ".");
            
            }
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }
        }

        // GET: Customers/Delete
        // /Customers/Delete?customerId=USE_EXISITNG_CUSTOMER_ID
        public ActionResult Delete(Guid customerId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);

            if (customer == null)
            {
                return Content("Customer ID #" + customerId + " does not exist.");
            }

            try
            {

                List<CustomerPetModel> customerPets = dbContext.CustomerPets.Where(x => x.CustomerId == customerId).ToList();
                dbContext.CustomerPets.RemoveRange(customerPets);

                List<EmergencyContactModel> emergencyContacts = dbContext.EmergencyContacts.Where(x => x.CustomerId == customerId).ToList();
                dbContext.EmergencyContacts.RemoveRange(emergencyContacts);

                List<BoardingModel> petBookings = dbContext.Boardings.Where(x => x.CustomerId == customerId).ToList();
                dbContext.Boardings.RemoveRange(petBookings);

                int paymentsCount = 0;
                int invoiceItemsCount = 0;

                List<InvoiceModel> invoices = dbContext.Invoices.Where(x => x.CustomerId == customerId).ToList();
                foreach (InvoiceModel invoice in invoices)
                {
                    Guid invoiceId = invoice.InvoiceId;

                    List<PaymentModel> payments = dbContext.Payments.Where(x => x.InvoiceId == invoiceId).ToList();
                    paymentsCount += payments.Count; 
                    dbContext.Payments.RemoveRange(payments);
                        
                    List<InvoiceItemModel> invoiceItems = dbContext.InvoiceItems.Where(x => x.InvoiceId == invoiceId).ToList();
                    invoiceItemsCount += invoiceItems.Count;
                    dbContext.InvoiceItems.RemoveRange(invoiceItems);
                        
                }

                dbContext.Invoices.RemoveRange(invoices);
                dbContext.Customers.Remove(customer);
                dbContext.SaveChanges();
                return Content(
                    "Customer ID #" + customerId + 
                    " was successfully deleted. Removed " + 
                    customerPets.Count + " customer-pet record(s), " +
                    emergencyContacts.Count + " emergency contact record(s), " +
                    petBookings.Count + " boarding record(s), and " +
                    paymentsCount + " payment record(s), and " +
                    invoiceItemsCount + " invoice-item record(s), and " +
                    invoices.Count + " invoice record(s)."
                );
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
