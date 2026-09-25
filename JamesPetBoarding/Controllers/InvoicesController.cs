using JamesPetBoarding.Enums;
using JamesPetBoarding.Migrations;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    [Authorize]
    public class InvoicesController : Controller
    {

        // GET: Invoices/Search
        public ActionResult Search()
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            InvoiceSearchVM invoiceSearch = new InvoiceSearchVM();

            invoiceSearch.CustomerSelectList =
                BuildCustomerSelectList(dbContext, includeInactive: true);

            invoiceSearch.PetSelectList =
                BuildPetSelectList(dbContext, includeInactive: true);

            invoiceSearch.BoardingSelectList =
                BuildBoardingSelectList(dbContext, includeInactive: true);

            return View(invoiceSearch);
        }


        // POST: Invoices/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(InvoiceSearchVM invoiceSearch)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            List<InvoiceModel> invoiceQuery = dbContext.Invoices
               .Include(x => x.Customer)
               .Include(x => x.Pet)
               .Include(x => x.Boarding)
               .Include(x => x.Boarding.BoardingUnit)
               .ToList();

            if (invoiceSearch.CustomerId.HasValue)
            {
                invoiceQuery = invoiceQuery
                    .Where(x => x.CustomerId == invoiceSearch.CustomerId.Value)
                    .ToList();
            }

            if (invoiceSearch.PetId.HasValue)
            {
                invoiceQuery = invoiceQuery
                    .Where(x => x.PetId == invoiceSearch.PetId.Value)
                    .ToList();
            }

            if (invoiceSearch.BoardingId.HasValue)
            {
                invoiceQuery = invoiceQuery
                    .Where(x => x.BoardingId == invoiceSearch.BoardingId.Value)
                    .ToList();
            }

            if (invoiceSearch.InvoiceStatus.HasValue)
            {
                invoiceQuery = invoiceQuery
                    .Where(x => x.InvoiceStatus == invoiceSearch.InvoiceStatus.Value)
                    .ToList();
            }

            invoiceSearch.InvoiceSearchResults = invoiceQuery
                .OrderByDescending(x => x.InvoiceDateTime)
                .Select(x => new InvoiceSummaryVM
                {
                    InvoiceId = x.InvoiceId,

                    CustomerNameDisplay =
                        x.Customer.LastName + ", " +
                        x.Customer.FirstName,

                    PetNameDisplay = x.Pet.PetName,

                    InvoiceDateTimeDisplay = x.InvoiceDateTime.ToString("MM/dd/yyyy h:mm tt"),

                    InvoiceStatus = x.InvoiceStatus,

                    StatusDisplay = GetEnumDisplayName(x.InvoiceStatus),

                    BoardingId = x.BoardingId,

                    BoardingDisplay = x.Boarding == null
                        ? "No Boarding(s)"
                        : $"{x.Boarding.BoardingUnit.UnitName} - " +
                            $"{x.Boarding.BoardingUnit.UnitNumber} - " +
                            $"{x.Boarding.StartDateTime:MM/dd/yyyy} - " +
                            $"{x.Boarding.EndDateTime:MM/dd/yyyy}",

                    TotalAmountDisplay = x.TotalAmount.ToString("C"),

                    BalanceDisplay = x.Balance.ToString("C"),

                })
                .ToList();

            invoiceSearch.CustomerSelectList =
                BuildCustomerSelectList(
                    dbContext,
                    invoiceSearch.PetId,
                    includeInactive: true);

            invoiceSearch.PetSelectList =
                BuildPetSelectList(
                    dbContext,
                    invoiceSearch.CustomerId,
                    includeInactive: true);

            invoiceSearch.BoardingSelectList =
                BuildBoardingSelectList(
                    dbContext,
                    invoiceSearch.CustomerId,
                    invoiceSearch.PetId,
                    includeInactive: true);

            if (invoiceSearch.BoardingId.HasValue)
            {
                invoiceSearch.BoardingSelectList =
                    invoiceSearch.BoardingSelectList
                        .Where(x =>
                            x.Value == invoiceSearch.BoardingId.Value.ToString())
                        .ToList();
            }

            return View(invoiceSearch);

        }


        // GET: Invoices/GetInvoiceSelections
        public JsonResult GetInvoiceSelections(
            Guid? customerId,
            Guid? petId,
            Guid? boardingId,
            bool includeInactive = false)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return Json(
                    new { Error = "Unable to load invoice selections." },
                    JsonRequestBehavior.AllowGet);
            }

            if (boardingId.HasValue)
            {
                BoardingModel selectedBoarding = dbContext.Boardings
                    .Include(x => x.Customer)
                    .Include(x => x.Pet)
                    .FirstOrDefault(x => x.BoardingId == boardingId.Value);

                if (selectedBoarding == null)
                {
                    return Json(
                        new { Error = "The selected boarding no longer exists." },
                        JsonRequestBehavior.AllowGet);
                }

                if (!includeInactive &&
                    (!selectedBoarding.Customer.IsActive ||
                     !selectedBoarding.Pet.IsActive))
                {
                    return Json(
                        new
                        {
                            Error = "This boarding cannot be used because " +
                                    "its customer or pet is inactive."
                        },
                        JsonRequestBehavior.AllowGet);
                }

                customerId = selectedBoarding.CustomerId;
                petId = selectedBoarding.PetId;
            }

            List<SelectListItem> boardings =
                BuildBoardingSelectList(
                    dbContext,
                    customerId,
                    petId,
                    includeInactive);

            if (boardingId.HasValue)
            {
                boardings = boardings
                    .Where(x => x.Value == boardingId.Value.ToString())
                    .ToList();
            }

            return Json(
                new
                {
                    CustomerId = customerId,
                    PetId = petId,
                    BoardingId = boardingId,
                    Customers = BuildCustomerSelectList(
                        dbContext,
                        petId,
                        includeInactive),
                    Pets = BuildPetSelectList(
                        dbContext,
                        customerId,
                        includeInactive),
                    Boardings = boardings
                },
                JsonRequestBehavior.AllowGet);
        }


        // GET: Invoices/Create
        public ActionResult Create()
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            InvoiceFormVM invoiceForm = new InvoiceFormVM();

            invoiceForm.CustomerSelectList = BuildCustomerSelectList(dbContext);
            invoiceForm.PetSelectList = BuildPetSelectList(dbContext);
            invoiceForm.BoardingSelectList = BuildBoardingSelectList(dbContext);

            return View(invoiceForm);

        }


        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InvoiceFormVM invoiceForm)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            if (!ModelState.IsValid)
            {
                PopulateInvoiceFormSelectLists(dbContext, invoiceForm);

                return View(invoiceForm);
            }

            CustomerModel customer = dbContext.Customers
                .FirstOrDefault(x => x.CustomerId == invoiceForm.CustomerId);

            if (customer == null)
            {
                ModelState.AddModelError(
                    "CustomerId",
                    "The selected customer does not exist.");
            }
            else if (!customer.IsActive)
            {
                ModelState.AddModelError(
                    "CustomerId",
                    "The selected customer is inactive.");
            }

            PetModel pet = dbContext.Pets
                .FirstOrDefault(x => x.PetId == invoiceForm.PetId);

            if (pet == null)
            {
                ModelState.AddModelError(
                    "PetId",
                    "The selected pet does not exist.");
            }
            else if (!pet.IsActive)
            {
                ModelState.AddModelError(
                    "PetId",
                    "The selected pet is inactive.");
            }

            CustomerPetModel customerPet = dbContext.CustomerPets
                    .FirstOrDefault(x =>
                        x.CustomerId == invoiceForm.CustomerId &&
                        x.PetId == invoiceForm.PetId);

            if (customerPet == null)
            {
                ModelState.AddModelError(
                    "PetId",
                    "The selected pet is not associated " +
                    "with the selected customer.");
            }

            if (invoiceForm.BoardingId.HasValue)
            {
                BoardingModel boarding = dbContext.Boardings
                        .FirstOrDefault(x => x.BoardingId == invoiceForm.BoardingId.Value);

                if (boarding == null)
                {
                    ModelState.AddModelError("BoardingId", "The selected boarding does not exist.");
                }
                else if (
                    boarding.CustomerId != invoiceForm.CustomerId || boarding.PetId != invoiceForm.PetId)
                {
                    ModelState.AddModelError(
                        "BoardingId",
                        "The selected boarding does not match " +
                        "the selected customer and pet.");
                }
            }

            if (!ModelState.IsValid)
            {
                PopulateInvoiceFormSelectLists(dbContext, invoiceForm);

                return View(invoiceForm);
            }

            InvoiceModel invoice = new InvoiceModel();

            invoice.CustomerId = invoiceForm.CustomerId;

            invoice.PetId = invoiceForm.PetId;

            invoice.BoardingId = invoiceForm.BoardingId;

            invoice.InvoiceDateTime = DateTime.Now;

            invoice.InvoiceStatus = InvoiceStatusEnum.Draft;

            invoice.Subtotal = 0m;
            invoice.TaxAmount = 0m;
            invoice.DiscountAmount = 0m;
            invoice.TotalAmount = 0m;
            invoice.AmountPaid = 0m;
            invoice.Balance = 0m;

            invoice.Notes = invoiceForm.Notes;

            dbContext.Invoices.Add(invoice);

            dbContext.SaveChanges();

            return RedirectToAction(
                "Read",
                new
                {
                    invoiceId = invoice.InvoiceId
                });
        }


        // GET: Invoices/Read
        public ActionResult Read(Guid invoiceId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            InvoiceDetailsVM invoiceDetails = BuildInvoiceDetails(dbContext, invoiceId);

            if (invoiceDetails == null)
            {
                return Content("Invoice ID #" + invoiceId + " does not exist.");
            }

            return View(invoiceDetails);

        }


        // GET: Invoices/Update
        public ActionResult Update(Guid invoiceId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            InvoiceModel invoice = dbContext.Invoices.FirstOrDefault(x => x.InvoiceId == invoiceId);

            if (invoice == null)
            {
                return Content("Invoice ID #" + invoiceId + " does not exist.");
            }

            if (invoice.InvoiceStatus == InvoiceStatusEnum.Void)
            {
                return Content("This invoice is void and cannot be updated.");
            }

            InvoiceFormVM invoiceForm = new InvoiceFormVM();

            invoiceForm.InvoiceId = invoice.InvoiceId;

            invoiceForm.CustomerId = invoice.CustomerId;

            invoiceForm.PetId = invoice.PetId;

            invoiceForm.BoardingId = invoice.BoardingId;

            invoiceForm.Notes = invoice.Notes;

            invoiceForm.CustomerSelectList = BuildCustomerSelectList(dbContext);
            invoiceForm.PetSelectList = BuildPetSelectList(dbContext, invoice.CustomerId);
            invoiceForm.BoardingSelectList = BuildBoardingSelectList(dbContext, invoice.CustomerId, invoice.PetId);

            return View(invoiceForm);

        }


        // POST: Invoices/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(InvoiceFormVM invoiceForm)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            InvoiceModel invoice = dbContext.Invoices
                .FirstOrDefault(x => x.InvoiceId == invoiceForm.InvoiceId);

            if (invoice == null)
            {
                return Content("Invoice ID #" + invoiceForm.InvoiceId + " does not exist.");
            }

            if (invoice.InvoiceStatus == InvoiceStatusEnum.Void)
            {
                return Content("This invoice is void and cannot be updated.");
            }

            if (!ModelState.IsValid)
            {
                PopulateInvoiceFormSelectLists(dbContext, invoiceForm);

                return View(invoiceForm);
            }

            CustomerModel customer = dbContext.Customers
                .FirstOrDefault(x => x.CustomerId == invoiceForm.CustomerId);

            if (customer == null)
            {
                ModelState.AddModelError(
                    "CustomerId",
                    "The selected customer does not exist.");
            }
            else if (!customer.IsActive)
            {
                ModelState.AddModelError(
                    "CustomerId",
                    "The selected customer is inactive.");
            }

            PetModel pet = dbContext.Pets
                .FirstOrDefault(x => x.PetId == invoiceForm.PetId);

            if (pet == null)
            {
                ModelState.AddModelError(
                    "PetId",
                    "The selected pet does not exist.");
            }
            else if (!pet.IsActive)
            {
                ModelState.AddModelError(
                    "PetId",
                    "The selected pet is inactive.");
            }

            CustomerPetModel customerPet = dbContext.CustomerPets
                .FirstOrDefault(x =>
                    x.CustomerId == invoiceForm.CustomerId &&
                    x.PetId == invoiceForm.PetId);

            if (customerPet == null)
            {
                ModelState.AddModelError(
                    "PetId",
                    "The selected pet is not associated with the selected customer.");
            }

            if (invoiceForm.BoardingId.HasValue)
            {
                BoardingModel boarding = dbContext.Boardings
                    .FirstOrDefault(x => x.BoardingId == invoiceForm.BoardingId.Value);

                if (boarding == null)
                {
                    ModelState.AddModelError(
                        "BoardingId",
                        "The selected boarding does not exist.");
                }
                else if (boarding.CustomerId != invoiceForm.CustomerId ||
                         boarding.PetId != invoiceForm.PetId)
                {
                    ModelState.AddModelError(
                        "BoardingId",
                        "The selected boarding does not match the selected customer and pet.");
                }
            }

            if (!ModelState.IsValid)
            {
                PopulateInvoiceFormSelectLists(dbContext, invoiceForm);

                return View(invoiceForm);
            }

            invoice.CustomerId = invoiceForm.CustomerId;
            invoice.PetId = invoiceForm.PetId;
            invoice.BoardingId = invoiceForm.BoardingId;
            invoice.Notes = invoiceForm.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { invoiceId = invoice.InvoiceId });
        }


        // GET: Invoices/Void
        public ActionResult Void(Guid invoiceId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            InvoiceModel invoice = dbContext.Invoices
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.Boarding)
                .Include(x => x.Boarding.BoardingUnit)
                .FirstOrDefault(x => x.InvoiceId == invoiceId);

            if (invoice == null)
            {
                return Content("Invoice ID #" + invoiceId + " does not exist.");
            }

            if (invoice.InvoiceStatus == InvoiceStatusEnum.Void)
            {
                return Content("This invoice has already been voided.");
            }

            InvoiceVoidVM invoiceVoid = new InvoiceVoidVM();

            invoiceVoid.InvoiceId = invoiceId;
            invoiceVoid.CustomerNameDisplay = invoice.Customer.FirstName + " " + invoice.Customer.LastName;
            invoiceVoid.PetNameDisplay = invoice.Pet.PetName;
            invoiceVoid.BoardingDisplay = invoice.Boarding == null
                ? "No Boarding"
                : $"{invoice.Boarding.BoardingUnit.UnitName} - " +
                  $"{invoice.Boarding.BoardingUnit.UnitNumber} - " +
                  $"{invoice.Boarding.StartDateTime:MM/dd/yyyy} to " +
                  $"{invoice.Boarding.EndDateTime:MM/dd/yyyy}";

            invoiceVoid.InvoiceDateTimeDisplay = $"{invoice.InvoiceDateTime:MM/dd/yyyy hh:mm tt}";
            invoiceVoid.StatusDisplay = GetEnumDisplayName(invoice.InvoiceStatus);
            invoiceVoid.TotalAmountDisplay = invoice.TotalAmount.ToString("C");
            invoiceVoid.AmountPaidDisplay = invoice.AmountPaid.ToString("C");
            invoiceVoid.BalanceDisplay = invoice.Balance.ToString("C");

            return View(invoiceVoid);
        }


        // POST: Invoices/Void
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Void(InvoiceVoidVM invoiceVoid)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            InvoiceModel invoice = dbContext.Invoices
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.Boarding)
                .Include(x => x.Boarding.BoardingUnit)
                .FirstOrDefault(x => x.InvoiceId == invoiceVoid.InvoiceId);

            if (invoice == null)
            {
                return Content("Invoice ID#" + invoiceVoid.InvoiceId + " does not exist.");
            }

            if (invoice.InvoiceStatus == InvoiceStatusEnum.Void)
            {
                return Content("This invoice has already been voided.");
            }

            if (!ModelState.IsValid)
            {
                invoiceVoid.InvoiceId = invoice.InvoiceId;

                invoiceVoid.CustomerNameDisplay = invoice.Customer.FirstName + " " + invoice.Customer.LastName;

                invoiceVoid.PetNameDisplay = invoice.Pet.PetName;

                invoiceVoid.BoardingDisplay = invoice.Boarding == null
                    ? "No Boarding"
                    : $"{invoice.Boarding.BoardingUnit.UnitName} - " +
                      $"{invoice.Boarding.BoardingUnit.UnitNumber} - " +
                      $"{invoice.Boarding.StartDateTime:MM/dd/yyyy} to " +
                      $"{invoice.Boarding.EndDateTime:MM/dd/yyyy}";

                invoiceVoid.InvoiceDateTimeDisplay = $"{invoice.InvoiceDateTime:MM/dd/yyyy hh:mm tt}";

                invoiceVoid.StatusDisplay = GetEnumDisplayName(invoice.InvoiceStatus);

                invoiceVoid.TotalAmountDisplay = invoice.TotalAmount.ToString("C");

                invoiceVoid.AmountPaidDisplay = invoice.AmountPaid.ToString("C");

                invoiceVoid.BalanceDisplay = invoice.Balance.ToString("C");

                return View(invoiceVoid);
            }

            invoice.InvoiceStatus = InvoiceStatusEnum.Void;
            invoice.VoidReason = invoiceVoid.VoidReason;
            invoice.VoidNotes = invoiceVoid.VoidNotes;

            invoice.VoidDateTime = DateTime.Now;

            invoice.VoidedByEmployeeId = currentEmployee.EmployeeId;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { invoiceId = invoice.InvoiceId });
        }


        // GET: Invoices/Receipt
        public ActionResult Receipt(Guid invoiceId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            InvoiceDetailsVM invoiceDetails = BuildInvoiceDetails(dbContext, invoiceId);

            if (invoiceDetails == null)
            {
                return Content("Invoice ID #" + invoiceId + " does not exist.");
            }

            return View(invoiceDetails);

        }

        private List<SelectListItem> BuildCustomerSelectList(
            ApplicationDbContext dbContext,
            Guid? petId = null,
            bool includeInactive = false)
        {
            List<CustomerModel> customers = dbContext.Customers
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToList();

            if (!includeInactive)
            {
                customers = customers
                    .Where(x => x.IsActive)
                    .ToList();
            }

            if (petId.HasValue)
            {
                List<Guid> customerIds = dbContext.CustomerPets
                    .Where(x => x.PetId == petId.Value)
                    .Select(x => x.CustomerId)
                    .Distinct()
                    .ToList();

                customers = customers
                    .Where(x => customerIds.Contains(x.CustomerId))
                    .ToList();
            }

            return customers
                .Select(x => new SelectListItem
                {
                    Value = x.CustomerId.ToString(),
                    Text = $"{x.LastName}, {x.FirstName}"
                })
                .ToList();
        }


        private List<SelectListItem> BuildPetSelectList(
            ApplicationDbContext dbContext,
            Guid? customerId = null,
            bool includeInactive = false)
        {
            List<PetModel> pets = dbContext.Pets
                .OrderBy(x => x.PetName)
                .ToList();

            if (!includeInactive)
            {
                pets = pets
                    .Where(x => x.IsActive)
                    .ToList();
            }

            if (customerId.HasValue)
            {
                List<Guid> petIds = dbContext.CustomerPets
                    .Where(x => x.CustomerId == customerId.Value)
                    .Select(x => x.PetId)
                    .Distinct()
                    .ToList();

                pets = pets
                    .Where(x => petIds.Contains(x.PetId))
                    .ToList();
            }

            return pets
                .Select(x => new SelectListItem
                {
                    Value = x.PetId.ToString(),
                    Text =
                        x.PetName + " - " +
                        GetEnumDisplayName(x.Species) + " - " +
                        x.Breed
                })
                .ToList();
        }

        private List<SelectListItem> BuildBoardingSelectList(
            ApplicationDbContext dbContext,
            Guid? customerId = null,
            Guid? petId = null,
            bool includeInactive = false)
        {
            List<BoardingModel> boardings = dbContext.Boardings
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.BoardingUnit)
                .OrderBy(x => x.Pet.PetName)
                .ThenBy(x => x.StartDateTime)
                .ThenBy(x => x.BoardingUnit.UnitName)
                .ThenBy(x => x.BoardingUnit.UnitNumber)
                .ToList();

            if (!includeInactive)
            {
                boardings = boardings
                    .Where(x => x.Customer.IsActive && x.Pet.IsActive)
                    .ToList();
            }

            if (customerId.HasValue)
            {
                boardings = boardings
                    .Where(x => x.CustomerId == customerId.Value)
                    .ToList();
            }

            if (petId.HasValue)
            {
                boardings = boardings
                    .Where(x => x.PetId == petId.Value)
                    .ToList();
            }

            return boardings
                .Select(x => new SelectListItem
                {
                    Value = x.BoardingId.ToString(),
                    Text =
                        x.Pet.PetName + " - " +
                        x.BoardingUnit.UnitName + " " +
                        x.BoardingUnit.UnitNumber + " - " +
                        x.StartDateTime.ToString("MM/dd/yyyy") +
                        " to " +
                        x.EndDateTime.ToString("MM/dd/yyyy")
                })
                .ToList();
        }

        private InvoiceDetailsVM BuildInvoiceDetails(ApplicationDbContext dbContext, Guid invoiceId) 
        {
            InvoiceModel invoice = dbContext.Invoices
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.Boarding)
                .Include(x => x.Boarding.BoardingUnit)
                .Include(x => x.VoidedByEmployee)
                .Include(x => x.InvoiceItems)
                .Include(x => x.Payments)
                .Include (x => x.Payments.Select(p => p.ProcessedByEmployee))
                .FirstOrDefault(x => x.InvoiceId == invoiceId);

            if (invoice == null)
            {
                return null;
            }

            InvoiceDetailsVM invoiceDetails = new InvoiceDetailsVM();

            invoiceDetails.InvoiceId = invoice.InvoiceId;

            invoiceDetails.CustomerId = invoice.CustomerId;

            invoiceDetails.CustomerNameDisplay = invoice.Customer.FirstName + " " + invoice.Customer.LastName;

            invoiceDetails.PetId = invoice.PetId;

            invoiceDetails.PetNameDisplay = invoice.Pet.PetName;

            invoiceDetails.BoardingId = invoice.BoardingId;

            invoiceDetails.BoardingDisplay = invoice.Boarding == null
                ? "No Boarding"
                : $"{invoice.Boarding.BoardingUnit.UnitName} " +
                  $"{invoice.Boarding.BoardingUnit.UnitNumber} - " +
                  $"{invoice.Boarding.StartDateTime:MM/dd/yyyy} to " +
                  $"{invoice.Boarding.EndDateTime:MM/dd/yyyy}";

            invoiceDetails.InvoiceDateTimeDisplay = $"{invoice.InvoiceDateTime:MM/dd/yyyy hh:mm tt}";

            invoiceDetails.InvoiceStatus = invoice.InvoiceStatus;

            invoiceDetails.StatusDisplay = GetEnumDisplayName(invoice.InvoiceStatus);

            invoiceDetails.SubtotalDisplay = invoice.Subtotal.ToString("C");

            invoiceDetails.TaxAmountDisplay = invoice.TaxAmount.ToString("C");

            invoiceDetails.DiscountAmountDisplay = invoice.DiscountAmount.ToString("C");

            invoiceDetails.TotalAmountDisplay = invoice.TotalAmount.ToString("C");

            invoiceDetails.AmountPaidDisplay = invoice.AmountPaid.ToString("C");

            invoiceDetails.BalanceDisplay = invoice.Balance.ToString("C");

            invoiceDetails.VoidReasonDisplay = invoice.VoidReason.HasValue
                    ? invoice.VoidReason.Value.ToString()
                    : "Not voided";

            invoiceDetails.VoidNotes = string.IsNullOrWhiteSpace(invoice.VoidNotes)
                ? "Not voided"
                : invoice.VoidNotes;

            invoiceDetails.VoidDateTimeDisplay = invoice.VoidDateTime.HasValue
                ? $"{invoice.VoidDateTime.Value:MM/dd/yyyy hh:mm tt}"
                : "Not voided";

            invoiceDetails.VoidedByEmployeeDisplay = invoice.VoidedByEmployee == null
                ? "Not voided"
                : invoice.VoidedByEmployee.FirstName + " " +
                  invoice.VoidedByEmployee.LastName;

            invoiceDetails.NotesDisplay = string.IsNullOrWhiteSpace(invoice.Notes)
                ? "No notes"
                : invoice.Notes;

            foreach (InvoiceItemModel invoiceItem in invoice.InvoiceItems.OrderBy(x => x.Description))
            {
                InvoiceItemSummaryVM invoiceItemSummary = new InvoiceItemSummaryVM();

                invoiceItemSummary.InvoiceItemId = invoiceItem.InvoiceItemId;

                invoiceItemSummary.ItemTypeDisplay = invoiceItem.ItemType.ToString();

                invoiceItemSummary.DescriptionDisplay = invoiceItem.Description;

                invoiceItemSummary.UnitPriceDisplay = invoiceItem.UnitPrice.ToString("C");

                invoiceItemSummary.QuantityDisplay = invoiceItem.Quantity.ToString();

                invoiceItemSummary.LineTotalDisplay = invoiceItem.LineTotal.ToString("C");

                invoiceDetails.InvoiceItems.Add(invoiceItemSummary);

            }

            foreach (PaymentModel payment in invoice.Payments.OrderBy(x => x.PaymentDateTime))
            {
                PaymentSummaryVM paymentSummary = new PaymentSummaryVM();

                paymentSummary.PaymentId = payment.PaymentId;

                paymentSummary.CustomerNameDisplay =
                    invoice.Customer.LastName + ", " +
                    invoice.Customer.FirstName;

                paymentSummary.AmountDisplay = payment.Amount.ToString("C");

                paymentSummary.PaymentMethodDisplay = payment.PaymentMethod.ToString();

                paymentSummary.PaymentDateTimeDisplay = $"{payment.PaymentDateTime:MM/dd/yyyy hh:mm tt}";

                paymentSummary.StatusDisplay = payment.IsVoided ? "Voided" : "Processed";

                paymentSummary.ProcessedByEmployeeDisplay = payment.ProcessedByEmployee == null 
                    ? "Unknown" 
                    : payment.ProcessedByEmployee.FirstName + " " + payment.ProcessedByEmployee.LastName;

                paymentSummary.IsVoided = payment.IsVoided;

                invoiceDetails.Payments.Add(paymentSummary);

            }

            return invoiceDetails;

        }

        private void PopulateInvoiceFormSelectLists(ApplicationDbContext dbContext, InvoiceFormVM invoiceForm)
        {
            Guid? customerId = invoiceForm.CustomerId == Guid.Empty
                ? (Guid?)null
                : invoiceForm.CustomerId;

            Guid? petId = invoiceForm.PetId == Guid.Empty
                ? (Guid?)null
                : invoiceForm.PetId;

            invoiceForm.CustomerSelectList = BuildCustomerSelectList(dbContext, petId);

            invoiceForm.PetSelectList = BuildPetSelectList(dbContext, customerId);

            invoiceForm.BoardingSelectList = BuildBoardingSelectList(dbContext, customerId, petId);
        }


        private string GetEnumDisplayName(Enum enumValue)
        {
            DisplayAttribute displayAttribute =
                enumValue
                    .GetType()
                    .GetField(enumValue.ToString())
                    .GetCustomAttribute<DisplayAttribute>();

            return displayAttribute != null
                ? displayAttribute.Name
                : enumValue.ToString();
        }


        private EmployeeModel GetCurrentEmployee(ApplicationDbContext dbContext)
        {
            string loggedInEmail = User.Identity.Name;

            return dbContext.Employees
                .FirstOrDefault(x =>
                    x.Email == loggedInEmail &&
                    x.IsActive);

        }

    }
}
