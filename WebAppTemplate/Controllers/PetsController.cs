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
        // /Pets/Create?vetId=275c80fc-8a49-410c-ae30-c33661e3eeb6&name=Steve&species=Dog&breed=German%20Shepard&sex=male&birthDate=11%2F11%2F2020&weight=57.8&notes=Brown%20and%20White
        public ActionResult Create(
            Guid? vetId, 
            string name, 
            string species, 
            string breed, 
            string sex, 
            DateTime birthDate, 
            decimal weight, 
            string notes
            )
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (string.IsNullOrWhiteSpace(name)) { return Content("A pet name is required."); }
            if (string.IsNullOrWhiteSpace(species)) { return Content("Species is required."); }
            if (string.IsNullOrWhiteSpace(breed)) { return Content("Breed is required."); }
            if (string.IsNullOrWhiteSpace(sex)) { return Content("Sex is required."); }
            if (birthDate > DateTime.Today) { return Content("Birth date cannot be in the future."); }
            if (weight <= 0) { return Content("Weight must be greater than zero."); }

            PetModel pet = new PetModel();

            int age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age)) 
            { 
                age--;  
            }

            pet.PetId = Guid.NewGuid();
            pet.VetId = vetId;
            pet.Name = name;
            pet.Species = species;
            pet.Breed = breed;
            pet.Sex = sex;
            pet.BirthDate = birthDate;
            pet.Age = age;
            pet.Weight = weight;
            pet.Notes = notes;

            try
            {
                pet.VetId = null; // Remove after Veterinarian Controller is built

                dbContext.Pets.Add(pet);
                dbContext.SaveChanges();

                return Content("Successfully added " + pet.Name + ".");

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

            //return View();
            return Content(
                   "Pet ID #" + pet.PetId +
                   "<br />Name: " + pet.Name + pet.Species +
                   "<br />Breed: " + pet.Breed +
                   "<br />Sex: " + pet.Sex +
                   "<br />Birthday: " + pet.BirthDate.ToString("MM/dd/yyyy") +
                   "<br />Age: " + pet.Age +
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
            string species, 
            string breed, 
            string sex, 
            DateTime birthDate, 
            decimal weight, 
            string notes
            ) 
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null) 
            {
                // Test cases added to database
                // Remove once validation is complete
                pet = new PetModel 
                { 
                    PetId =  petId, 
                    VetId = null, 
                    Name = "Steve", 
                    Species = "Dog", 
                    Breed = "German Shepard", 
                    Sex = "Male", 
                    BirthDate = new DateTime(2020, 11, 11), 
                    Age = 5, 
                    Weight = 57.8m, 
                    Notes = "Temporary test pet for edit"
                };

                dbContext.Pets.Add(pet);
                dbContext.SaveChanges();

                // Uncomment below once validation is complete
                // return Content("Pet ID #" + petId + " does not exist."); 

            }

            if (string.IsNullOrWhiteSpace(name)) { return Content("A pet name is required."); }
            if (string.IsNullOrWhiteSpace(species)) { return Content("Species is required."); }
            if (string.IsNullOrWhiteSpace(breed)) { return Content("Breed is required."); }
            if (string.IsNullOrWhiteSpace(sex)) { return Content("Sex is required."); }
            if (birthDate > DateTime.Today) { return Content("Birth date cannot be in the future."); }
            if (weight <= 0) { return Content("Weight must be greater than zero."); }

            int age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age))
            { 
                age--;
            }

            pet.VetId = vetId;
            pet.Name = name;
            pet.Species = species;
            pet.Breed = breed;
            pet.Sex = sex;
            pet.BirthDate = birthDate;
            pet.Age = age;
            pet.Weight = weight;
            pet.Notes = notes;

            try
            {
                dbContext.SaveChanges();
                return Content("Successfully updated " + pet.Name + ".");
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

            // Remove once validation is complete
            if (pet == null)
            { 
                // Test case added to database
                // Remove once validation is complete
                pet = new PetModel
                {
                    PetId = petId,
                    VetId = null,
                    Name = "Steve",
                    Species = "Dog",
                    Breed = "German Shepard",
                    Sex = "Male",
                    BirthDate = new DateTime(2020, 11, 11),
                    Age = 5,
                    Weight = 57.8m,
                    Notes = "Temporary test pet for removal"
                };
                dbContext.Pets.Add(pet);
                dbContext.SaveChanges();
                return Content("Temporary pet created.");

                // Uncomment below once validation is complete
                //return Content("Pet ID #" + petId + " does not exist.");
            }

            if (pet != null)
            {
                try
                {

                    List<CustomerPetModel> customerPets = dbContext.CustomerPets.Where(x => x.PetId == petId).ToList();
                    dbContext.CustomerPets.RemoveRange(customerPets);

                    List<DietModel> diets = dbContext.Diets.Where(x => x.PetId == petId).ToList();
                    dbContext.Diets.RemoveRange(diets);

                    List<MedicationModel> medications = dbContext.Medications.Where(x => x.PetId == petId).ToList();
                    dbContext.Medications.RemoveRange(medications);

                    List<PetVaccineModel> petVaccines = dbContext.PetVaccines.Where(x => x.PetId == petId).ToList();
                    dbContext.PetVaccines.RemoveRange(petVaccines);

                    List<BoardingModel> petBookings = dbContext.Boardings.Where(x => x.PetId == petId).ToList();
                    dbContext.Boardings.RemoveRange(petBookings);
                    
                    dbContext.Pets.Remove(pet);
                    dbContext.SaveChanges();

                    return Content(
                        "Pet Id #" + petId + 
                        " was successfully deleted. Removed " +
                        customerPets.Count + " customer-pet record(s), " +
                        diets.Count + " diet record(s), " +
                        medications.Count + " medication record(s), "+
                        petVaccines.Count + " pet-vaccine record(s), and "+
                        petBookings.Count + " boarding record(s)."
                    );
                }
                catch (Exception ex) 
                {
                    return Content(ex.Message);
                }  
            }
            else 
            {
                return Content("Pet Id #" + petId + " does not exist."); 
            }
        }
    }
}