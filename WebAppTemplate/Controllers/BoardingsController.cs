using JamesPetBoarding.Models;
using JamesPetBoarding.Enums;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JamesPetBoarding.ViewModels;
using JamesPetBoarding.Migrations;

namespace JamesPetBoarding.Controllers
{
    [Authorize]
    public class BoardingsController : Controller
    {

        // GET: Boardings/Search
        public ActionResult Search() 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            ViewBag.CanManageBoardings = CanManageBoardings(currentEmployee);

            BoardingSearchVM boardingSearch = new BoardingSearchVM();

            boardingSearch.CustomerSelectList = BuildCustomerSelectList(dbContext);
            boardingSearch.PetSelectList = BuildPetSelectList(dbContext);
            boardingSearch.BoardingUnitSelectList = BuildBoardingUnitSelectList(dbContext);

            return View(boardingSearch);
        
        }


        // POST: Boardings/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(BoardingSearchVM boardingSearch) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            ViewBag.CanManageBoardings = CanManageBoardings(currentEmployee);

            List<BoardingModel> boardingQuery = dbContext.Boardings
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.BoardingUnit)
                .ToList();

            if (boardingSearch.CustomerId.HasValue)
            {
                boardingQuery = boardingQuery
                    .Where(x => x.CustomerId == boardingSearch.CustomerId.Value)
                    .ToList();
            }

            if (boardingSearch.PetId.HasValue)
            {
                boardingQuery = boardingQuery
                    .Where(x => x.PetId == boardingSearch.PetId.Value)
                    .ToList();
            }

            if (boardingSearch.BoardingUnitId.HasValue)
            {
                boardingQuery = boardingQuery
                    .Where(x => x.BoardingUnitId == boardingSearch.BoardingUnitId.Value)
                    .ToList();
            }

            if (boardingSearch.BoardingStatus.HasValue)
            {
                boardingQuery = boardingQuery
                    .Where(x => x.Status == boardingSearch.BoardingStatus.Value)
                    .ToList();
            }

            if (boardingSearch.StartDateTime.HasValue)
            {
                boardingQuery = boardingQuery
                    .Where(x => x.StartDateTime >= boardingSearch.StartDateTime.Value)
                    .ToList();
            }

            if (boardingSearch.EndDateTime.HasValue)
            {
                boardingQuery = boardingQuery
                    .Where(x => x.EndDateTime <= boardingSearch.EndDateTime.Value)
                    .ToList();
            }

            boardingSearch.BoardingSummaryResults = boardingQuery
                .OrderByDescending(x => x.StartDateTime)
                .Select(x => new BoardingSummaryVM
                {
                    BoardingId = x.BoardingId,

                    CustomerId = x.CustomerId,
                    CustomerNameDisplay = 
                        x.Customer.LastName + ", " + 
                        x.Customer.FirstName,
                    
                    PetId = x.PetId,
                    PetNameDisplay = x.Pet.PetName,

                    BoardingUnitId = x.BoardingUnitId,
                    BoardingUnitDisplay = 
                        x.BoardingUnit.UnitName + " - " + 
                        x.BoardingUnit.UnitNumber + " - " + 
                        x.BoardingUnit.UnitType,

                    BoardingStatus = x.Status,
                    StatusDisplay = x.Status.ToString(),

                    StartDateTimeDisplay = x.StartDateTime.ToString("MM/dd/yyyy h:mm tt"),

                    EndDateTimeDisplay = x.EndDateTime.ToString("MM/dd/yyyy h:mm tt")

                })
                .ToList();

            boardingSearch.CustomerSelectList = BuildCustomerSelectList(dbContext);
            boardingSearch.PetSelectList = BuildPetSelectList(dbContext);
            boardingSearch.BoardingUnitSelectList = BuildBoardingUnitSelectList(dbContext);

            return View(boardingSearch);

        }


        // GET: Boardings/Create
        public ActionResult Create() 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            BoardingFormVM boardingForm = new BoardingFormVM();
            boardingForm.CustomerSelectList = BuildCustomerSelectList(dbContext);
            boardingForm.PetSelectList = BuildPetSelectList(dbContext);
            boardingForm.BoardingUnitSelectList = BuildBoardingUnitSelectList(dbContext);

            return View (boardingForm);

        }


        // POST: Boardings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BoardingFormVM boardingForm)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            if (boardingForm.EndDateTime <= boardingForm.StartDateTime)
            {
                ModelState.AddModelError("EndDateTime", "End date and time must be later than the start date and time.");
            }

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == boardingForm.CustomerId);

            if (customer == null)
            {
                return Content("Customer ID #" + boardingForm.CustomerId + " does not exist.");
            }

            if (!customer.IsActive)
            {
                return Content("Customer ID #" + boardingForm.CustomerId + " is inactive.");
            }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == boardingForm.PetId);

            if (pet == null)
            {
                return Content("Pet ID #" + boardingForm.PetId + " does not exist.");
            }

            if (!pet.IsActive)
            {
                return Content("Pet ID #" + boardingForm.PetId + " is inactive.");
            }

            CustomerPetModel customerPet = dbContext.CustomerPets.FirstOrDefault(x => x.CustomerId == boardingForm.CustomerId && x.PetId == boardingForm.PetId);

            if (customerPet == null)
            {
                return Content("This customer is not associated with this pet.");
            }

            BoardingUnitModel boardingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingForm.BoardingUnitId);
            
            if (boardingUnit == null)
            {
                return Content("Boarding Unit ID #" + boardingForm.BoardingUnitId + " does not exist.");
            }

            if (!boardingUnit.IsActive)
            {
                return Content("Boarding Unit ID #" + boardingForm.BoardingUnitId + " is inactive.");
            }

            BoardingModel conflictingBoarding = dbContext.Boardings
                .FirstOrDefault(x => 
                    x.BoardingUnitId == boardingForm.BoardingUnitId && 
                    x.Status != BoardingStatusEnum.Cancelled &&
                    x.Status != BoardingStatusEnum.NoShow &&
                    x.StartDateTime < boardingForm.EndDateTime && 
                    x.EndDateTime > boardingForm.StartDateTime);

            if (conflictingBoarding != null)
            { 
                ModelState.AddModelError(
                    "BoardingUnitId", 
                    "The boarding unit is not available for the selected dates and times."); 
            }

            if (!ModelState.IsValid)
            {
                boardingForm.CustomerSelectList = BuildCustomerSelectList(dbContext);
                boardingForm.PetSelectList = BuildPetSelectList(dbContext);
                boardingForm.BoardingUnitSelectList = BuildBoardingUnitSelectList(dbContext);

                return View(boardingForm);
            }

            BoardingModel boarding = new BoardingModel();

            boarding.CustomerId = boardingForm.CustomerId;
            boarding.PetId = boardingForm.PetId;
            boarding.BoardingUnitId = boardingForm.BoardingUnitId;
            boarding.StartDateTime = boardingForm.StartDateTime;
            boarding.EndDateTime = boardingForm.EndDateTime;
            boarding.Notes = boardingForm.Notes;
            boarding.Status = BoardingStatusEnum.Scheduled;

            dbContext.Boardings.Add(boarding);
            dbContext.SaveChanges();

            return RedirectToAction("Read", new { boardingId = boarding.BoardingId });

        }


        // GET: Boardings/Read
        public ActionResult Read(Guid boardingId) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            BoardingModel boarding = dbContext.Boardings
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.BoardingUnit)
                .Include(x => x.CheckedInByEmployee)
                .Include(x => x.CheckedOutByEmployee)
                .Include(x => x.CancelledByEmployee)
                .Include(x => x.NoShowByEmployee)
                .FirstOrDefault(x => x.BoardingId == boardingId);

            if (boarding == null)
            {
                return Content("Boarding ID# " + boardingId + " does not exist.");
            }

            BoardingDetailsVM boardingDetails = new BoardingDetailsVM();

            boardingDetails.BoardingId = boarding.BoardingId;

            boardingDetails.CustomerId = boarding.CustomerId;

            boardingDetails.CustomerNameDisplay = 
                boarding.Customer.LastName + ", " + 
                boarding.Customer.FirstName;

            boardingDetails.PetId = boarding.PetId;

            boardingDetails.PetNameDisplay = boarding.Pet.PetName;

            boardingDetails.BoardingUnitId = boarding.BoardingUnitId;

            boardingDetails.BoardingUnitDisplay =
                boarding.BoardingUnit.UnitName + " - " +
                boarding.BoardingUnit.UnitNumber + " - " +
                boarding.BoardingUnit.UnitType;

            boardingDetails.BoardingStatus = boarding.Status;

            boardingDetails.StartDateTimeDisplay = boarding.StartDateTime.ToString("MM/dd/yyyy hh:mm tt");

            boardingDetails.EndDateTimeDisplay = boarding.EndDateTime.ToString("MM/dd/yyyy hh:mm tt");

            boardingDetails.StatusDisplay = boarding.Status.ToString();

            boardingDetails.ActualCheckInDateTimeDisplay = 
                boarding.ActualCheckInDateTime.HasValue
                    ? boarding.ActualCheckInDateTime.Value.ToString("MM/dd/yyyy hh:mm tt")
                    : "Not checked in";

            boardingDetails.CheckedInByEmployeeNameDisplay = 
                boarding.CheckedInByEmployee == null 
                    ? "Not checked in"
                    : boarding.CheckedInByEmployee.FirstName + " " + boarding.CheckedInByEmployee.LastName;

            boardingDetails.ActualCheckOutDateTimeDisplay = boarding.ActualCheckOutDateTime.HasValue
                ? boarding.ActualCheckOutDateTime.Value.ToString("MM/dd/yyyy hh:mm tt")
                : "Not checked out";

            boardingDetails.CheckedOutByEmployeeNameDisplay = 
                boarding.CheckedOutByEmployee == null   
                    ? "Not checked out" 
                    : boarding.CheckedOutByEmployee.FirstName + " " + boarding.CheckedOutByEmployee.LastName;

            boardingDetails.CancelledDateTimeDisplay = boarding.CancelledDateTime.HasValue 
                ? boarding.CancelledDateTime.Value.ToString("MM/dd/yyyy hh:mm tt")
                : "Not cancelled";

            boardingDetails.CancelledByEmployeeNameDisplay = 
                boarding.CancelledByEmployee == null 
                    ? "Not cancelled" 
                    : boarding.CancelledByEmployee.FirstName + " " + boarding.CancelledByEmployee.LastName;

            boardingDetails.CancelledReasonDisplay = 
                string.IsNullOrWhiteSpace(boarding.CancelledReason) 
                    ? "No cancellation reason"
                    : boarding.CancelledReason;

            boardingDetails.NoShowDateTimeDisplay = boarding.NoShowDateTime.HasValue
                ? boarding.NoShowDateTime.Value.ToString("MM/dd/yyyy hh:mm tt")
                : "Not marked as a no-show";

            boardingDetails.NoShowByEmployeeNameDisplay =
                boarding.NoShowByEmployee == null
                    ? "Not marked as a no-show"
                    : boarding.NoShowByEmployee.FirstName + " " + boarding.NoShowByEmployee.LastName;

            boardingDetails.NotesDisplay = string.IsNullOrWhiteSpace(boarding.Notes)
                ? "No notes"
                : boarding.Notes;

            return View(boardingDetails);

        }


        // GET: Boardings/Update
        public ActionResult Update(Guid boardingId) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            BoardingModel boarding = dbContext.Boardings
               .Include(x => x.Customer)
               .Include(x => x.Pet)
               .Include(x => x.BoardingUnit)
               .FirstOrDefault(x => x.BoardingId == boardingId);

            if (boarding == null)
            {
                return Content("Boarding ID# " + boardingId + " does not exist.");
            }

            BoardingFormVM boardingForm = new BoardingFormVM();

            boardingForm.BoardingId = boarding.BoardingId;

            boardingForm.CustomerId = boarding.CustomerId;

            boardingForm.CustomerNameDisplay =
                boarding.Customer.LastName + ", " +
                boarding.Customer.FirstName;

            boardingForm.PetId = boarding.PetId;

            boardingForm.PetNameDisplay = boarding.Pet.PetName;

            boardingForm.BoardingUnitId = boarding.BoardingUnitId;

            boardingForm.BoardingUnitDisplay =
                boarding.BoardingUnit.UnitName + " - " +
                boarding.BoardingUnit.UnitNumber + " - " +
                boarding.BoardingUnit.UnitType;

            boardingForm.StartDateTime = boarding.StartDateTime;

            boardingForm.EndDateTime = boarding.EndDateTime; 

            boardingForm.Notes = boarding.Notes;

            boardingForm.CustomerSelectList = BuildCustomerSelectList(dbContext);

            boardingForm.PetSelectList = BuildPetSelectList(dbContext);

            boardingForm.BoardingUnitSelectList = BuildBoardingUnitSelectList(dbContext);

            return View(boardingForm);

        }


        // POST: Boardings/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(BoardingFormVM boardingForm) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            BoardingModel boarding = dbContext.Boardings
               .FirstOrDefault(x => x.BoardingId == boardingForm.BoardingId);

            if (boarding == null)
            {
                return Content("Boarding ID# " + boardingForm.BoardingId + " does not exist.");
            }

            if (boardingForm.EndDateTime <= boardingForm.StartDateTime)
            {
                ModelState.AddModelError(
                    "EndDateTime", 
                    "End date and time must be later than the start date and time.");
            }

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == boardingForm.CustomerId);
            if (customer == null)
            {
                return Content("Customer ID #" + boardingForm.CustomerId + " does not exist.");
            }

            if (!customer.IsActive)
            {
                return Content("Customer ID #" + boardingForm.CustomerId + " is inactive.");
            }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == boardingForm.PetId);
            if (pet == null)
            {
                return Content("Pet ID #" + boardingForm.PetId + " does not exist.");
            }

            if (!pet.IsActive)
            {
                return Content("Pet ID #" + boardingForm.PetId + " is inactive.");
            }

            CustomerPetModel customerPet = dbContext.CustomerPets.FirstOrDefault(x => x.CustomerId == boardingForm.CustomerId && x.PetId == boardingForm.PetId);

            if (customerPet == null)
            {
                return Content("This customer is not associated with this pet.");
            }

            BoardingUnitModel boardingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingForm.BoardingUnitId);
            if (boardingUnit == null)
            {
                return Content("Boarding Unit ID #" + boardingForm.BoardingUnitId + " does not exist.");
            }

            if (!boardingUnit.IsActive)
            {
                return Content("Boarding Unit ID #" + boardingForm.BoardingUnitId + " is inactive.");
            }

            BoardingModel conflictingBoarding = dbContext.Boardings
                .FirstOrDefault(x =>
                    x.BoardingId != boardingForm.BoardingId &&
                    x.BoardingUnitId == boardingForm.BoardingUnitId &&
                    x.Status != BoardingStatusEnum.Cancelled &&
                    x.Status != BoardingStatusEnum.NoShow &&
                    x.StartDateTime < boardingForm.EndDateTime &&
                    x.EndDateTime > boardingForm.StartDateTime);

            if (conflictingBoarding != null)
            {
                ModelState.AddModelError(
                    "BoardingUnitId",
                    "The boarding unit is not available for the selected dates and times.");
            }

            if (!ModelState.IsValid)
            {
                boardingForm.CustomerSelectList = BuildCustomerSelectList(dbContext);
                boardingForm.PetSelectList = BuildPetSelectList(dbContext);
                boardingForm.BoardingUnitSelectList = BuildBoardingUnitSelectList(dbContext);

                return View(boardingForm);
            }

            boarding.CustomerId = boardingForm.CustomerId;
            boarding.PetId = boardingForm.PetId;
            boarding.BoardingUnitId = boardingForm.BoardingUnitId;
            boarding.StartDateTime = boardingForm.StartDateTime;
            boarding.EndDateTime = boardingForm.EndDateTime;
            boarding.Notes = boardingForm.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { boardingId = boarding.BoardingId });

        }


        // GET: Boardings/Cancel
        public ActionResult Cancel(Guid boardingId) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            BoardingModel boarding = dbContext.Boardings
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.BoardingUnit)
                .FirstOrDefault(x => x.BoardingId == boardingId);

            if (boarding == null)
            {
                return Content("Boarding ID# " + boardingId + " does not exist.");
            }

            if (boarding.Status == BoardingStatusEnum.Cancelled || 
                boarding.Status == BoardingStatusEnum.CheckedOut || 
                boarding.Status == BoardingStatusEnum.NoShow)
            { 
                return Content("Boarding ID #" + boarding.BoardingId + " cannot be cancelled."); 
            }

            BoardingCancelVM boardingCancel = new BoardingCancelVM();

            boardingCancel.BoardingId = boarding.BoardingId;

            boardingCancel.CustomerNameDisplay =
                boarding.Customer.LastName + ", " +
                boarding.Customer.FirstName;

            boardingCancel.PetNameDisplay = boarding.Pet.PetName;

            boardingCancel.BoardingUnitDisplay =
                boarding.BoardingUnit.UnitName + " - " +
                boarding.BoardingUnit.UnitNumber + " - " +
                boarding.BoardingUnit.UnitType;

            boardingCancel.BoardingStatus = boarding.Status;

            boardingCancel.StatusDisplay = boarding.Status.ToString();

            boardingCancel.StartDateTimeDisplay = boarding.StartDateTime.ToString("MM/dd/yyyy hh:mm tt");

            boardingCancel.EndDateTimeDisplay = boarding.EndDateTime.ToString("MM/dd/yyyy hh:mm tt");

            return View(boardingCancel);
                
        }


        // POST: Boardings/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(BoardingCancelVM boardingCancel)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            BoardingModel boarding = dbContext.Boardings
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.BoardingUnit)
                .FirstOrDefault(x => x.BoardingId == boardingCancel.BoardingId);

            if (boarding == null)
            {
                return Content("Boarding ID #" + boardingCancel.BoardingId + " does not exist.");
            }

            if (boarding.Status == BoardingStatusEnum.Cancelled ||
                boarding.Status == BoardingStatusEnum.CheckedOut ||
                boarding.Status == BoardingStatusEnum.NoShow)
            {
                return Content("Boarding ID #" + boarding.BoardingId + " cannot be cancelled.");
            }

            if (!ModelState.IsValid)
            {
                boardingCancel.CustomerNameDisplay =
                    boarding.Customer.LastName + ", " +
                    boarding.Customer.FirstName;

                boardingCancel.PetNameDisplay = boarding.Pet.PetName;

                boardingCancel.BoardingUnitDisplay =
                    boarding.BoardingUnit.UnitName + " - " +
                    boarding.BoardingUnit.UnitNumber + " - " +
                    boarding.BoardingUnit.UnitType;

                boardingCancel.BoardingStatus = boarding.Status;

                boardingCancel.StatusDisplay = boarding.Status.ToString();

                boardingCancel.StartDateTimeDisplay = boarding.StartDateTime.ToString("MM/dd/yyyy hh:mm tt");

                boardingCancel.EndDateTimeDisplay = boarding.EndDateTime.ToString("MM/dd/yyyy hh:mm tt");

                return View(boardingCancel);
            }

            boarding.Status = BoardingStatusEnum.Cancelled;
            boarding.CancelledDateTime = DateTime.Now;
            boarding.CancelledByEmployeeId = currentEmployee.EmployeeId;
            boarding.CancelledReason = boardingCancel.CancelledReason;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { boardingId = boarding.BoardingId });

        }


        // GET: Boardings/CheckIn
        public ActionResult CheckIn(Guid boardingId) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            BoardingModel boarding = dbContext.Boardings
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.BoardingUnit)
                .FirstOrDefault(x => x.BoardingId == boardingId);

            if (boarding == null)
            {
                return Content("Boarding ID #" + boardingId + " does not exist.");
            }

            if (boarding.Status != BoardingStatusEnum.Scheduled &&
                boarding.Status != BoardingStatusEnum.Confirmed)
            {
                return Content("Boarding ID #" + boarding.BoardingId + " cannot be checked in.");
            }

            BoardingCheckInVM boardingCheckIn = new BoardingCheckInVM();

            boardingCheckIn.BoardingId = boarding.BoardingId;

            boardingCheckIn.CustomerId = boarding.CustomerId;

            boardingCheckIn.CustomerNameDisplay =
                boarding.Customer.LastName + ", " +
                boarding.Customer.FirstName;

            boardingCheckIn.PetId = boarding.PetId;

            boardingCheckIn.PetNameDisplay = boarding.Pet.PetName;

            boardingCheckIn.BoardingUnitId = boarding.BoardingUnitId;

            boardingCheckIn.BoardingUnitDisplay =
                boarding.BoardingUnit.UnitName + " - " +
                boarding.BoardingUnit.UnitNumber + " - " +
                boarding.BoardingUnit.UnitType;

            boardingCheckIn.BoardingStatus = boarding.Status;

            boardingCheckIn.StatusDisplay = boarding.Status.ToString();

            boardingCheckIn.StartDateTimeDisplay = boarding.StartDateTime.ToString("MM/dd/yyyy hh:mm tt");

            boardingCheckIn.EndDateTimeDisplay = boarding.EndDateTime.ToString("MM/dd/yyyy hh:mm tt");

            boardingCheckIn.Notes = boarding.Notes;

            return View(boardingCheckIn);
        }


        // POST: Boardings/CheckIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(BoardingCheckInVM boardingCheckIn) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            BoardingModel boarding = dbContext.Boardings
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.BoardingUnit)
                .FirstOrDefault(x => x.BoardingId == boardingCheckIn.BoardingId);

            if (boarding == null)
            {
                return Content("Boarding ID #" + boardingCheckIn.BoardingId + " does not exist.");
            }

            if (boarding.Status != BoardingStatusEnum.Scheduled &&
                boarding.Status != BoardingStatusEnum.Confirmed)
            {
                return Content("Boarding ID #" + boarding.BoardingId + " cannot be checked in.");
            }
 
            if (!ModelState.IsValid)
            {
                boardingCheckIn.BoardingId = boarding.BoardingId;

                boardingCheckIn.CustomerId = boarding.CustomerId;
                boardingCheckIn.CustomerNameDisplay =
                    boarding.Customer.LastName + ", " +
                    boarding.Customer.FirstName;

                boardingCheckIn.PetId = boarding.PetId;
                boardingCheckIn.PetNameDisplay = boarding.Pet.PetName;

                boardingCheckIn.BoardingUnitId = boarding.BoardingUnitId;
                boardingCheckIn.BoardingUnitDisplay =
                    boarding.BoardingUnit.UnitName + " - " +
                    boarding.BoardingUnit.UnitNumber + " - " +
                    boarding.BoardingUnit.UnitType;

                boardingCheckIn.BoardingStatus = boarding.Status;
                boardingCheckIn.StatusDisplay = boarding.Status.ToString();

                boardingCheckIn.StartDateTimeDisplay = boarding.StartDateTime.ToString("MM/dd/yyyy hh:mm tt");

                boardingCheckIn.EndDateTimeDisplay = boarding.EndDateTime.ToString("MM/dd/yyyy hh:mm tt");

                return View(boardingCheckIn);
            }


            boarding.ActualCheckInDateTime = DateTime.Now;

            boarding.CheckedInByEmployeeId = currentEmployee.EmployeeId;

            boarding.Status = BoardingStatusEnum.CheckedIn;

            boarding.Notes = boardingCheckIn.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { boardingId = boarding.BoardingId });

        }


        // GET: Boardings/CheckOut
        public ActionResult CheckOut(Guid boardingId) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            BoardingModel boarding = dbContext.Boardings
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.BoardingUnit)
                .FirstOrDefault(x => x.BoardingId == boardingId);

            if (boarding == null)
            {
                return Content("Boarding ID #" + boardingId + " does not exist.");
            }

            if (boarding.Status != BoardingStatusEnum.CheckedIn)
            {
                return Content("Boarding ID #" + boarding.BoardingId + " cannot be checked out.");
            }

            BoardingCheckOutVM boardingCheckOut = new BoardingCheckOutVM();

            boardingCheckOut.BoardingId = boarding.BoardingId;

            boardingCheckOut.CustomerId = boarding.CustomerId;

            boardingCheckOut.CustomerNameDisplay =
                boarding.Customer.LastName + ", " +
                boarding.Customer.FirstName;

            boardingCheckOut.PetId = boarding.PetId;

            boardingCheckOut.PetNameDisplay = boarding.Pet.PetName;

            boardingCheckOut.BoardingUnitId = boarding.BoardingUnitId;

            boardingCheckOut.BoardingUnitDisplay =
                boarding.BoardingUnit.UnitName + " - " +
                boarding.BoardingUnit.UnitNumber + " - " +
                boarding.BoardingUnit.UnitType;

            boardingCheckOut.BoardingStatus = boarding.Status;

            boardingCheckOut.StatusDisplay = boarding.Status.ToString();

            boardingCheckOut.StartDateTimeDisplay = boarding.StartDateTime.ToString("MM/dd/yyyy hh:mm tt");

            boardingCheckOut.EndDateTimeDisplay = boarding.EndDateTime.ToString("MM/dd/yyyy hh:mm tt");

            boardingCheckOut.ActualCheckInDateTimeDisplay = boarding.ActualCheckInDateTime.HasValue
                ? boarding.ActualCheckInDateTime.Value.ToString("MM/dd/yyyy hh:mm tt")
                : "Not checked in";

            boardingCheckOut.Notes = boarding.Notes;

            return View(boardingCheckOut);

        }


        // POST: Boardings/CheckOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(BoardingCheckOutVM boardingCheckOut) 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            BoardingModel boarding = dbContext.Boardings
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.BoardingUnit)
                .FirstOrDefault(x => x.BoardingId == boardingCheckOut.BoardingId);

            if (boarding == null)
            {
                return Content("Boarding ID #" + boardingCheckOut.BoardingId + " does not exist.");
            }

            if (boarding.Status != BoardingStatusEnum.CheckedIn)
            {
                return Content("Boarding ID #" + boarding.BoardingId + " cannot be checked out.");
            }

            DateTime actualCheckOutDateTime = DateTime.Now;

            if (boarding.ActualCheckInDateTime.HasValue && 
                actualCheckOutDateTime <= boarding.ActualCheckInDateTime.Value)
            {
                ModelState.AddModelError(
                    "",
                    "The checkout date and time must be later than the checkin date and time.");
            }

            if (!ModelState.IsValid)
            {
                boardingCheckOut.BoardingId = boarding.BoardingId;

                boardingCheckOut.CustomerId = boarding.CustomerId;
                boardingCheckOut.CustomerNameDisplay =
                    boarding.Customer.LastName + ", " +
                    boarding.Customer.FirstName;

                boardingCheckOut.PetId = boarding.PetId;
                boardingCheckOut.PetNameDisplay = boarding.Pet.PetName;

                boardingCheckOut.BoardingUnitId = boarding.BoardingUnitId;
                boardingCheckOut.BoardingUnitDisplay =
                    boarding.BoardingUnit.UnitName + " - " +
                    boarding.BoardingUnit.UnitNumber + " - " +
                    boarding.BoardingUnit.UnitType;

                boardingCheckOut.BoardingStatus = boarding.Status;
                boardingCheckOut.StatusDisplay = boarding.Status.ToString();

                boardingCheckOut.StartDateTimeDisplay = boarding.StartDateTime.ToString("MM/dd/yyyy hh:mm tt");

                boardingCheckOut.EndDateTimeDisplay = boarding.EndDateTime.ToString("MM/dd/yyyy hh:mm tt");

                boardingCheckOut.ActualCheckInDateTimeDisplay = boarding.ActualCheckInDateTime.HasValue
                    ? boarding.ActualCheckInDateTime.Value.ToString("MM/dd/yyyy hh:mm tt")
                    : "Not checked in";

                return View(boardingCheckOut);
            }


            boarding.ActualCheckOutDateTime = actualCheckOutDateTime;

            boarding.CheckedOutByEmployeeId = currentEmployee.EmployeeId;

            boarding.Status = BoardingStatusEnum.CheckedOut;

            boarding.Notes = boardingCheckOut.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { boardingId = boarding.BoardingId });

        }


        // GET: Boardings/NoShow
        public ActionResult NoShow(Guid boardingId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            BoardingModel boarding = dbContext.Boardings
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.BoardingUnit)
                .FirstOrDefault(x => x.BoardingId == boardingId);

            if (boarding == null)
            {
                return Content("Boarding ID #" + boardingId + " does not exist.");
            }

            if (boarding.Status != BoardingStatusEnum.Scheduled && 
                boarding.Status != BoardingStatusEnum.Confirmed)
            {
                return Content("Boarding ID #" + boarding.BoardingId + " cannot be marked as a no-show.");
            }

            BoardingNoShowVM boardingNoShow = new BoardingNoShowVM();

            boardingNoShow.BoardingId = boarding.BoardingId;

            boardingNoShow.CustomerNameDisplay =
                boarding.Customer.LastName + ", " +
                boarding.Customer.FirstName;

            boardingNoShow.PetNameDisplay = boarding.Pet.PetName;

            boardingNoShow.BoardingUnitDisplay =
                boarding.BoardingUnit.UnitName + " - " +
                boarding.BoardingUnit.UnitNumber + " - " +
                boarding.BoardingUnit.UnitType;

            boardingNoShow.BoardingStatus = boarding.Status;

            boardingNoShow.StatusDisplay = boarding.Status.ToString();

            boardingNoShow.StartDateTimeDisplay = boarding.StartDateTime.ToString("MM/dd/yyyy hh:mm tt");

            boardingNoShow.EndDateTimeDisplay = boarding.EndDateTime.ToString("MM/dd/yyyy hh:mm tt");

            boardingNoShow.Notes = boarding.Notes;

            return View(boardingNoShow);

        }


        // POST: Boardings/NoShow
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NoShow(BoardingNoShowVM boardingNoShow)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageBoardings(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            BoardingModel boarding = dbContext.Boardings
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.BoardingUnit)
                .FirstOrDefault(x => x.BoardingId == boardingNoShow.BoardingId);

            if (boarding == null)
            {
                return Content("Boarding ID #" + boardingNoShow.BoardingId + " does not exist.");
            }

            if (boarding.Status != BoardingStatusEnum.Scheduled &&
                boarding.Status != BoardingStatusEnum.Confirmed)
            {
                return Content("Boarding ID #" + boarding.BoardingId + " cannot be marked as a no-show.");
            }

            if (!ModelState.IsValid)
            {
                boardingNoShow.BoardingId = boarding.BoardingId;

                boardingNoShow.CustomerNameDisplay =
                    boarding.Customer.LastName + ", " +
                    boarding.Customer.FirstName;

                boardingNoShow.PetNameDisplay = boarding.Pet.PetName;

                boardingNoShow.BoardingUnitDisplay =
                    boarding.BoardingUnit.UnitName + " - " +
                    boarding.BoardingUnit.UnitNumber + " - " +
                    boarding.BoardingUnit.UnitType;

                boardingNoShow.BoardingStatus = boarding.Status;
                boardingNoShow.StatusDisplay = boarding.Status.ToString();

                boardingNoShow.StartDateTimeDisplay = boarding.StartDateTime.ToString("MM/dd/yyyy hh:mm tt");

                boardingNoShow.EndDateTimeDisplay = boarding.EndDateTime.ToString("MM/dd/yyyy hh:mm tt");

                return View(boardingNoShow);
            }

            boarding.Status = BoardingStatusEnum.NoShow;

            boarding.NoShowDateTime = DateTime.Now;

            boarding.NoShowByEmployeeId = currentEmployee.EmployeeId;

            boarding.Notes = boardingNoShow.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { boardingId = boarding.BoardingId });
        }


        private List<SelectListItem> BuildCustomerSelectList(ApplicationDbContext dbContext)
        {
            return dbContext.Customers
                .Where(x => x.IsActive)
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToList()
                .Select(x => new SelectListItem
                {
                    Value = x.CustomerId.ToString(),
                    Text = $"{x.LastName}, {x.FirstName}",
                })
                .ToList();
        }

        private List<SelectListItem> BuildPetSelectList(ApplicationDbContext dbContext)
        {
            return dbContext.Pets
                .Where(x => x.IsActive)
                .OrderBy(x => x.PetName)
                .ToList()
                .Select(x => new SelectListItem
                {
                    Value = x.PetId.ToString(),
                    Text = $"{x.PetName} - {x.Species} - {x.Breed}",
                })
                .ToList();
        }

        private List<SelectListItem> BuildBoardingUnitSelectList(ApplicationDbContext dbContext)
        {
            return dbContext.BoardingUnits
                .Where(x => x.IsActive)
                .OrderBy(x => x.UnitName)
                .ThenBy(x => x.UnitNumber)
                .ToList()
                .Select(x => new SelectListItem
                {
                    Value = x.BoardingUnitId.ToString(),
                    Text = $"{x.UnitName} - {x.UnitNumber} - {x.UnitType}",
                })
                .ToList();
        }


        private EmployeeModel GetCurrentEmployee(ApplicationDbContext dbContext)
        {
            string loggedInEmail = User.Identity.Name;

            return dbContext.Employees
                .FirstOrDefault(x =>
                    x.Email == loggedInEmail &&
                    x.IsActive);

        }


        private bool CanViewBoardings(EmployeeModel employee)
        {
            return employee != null &&
                (employee.Role == EmployeeRoleEnum.Admin ||
                 employee.Role == EmployeeRoleEnum.Manager ||
                 employee.Role == EmployeeRoleEnum.Supervisor ||
                 employee.Role == EmployeeRoleEnum.FrontDesk ||
                 employee.Role == EmployeeRoleEnum.KennelStaff ||
                 employee.Role == EmployeeRoleEnum.Caretaker ||
                 employee.Role == EmployeeRoleEnum.Groomer ||
                 employee.Role == EmployeeRoleEnum.VeterinaryTechnician ||
                 employee.Role == EmployeeRoleEnum.Veterinarian ||
                 employee.Role == EmployeeRoleEnum.Trainer);
        }


        private bool CanManageBoardings(EmployeeModel employee)
        {
            return employee != null &&
                (employee.Role == EmployeeRoleEnum.Admin ||
                 employee.Role == EmployeeRoleEnum.Manager ||
                 employee.Role == EmployeeRoleEnum.Supervisor ||
                 employee.Role == EmployeeRoleEnum.FrontDesk ||
                 employee.Role == EmployeeRoleEnum.KennelStaff ||
                 employee.Role == EmployeeRoleEnum.Caretaker ||
                 employee.Role == EmployeeRoleEnum.Groomer ||
                 employee.Role == EmployeeRoleEnum.VeterinaryTechnician ||
                 employee.Role == EmployeeRoleEnum.Veterinarian ||
                 employee.Role == EmployeeRoleEnum.Trainer);
        }
    }
}
