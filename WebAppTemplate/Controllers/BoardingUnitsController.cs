using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
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

        // GET: BoardingUnits/Search
        public ActionResult Search()
        {
            BoardingUnitSearchVM boardingUnitSearch = new BoardingUnitSearchVM();

            return View(boardingUnitSearch);
        }


        // POST: BoardingUnits/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(BoardingUnitSearchVM boardingUnitSearch)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (!ModelState.IsValid)
            {
                return View(boardingUnitSearch);
            }

            List<BoardingUnitModel> boardingUnits = dbContext.BoardingUnits.ToList();

            if (boardingUnitSearch.UnitType.HasValue)
            {
                boardingUnits = boardingUnits
                    .Where(x => x.UnitType == boardingUnitSearch.UnitType.Value)
                    .ToList();
            }

            if (boardingUnitSearch.UnitName.HasValue)
            {
                boardingUnits = boardingUnits
                    .Where(x => x.UnitName == boardingUnitSearch.UnitName.Value)
                    .ToList();
            }

            if (boardingUnitSearch.UnitNumber.HasValue)
            {
                boardingUnits = boardingUnits
                    .Where(x => x.UnitNumber == boardingUnitSearch.UnitNumber.Value)
                    .ToList();
            }

            if (boardingUnitSearch.SpeciesAllowed.HasValue)
            {
                boardingUnits = boardingUnits
                    .Where(x => x.SpeciesAllowed == boardingUnitSearch.SpeciesAllowed.Value)
                    .ToList();
            }

            if (boardingUnitSearch.SizeCategory.HasValue)
            {
                boardingUnits = boardingUnits
                    .Where(x => x.SizeCategory == boardingUnitSearch.SizeCategory.Value)
                    .ToList();
            }

            if (boardingUnitSearch.IsActive.HasValue)
            {
                boardingUnits = boardingUnits
                    .Where(x => x.IsActive == boardingUnitSearch.IsActive.Value)
                    .ToList();
            }

            boardingUnitSearch.BoardingUnitResults.Clear();

            foreach (BoardingUnitModel boardingUnit in boardingUnits)
            {
                boardingUnitSearch.BoardingUnitResults.Add(new BoardingUnitSummaryVM
                {
                    BoardingUnitId = boardingUnit.BoardingUnitId,
                    UnitTypeDisplay = boardingUnit.UnitType.ToString(),
                    FullUnitNameDisplay = boardingUnit.UnitName + "-" + boardingUnit.UnitNumber,
                    SpeciesAllowedDisplay = boardingUnit.SpeciesAllowed.ToString(),
                    SizeCategoryDisplay = boardingUnit.SizeCategory.ToString(),
                    IsActive = boardingUnit.IsActive,
                    ActiveStatusDisplay = boardingUnit.IsActive
                        ? "Active"
                        : "Inactive"
                });
            }

            return View(boardingUnitSearch);
        }


        // GET: BoardingUnits/Create
        public ActionResult Create()
        {
            BoardingUnitFormVM boardingUnitForm = new BoardingUnitFormVM();

            return View(boardingUnitForm);

        }


        // POST: BoardingUnits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BoardingUnitFormVM boardingUnitForm)
        {
            if (!ModelState.IsValid)
            {
                return View(boardingUnitForm);
            }

            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingUnitModel existingUnit = dbContext.BoardingUnits
                .FirstOrDefault(x =>
                    x.UnitName == boardingUnitForm.UnitName &&
                    x.UnitNumber == boardingUnitForm.UnitNumber
                );

            if (existingUnit != null)
            {
                ModelState.AddModelError("",
                    boardingUnitForm.UnitName + "-" +
                    boardingUnitForm.UnitNumber +
                    " already exists.");

                return View(boardingUnitForm);
            }

            BoardingUnitModel boardingUnit = new BoardingUnitModel();

            boardingUnit.UnitType = boardingUnitForm.UnitType;
            boardingUnit.UnitName = boardingUnitForm.UnitName;
            boardingUnit.UnitNumber = boardingUnitForm.UnitNumber;
            boardingUnit.SpeciesAllowed = boardingUnitForm.SpeciesAllowed;
            boardingUnit.SizeCategory = boardingUnitForm.SizeCategory;
            boardingUnit.Notes = boardingUnitForm.Notes;

            dbContext.BoardingUnits.Add(boardingUnit);
            dbContext.SaveChanges();

            return RedirectToAction("Read", new { boardingUnitId = boardingUnit.BoardingUnitId });

        }


        // GET: BoardingUnits/Read
        public ActionResult Read(Guid boardingUnitId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingUnitModel boardingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitId);

            if (boardingUnit == null)
            {
                return Content("Boarding Unit ID #" + boardingUnitId + " does not exist.");
            }

            BoardingUnitDetailsVM boardingUnitDetails = new BoardingUnitDetailsVM();

            boardingUnitDetails.BoardingUnitId = boardingUnit.BoardingUnitId;
            boardingUnitDetails.UnitTypeDisplay = boardingUnit.UnitType.ToString();
            boardingUnitDetails.FullUnitNameDisplay = boardingUnit.UnitName + "-" + boardingUnit.UnitNumber;
            boardingUnitDetails.SpeciesAllowedDisplay = boardingUnit.SpeciesAllowed.ToString();
            boardingUnitDetails.SizeCategoryDisplay = boardingUnit.SizeCategory.ToString();
            boardingUnitDetails.IsActive = boardingUnit.IsActive;
            boardingUnitDetails.ActiveStatusDisplay = boardingUnit.IsActive
                ? "Active"
                : "Inactive";
            boardingUnitDetails.Notes = string.IsNullOrWhiteSpace(boardingUnit.Notes)
                ? "No notes"
                : boardingUnit.Notes;

            return View(boardingUnitDetails);
        }


        // GET: BoardingUnits/Update
        public ActionResult Update(Guid boardingUnitId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingUnitModel boardingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitId);

            if (boardingUnit == null)
            {
                return Content("Boarding Unit ID #" + boardingUnitId + " does not exist.");
            }

            BoardingUnitFormVM boardingUnitForm = new BoardingUnitFormVM();

            boardingUnitForm.BoardingUnitId = boardingUnit.BoardingUnitId;
            boardingUnitForm.UnitType = boardingUnit.UnitType;
            boardingUnitForm.UnitName = boardingUnit.UnitName;
            boardingUnitForm.UnitNumber = boardingUnit.UnitNumber;
            boardingUnitForm.SpeciesAllowed = boardingUnit.SpeciesAllowed;
            boardingUnitForm.SizeCategory = boardingUnit.SizeCategory;
            boardingUnitForm.Notes = boardingUnit.Notes;

            return View(boardingUnitForm);

        }


        // POST: BoardingUnits/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(BoardingUnitFormVM boardingUnitForm)

        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingUnitModel boardingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitForm.BoardingUnitId);

            if (boardingUnit == null)
            {
                return Content("Boarding Unit ID #" + boardingUnitForm.BoardingUnitId + " does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return View(boardingUnitForm);
            }

            BoardingUnitModel existingUnit = dbContext.BoardingUnits
                .FirstOrDefault(x =>
                    x.BoardingUnitId != boardingUnitForm.BoardingUnitId &&
                    x.UnitName == boardingUnitForm.UnitName &&
                    x.UnitNumber == boardingUnitForm.UnitNumber);

            if (existingUnit != null)
            {
                ModelState.AddModelError("",
                    boardingUnitForm.UnitName + "-" +
                    boardingUnitForm.UnitNumber +
                    " already exists.");

                return View(boardingUnitForm);
            }

            boardingUnit.UnitType = boardingUnitForm.UnitType;
            boardingUnit.UnitName = boardingUnitForm.UnitName;
            boardingUnit.UnitNumber = boardingUnitForm.UnitNumber;
            boardingUnit.SpeciesAllowed = boardingUnitForm.SpeciesAllowed;
            boardingUnit.SizeCategory = boardingUnitForm.SizeCategory;
            boardingUnit.Notes = boardingUnitForm.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { boardingUnitId = boardingUnit.BoardingUnitId });

        }







        // GET: BoardingUnits/Delete
        public ActionResult Delete(Guid boardingUnitId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingUnitModel boardingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitId);

            if (boardingUnit == null)
            {
                return Content("Boarding Unit ID #" + boardingUnitId + " does not exist.");
            }

            BoardingUnitDeleteVM boardingUnitDelete = new BoardingUnitDeleteVM();

            boardingUnitDelete.BoardingUnitId = boardingUnit.BoardingUnitId;
            boardingUnitDelete.UnitTypeDisplay = boardingUnit.UnitType.ToString();
            boardingUnitDelete.FullUnitNameDisplay = boardingUnit.UnitName + "-" + boardingUnit.UnitNumber;
            boardingUnitDelete.SpeciesAllowedDisplay = boardingUnit.SpeciesAllowed.ToString();
            boardingUnitDelete.SizeCategoryDisplay = boardingUnit.SizeCategory.ToString();
            boardingUnitDelete.IsActive = boardingUnit.IsActive;
            boardingUnitDelete.ActiveStatusDisplay = boardingUnit.IsActive 
                ? "Active"
                : "Inactive";
            boardingUnitDelete.Notes = string.IsNullOrWhiteSpace(boardingUnit.Notes)
                ? "No notes"
                : boardingUnit.Notes;

            return View(boardingUnitDelete);
        }


        // POST: BoardingUnits/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(BoardingUnitDeleteVM boardingUnitDelete)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingUnitModel boardingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitDelete.BoardingUnitId);

            if (boardingUnit == null) 
            { 
                return Content("Boarding Unit ID #" + boardingUnitDelete.BoardingUnitId + " does not exist."); 
            }

            int boardingCount = dbContext.Boardings.Count(x => x.BoardingUnitId == boardingUnitDelete.BoardingUnitId);

            if (boardingCount > 0)
            {
                ModelState.AddModelError("", 
                    "This boarding unit has " + 
                    boardingCount + 
                    " boarding record(s) and cannot be deleted.");

                boardingUnitDelete.BoardingUnitId = boardingUnit.BoardingUnitId;
                boardingUnitDelete.UnitTypeDisplay = boardingUnit.UnitType.ToString();
                boardingUnitDelete.FullUnitNameDisplay = boardingUnit.UnitName + "-" + boardingUnit.UnitNumber;
                boardingUnitDelete.SpeciesAllowedDisplay = boardingUnit.SpeciesAllowed.ToString();
                boardingUnitDelete.SizeCategoryDisplay = boardingUnit.SizeCategory.ToString();
                boardingUnitDelete.IsActive = boardingUnit.IsActive;
                boardingUnitDelete.ActiveStatusDisplay = boardingUnit.IsActive
                    ? "Active"
                    : "Inactive";
                boardingUnitDelete.Notes = string.IsNullOrWhiteSpace(boardingUnit.Notes)
                    ? "No notes"
                    : boardingUnit.Notes;

                return View(boardingUnitDelete);
            }
        
            dbContext.BoardingUnits.Remove(boardingUnit);
            dbContext.SaveChanges();

            return RedirectToAction("Search");
        }
    }
}

