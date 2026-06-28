using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    public class InvoiceItemsController : Controller
    {
        // GET: InvoiceItems
        public ActionResult Index()
        {
            return View();
        }


        // GET: InvoiceItems/Create
        // InvoiceItems/Create?boardingId=USE_EXISTING_BOARDING_ID&invoiceId=USE_EXISTING_INVOICE_ID&serviceId=USE_EXISTING_SERVICE_ID&itemType=Service&description=Medicatn%20Administrion&quantity=1&unitPrice=10.00&notes=
        public ActionResult Create(
            Guid boardingId,
            Guid invoiceId,
            Guid serviceId,
            InvoiceItemTypeEnum itemType,
            string description,
            int quantity,
            decimal unitPrice,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            BoardingModel boarding = dbContext.Boardings.FirstOrDefault(x => x.BoardingId == boardingId);
            if (boarding == null) { return Content("Boarding ID #" + boardingId + " does not exist."); }

            InvoiceModel invoice = dbContext.Invoices.FirstOrDefault(x => x.InvoiceId == invoiceId);
            if (invoice == null) { return Content("Invoice ID #" + invoiceId + " does not exist."); }

            ServiceModel service = dbContext.Services.FirstOrDefault(x => x.ServiceId == serviceId);
            if (service == null) { return Content("Service ID #" + serviceId + " does not exist."); }

            if (string.IsNullOrWhiteSpace(description)) { return Content("A description is required."); }

            if (quantity <= 0) { return Content("Quantity must be greater than zero."); }

            if (unitPrice < 0) { return Content("Unit price cannot be negative."); }

            InvoiceItemModel invoiceItem = new InvoiceItemModel();

            invoiceItem.BoardingId = boardingId;
            invoiceItem.InvoiceId = invoiceId;
            invoiceItem.ServiceId = serviceId;
            invoiceItem.ItemType = itemType;
            invoiceItem.Description = description;
            invoiceItem.Quantity = quantity;
            invoiceItem.UnitPrice = unitPrice;
            invoiceItem.LineTotal = quantity * unitPrice;
            invoiceItem.Notes = notes;

            try
            {
                dbContext.InvoiceItems.Add(invoiceItem);
                dbContext.SaveChanges();

                decimal subtotal = dbContext.InvoiceItems
                    .Where(x => x.InvoiceId == invoiceId)
                    .Select(x => x.LineTotal)
                    .DefaultIfEmpty(0)
                    .Sum();

                decimal taxAmount = subtotal * 0.0825m;
                decimal totalAmount = subtotal + taxAmount - invoice.DiscountAmount;

                decimal amountPaid = dbContext.Payments
                    .Where(x => x.InvoiceId == invoiceId)
                    .Select(x => x.Amount)
                    .DefaultIfEmpty(0)
                    .Sum();

                invoice.Subtotal = subtotal;
                invoice.TaxAmount = taxAmount;
                invoice.TotalAmount = totalAmount;
                invoice.AmountPaid = amountPaid;
                invoice.Balance = totalAmount - amountPaid;

                dbContext.SaveChanges();
                return Content("An invoice item was successfully created.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        // GET: InvoiceItems/Read
        // /InvoiceItems/Read?invoiceItemId=USE_EXISTING_INVOICE_ITEM_ID
        public ActionResult Read(Guid invoiceItemId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            InvoiceItemModel invoiceItem = dbContext.InvoiceItems.FirstOrDefault(x => x.InvoiceItemId == invoiceItemId);
            if (invoiceItem == null) { return Content("InvoiceItem ID #" + invoiceItemId + " does not exist."); }

            string notesDisplay = string.IsNullOrWhiteSpace(invoiceItem.Notes)
                ? "No notes"
                : invoiceItem.Notes;

            string invoiceItemTypeDisplay = invoiceItem.ItemType.ToString();

            switch (invoiceItem.ItemType)
            { 
                case InvoiceItemTypeEnum.VeterinarianCare:
                    invoiceItemTypeDisplay = "Veterinarian Care";
                    break;
            }

            return Content(
                "InvoiceItem ID #" + invoiceItem.InvoiceItemId +
                "<br />Boarding ID #" + invoiceItem.BoardingId +
                "<br />Invoice ID #" + invoiceItem.InvoiceId +
                "<br />Service ID #" + invoiceItem.ServiceId +
                "<br />Item Type: " + invoiceItemTypeDisplay +
                "<br />Description: " + invoiceItem.Description +
                "<br />Quantity: " + invoiceItem.Quantity +
                "<br />Unit Price: $" + invoiceItem.UnitPrice.ToString("F2") +
                "<br />Line Total: $" + invoiceItem.LineTotal.ToString("F2") +
                "<br />Notes: " + notesDisplay
            );
        }


        // GET: InvoiceItems/Update
        // /InvoiceItems/Update?invoiceItemId=USE_EXISTING_INVOICE_ITEM_ID&boardingId=USE_EXISTING_BOARDING_ID&invoiceId=USE_EXISTING_INVOICE_ID&serviceId=USE_EXISTING_SERVICE_ID&itemType=Service&description=Medication%20Administration&quantity=2&unitPrice=9.99&notes=updated%20price%20and%20quantity
        public ActionResult Update(
            Guid invoiceItemId,
            Guid boardingId, 
            Guid invoiceId, 
            Guid serviceId, 
            InvoiceItemTypeEnum itemType, 
            string description, 
            int quantity, 
            decimal unitPrice, 
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            InvoiceItemModel invoiceItem = dbContext.InvoiceItems.FirstOrDefault(x => x.InvoiceItemId == invoiceItemId);
            if (invoiceItem == null) { return Content("InvoiceItem ID #" + invoiceItemId + " does not exist."); }

            decimal previousLineTotal = invoiceItem.LineTotal;

            BoardingModel boarding = dbContext.Boardings.FirstOrDefault(x => x.BoardingId == boardingId);
            if (boarding == null) { return Content("Boarding ID #" + boardingId + " does not exist."); }

            InvoiceModel invoice = dbContext.Invoices.FirstOrDefault(x => x.InvoiceId == invoiceId);
            if (invoice == null) { return Content("Invoice ID #" + invoiceId + " does not exist."); }

            ServiceModel service = dbContext.Services.FirstOrDefault(x => x.ServiceId == serviceId);
            if (service == null) { return Content("Service ID #" + serviceId + " does not exist."); }

            if (string.IsNullOrWhiteSpace(description)) { return Content("A description is required."); }
            if (quantity <= 0) { return Content("Quantity must be greater than zero."); }
            if (unitPrice < 0) { return Content("Unit price cannot be negative."); }

            if (invoiceItem.InvoiceId != invoiceId)
            { 
                return Content("Invoice item cannot be moved to a different invoice."); 
            }

            if (invoice.AmountPaid > 0)
            {
                return Content("Invoice has payments and cannot be modified.");
            }

            invoiceItem.BoardingId = boardingId;
            invoiceItem.InvoiceId = invoiceId;
            invoiceItem.ServiceId = serviceId;
            invoiceItem.ItemType = itemType;
            invoiceItem.Description = description;
            invoiceItem.Quantity = quantity;
            invoiceItem.UnitPrice = unitPrice;
            invoiceItem.LineTotal = quantity * unitPrice;
            invoiceItem.Notes = notes;

            try
            {
                
                decimal subtotal = dbContext.InvoiceItems
                    .Where(x => x.InvoiceId == invoiceId)
                    .Select(x => x.LineTotal)
                    .DefaultIfEmpty(0)
                    .Sum();

                subtotal = subtotal - previousLineTotal + invoiceItem.LineTotal;

                decimal taxAmount = subtotal * 0.0825m;
                decimal totalAmount = subtotal + taxAmount - invoice.DiscountAmount;

                decimal amountPaid = dbContext.Payments
                    .Where(x => x.InvoiceId == invoiceId)
                    .Select(x => x.Amount)
                    .DefaultIfEmpty(0)
                    .Sum();

                invoice.Subtotal = subtotal;
                invoice.TaxAmount = taxAmount;
                invoice.TotalAmount = totalAmount;
                invoice.AmountPaid = amountPaid;
                invoice.Balance = totalAmount - amountPaid;

                dbContext.SaveChanges();
                return Content("InvoiceItem ID #" + invoiceItem.InvoiceItemId + " was successfully updated.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        // GET: InvoiceItems/Delete
        // /InvoiceItems/Delete?invoiceItemId=USE_EXISTING_INVOICE_ITEM_ID
        public ActionResult Delete(Guid invoiceItemId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            InvoiceItemModel invoiceItem = dbContext.InvoiceItems.FirstOrDefault(x => x.InvoiceItemId == invoiceItemId);
            if (invoiceItem == null) { return Content("InvoiceItem ID #" + invoiceItemId + " does not exist."); }

            Guid invoiceId = invoiceItem.InvoiceId;

            InvoiceModel invoice = dbContext.Invoices.FirstOrDefault(x => x.InvoiceId == invoiceId);
            if (invoice == null) { return Content("Invoice ID #" + invoiceId + " does not exist."); }

            try
            {
                if (invoice.AmountPaid > 0) 
                {
                    return Content("Invoice has payments and invoice items cannot be deleted.");
                }

                dbContext.InvoiceItems.Remove(invoiceItem);

                decimal subtotal = dbContext.InvoiceItems
                    .Where(x => x.InvoiceId == invoiceId && x.InvoiceItemId != invoiceItemId)
                    .Select(x => x.LineTotal)
                    .DefaultIfEmpty(0)
                    .Sum();

                decimal taxAmount = subtotal * 0.0825m;
                decimal totalAmount = subtotal + taxAmount - invoice.DiscountAmount;

                decimal amountPaid = dbContext.Payments
                    .Where(x => x.InvoiceId == invoiceId)
                    .Select(x => x.Amount)
                    .DefaultIfEmpty(0)
                    .Sum();

                invoice.Subtotal = subtotal;
                invoice.TaxAmount = taxAmount;
                invoice.TotalAmount = totalAmount;
                invoice.AmountPaid = amountPaid;
                invoice.Balance = totalAmount - amountPaid;

                dbContext.SaveChanges();
                return Content("InvoiceItem ID #" + invoiceItem.InvoiceItemId + " was successfully deleted.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
