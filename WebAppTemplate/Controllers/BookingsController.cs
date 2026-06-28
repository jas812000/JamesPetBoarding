/*
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    public class BookingsController : Controller
    {
        // GET: Bookings
        public ActionResult Index()
        {
            return View();
        }

        // /Bookings/Add?customerId=6d163d10-e86c-4957-b5ff-b38addc6d9cc&petId=5DAB7E6F-1B32-474A-A963-409D21D349FE&boardingUnitId=e3c69ba4-f693-44fd-8e1c-f86190437605&startDateTime=03%2F01%2F2026&endDateTime=04%2F15%2F2026&status=Booked&notes=No%20issues
        public ActionResult Add(
            Guid customerId, Guid petId, Guid boardingUnitId, 
            DateTime startDateTime, DateTime endDateTime, 
            DateTime? actualCheckInDateTime, Guid? checkedInByEmployeeId, 
            DateTime? actualCheckOutDateTime, Guid? checkedOutByEmployeeId, 
            DateTime? cancelledDateTime, Guid? cancelledByEmployeeId, 
            string cancelledReason, string status, string notes)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (customerId == Guid.Empty) { return Content("A customer is needed for the booking."); }
            if (petId == Guid.Empty) { return Content("A pet is needed for the booking."); }
            if (boardingUnitId == Guid.Empty) { return Content("A boarding unit is needed for the booking."); }

            if (startDateTime < DateTime.Today) { return Content("The booking start date cannot be in the past."); }
            if (endDateTime <= startDateTime) { return Content("Verify the booking dates."); }

            if (actualCheckInDateTime != null && checkedInByEmployeeId == null) { return Content("An employee ID is required when a booking is checked in."); }
            if (actualCheckOutDateTime != null && checkedOutByEmployeeId == null) { return Content("An employee ID is required when a booking is checked out."); }

            if (cancelledDateTime != null && cancelledByEmployeeId == null) { return Content("An employee ID is required when a booking is cancelled."); }
            if (cancelledDateTime != null && string.IsNullOrWhiteSpace(cancelledReason)) { return Content("A cancellation reason is required when a booking is cancelled."); }

            // Test Customer
            // Delete once Customer controller is built
            CustomerModel customer = dbContext.Customers.FirstOrDefault(x=> x.CustomerId == customerId);

            if (customer == null) 
            {
                customer = new CustomerModel
                {
                    CustomerId = customerId,
                    LastName = "Stevens",
                    FirstName = "James",
                    Address = "123 Main Street",
                    City = "Dallas",
                    State = "TX",
                    ZipCode = "75221",
                    Phone = "8175551234",
                    Email = "james.stevens@email.com",
                    IsActive = true,
                    Notes = "Temporary test customer"
                };

                dbContext.Customers.Add(customer);
            }

            // Test Pet
            // Delete once Pet controller is built
            PetModel pet = dbContext.Pets.FirstOrDefault(x=> x.PetId == petId);

            if (pet == null)
            {
                pet = new PetModel 
                { 
                    PetId = petId, 
                    VetId = null, 
                    Name = "Steve", 
                    Species = "Dog", 
                    Breed = "German Shepard", 
                    Sex = "Male", 
                    BirthDate = new DateTime(2020, 12, 12), 
                    Age = 5, 
                    Weight = 61.8m, 
                    Notes = "Temporary test pet"
                };

                dbContext.Pets.Add(pet);
            }

            // Test Boarding Unit
            // Delete once BoardingUnit controller is built
            BoardingUnitModel unit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitId);

            if (unit == null) 
            {
                unit = new BoardingUnitModel
                {
                    BoardingUnitId = boardingUnitId,
                    UnitName = "A1",
                    UnitType = "Kennel",
                    SpeciesAllowed = "Dog",
                    SizeCategory = "Large",
                    IsActive = true
                };
                dbContext.BoardingUnits.Add(unit);
            }
                
            // Test cases added to database
            // Delete once Customer and BoardingUnit controller are built
            dbContext.SaveChanges();

            BoardingModel boarding = new BoardingModel();

            boarding.BoardingId = Guid.NewGuid();
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
                dbContext.Boardings.Add(boarding);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return Content("Successfully added booking.");
        }

        // /Bookings/Edit?boardingId=31f80330-e472-4681-b1b4-f47ee2061247&customerId=9f8aabc3-2c59-4558-ba43-892afbd9321e&petId=bf8cae55-5b23-4bd8-9f49-dae711a29695&boardingUnitId=d9fc5155-18ee-4adb-a6e3-92585202ae7d&startDateTime=07%2F15%2F2026&endDateTime=09%2F01%2F2026&status=Booked&notes=Any%20issues,%20send%20email;%20to%20customer.%20Emergencies,%20call%20emergency%20contact
        public ActionResult Edit(
            Guid boardingId, Guid customerId, Guid petId, Guid boardingUnitId,
            DateTime startDateTime, DateTime endDateTime,
            DateTime? actualCheckInDateTime, Guid? checkedInByEmployeeId,
            DateTime? actualCheckOutDateTime, Guid? checkedOutByEmployeeId,
            DateTime? cancelledDateTime, Guid? cancelledByEmployeeId,
            string cancelledReason, string status, string notes)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            // Test Customer
            // Delete once Customer controller is built
            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);

            if (customer == null)
            {
                customer = new CustomerModel
                {
                    CustomerId = customerId,
                    LastName = "Stevens",
                    FirstName = "James",
                    Address = "123 Main Street",
                    City = "Dallas",
                    State = "TX",
                    ZipCode = "75221",
                    Phone = "8175551234",
                    Email = "james.stevens@email.com",
                    IsActive = true,
                    Notes = "Temporary test customer"
                };

                dbContext.Customers.Add(customer);
            }

            // Test Pet
            // Delete once Pet controller is built
            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null)
            {
                pet = new PetModel
                {
                    PetId = petId,
                    VetId = null,
                    Name = "Steve",
                    Species = "Dog",
                    Breed = "German Shepard",
                    Sex = "Male",
                    BirthDate = new DateTime(2020, 12, 12),
                    Age = 5,
                    Weight = 61.8m,
                    Notes = "Temporary test pet"
                };

                dbContext.Pets.Add(pet);
            }

            // Test Boarding Unit
            // Delete once BoardingUnit controller is built
            BoardingUnitModel unit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitId);

            if (unit == null)
            {
                unit = new BoardingUnitModel
                {
                    BoardingUnitId = boardingUnitId,
                    UnitName = "A1",
                    UnitType = "Kennel",
                    SpeciesAllowed = "Dog",
                    SizeCategory = "Large",
                    IsActive = true
                };
                dbContext.BoardingUnits.Add(unit);
            }

            // Test Booking
            // Delete once Boarding controller is built
            BoardingModel boarding = dbContext.Boardings.FirstOrDefault(x => x.BoardingId == boardingId);

            if (boarding == null) 
            {
                boarding = new BoardingModel
                {
                    BoardingId = boardingId,
                    CustomerId = customerId,
                    PetId = petId,
                    BoardingUnitId = boardingUnitId,
                    StartDateTime = startDateTime,
                    EndDateTime = endDateTime,
                    Status = "Booked",
                    Notes = "Temporary test booking for edit"
                };
                dbContext.Boardings.Add(boarding);

                // Uncomment below once validation is complete
                //return Content("Boarding ID #" + boardingId + " does not exist.");
            }
            // Delete once related controllers are built
            dbContext.SaveChanges();


            if (customerId == Guid.Empty) { return Content("A customer is needed for the booking."); }
            if (petId == Guid.Empty) { return Content("A pet is needed for the booking."); }
            if (boardingUnitId == Guid.Empty) { return Content("A boarding unit is needed for the booking."); }

            // Uncomment below once validation is complete
            //CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);
            //PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);
            //BoardingUnitModel unit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitId);

            if (customer == null) { return Content("Customer ID #" + customerId + " does not exist."); }
            if (pet == null) { return Content("Pet ID #" + petId + " does not exist."); }
            if (unit == null) { return Content("Boarding Unit ID #" + boardingUnitId + " does not exist."); }

            if (startDateTime < DateTime.Today) { return Content("The booking start date cannot be in the past."); }
            if (endDateTime <= startDateTime) { return Content("Verify the booking dates."); }

            if (actualCheckInDateTime != null && checkedInByEmployeeId == null) { return Content("An employee ID is required when a booking is checked in."); }
            if (actualCheckOutDateTime != null && checkedOutByEmployeeId == null) { return Content("An employee ID is required when a booking is checked out."); }

            if (cancelledDateTime != null && cancelledByEmployeeId == null) { return Content("An employee ID is required when a booking is cancelled."); }
            if (cancelledDateTime != null && string.IsNullOrWhiteSpace(cancelledReason)) { return Content("A cancellation reason is required when a booking is cancelled."); }

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

            try { 
                dbContext.SaveChanges(); 
                return Content("Successfully updated boarding ID #" + boardingId + ".");
            }
            catch (Exception ex) 
            { 
                return Content(ex.Message); 
            }
        }

        // /Bookings/Remove?boardingId=2589b987-46b0-4372-a164-ced50db0b195
        public ActionResult Remove(Guid boardingId) 
        { 
            ApplicationDbContext dbContext = new ApplicationDbContext();

            // Test GUIDs
            // Delete once validation is complete
            Guid customerId = Guid.Parse("9f8aabc3-2c59-4558-ba43-892afbd9321e");
            Guid petId = Guid.Parse("bf8cae55-5b23-4bd8-9f49-dae711a29695");
            Guid boardingUnitId = Guid.Parse("d9fc5155-18ee-4adb-a6e3-92585202ae7d");


            // Test Customer
            // Delete once Customer controller is built
            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);

            if (customer == null)
            {
                customer = new CustomerModel
                {
                    CustomerId = customerId,
                    LastName = "Stevens",
                    FirstName = "James",
                    Address = "123 Main Street",
                    City = "Dallas",
                    State = "TX",
                    ZipCode = "75221",
                    Phone = "8175551234",
                    Email = "james.stevens@email.com",
                    IsActive = true,
                    Notes = "Temporary test customer"
                };

                dbContext.Customers.Add(customer);
            }

            // Test Pet
            // Delete once Pet controller is built
            PetModel pet = dbContext.Pets.FirstOrDefault(x => x.PetId == petId);

            if (pet == null)
            {
                pet = new PetModel
                {
                    PetId = petId,
                    VetId = null,
                    Name = "Steve",
                    Species = "Dog",
                    Breed = "German Shepard",
                    Sex = "Male",
                    BirthDate = new DateTime(2020, 12, 12),
                    Age = 5,
                    Weight = 61.8m,
                    Notes = "Temporary test pet"
                };

                dbContext.Pets.Add(pet);
            }

            // Test Boarding Unit
            // Delete once BoardingUnit controller is built
            BoardingUnitModel unit = dbContext.BoardingUnits.FirstOrDefault(x => x.BoardingUnitId == boardingUnitId);

            if (unit == null)
            {
                unit = new BoardingUnitModel
                {
                    BoardingUnitId = boardingUnitId,
                    UnitName = "A1",
                    UnitType = "Kennel",
                    SpeciesAllowed = "Dog",
                    SizeCategory = "Large",
                    IsActive = true
                };
                dbContext.BoardingUnits.Add(unit);
            }

            // Test Booking
            // Delete once Boarding controller is built
            BoardingModel boarding = dbContext.Boardings.FirstOrDefault(x => x.BoardingId == boardingId);

            if (boarding == null)
            {
                boarding = new BoardingModel
                {
                    BoardingId = boardingId,
                    CustomerId = customerId,
                    PetId = petId,
                    BoardingUnitId = boardingUnitId,
                    StartDateTime = new DateTime(2026, 7, 15),
                    EndDateTime = new DateTime(2026, 9, 15),
                    Status = "Booked",
                    Notes = "Temporary test booking for edit"
                };
                dbContext.Boardings.Add(boarding);

                // Uncomment below once validation is complete
                //return Content("Boarding ID #" + boardingId + " does not exist.");
            }
            // Delete once related controllers are built
            dbContext.SaveChanges();


            // Uncomment below once validation is complete
            //BoardingModel boarding = dbContext.Boardings.FirstOrDefault(x => x.BoardingId == boardingId);

            if (boarding != null) 
            {
                try
                {
                    dbContext.Boardings.Remove(boarding);
                    dbContext.SaveChanges();
                    return Content("Boarding Id #" + boardingId + " successfully deleted.");
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }
            else 
            {
                return Content("Boarding Id #" + boardingId + " does not exist.");
            }
        }
    }
}
*/