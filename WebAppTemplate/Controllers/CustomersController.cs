using JamesPetBoarding.Enums;
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
        // /Customers/Create?lastName=Chan&firstName=Nancy&address=4522BeltLine%20Road&city=Dallas&state=TX&zipCode=75150&phone=6825559991&email=nancy.chan@anymail.com&notes=10%20year%20customer
        public ActionResult Create(
            string lastName, 
            string firstName, 
            string address, 
            string city, 
            StateEnum state, 
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
            if (string.IsNullOrWhiteSpace(zipCode)) { return Content("A zip code is required."); }
            if (string.IsNullOrWhiteSpace(phone)) { return Content("A phone number is required."); }
            if (string.IsNullOrWhiteSpace(email)) { return Content("An email address is required."); }

            CustomerModel customer = new CustomerModel();

            try
            {
                customer.LastName = lastName;
                customer.FirstName = firstName;
                customer.Address = address;
                customer.City = city;
                customer.State = state;
                customer.ZipCode = zipCode;
                customer.Phone = phone;
                customer.Email = email;
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

            string stateDisplay = customer.State.ToString();

            //return View();
            return Content(
                "Customer ID #" + customerId + 
                "<br />Last Name: " + customer.LastName + 
                "<br />First Name: " + customer.FirstName + 
                "<br />Address: " + customer.Address + 
                "<br />City: " + customer.City + 
                "<br />State: " + stateDisplay + 
                "<br />Zip Code: " + customer.ZipCode + 
                "<br />Phone Number: " + customer.Phone + 
                "<br />Email Address: " + customer.Email + 
                "<br />Customer active? " + customerStatus + 
                "<br />Notes: " + notesDisplay
            );
        }

        // GET: Customers/Update
        // /Customers/Update?customerId=USE_EXISITNG_CUSTOMER_ID&lastName=Chan&firstName=Nancy&address=4522%20BeltLine%20Road&city=Dallas&state=TX&zipCode=75150&phone=6825559991&email=nancy.chan@anymail.com&notes=10%20year%20customer
        public ActionResult Update(
            Guid customerId,
            string lastName,
            string firstName,
            string address,
            string city,
            StateEnum state,
            string zipCode,
            string phone,
            string email,
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
        public ActionResult Delete(
            Guid customerId,
            InactiveReasonEnum inactiveReason,
            string inactiveNotes
            
            )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);

            if (customer == null)
            {
                return Content("Customer ID #" + customerId + " does not exist.");
            }

            try
            {

                customer.IsActive = false;
                customer.InactiveReason = inactiveReason;
                customer.InactivatedDate = DateTime.Now;
                customer.InactiveNotes = inactiveNotes;
                dbContext.SaveChanges();
                return Content(
                    "Customer ID #" + customerId + " was successfully deactivated."

                );
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
