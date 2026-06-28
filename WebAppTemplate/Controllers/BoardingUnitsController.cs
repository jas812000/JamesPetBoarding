using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;

namespace JamesPetBoarding.Controllers
{
    public class BoardingUnitsController : Controller
    {
        // GET: BoardingUnits
        public ActionResult Index()
        {
            return View();
        }


        // GET: BoardingUnits/Create
        // /BoardingUnits/Create?unitName=Mercury&unitType=Kennel&unitNumber=1&speciesAllowed=Canine&sizeCategory=Large&isActive=true&notes=
        public ActionResult Create(
            UnitNameEnum unitName,
            UnitTypeEnum unitType,
            int unitNumber,
            SpeciesAllowedEnum speciesAllowed,
            SizeCategoryEnum sizeCategory,
            bool isActive,
            string notes   
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (unitNumber < 1 || unitNumber > 10)
            {
                return Content("Unit number must be between 1 and 10.");
            }

            BoardingUnitModel existingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.UnitName == unitName && x.UnitNumber == unitNumber);

            if (existingUnit != null)
            {
                return Content(unitName + "-" + unitNumber + " already exists.");
            }

            BoardingUnitModel boardingUnit = new BoardingUnitModel();

            boardingUnit.UnitName = unitName;
            boardingUnit.UnitType = unitType;
            boardingUnit.UnitNumber = unitNumber;
            boardingUnit.SpeciesAllowed = speciesAllowed;
            boardingUnit.SizeCategory = sizeCategory;
            boardingUnit.IsActive = isActive;
            boardingUnit.Notes = notes;

            try 
            { 
                dbContext.BoardingUnits.Add(boardingUnit);
                dbContext.SaveChanges();
                
                return Content(boardingUnit.UnitName + "-" + boardingUnit.UnitNumber + " successfully added to the database.");
            }
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }
        }


        // GET: BoardingUnits/Read
        // /BoardingUnits/Read?boardingUnitId=USE_EXISTING_BOARDING_UNIT_ID
        public ActionResult Read(Guid boardingUnitId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingUnitModel boardingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitId);

            if (boardingUnit == null) 
            { 
                return Content("Boarding Unit ID #" + boardingUnitId + " does not exist."); 
            }

            string unitAvailable = boardingUnit.IsActive 
                ? "Yes" 
                : "No";

            string notesDisplay = string.IsNullOrWhiteSpace(boardingUnit.Notes)
                ? "No notes"
                : boardingUnit.Notes;

            string unitTypeDisplay = boardingUnit.UnitType.ToString();
            string speciesAllowedDisplay = boardingUnit.SpeciesAllowed.ToString();
            string sizeCategoryDisplay = boardingUnit.SizeCategory.ToString();

            switch (boardingUnit.UnitType)
            {
                case UnitTypeEnum.BirdCage:
                    unitTypeDisplay = "Bird Cage";
                    break;

                case UnitTypeEnum.CatCondo:
                    unitTypeDisplay = "Cat Condo";
                    break;

                case UnitTypeEnum.SmallAnimalEnclosure:
                    unitTypeDisplay = "Small Animal Enclosure";
                    break;

                case UnitTypeEnum.LargeAnimalEnclosure:
                    unitTypeDisplay = "Large Animal Enclosure";
                    break;
            
            }

            switch (boardingUnit.SpeciesAllowed) 
            {
                case SpeciesAllowedEnum.SmallMammal:
                    speciesAllowedDisplay = "Small Mammal";
                    break;
            
            }
        
            switch (boardingUnit.SizeCategory) 
            {
                case SizeCategoryEnum.AnySize:
                    sizeCategoryDisplay = "Any Size";
                    break;
            
            }

            return Content(
                "Boarding Unit ID #" + boardingUnit.BoardingUnitId + 
                "<br />Unit: " + boardingUnit.UnitName + "-" + boardingUnit.UnitNumber +
                "<br />Unit Type: " + unitTypeDisplay +
                "<br />Species Allowed: " + speciesAllowedDisplay +
                "<br />Unit Size: " + sizeCategoryDisplay +
                "<br />Unit Available: " + unitAvailable +
                "<br />Notes: " + notesDisplay
            );
        }

        // GET: BoardingUnits/Update
        // /BoardingUnits/Update?boardingUnitId=USE_EXISTING_BOARDING_UNIT_ID&unitName=Mercury&unitType=Kennel&unitNumber=1&speciesAllowed=Canine&sizeCategory=Large&isActive=true&notes=
        public ActionResult Update(
            Guid boardingUnitId,
            UnitNameEnum unitName,
            UnitTypeEnum unitType,
            int unitNumber,
            SpeciesAllowedEnum speciesAllowed,
            SizeCategoryEnum sizeCategory,
            bool isActive,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingUnitModel boardingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitId);

            if (boardingUnit == null) 
            { 
                return Content("Boarding Unit ID #" + boardingUnitId + " does not exist."); 
            }

            if (unitNumber < 1 || unitNumber > 10)
            {
                return Content("Unit number must be between 1 and 10.");
            }

            BoardingUnitModel existingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId != boardingUnitId && x.UnitName == unitName && x.UnitNumber == unitNumber);

            if (existingUnit != null)
            {
                return Content(unitName + "-" + unitNumber + " already exists.");
            }

            boardingUnit.UnitName = unitName;
            boardingUnit.UnitType = unitType;
            boardingUnit.UnitNumber = unitNumber;
            boardingUnit.SpeciesAllowed = speciesAllowed;
            boardingUnit.SizeCategory = sizeCategory;
            boardingUnit.IsActive = isActive;
            boardingUnit.Notes = notes;

            try 
            { 
                dbContext.SaveChanges();
                return Content("Boarding Unit ID #" + boardingUnit.BoardingUnitId + " was successfully updated.");
            }
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }
        }


        // GET: BoardingUnits/Delete
        // /BoardingUnits/Delete?boardingUnitId=USE_EXISTING_BOARDING_UNIT_ID
        public ActionResult Delete(Guid boardingUnitId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingUnitModel boardingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitId);

            if (boardingUnit == null) 
            { 
                return Content("Boarding Unit ID #" + boardingUnitId + " does not exist."); 
            }

            List<BoardingModel> boardings = dbContext.Boardings.Where(x => x.BoardingUnitId == boardingUnitId).ToList();

            if (boardings.Count > 0)
            {
                return Content("This boarding unit has boarding records and cannot be deleted.");
            }
        
            try 
            {
                dbContext.BoardingUnits.Remove(boardingUnit);
                dbContext.SaveChanges();
                return Content("Boarding Unit ID #" + boardingUnit.BoardingUnitId + " was successfully removed.");
            }
            catch (Exception ex) 
            {  
                return Content(ex.Message); 
            }
        }
    }
}

