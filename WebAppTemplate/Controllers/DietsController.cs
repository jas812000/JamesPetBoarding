using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.BuilderProperties;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    public class DietsController : Controller
    {
        // GET: Diets
        public ActionResult Index()
        {
            return View();
        }

        // GET: Diets/Create
        public ActionResult Create(Guid petId)
        {
            
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null) 
            {
                return Content("Pet ID #" + petId + " does not exist."); 
            }

            DietFormVM dietForm = new DietFormVM();

            dietForm.PetId = petId;

            dietForm.PetNameDisplay = pet.PetName;

            return View(dietForm); 

        }

        // POST: Diets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DietFormVM dietForm)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == dietForm.PetId);

            if (pet == null)
            {
                return Content("Pet ID #" + dietForm.PetId + " does not exist.");
            }

            if (!ModelState.IsValid)
            {
                dietForm.PetNameDisplay = pet.PetName;
                return View(dietForm);
            }

            DietModel diet = new DietModel();

            diet.PetId = dietForm.PetId;
            diet.FoodName = dietForm.FoodName;
            diet.Amount = dietForm.Amount;
            diet.Frequency = dietForm.Frequency;
            diet.Notes = dietForm.Notes;

            dbContext.Diets.Add(diet);
            dbContext.SaveChanges();

            return RedirectToAction("Read", "Diets", new { dietId = diet.DietId }); 
        }


        // GET: Diets/Read
        public ActionResult Read(Guid dietId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            DietModel diet = dbContext.Diets
                .Include(x => x.Pet)
                .FirstOrDefault(x => x.DietId == dietId);

            if (diet == null) 
            { 
                return Content("Diet ID #" +  dietId + " does not exist."); 
            }

            DietDetailsVM dietDetails = new DietDetailsVM();

            dietDetails.DietId = diet.DietId;
            dietDetails.PetId = diet.PetId;
            dietDetails.PetNameDisplay = diet.Pet.PetName;
            dietDetails.FoodName = diet.FoodName;
            dietDetails.Amount = diet.Amount;
            dietDetails.FrequencyDisplay = diet.Frequency.ToString();

            string notesDisplay = string.IsNullOrWhiteSpace(diet.Notes)
                ? "No notes"
                : diet.Notes;

            dietDetails.NotesDisplay = notesDisplay;

            return View(dietDetails);

        }


        // GET: Diets/Update
        public ActionResult Update(Guid dietId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            DietModel diet = dbContext.Diets
                .Include(x => x.Pet)
                .FirstOrDefault(x => x.DietId == dietId);

            if (diet == null)
            {
                return Content("Diet ID #" + dietId + " does not exist.");
            }

            DietFormVM dietForm = new DietFormVM();

            dietForm.DietId = diet.DietId;

            dietForm.PetId = diet.PetId;

            dietForm.PetNameDisplay = diet.Pet.PetName;

            dietForm.FoodName = diet.FoodName;

            dietForm.Amount = diet.Amount;

            dietForm.Frequency = diet.Frequency;

            dietForm.Notes = diet.Notes;

            return View(dietForm); 
  
        }


        // POST: Diets/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(DietFormVM dietForm)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            DietModel diet = dbContext.Diets
                .Include(x => x.Pet)
                .FirstOrDefault(x => x.DietId == dietForm.DietId);

            if (diet == null)
            {
                return Content("Diet ID #" + dietForm.DietId + " does not exist.");
            }

            if (!ModelState.IsValid)
            {
                dietForm.PetNameDisplay = diet.Pet.PetName;
                return View(dietForm);
            }

            diet.FoodName = dietForm.FoodName;
            diet.Amount = dietForm.Amount;
            diet.Frequency = dietForm.Frequency;
            diet.Notes = dietForm.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", "Diets", new { dietId = diet.DietId });

        }


        // GET: Diets/Delete
        public ActionResult Delete(Guid dietId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            DietModel diet = dbContext.Diets
                .Include(x => x.Pet)
                .FirstOrDefault(x => x.DietId == dietId);

            if (diet == null) 
            {      
                return Content("Diet ID #" + dietId + " does not exist."); 
            }

            DietDeleteVM dietDelete = new DietDeleteVM();

            dietDelete.DietId = diet.DietId;
            dietDelete.PetId = diet.PetId;
            dietDelete.PetNameDisplay = diet.Pet.PetName;
            dietDelete.FoodName = diet.FoodName;
            dietDelete.Amount = diet.Amount;
            dietDelete.FrequencyDisplay = diet.Frequency.ToString();

            string notesDisplay = string.IsNullOrWhiteSpace(diet.Notes)
                ? "No notes"
                : diet.Notes;

            dietDelete.NotesDisplay = notesDisplay;
   
            return View(dietDelete); 

        }


        // POST: Diets/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DietDeleteVM dietDelete)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            DietModel diet = dbContext.Diets.FirstOrDefault(x => x.DietId == dietDelete.DietId);

            if (diet == null)
            {
                return Content("Diet ID #" + dietDelete.DietId + " does not exist.");
            }

            Guid petId = diet.PetId;

            dbContext.Diets.Remove(diet);
            dbContext.SaveChanges();

            return RedirectToAction("Read", "Pets", new { petId });
        }

    }
}
