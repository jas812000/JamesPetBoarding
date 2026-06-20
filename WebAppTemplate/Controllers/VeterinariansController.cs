using JamesPetBoarding.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.BuilderProperties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.ClientServices.Providers;
using System.Web.Mvc;
using System.Xml.Linq;

namespace WebAppTemplate.Controllers
{
    public class VeterinariansController : Controller
    {
        // GET: Veterinarians
        public ActionResult Index()
        {
            return View();
        }

        // POST: Veterinarians/Create
        //[HttpPost]
        // /Veterinarians/Create?clinicName=Broadway%20Vet%20Clinic&lastName=Smith&firstName=Sam&credentials=DVM&address=123%20Main%20Street&city=Dallas&state=Texas&zipCode=75225&phone=9725554569&email=sam.smith@anymail.com&notes=Sees%20pets%20on%20saturdays
        public ActionResult Create(
            string clinicName, 
            string lastName, 
            string firstName, 
            string credentials, 
            string address, 
            string city, 
            string state, 
            string zipCode, 
            string phone, 
            string email, 
            string notes
            ) //FormCollection collection
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (string.IsNullOrWhiteSpace(clinicName)) { return Content("A clinic name is required."); }
            if (string.IsNullOrWhiteSpace(lastName)) { return Content("A last name is required."); }
            if (string.IsNullOrWhiteSpace(firstName)) { return Content("A first name is required."); }
            if (string.IsNullOrWhiteSpace(credentials)) { return Content("Credentials are required."); }
            if (string.IsNullOrWhiteSpace(address)) { return Content("A address is required."); }
            if (string.IsNullOrWhiteSpace(city)) { return Content("A city is required."); }
            if (string.IsNullOrWhiteSpace(state)) { return Content("A state is required."); }
            if (string.IsNullOrWhiteSpace(zipCode)) { return Content("A ZipCode is required."); }
            if (string.IsNullOrWhiteSpace(phone)) { return Content("A phone number is required."); }
            if (string.IsNullOrWhiteSpace(email)) { return Content("An email is required."); }

            VeterinarianModel veterinarian = new VeterinarianModel();

            veterinarian.VetId = Guid.NewGuid();
            veterinarian.ClinicName = clinicName;
            veterinarian.LastName = lastName;
            veterinarian.FirstName = firstName;
            veterinarian.Credentials = credentials;
            veterinarian.Address = address;
            veterinarian.City = city;
            veterinarian.State = state;
            veterinarian.ZipCode = zipCode;
            veterinarian.Phone = phone;
            veterinarian.Email = email;
            veterinarian.Notes = notes;

            try
            {
                dbContext.Veterinarians.Add(veterinarian);
                dbContext.SaveChanges();

                //return RedirectToAction("Index");
                return Content("Successfully added " + veterinarian.FirstName + " " + veterinarian.LastName + ".");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: Veterinarians/Read
        public ActionResult Read(Guid vetId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            VeterinarianModel veterinarian = dbContext.Veterinarians.FirstOrDefault(x => x.VetId == vetId);

            if (veterinarian == null)
            {
                return Content("Vet Id #" + vetId + " does not exist.");
            }

            string notesDisplay = string.IsNullOrWhiteSpace(veterinarian.Notes)
                ? "No notes"
                : veterinarian.Notes;

            //return View();
            return Content(
                "Vet ID: " + veterinarian.VetId +
                "<br />Clinic Name: " + veterinarian.ClinicName +
                "<br />Last Name: "+ veterinarian.LastName +
                "<br />First Name: " + veterinarian.FirstName +
                "<br />Credentials: " + veterinarian.Credentials +
                "<br />Address: " + veterinarian.Address +
                "<br />City: " + veterinarian.City +
                "<br />State: " + veterinarian.State +
                "<br />ZipCode: " + veterinarian.ZipCode +
                "<br />Phone Number: " + veterinarian.Phone +
                "<br />Email: " + veterinarian.Email +
                "<br />Notes: " + notesDisplay
            );
        }

        // GET: Veterinarians/Update
        // /Veterinarians/Update?vetId=9589b987-56b0-4372-a164-ced60db0b195&clinicName=Broadway%20Vet%20Clinic&lastName=Smith&firstName=Sam&credentials=DVM&address=123%20Main%20Street&city=Dallas&state=Texas&zipCode=75225&phone=9725554569&email=sam.smith@anymail.com&notes=Sees%20pets%20on%20saturdays
        public ActionResult Update(
            Guid vetId, 
            string clinicName, 
            string lastName, 
            string firstName, 
            string credentials,
            string address, 
            string city, 
            string state, 
            string zipCode, 
            string phone,
            string email, 
            string notes
            )//FormCollection collection)
        { 

            ApplicationDbContext dbContext = new ApplicationDbContext();

            VeterinarianModel veterinarian = dbContext.Veterinarians.FirstOrDefault(x => x.VetId == vetId);

            if (veterinarian == null)
            {
                
                // Test case added to database
                // Remove once validation is complete
                veterinarian = new VeterinarianModel
                {
                    VetId = vetId,
                    ClinicName = "Dallas Animal Clinic",
                    LastName = "Jones",
                    FirstName = "Charles",
                    Credentials = "DVM",
                    Address = "5290 Beltline Road",
                    City = "Dallas",
                    State = "Texas",
                    ZipCode = "75254",
                    Phone = "4693739338",
                    Email = "dallas_vet@anymail.com",
                    Notes = "Select weekend availability"
                };
                dbContext.Veterinarians.Add(veterinarian);
                dbContext.SaveChanges();
                return Content("Temporary vet created.");

                // Uncomment below once validation is complete
                // return Content("Vet ID #" + vetId + " does not exist.");
            }

            if (string.IsNullOrWhiteSpace(clinicName)) { return Content("A clinic name is required."); }
            if (string.IsNullOrWhiteSpace(lastName)) { return Content("A last name is required."); }
            if (string.IsNullOrWhiteSpace(firstName)) { return Content("A first name is required."); }
            if (string.IsNullOrWhiteSpace(credentials)) { return Content("Credentials are required."); }
            if (string.IsNullOrWhiteSpace(address)) { return Content("A address is required."); }
            if (string.IsNullOrWhiteSpace(city)) { return Content("A city is required."); }
            if (string.IsNullOrWhiteSpace(state)) { return Content("A state is required."); }
            if (string.IsNullOrWhiteSpace(zipCode)) { return Content("A ZipCode is required."); }
            if (string.IsNullOrWhiteSpace(phone)) { return Content("A phone number is required."); }
            if (string.IsNullOrWhiteSpace(email)) { return Content("An email is required."); }

            veterinarian.ClinicName = clinicName;
            veterinarian.LastName = lastName;
            veterinarian.FirstName = firstName;
            veterinarian.Credentials = credentials;
            veterinarian.Address = address;
            veterinarian.City = city;
            veterinarian.State = state;
            veterinarian.ZipCode = zipCode;
            veterinarian.Phone = phone;
            veterinarian.Email = email;
            veterinarian.Notes = notes;

            try
            {
                dbContext.SaveChanges();

                //return RedirectToAction("Index");
                return Content("Successfully updated " + veterinarian.FirstName + " " + veterinarian.LastName + ".");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }    


        // GET: Veterinarians/Delete
        // /Veterinarians/Delete?vetId=9589b987-56b0-4372-a164-ced60db0b195
        public ActionResult Delete(Guid vetId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            VeterinarianModel veterinarian = dbContext.Veterinarians.FirstOrDefault(x => x.VetId == vetId);

            if (veterinarian == null) 
            { // Test case added to database
              // Remove once validation is complete
              veterinarian = new VeterinarianModel 
              { 
                  VetId = vetId,
                  ClinicName = "Dallas Animal Clinic",
                  LastName = "Jones",
                  FirstName = "Charles",
                  Credentials = "DVM",
                  Address = "5290 Beltline Road",
                  City = "Dallas",
                  State = "Texas",
                  ZipCode = "75254",
                  Phone = "4693739338",
                  Email = "dallas_vet@anymail.com",
                  Notes = "Select weekend availability"
              };
                dbContext.Veterinarians.Add(veterinarian);
                dbContext.SaveChanges();
                return Content("Temporary vet created.");
            }

            if (veterinarian != null) 
            {
                try
                {

                    List<PetModel> petsWithVet = dbContext.Pets.Where(x => x.VetId == vetId).ToList();

                    foreach (PetModel pet in petsWithVet)
                    {
                        pet.VetId = null;
                    }

                    dbContext.Veterinarians.Remove(veterinarian);
                    dbContext.SaveChanges();
                    return Content("Vet ID #" + vetId + " was successfully deleted.");
                    //return RedirectToAction("Index");

                }
                catch (Exception ex) 
                { 
                    return Content(ex.Message); 
                }
            }
            else 
            { 
                return Content("Vet ID #" + vetId + " does not exist."); 
            }
        }
    }
}
