using JamesPetBoarding.Models;
using JamesPetBoarding.Enums;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    public class BoardingsController : Controller
    {
        // GET: Boardings
        public ActionResult Index()
        {
            return View();
        }

        // GET: Boardings/Create
        // /Boardings/Create?customerId=USE_EXISTING_CUSTOMER_ID&petId=USE_EXISTING_PET_ID&boardingUnitId=USE_EXISTING_BOARDING_UNIT_ID&startDateTime=2026-06-10%2008:00:00&endDateTime=2026-06-15%2017:00:00&notes=
        public ActionResult Create(
            Guid customerId,
            Guid petId,
            Guid boardingUnitId,
            DateTime startDateTime,
            DateTime endDateTime,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer == null) 
            { 
                return Content("Customer ID #" + customerId + " does not exist."); 
            }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);
            if (pet == null) 
            { 
                return Content("Pet ID #" + petId + " does not exist."); 
            }

            CustomerPetModel customerPet = dbContext.CustomerPets.FirstOrDefault(x => x.CustomerId == customerId && x.PetId == petId);

            if (customerPet == null) 
            { 
                return Content("This customer is not associated with this pet."); 
            }

            BoardingUnitModel boardingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitId);
            if (boardingUnit == null) 
            { 
                return Content("Boarding Unit ID #" + boardingUnitId + " does not exist."); 
            }

            if (endDateTime < startDateTime) 
            { 
                return Content("End date cannot be before start date."); 
            }

            BoardingModel boarding = new BoardingModel();

            boarding.CustomerId = customerId;
            boarding.PetId = petId;
            boarding.BoardingUnitId = boardingUnitId;
            boarding.StartDateTime = startDateTime;
            boarding.EndDateTime = endDateTime;
            boarding.ActualCheckInDateTime = null;
            boarding.CheckedInByEmployeeId = null;
            boarding.ActualCheckOutDateTime = null;
            boarding.CheckedOutByEmployeeId = null;
            boarding.CancelledDateTime = null;
            boarding.CancelledByEmployeeId = null;
            boarding.CancelledReason = null;
            boarding.Status = BoardingStatusEnum.Scheduled;
            boarding.Notes = notes;

            try
            {
                dbContext.Boardings.Add(boarding);
                dbContext.SaveChanges();

                return Content("A Pet boarding was created.");

            }
            catch (Exception ex) 
            {
                return Content(ex.Message);
            }
        }

        // GET: Boardings/Read
        // /Boardings/Read?boardingId=USE_EXISTING_BOARDING_ID
        public ActionResult Read(Guid boardingId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingModel boarding = dbContext.Boardings.FirstOrDefault(x => x.BoardingId == boardingId);
            if (boarding == null) { return Content("Boarding ID #" + boardingId + " does not exist."); }

            string actualCheckInDateTimeDisplay = "Not checked in";
            string checkedInEmployeeDisplay = "n/a";
            string actualCheckOutDateTimeDisplay = "n/a";
            string checkedOutEmployeeDisplay = "n/a";
            string cancelledDateTimeDisplay = "n/a";
            string cancelledByEmployeeDisplay = "n/a";
            string cancelledReasonDisplay = "n/a";

            string statusDisplay = boarding.Status.ToString();

            switch (boarding.Status) 
            {

                case BoardingStatusEnum.Scheduled:
                    statusDisplay = "Scheduled";
                    break;

                case BoardingStatusEnum.Confirmed:
                    statusDisplay = "Confirmed";
                    break;

                case BoardingStatusEnum.NoShow:
                    statusDisplay = "No Show";
                    break;

                case BoardingStatusEnum.CheckedIn:
                    statusDisplay = "Checked In";
                    actualCheckInDateTimeDisplay = boarding.ActualCheckInDateTime.Value.ToString("MM/dd/yyyy hh:mm tt");
                    checkedInEmployeeDisplay = boarding.CheckedInByEmployeeId.Value.ToString();
                    actualCheckOutDateTimeDisplay = "Not checked out";
                    break;

                case BoardingStatusEnum.CheckedOut:
                    statusDisplay = "Checked Out";
                    actualCheckInDateTimeDisplay = boarding.ActualCheckInDateTime.Value.ToString("MM/dd/yyyy hh:mm tt");
                    checkedInEmployeeDisplay = boarding.CheckedInByEmployeeId.Value.ToString();
                    actualCheckOutDateTimeDisplay = boarding.ActualCheckOutDateTime.Value.ToString("MM/dd/yyyy hh:mm tt");
                    checkedOutEmployeeDisplay = boarding.CheckedOutByEmployeeId.Value.ToString();
                    break;

                case BoardingStatusEnum.Cancelled:
                    statusDisplay = "Cancelled";
                    cancelledDateTimeDisplay = boarding.CancelledDateTime.Value.ToString("MM/dd/yyyy hh:mm tt");
                    cancelledByEmployeeDisplay = boarding.CancelledByEmployeeId.Value.ToString();
                    cancelledReasonDisplay = boarding.CancelledReason;
                    break;

                default:
                    break;
            }

            string notesDisplay = string.IsNullOrWhiteSpace(boarding.Notes)
                ? "No notes"
                : boarding.Notes;

            return Content(
            "Boarding ID #" + boarding.BoardingId +
            "<br />Pet ID #" + boarding.PetId +
            "<br />Customer ID #" + boarding.CustomerId +
            "<br />Boarding Unit ID #" + boarding.BoardingUnitId +
            "<br />Start Date/Time: " + boarding.StartDateTime.ToString("MM/dd/yyyy") + 
            "<br />End Date/Time: " + boarding.EndDateTime.ToString("MM/dd/yyyy") +
            "<br />Actual Check-In Date/Time: " + actualCheckInDateTimeDisplay +
            "<br />Checked-In By Employee ID #" + checkedInEmployeeDisplay +
            "<br />Actual Check-Out Date/Time: " + actualCheckOutDateTimeDisplay +
            "<br />Checked-Out By Employee ID #" + checkedOutEmployeeDisplay +
            "<br />Cancelled Date/Time: " + cancelledDateTimeDisplay +
            "<br />Cancelled By Employee ID #" + cancelledByEmployeeDisplay +
            "<br />Cancellation Reason: " + cancelledReasonDisplay +
            "<br />Status: " + statusDisplay +
            "<br />Notes: " + notesDisplay
            );
        }


        // GET: Boardings/Update
        // /Boardings/Update?boardingId=USE_EXISTING_BOARDING_ID&customerId=USE_EXISTING_CUSTOMER_ID&petId=USE_EXISTING_PET_ID&boardingUnitId=USE_EXISTING_BOARDING_UNIT_ID&startDateTime=2026-06-10%2008:00:00&endDateTime=2026-06-15%2017:00:00&actualCheckInDateTime=2026-06-10%2008:15:00&checkedInByEmployeeId=USE_EXISTING_EMPLOYEE_ID&actualCheckOutDateTime=&checkedOutByEmployeeId=&cancelledDateTime=&cancelledByEmployeeId=&cancelledReason=&status=CheckedIn&notes=
        public ActionResult Update(
            Guid boardingId,
            Guid customerId,
            Guid petId,
            Guid boardingUnitId,
            DateTime startDateTime,
            DateTime endDateTime,
            DateTime? actualCheckInDateTime,
            Guid? checkedInByEmployeeId,
            DateTime? actualCheckOutDateTime,
            Guid? checkedOutByEmployeeId,
            DateTime? cancelledDateTime,
            Guid? cancelledByEmployeeId,
            string cancelledReason,
            BoardingStatusEnum status,
            string notes
        )
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingModel boarding = dbContext.Boardings.FirstOrDefault(x => x.BoardingId == boardingId);
            if (boarding == null) 
            { 
                return Content("Boarding ID #" + boardingId +" does not exist."); 
            }

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer == null) 
            { 
                return Content("Customer ID #" + customerId + " does not exist."); 
            }

            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null) 
            { 
                return Content("Pet ID #" + petId + " does not exist."); 
            }

            CustomerPetModel customerPet = dbContext.CustomerPets.FirstOrDefault(x => x.CustomerId == customerId && x.PetId == petId);

            if (customerPet == null) 
            { 
                return Content("This customer is not associated with this pet."); 
            }

            BoardingUnitModel boardingUnit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitId);

            if (boardingUnit == null) 
            { 
                return Content("Boarding Unit ID #" + boardingUnitId + " does not exist."); 
            }

            if (endDateTime < startDateTime) 
            { 
                return Content("End date cannot be before start date."); 
            }

            if (actualCheckInDateTime != null && 
                actualCheckOutDateTime != null && 
                actualCheckOutDateTime < actualCheckInDateTime) 
            { 
                return Content("Actual checkout date/time cannot be before actual checkin date/time."); 
            }

            if ((actualCheckInDateTime != null) && 
                (checkedInByEmployeeId == null)) 
            { 
                return Content("An employee needs to be selected for check in."); 
            }
            
            if (checkedInByEmployeeId != null)
            { 
                EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == checkedInByEmployeeId);

                if (employee == null) 
                {
                    return Content("Checked-in employee ID #" + checkedInByEmployeeId + " does not exist.");
                }
            }  

            if ((actualCheckOutDateTime != null) && 
                (checkedOutByEmployeeId == null)) 
            { 
                return Content("An employee needs to be selected for check out."); 
            }
            
            if (checkedOutByEmployeeId != null)
            {
                EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == checkedOutByEmployeeId);

                if (employee == null)
                {
                    return Content("Checked-out employee ID #" + checkedOutByEmployeeId + " does not exist.");
                }
            }
            
            if ((cancelledDateTime != null) && 
                (cancelledByEmployeeId == null)) 
            { 
                return Content("An employee needs to be selected for cancellation."); 
            }
 
            if (cancelledByEmployeeId != null)
            {
                EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == cancelledByEmployeeId);

                if (employee == null)
                {
                    return Content("Cancelled employee ID #" + cancelledByEmployeeId + " does not exist.");
                }
            }
            
            if (cancelledDateTime != null && 
                string.IsNullOrWhiteSpace(cancelledReason)) 
            {
                return Content("A cancellation reason needs to be selected."); 
            }

            if (status == BoardingStatusEnum.NoShow && 
                boarding.Status != BoardingStatusEnum.Scheduled && 
                boarding.Status != BoardingStatusEnum.Confirmed) 
            { 
                return Content("Only scheduled or confirmed boardings can be marked as no show."); 
            }


            if (status == BoardingStatusEnum.CheckedIn) 
            {
                if (actualCheckInDateTime == null) 
                { 
                    return Content("Check-in date and time are required."); 
                }

                if (checkedInByEmployeeId == null) 
                { 
                    return Content("Must select check-in employee."); 
                }
            }

            if (status == BoardingStatusEnum.CheckedOut && 
                boarding.Status != BoardingStatusEnum.CheckedIn) 
            { 
                return Content("Boarding must be checked in before it can be checked out."); 
            }

            if (status == BoardingStatusEnum.CheckedOut)
            {
                if (actualCheckInDateTime == null) 
                { 
                    return Content("Check-in date and time are required."); 
                }

                if (checkedInByEmployeeId == null) 
                { 
                    return Content("Must select check-in employee."); 
                }

                if (actualCheckOutDateTime == null) 
                { 
                    return Content("Check-out date and time are required."); 
                }

                if (checkedOutByEmployeeId == null) 
                { 
                    return Content("Must select check-out employee."); 
                }
            }

            if (status == BoardingStatusEnum.Cancelled)
            {
                if (cancelledDateTime == null) 
                { 
                    return Content("Cancellation date and time are required."); 
                }

                if (cancelledByEmployeeId == null) 
                { 
                    return Content("Must select cancellation employee."); 
                }

                if (string.IsNullOrWhiteSpace(cancelledReason)) 
                { 
                    return Content("A cancellation reason is required."); 
                }
            }

            boarding.CustomerId = customerId;
            boarding.PetId = petId;
            boarding.BoardingUnitId = boardingUnitId;
            boarding.StartDateTime = startDateTime;
            boarding.EndDateTime = endDateTime;
            boarding.ActualCheckInDateTime = actualCheckInDateTime;
            boarding.CheckedInByEmployeeId = checkedInByEmployeeId;
            boarding.ActualCheckOutDateTime = actualCheckOutDateTime;
            boarding.CheckedOutByEmployeeId = checkedOutByEmployeeId;
            boarding.CancelledDateTime = cancelledDateTime;
            boarding.CancelledByEmployeeId = cancelledByEmployeeId;
            boarding.CancelledReason = cancelledReason;
            boarding.Status = status;
            boarding.Notes = notes;

            try
            {
                dbContext.SaveChanges();
                return Content("Boarding ID #" + boarding.BoardingId + " was successfully updated.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: Boardings/Delete
        // /Boardings/Delete?boardingId=USE_EXISTING_BOARDING_ID
        public ActionResult Delete(Guid boardingId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingModel boarding = dbContext.Boardings.FirstOrDefault(x =>  x.BoardingId == boardingId);

            if (boarding == null) { return Content("Boarding ID #" + boardingId + " does not exist."); }

            List<InvoiceItemModel> invoiceItems = dbContext.InvoiceItems.Where(x => x.BoardingId == boardingId).ToList();

            try
            {
                if (invoiceItems.Count > 0) 
                { 
                    return Content("This boarding has invoice records and cannot be deleted."); 
                }

                dbContext.Boardings.Remove(boarding);
                dbContext.SaveChanges();
                return Content("Boarding ID #" + boarding.BoardingId + " was successfully deleted.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
