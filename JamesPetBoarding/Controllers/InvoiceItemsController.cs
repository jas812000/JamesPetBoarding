using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    [Authorize]
    public class InvoiceItemsController : Controller
    {

        // GET: InvoiceItems/Search
        public ActionResult Search()
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            InvoiceItemSearchVM invoiceItemSearch = new InvoiceItemSearchVM();

            invoiceItemSearch.InvoiceSelectList = BuildInvoiceSelectList(dbContext);

            invoiceItemSearch.BoardingSelectList = BuildBoardingSelectList(dbContext);

            invoiceItemSearch.ServiceSelectList = BuildServiceSelectList(dbContext);

            return View(invoiceItemSearch);

        }


        // POST: InvoiceItems/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(InvoiceItemSearchVM invoiceItemSearch)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            invoiceItemSearch.InvoiceSelectList = BuildInvoiceSelectList(dbContext);

            invoiceItemSearch.BoardingSelectList = BuildBoardingSelectList(dbContext);

            invoiceItemSearch.ServiceSelectList = BuildServiceSelectList(dbContext);

            List<InvoiceItemModel> invoiceItemsQuery = dbContext.InvoiceItems
                .Include(x => x.Invoice)
                .Include(x => x.Invoice.Customer)
                .Include(x => x.Invoice.Pet)
                .Include(x => x.Boarding)
                .Include(x => x.Boarding.BoardingUnit)
                .Include(x => x.Service)
                .ToList();

            if (invoiceItemSearch.InvoiceId.HasValue)
            {
                invoiceItemsQuery = invoiceItemsQuery
                    .Where(x => x.InvoiceId == invoiceItemSearch.InvoiceId.Value)
                    .ToList();
            }

            if (invoiceItemSearch.BoardingId.HasValue)
            {
                invoiceItemsQuery = invoiceItemsQuery
                    .Where(x => x.BoardingId == invoiceItemSearch.BoardingId.Value)
                    .ToList();
            }

            if (invoiceItemSearch.ServiceId.HasValue)
            {
                invoiceItemsQuery = invoiceItemsQuery
                    .Where(x => x.ServiceId == invoiceItemSearch.ServiceId.Value)
                    .ToList();
            }

            if (invoiceItemSearch.ItemType.HasValue)
            {
                invoiceItemsQuery = invoiceItemsQuery
                    .Where(x => x.ItemType == invoiceItemSearch.ItemType.Value)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(invoiceItemSearch.Description))
            {
                string descriptionSearch = invoiceItemSearch.Description.ToLower();

                invoiceItemsQuery = invoiceItemsQuery
                    .Where(x => x.Description != null &&
                                x.Description.ToLower().Contains(descriptionSearch))
                    .ToList();
            }

            invoiceItemSearch.InvoiceItemSearchResults = invoiceItemsQuery
                .OrderBy(x => x.Invoice.Customer.LastName)
                .ThenBy(x => x.Invoice.Customer.FirstName)
                .ThenBy(x => x.Invoice.Pet.PetName)
                .ThenBy(x => x.Invoice.InvoiceDateTime)
                .ThenBy(x => x.ItemType)
                .Select(x => new InvoiceItemSummaryVM
                {
                    InvoiceItemId = x.InvoiceItemId,
                    InvoiceId = x.InvoiceId,

                    InvoiceDisplay =
                        $"{x.Invoice.Customer.LastName}, " +
                        $"{x.Invoice.Customer.FirstName} - " +
                        $"{x.Invoice.Pet.PetName} - " +
                        $"{x.Invoice.InvoiceDateTime:MM/dd/yyyy} - " +
                        $"{x.Invoice.InvoiceStatus} (#" +
                        $"{x.Invoice.InvoiceId.ToString().Substring(0, 6)})",

                    InvoiceStatus = x.Invoice.InvoiceStatus,

                    BoardingDisplay = x.Boarding == null
                        ? "No Boarding"
                        : $"{x.Boarding.BoardingUnit.UnitName} " +
                          $"{x.Boarding.BoardingUnit.UnitNumber} - " +
                          $"{x.Boarding.StartDateTime:MM/dd/yyyy} to " +
                          $"{x.Boarding.EndDateTime:MM/dd/yyyy}",

                    ServiceNameDisplay = x.Service != null
                        ? x.Service.ServiceName.ToString()
                        : "No Service",

                    ItemTypeDisplay = x.ItemType.ToString(),
                    DescriptionDisplay = x.Description,
                    QuantityDisplay = x.Quantity.ToString(),
                    UnitPriceDisplay = x.UnitPrice.ToString("C"),
                    LineTotalDisplay = x.LineTotal.ToString("C")

                })
                .ToList();

            return View(invoiceItemSearch);
        }


        // GET: InvoiceItems/Create
        public ActionResult Create(Guid invoiceId)
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
                .FirstOrDefault(x => x.InvoiceId == invoiceId);

            if (invoice == null)
            {
                return Content("Invoice ID #" + invoiceId + " does not exist.");
            }

            if (invoice.InvoiceStatus == InvoiceStatusEnum.Void)
            {
                return Content("This invoice is void and cannot receive new invoice items.");
            }

            InvoiceItemFormVM invoiceItemForm = new InvoiceItemFormVM();

            invoiceItemForm.InvoiceId = invoiceId;

            invoiceItemForm.InvoiceDisplay =
                $"{invoice.Customer.LastName}, " +
                $"{invoice.Customer.FirstName} - " +
                $"{invoice.Pet.PetName} - " +
                $"{invoice.InvoiceDateTime:MM/dd/yyyy} - " +
                $"{invoice.InvoiceStatus} (#" +
                $"{invoice.InvoiceId.ToString().Substring(0, 6)})";

            invoiceItemForm.BoardingId = invoice.BoardingId;

            invoiceItemForm.BoardingSelectList = BuildBoardingSelectList(dbContext);

            invoiceItemForm.ServiceSelectList = BuildServiceSelectList(dbContext);

            return View(invoiceItemForm);
        }


        // POST: InvoiceItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InvoiceItemFormVM invoiceItemForm)
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
                .FirstOrDefault(x => x.InvoiceId == invoiceItemForm.InvoiceId);

            if (invoice == null)
            {
                return Content("Invoice ID #" + invoiceItemForm.InvoiceId + " does not exist.");
            }

            if (invoice.InvoiceStatus == InvoiceStatusEnum.Void)
            {
                return Content("This invoice is void and cannot receive new invoice items.");
            }

            invoiceItemForm.InvoiceDisplay =
                $"{invoice.Customer.LastName}, " +
                $"{invoice.Customer.FirstName} - " +
                $"{invoice.Pet.PetName} - " +
                $"{invoice.InvoiceDateTime:MM/dd/yyyy} - " +
                $"{invoice.InvoiceStatus} (#" +
                $"{invoice.InvoiceId.ToString().Substring(0, 6)})";

            if (!ModelState.IsValid)
            {

                invoiceItemForm.BoardingSelectList = BuildBoardingSelectList(dbContext);

                invoiceItemForm.ServiceSelectList = BuildServiceSelectList(dbContext);

                return View(invoiceItemForm);

            }

            if (!invoiceItemForm.BoardingId.HasValue && !invoiceItemForm.ServiceId.HasValue)
            {
                ModelState.AddModelError("", "Select a boarding reservation, a service, or both.");

                invoiceItemForm.BoardingSelectList = BuildBoardingSelectList(dbContext);

                invoiceItemForm.ServiceSelectList = BuildServiceSelectList(dbContext);

                return View(invoiceItemForm);
            }

            if (invoiceItemForm.BoardingId.HasValue)
            {

                BoardingModel boarding = dbContext.Boardings
                    .FirstOrDefault(x => x.BoardingId == invoiceItemForm.BoardingId.Value);

                if (boarding == null)
                {
                    return Content("Boarding ID #" + invoiceItemForm.BoardingId.Value + " does not exist.");
                }

                if (boarding.CustomerId != invoice.CustomerId || boarding.PetId != invoice.PetId)
                {
                    return Content("The selected boarding reservation does not belong to the invoice's customer and pet.");
                }

                if (invoice.BoardingId.HasValue && invoice.BoardingId.Value != boarding.BoardingId)
                {
                    return Content("The selected boarding reservation does not match the boarding reservation assigned to this invoice.");
                }

            }

            if (invoiceItemForm.ServiceId.HasValue)
            {
                ServiceModel service = dbContext.Services
                    .FirstOrDefault(x => x.ServiceId == invoiceItemForm.ServiceId.Value);

                if (service == null)
                {
                    return Content("Service ID #" + invoiceItemForm.ServiceId.Value + " does not exist.");
                }
            }

            decimal lineTotal = invoiceItemForm.Quantity * invoiceItemForm.UnitPrice;

            InvoiceItemModel invoiceItem = new InvoiceItemModel();

            invoiceItem.InvoiceId = invoiceItemForm.InvoiceId;
            invoiceItem.BoardingId = invoiceItemForm.BoardingId;
            invoiceItem.ServiceId = invoiceItemForm.ServiceId;
            invoiceItem.ItemType = invoiceItemForm.ItemType;
            invoiceItem.Description = invoiceItemForm.Description;
            invoiceItem.Quantity = invoiceItemForm.Quantity;
            invoiceItem.UnitPrice = invoiceItemForm.UnitPrice;
            invoiceItem.LineTotal = lineTotal;
            invoiceItem.Notes = invoiceItemForm.Notes;

            dbContext.InvoiceItems.Add(invoiceItem);

            invoice.Subtotal += lineTotal;

            invoice.TotalAmount = invoice.Subtotal + invoice.TaxAmount - invoice.DiscountAmount;

            invoice.Balance = invoice.TotalAmount - invoice.AmountPaid;

            dbContext.SaveChanges();

            return RedirectToAction(
                "Read",
                "Invoices",
                new { invoiceId = invoice.InvoiceId });

        }


        // GET: InvoiceItems/Read
        public ActionResult Read(Guid invoiceItemId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            InvoiceItemModel invoiceItem = dbContext.InvoiceItems
                .Include(x => x.Invoice)
                .Include(x => x.Invoice.Customer)
                .Include(x => x.Invoice.Pet)
                .Include(x => x.Boarding)
                .Include(x => x.Boarding.BoardingUnit)
                .Include(x => x.Service)
                .FirstOrDefault(x => x.InvoiceItemId == invoiceItemId);

            if (invoiceItem == null)
            {
                return Content("InvoiceItem ID #" + invoiceItemId + " does not exist.");
            }

            InvoiceItemDetailsVM invoiceItemDetails = new InvoiceItemDetailsVM();

            invoiceItemDetails.InvoiceItemId = invoiceItem.InvoiceItemId;

            invoiceItemDetails.InvoiceId = invoiceItem.InvoiceId;

            invoiceItemDetails.BoardingId = invoiceItem.BoardingId;

            invoiceItemDetails.ServiceId = invoiceItem.ServiceId;

            invoiceItemDetails.InvoiceDisplay = invoiceItem.Invoice == null
                ? "No Invoice"
                : $"{invoiceItem.Invoice.Customer.LastName}, " +
                  $"{invoiceItem.Invoice.Customer.FirstName} - " +
                  $"{invoiceItem.Invoice.Pet.PetName} - " +
                  $"{invoiceItem.Invoice.InvoiceDateTime:MM/dd/yyyy} - " +
                  $"{invoiceItem.Invoice.InvoiceStatus} (#" +
                  $"{invoiceItem.Invoice.InvoiceId.ToString().Substring(0, 6)})";

            invoiceItemDetails.BoardingDisplay = invoiceItem.Boarding == null
                ? "No Boarding"
                : $"{invoiceItem.Boarding.BoardingUnit.UnitName} " +
                  $"{invoiceItem.Boarding.BoardingUnit.UnitNumber} - " +
                  $"{invoiceItem.Boarding.StartDateTime:MM/dd/yyyy} to " +
                  $"{invoiceItem.Boarding.EndDateTime:MM/dd/yyyy}";

            invoiceItemDetails.ServiceNameDisplay = invoiceItem.Service != null
                ? invoiceItem.Service.ServiceName.ToString()
                : "No Service";

            invoiceItemDetails.ItemTypeDisplay = invoiceItem.ItemType.ToString();

            invoiceItemDetails.DescriptionDisplay = invoiceItem.Description;

            invoiceItemDetails.QuantityDisplay = invoiceItem.Quantity.ToString();

            invoiceItemDetails.UnitPriceDisplay = invoiceItem.UnitPrice.ToString("C");

            invoiceItemDetails.LineTotalDisplay = invoiceItem.LineTotal.ToString("C");

            invoiceItemDetails.NotesDisplay = invoiceItem.Notes;

            return View(invoiceItemDetails);

        }


        // GET: InvoiceItems/Update
        public ActionResult Update(Guid invoiceItemId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            InvoiceItemModel invoiceItem = dbContext.InvoiceItems
                .Include(x => x.Invoice)
                .Include(x => x.Boarding)
                .Include(x => x.Service)
                .FirstOrDefault(x => x.InvoiceItemId == invoiceItemId);

            if (invoiceItem == null)
            {
                return Content("InvoiceItem ID #" + invoiceItemId + " does not exist.");
            }

            if (invoiceItem.Invoice.InvoiceStatus == InvoiceStatusEnum.Void)
            {
                return Content("This invoice is void and its invoice items cannot be updated.");
            }

            InvoiceItemFormVM invoiceItemForm = new InvoiceItemFormVM();

            invoiceItemForm.InvoiceItemId = invoiceItem.InvoiceItemId;

            invoiceItemForm.InvoiceId = invoiceItem.InvoiceId;

            invoiceItemForm.BoardingId = invoiceItem.BoardingId;

            invoiceItemForm.ServiceId = invoiceItem.ServiceId;

            invoiceItemForm.ItemType = invoiceItem.ItemType;

            invoiceItemForm.Description = invoiceItem.Description;

            invoiceItemForm.Quantity = invoiceItem.Quantity;

            invoiceItemForm.UnitPrice = invoiceItem.UnitPrice;

            invoiceItemForm.Notes = invoiceItem.Notes;

            invoiceItemForm.BoardingSelectList = BuildBoardingSelectList(dbContext);

            invoiceItemForm.ServiceSelectList = BuildServiceSelectList(dbContext);

            return View(invoiceItemForm);
        }


        // POST: InvoiceItems/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(InvoiceItemFormVM invoiceItemForm)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            if (!ModelState.IsValid)
            {

                invoiceItemForm.BoardingSelectList = BuildBoardingSelectList(dbContext);

                invoiceItemForm.ServiceSelectList = BuildServiceSelectList(dbContext);

                return View(invoiceItemForm);

            }

            InvoiceItemModel invoiceItem = dbContext.InvoiceItems
                .Include(x => x.Invoice)
                .FirstOrDefault(x => x.InvoiceItemId == invoiceItemForm.InvoiceItemId);

            if (invoiceItem == null)
            {
                return Content("InvoiceItem ID #" + invoiceItemForm.InvoiceItemId + " does not exist.");
            }

            if (invoiceItem.Invoice.InvoiceStatus == InvoiceStatusEnum.Void)
            {
                return Content("This invoice is void and its invoice items cannot be updated.");
            }

            if (invoiceItemForm.InvoiceId != invoiceItem.InvoiceId)
            {
                return Content("The submitted Invoice ID does not match the Invoice assigned to this InvoiceItem.");
            }

            if (!invoiceItemForm.BoardingId.HasValue && !invoiceItemForm.ServiceId.HasValue)
            {
                ModelState.AddModelError("", "Select a boarding reservation, a service, or both.");

                invoiceItemForm.BoardingSelectList = BuildBoardingSelectList(dbContext);

                invoiceItemForm.ServiceSelectList = BuildServiceSelectList(dbContext);

                return View(invoiceItemForm);
            }

            if (invoiceItemForm.BoardingId.HasValue)
            {

                BoardingModel boarding = dbContext.Boardings
                    .FirstOrDefault(x => x.BoardingId == invoiceItemForm.BoardingId.Value);

                if (boarding == null)
                {
                    return Content("Boarding ID #" + invoiceItemForm.BoardingId.Value + " does not exist.");
                }

                if (boarding.CustomerId != invoiceItem.Invoice.CustomerId || boarding.PetId != invoiceItem.Invoice.PetId)
                {
                    return Content("The selected boarding reservation does not belong to the invoice's customer and pet.");
                }

                if (invoiceItem.Invoice.BoardingId.HasValue && invoiceItem.Invoice.BoardingId.Value != boarding.BoardingId)
                {
                    return Content("The selected boarding reservation does not match the boarding reservation assigned to this invoice.");
                }

            }

            if (invoiceItemForm.ServiceId.HasValue)
            {
                ServiceModel service = dbContext.Services
                    .FirstOrDefault(x => x.ServiceId == invoiceItemForm.ServiceId.Value);

                if (service == null)
                {
                    return Content("Service ID #" + invoiceItemForm.ServiceId.Value + " does not exist.");
                }
            }

            decimal oldLineTotal = invoiceItem.LineTotal;

            decimal newLineTotal = invoiceItemForm.Quantity * invoiceItemForm.UnitPrice;

            invoiceItem.BoardingId = invoiceItemForm.BoardingId;

            invoiceItem.ServiceId = invoiceItemForm.ServiceId;

            invoiceItem.ItemType = invoiceItemForm.ItemType;

            invoiceItem.Description = invoiceItemForm.Description;

            invoiceItem.Quantity = invoiceItemForm.Quantity;

            invoiceItem.UnitPrice = invoiceItemForm.UnitPrice;

            invoiceItem.LineTotal = newLineTotal;

            invoiceItem.Notes = invoiceItemForm.Notes;

            invoiceItem.Invoice.Subtotal += newLineTotal - oldLineTotal;

            invoiceItem.Invoice.TotalAmount = invoiceItem.Invoice.Subtotal + invoiceItem.Invoice.TaxAmount - invoiceItem.Invoice.DiscountAmount;

            invoiceItem.Invoice.Balance = invoiceItem.Invoice.TotalAmount - invoiceItem.Invoice.AmountPaid;

            dbContext.SaveChanges();

            return RedirectToAction(
                "Read",
                "Invoices",
                new { invoiceId = invoiceItem.InvoiceId });

        }


        // GET: InvoiceItems/Delete
        public ActionResult Delete(Guid invoiceItemId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            InvoiceItemModel invoiceItem = dbContext.InvoiceItems
                .Include(x => x.Invoice)
                .Include(x => x.Invoice.Customer)
                .Include(x => x.Invoice.Pet)
                .Include(x => x.Boarding)
                .Include(x => x.Boarding.BoardingUnit)
                .Include(x => x.Service)
                .FirstOrDefault(x => x.InvoiceItemId == invoiceItemId);

            if (invoiceItem == null)
            {
                return Content("InvoiceItem ID #" + invoiceItemId + " does not exist.");
            }

            if (invoiceItem.Invoice.InvoiceStatus == InvoiceStatusEnum.Void)
            {
                return Content("This invoice is void and its invoice items cannot be deleted.");
            }

            InvoiceItemDeleteVM invoiceItemDelete = new InvoiceItemDeleteVM();

            invoiceItemDelete.InvoiceItemId = invoiceItem.InvoiceItemId;

            invoiceItemDelete.InvoiceId = invoiceItem.InvoiceId;

            invoiceItemDelete.BoardingId = invoiceItem.BoardingId;

            invoiceItemDelete.ServiceId = invoiceItem.ServiceId;

            invoiceItemDelete.InvoiceDisplay = 
                $"{invoiceItem.Invoice.Customer.LastName}, " +
                $"{invoiceItem.Invoice.Customer.FirstName} - " +
                $"{invoiceItem.Invoice.Pet.PetName} - " +
                $"{invoiceItem.Invoice.InvoiceDateTime:MM/dd/yyyy} - " +
                $"{invoiceItem.Invoice.InvoiceStatus} (#" +
                $"{invoiceItem.Invoice.InvoiceId.ToString().Substring(0, 6)})";

            invoiceItemDelete.BoardingDisplay = invoiceItem.Boarding == null
                ? "No Boarding"
                : $"{invoiceItem.Boarding.BoardingUnit.UnitName} " +
                  $"{invoiceItem.Boarding.BoardingUnit.UnitNumber} - " +
                  $"{invoiceItem.Boarding.StartDateTime:MM/dd/yyyy} to " +
                  $"{invoiceItem.Boarding.EndDateTime:MM/dd/yyyy}";

            invoiceItemDelete.ServiceNameDisplay = invoiceItem.Service != null
                ? invoiceItem.Service.ServiceName.ToString()
                : "No Service";

            invoiceItemDelete.ItemTypeDisplay = invoiceItem.ItemType.ToString();

            invoiceItemDelete.DescriptionDisplay = invoiceItem.Description;

            invoiceItemDelete.QuantityDisplay = invoiceItem.Quantity.ToString();

            invoiceItemDelete.UnitPriceDisplay = invoiceItem.UnitPrice.ToString("C");

            invoiceItemDelete.LineTotalDisplay = invoiceItem.LineTotal.ToString("C");

            invoiceItemDelete.InvoiceStatusDisplay = invoiceItem.Invoice.InvoiceStatus.ToString();

            invoiceItemDelete.NotesDisplay = string.IsNullOrWhiteSpace(invoiceItem.Notes) 
                ? "No Notes" 
                : invoiceItem.Notes;

            return View(invoiceItemDelete);

        }


        // POST: InvoiceItems/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(InvoiceItemDeleteVM invoiceItemDelete)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            InvoiceItemModel invoiceItem = dbContext.InvoiceItems
                .Include(x => x.Invoice)
                .FirstOrDefault(x => x.InvoiceItemId == invoiceItemDelete.InvoiceItemId);

            if (invoiceItem == null)
            {
                return Content("InvoiceItem ID #" + invoiceItemDelete.InvoiceItemId + " does not exist.");
            }

            if (invoiceItem.Invoice.InvoiceStatus == InvoiceStatusEnum.Void)
            {
                return Content("This invoice is void and its invoice items cannot be deleted.");
            }

            if (invoiceItemDelete.InvoiceId != invoiceItem.InvoiceId)
            {
                return Content("The submitted Invoice ID does not match the Invoice assigned to this InvoiceItem.");
            }

            decimal deletedLineTotal = invoiceItem.LineTotal;

            if (deletedLineTotal > invoiceItem.Invoice.Subtotal)
            {
                return Content("The InvoiceItem total exceeds the Invoice subtotal. The InvoiceItem cannot be deleted.");
            }

            invoiceItem.Invoice.Subtotal -= deletedLineTotal;

            invoiceItem.Invoice.TotalAmount = invoiceItem.Invoice.Subtotal + invoiceItem.Invoice.TaxAmount - invoiceItem.Invoice.DiscountAmount;

            invoiceItem.Invoice.Balance = invoiceItem.Invoice.TotalAmount - invoiceItem.Invoice.AmountPaid;

            dbContext.InvoiceItems.Remove(invoiceItem);

            dbContext.SaveChanges();

            return RedirectToAction(
                "Read",
                "Invoices",
                new { invoiceId = invoiceItem.InvoiceId });

        }


        private List<SelectListItem> BuildInvoiceSelectList(ApplicationDbContext dbContext)
        {
            return dbContext.Invoices
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .Include(x => x.Boarding)
                .OrderBy(x => x.Customer.LastName)
                .ThenBy(x => x.Customer.FirstName)
                .ThenBy(x => x.Pet.PetName)
                .ThenBy(x => x.InvoiceDateTime)
                .ThenBy(x => x.InvoiceId)
                .ToList()
                .Select(x => new SelectListItem
                {
                    Value = x.InvoiceId.ToString(),
                    Text = $"{x.Customer.LastName}, " +
                    $"{x.Customer.FirstName}, " +
                    $"{x.Pet.PetName} - " +
                    $"{x.InvoiceDateTime.ToShortDateString()} - " +
                    $"{x.InvoiceStatus} - (#" +
                    $"{x.InvoiceId.ToString().Substring(0, 6)})"
                })
                .ToList();
        }

        private List<SelectListItem> BuildBoardingSelectList(ApplicationDbContext dbContext)
        {
            return dbContext.Boardings
                .Include(x => x.Pet)
                .Include(x => x.BoardingUnit)
                .OrderBy(x => x.Pet.PetName)
                .ThenBy(x => x.StartDateTime)
                .ThenBy(x => x.BoardingUnit.UnitName)
                .ThenBy(x => x.BoardingUnit.UnitNumber)
                .ToList()
                .Select(x => new SelectListItem
                {
                    Value = x.BoardingId.ToString(),
                    Text = $"{x.Pet.PetName} - " +
                    $"{x.BoardingUnit.UnitName} {x.BoardingUnit.UnitNumber} - " +
                    $"{x.StartDateTime:MM/dd/yyyy} to {x.EndDateTime:MM/dd/yyyy}"
                })
                .ToList();
        }

        private List<SelectListItem> BuildServiceSelectList(ApplicationDbContext dbContext)
        {
            return dbContext.Services
                .OrderBy(x => x.ServiceName)
                .ThenBy(x => x.Species)
                .ThenBy(x => x.BasePrice)
                .ToList()
                .Select(x => new SelectListItem
                {
                    Value = x.ServiceId.ToString(),
                    Text = $"{x.ServiceName} - {x.Species} - {x.BasePrice:C}",
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

    }
}

