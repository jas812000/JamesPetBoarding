using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.BuilderProperties;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
        // /Diets/Update?dietId=USE_EXISTING_DIET_ID&petId=USE_EXISTING_PET_ID&foodName=Nutro&amount=0.5%20cup&frequency=OnceDaily&notes=food%20already%20separated%20in%20containers
        public ActionResult Create(
            Guid petId,
            string foodName,
            string amount,
            FrequencyEnum frequency,
            string notes
        )
        {
            
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (string.IsNullOrWhiteSpace(foodName)) { return Content("Name of the food product is required."); }
            if (string.IsNullOrWhiteSpace(amount)) { return Content("Amount to be fed is required."); }

            PetModel petModel = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (petModel == null) 
            {
                return Content("Pet ID #" + petId + " does not exist."); 
            }

            DietModel dietModel = new DietModel();

            dietModel.PetId = petId;
            dietModel.FoodName = foodName;
            dietModel.Amount = amount;
            dietModel.Frequency = frequency;
            dietModel.Notes = notes;

            try 
            { 
                dbContext.Diets.Add( dietModel );
                dbContext.SaveChanges();

                return Content("Successfully added " + foodName + " to the database.");
            } 
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }

        }


        // GET: Diets/Read
        // Diets/Read?dietId=USE_EXISTING_DIET_ID
        public ActionResult Read(Guid dietId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            DietModel diet = dbContext.Diets.FirstOrDefault(x => x.DietId == dietId);

            if (diet == null) 
            { 
                return Content("Diet ID #" +  dietId + " does not exist."); 
            }

            string notesDisplay = string.IsNullOrWhiteSpace(diet.Notes)
                ? "No notes"
                : diet.Notes;

            string frequencyDisplay = diet.Frequency.ToString();

            switch (diet.Frequency) 
            {
                case FrequencyEnum.OnceDaily:
                    frequencyDisplay = "Once Daily";
                    break;

                case FrequencyEnum.TwiceDaily:
                    frequencyDisplay = "Twice Daily";
                    break;

                case FrequencyEnum.ThreeTimesDaily:
                    frequencyDisplay = "Three Times Daily";
                    break;

                case FrequencyEnum.FourTimesDaily:
                    frequencyDisplay = "Four Times Daily";
                    break;

                case FrequencyEnum.EveryOtherDay:
                    frequencyDisplay = "Every Other Day";
                    break;

                case FrequencyEnum.OnceWeekly:
                    frequencyDisplay = "Once Weekly";
                    break;

                case FrequencyEnum.OnceMonthly:
                    frequencyDisplay = "Once Monthly";
                    break;

                case FrequencyEnum.AsNeeded:
                    frequencyDisplay = "As Needed";
                    break;
            }

            return Content(
                "Diet ID #" + diet.DietId + 
                "<br />Pet ID #" + diet.PetId +
                "<br />Food Name: " + diet.FoodName +
                "<br />Amount: " + diet.Amount +
                "<br />Frequency: " + frequencyDisplay +
                "<br />Notes: " + notesDisplay
            );
        }


        // GET: Diets/Update
        // /Diets/Create?petId=USE_EXISTING_PET_ID&foodName=Nutro&amount=0.5%20cup&frequency=OnceDaily&notes=food%20already%20separated%20in%20containers
        public ActionResult Update(
            Guid dietId, 
            Guid petId, 
            string foodName, 
            string amount, 
            FrequencyEnum frequency, 
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            DietModel diet = dbContext.Diets.FirstOrDefault(x => x.DietId == dietId);

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (diet == null) { return Content("Diet ID #" + dietId + " does not exist."); }

            if (pet == null) { return Content("Pet ID #" + petId + " does not exist."); }

            if (string.IsNullOrWhiteSpace(foodName)) { return Content("Name of the food product is required."); }
            if (string.IsNullOrWhiteSpace(amount)) { return Content("Amount to be fed is required."); }

            diet.PetId = petId;
            diet.FoodName = foodName;
            diet.Amount = amount;
            diet.Frequency = frequency;
            diet.Notes = notes;

            try 
            {
                dbContext.SaveChanges();
                return Content("Diet ID #" + diet.DietId + " successfully updated.");
            }
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }
        }


        // GET: Diets/Delete
        // /Diets/Delete?dietId=USE_EXISTING_DIET_ID
        public ActionResult Delete(Guid dietId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            DietModel diet = dbContext.Diets.FirstOrDefault(x => x.DietId == dietId);

            if (diet == null) { return Content("Diet ID #" + dietId + " does not exist."); }

            try 
            {
                dbContext.Diets.Remove(diet);
                dbContext.SaveChanges();
                return Content("Diet ID #" + dietId + " has been successfully deleted.");
            }
            catch (Exception ex)
            {  
                return Content(ex.Message); 
            }
        }

    }
}
