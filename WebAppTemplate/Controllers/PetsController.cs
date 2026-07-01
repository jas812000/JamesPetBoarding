using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Cache;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
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

        // GET: Pets/Create
        // /Pets/Create?vetId=275c80fc-8a49-410c-ae30-c33661e3eeb6&name=Steve&species=Dog&breed=German%20Shepard&sex=Male&birthDate=11%2F11%2F2020&weight=57.8&notes=Brown%20and%20White
        public ActionResult Create(
            Guid? vetId, 
            string name, 
            SpeciesEnum species, 
            string breed, 
            SexEnum sex, 
            DateTime birthDate, 
            decimal weight, 
            string notes
            )
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (string.IsNullOrWhiteSpace(name)) { return Content("A pet name is required."); }
            if (string.IsNullOrWhiteSpace(breed)) { return Content("Breed is required."); }
            if (birthDate > DateTime.Today) { return Content("Birth date cannot be in the future."); }
            if (weight <= 0) { return Content("Weight must be greater than zero."); }

            if (vetId != null) 
            { 
                VeterinarianModel veterinarian = dbContext.Veterinarians.FirstOrDefault(x => x.VetId == vetId);

                if (veterinarian == null)
                {
                    return Content("Vet ID #" + vetId + " does not exist.");
                
                }       
            }

            PetModel pet = new PetModel();

            pet.VetId = vetId;
            pet.PetName = name;
            pet.Species = species;
            pet.Breed = breed;
            pet.Sex = sex;
            pet.BirthDate = birthDate;
            pet.Weight = weight;
            pet.Notes = notes;

            try
            {

                dbContext.Pets.Add(pet);
                dbContext.SaveChanges();

                return Content("Successfully added " + pet.PetName + ".");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }             
        }
        

        // GET: Pets/Read
        // /Pets/Read?petId=2589b987-46b0-4372-a164-ced50db0b195
        public ActionResult Read(Guid petId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null)
            {
                return Content("Pet Id #" + petId + " does not exist.");
            }
            
            string notesDisplay = string.IsNullOrWhiteSpace(pet.Notes)
                ? "No notes"
                : pet.Notes;

            string speciesDisplay = pet.Species.ToString();

            int age = DateTime.Today.Year - pet.BirthDate.Year;
            if (pet.BirthDate.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            //return View();
            return Content(
                   "Pet ID #" + pet.PetId +
                   "<br />Name: " + pet.PetName + 
                   "<br />Species: " + speciesDisplay +
                   "<br />Breed: " + pet.Breed +
                   "<br />Sex: " + pet.Sex +
                   "<br />Birthday: " + pet.BirthDate.ToString("MM/dd/yyyy") +
                   "<br />Age: " + age +
                   "<br />Weight: " + pet.Weight +
                   "<br />Notes: " + notesDisplay
             );
        }

        // /Pets/Update
        // /Pets/Update?petId=11e84abe-3ab7-4b27-934a-51b4ccb7b5c7&vetId=275c80fc-8a49-410c-ae30-c33661e3eeb6&name=Steve&species=Dog&breed=German%20Shepard&sex=male&birthDate=04%2F15%2F2018&weight=61.8&notes=Brown%20and%20White
        public ActionResult Update(
            Guid petId, 
            Guid? vetId, 
            string name, 
            SpeciesEnum species, 
            string breed, 
            SexEnum sex, 
            DateTime birthDate, 
            decimal weight, 
            string notes
            ) 
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null) 
            {
                return Content("Pet ID #" + petId + " does not exist."); 
            }

            if (vetId != null)
            {
                VeterinarianModel veterinarian = dbContext.Veterinarians.FirstOrDefault(x => x.VetId == vetId);

                if (veterinarian == null)
                {
                    return Content("Vet ID #" + vetId + " does not exist.");

                }
            }

            if (string.IsNullOrWhiteSpace(name)) { return Content("A pet name is required."); }
            if (string.IsNullOrWhiteSpace(breed)) { return Content("Breed is required."); }
            if (birthDate > DateTime.Today) { return Content("Birth date cannot be in the future."); }
            if (weight <= 0) { return Content("Weight must be greater than zero."); }

            pet.VetId = vetId;
            pet.PetName = name;
            pet.Species = species;
            pet.Breed = breed;
            pet.Sex = sex;
            pet.BirthDate = birthDate;
            pet.Weight = weight;
            pet.Notes = notes;

            try
            {
                dbContext.SaveChanges();
                return Content("Successfully updated " + pet.PetName + ".");
            }
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }
        }

        // /Pets/Delete
        // /Pets/Delete?petId=2589b987-46b0-4372-a164-ced50db0b195
        public ActionResult Delete(Guid petId) { 

            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null)
            { 
                return Content("Pet ID #" + petId + " does not exist.");
            }

            try
            {
                pet.IsActive = false;
                dbContext.SaveChanges();

                return Content(
                    "Pet Id #" + petId + " was successfully deactivated.");
            }
            catch (Exception ex) 
            {
                return Content(ex.Message);
            }  
        }
    }
}