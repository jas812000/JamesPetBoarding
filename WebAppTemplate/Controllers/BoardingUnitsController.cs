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
        // /BoardingUnits/Create?unitName=Texas&unitType=Kennel&speciesAllowed=Dog&sizeCategory=Lgarge&isActive=false&notes=floor%20being%20repaved
        public ActionResult Create(
            string unitName,
            string unitType,
            string speciesAllowed,
            string sizeCategory,
            bool isActive,
            string notes   
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (string.IsNullOrWhiteSpace(unitName)) { return Content("Unit name is required."); }
            if (string.IsNullOrWhiteSpace(unitType)) { return Content("Unit type is required."); }
            if (string.IsNullOrWhiteSpace(speciesAllowed)) { return Content("Species allowed in unit is required."); }
            if (string.IsNullOrWhiteSpace(sizeCategory)) { return Content("Size category of unit is required."); }

            BoardingUnitModel boardingUnit = new BoardingUnitModel();

            boardingUnit.UnitName = unitName;
            boardingUnit.UnitType = unitType;
            boardingUnit.SpeciesAllowed = speciesAllowed;
            boardingUnit.SizeCategory = sizeCategory;
            boardingUnit.IsActive = isActive;
            boardingUnit.Notes = notes;

            try 
            { 
                dbContext.BoardingUnits.Add(boardingUnit);
                dbContext.SaveChanges();
                
                return Content(boardingUnit.UnitName + " successfully added to the database.");
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

            if (boardingUnit == null) { return Content("Boarding Unit ID #" + boardingUnitId + " does not exist."); }

            string unitAvailable = boardingUnit.IsActive 
                ? "Yes" 
                : "No";

            string notesDisplay = string.IsNullOrWhiteSpace(boardingUnit.Notes)
                ? "No notes"
                : boardingUnit.Notes;

            return Content(
                "Boarding Unit ID #" + boardingUnit.BoardingUnitId + 
                "<br />Unit Name: " + boardingUnit.UnitName +
                "<br />Unit Type: " + boardingUnit.UnitType +
                "<br />Species Allowed: " + boardingUnit.SpeciesAllowed +
                "<br />Unit Size: " + boardingUnit.SizeCategory +
                "<br />Unit Available: " + unitAvailable +
                "<br />Notes: " + notesDisplay
            );
        }

        // GET: BoardingUnits/Update
        // /BoardingUnits/Update?boardingUnitId=USE_EXISTING_BOARDING_UNIT_ID&unitName=Texas&unitType=Kennel&speciesAllowed=Dog&sizeCategory=Large&isActive=true&notes=floor%20repaved
        public ActionResult Update(
            Guid boardingUnitId,
            string unitName,
            string unitType,
            string speciesAllowed,
            string sizeCategory,
            bool isActive,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingUnitModel boardingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitId);

            if (boardingUnit == null) { return Content("Boarding Unit ID #" + boardingUnitId + " does not exist."); }

            if (string.IsNullOrWhiteSpace(unitName)) { return Content("Unit name is required."); }
            if (string.IsNullOrWhiteSpace(unitType)) { return Content("Unit type is required."); }
            if (string.IsNullOrWhiteSpace(speciesAllowed)) { return Content("Species allowed in unit is required."); }
            if (string.IsNullOrWhiteSpace(sizeCategory)) { return Content("Size category of unit is required."); }

            boardingUnit.UnitName = unitName;
            boardingUnit.UnitType = unitType;
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

            if (boardingUnit == null) { return Content("Boarding Unit ID #" + boardingUnitId + " does not exist."); }

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
