using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    public class InvoicesController : Controller
    {
        // GET: Invoices
        public ActionResult Index()
        {
            return View();
        }


        // GET: Invoices/Create
        // Invoices/Create?customerId=USE_EXISTING_CUSTOMER_ID&invoiceDateTime=2026-06-09&status=Open&discountAmount=0.00&notes=Initial%20invoice
        public ActionResult Create(     
            Guid customerId,
            DateTime invoiceDateTime,
            string status,
            decimal discountAmount,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            CustomerModel customer = dbContext.Customers.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer == null) { return Content("Customer ID #" + customerId + " does not exist."); }

            if (string.IsNullOrWhiteSpace(status)) { return Content("Invoice status is required."); }
            if (discountAmount < 0) { return Content("Discount amount cannot be negative."); }

            InvoiceModel invoice = new InvoiceModel();

            invoice.CustomerId = customerId;
            invoice.InvoiceDateTime = invoiceDateTime;
            invoice.Status = status;
            invoice.Subtotal = 0;
            invoice.TaxAmount = 0;
            invoice.DiscountAmount = discountAmount;
            invoice.TotalAmount = 0;
            invoice.AmountPaid = 0;
            invoice.Balance = 0;
            invoice.Notes = notes;

            try
            {
                dbContext.Invoices.Add(invoice);
                dbContext.SaveChanges();
               
                return Content("An invoice was successfully created.");
            }
            catch ( Exception ex )
            {
                return Content(ex.Message);
            }
        }



        // GET: Invoices/Read
        // /Invoices/Read?invoiceId=USE_EXISTING_INVOICE_ID
        public ActionResult Read(Guid invoiceId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            InvoiceModel invoice = dbContext.Invoices.FirstOrDefault(x => x.InvoiceId == invoiceId);
            if (invoice == null) { return Content("Invoice ID #" + invoiceId + " does not exist."); }

            string notesDisplay = string.IsNullOrWhiteSpace(invoice.Notes)
                ? "No notes"
                : invoice.Notes;

            return Content(
                "Invoice Id # " + invoice.InvoiceId +
                "<br />Customer ID #" + invoice.CustomerId +
                "<br />Invoice Date/Time: " + invoice.InvoiceDateTime.ToString("MM/dd/yyyy hh:mm tt") +
                "<br />Status: " + invoice.Status +
                "<br />Subtotal: $" + invoice.Subtotal.ToString("F2") +
                "<br />Tax Amount: $" + invoice.TaxAmount.ToString("F2")+
                "<br />Discount Amount: $" + invoice.DiscountAmount.ToString("F2") +
                "<br />Total Amount: $" + invoice.TotalAmount.ToString("F2") +
                "<br />Amount Paid: $" + invoice.AmountPaid.ToString("F2") +
                "<br />Balance: $" + invoice.Balance.ToString("F2") +
                "<br />Notes: " + notesDisplay
            );
        }


        // GET: Invoices/Update
        // /Invoices/Update?invoiceId=USE_EXISTING_INVOICE_ID&invoiceDateTime=2026-06-09&status=Open&discountAmount=0.00&notes=Updated%20invoice
        public ActionResult Update(
            Guid invoiceId,
            DateTime invoiceDateTime,
            string status,
            decimal discountAmount,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            InvoiceModel invoice = dbContext.Invoices.FirstOrDefault(x => x.InvoiceId == invoiceId);

            if (invoice == null) { return Content("Invoice ID #" + invoiceId + " does not exist."); }

            if (string.IsNullOrWhiteSpace(status)) { return Content("Invoice status is required."); }

            if (discountAmount < 0) { return Content("Discount amount cannot be negative."); }

            invoice.InvoiceDateTime = invoiceDateTime;
            invoice.Status = status;
            invoice.DiscountAmount = discountAmount;
            invoice.Notes = notes;

            try
            {
                dbContext.SaveChanges();
                return Content("Invoice ID #" + invoice.InvoiceId + " was successfully updated.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        // GET: Invoices/Delete
        // /Invoices/Delete?invoiceId=USE_EXISTING_INVOICE_ID
        public ActionResult Delete(Guid invoiceId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            InvoiceModel invoice = dbContext.Invoices.FirstOrDefault(x =>x.InvoiceId == invoiceId);

            if (invoice == null) { return Content("Invoice ID #" + invoiceId + " does not exist."); }

            List<InvoiceItemModel> invoiceItems = dbContext.InvoiceItems.Where(x => x.InvoiceId == invoiceId).ToList();
            List<PaymentModel> payments = dbContext.Payments.Where(x => x.InvoiceId == invoiceId).ToList();

            try
            {
                if (invoiceItems.Count > 0) { return Content("This invoice has invoice item records and cannot be deleted."); }
                if (payments.Count > 0) { return Content("This invoice has payment records and cannot be deleted."); }
                dbContext.Invoices.Remove(invoice);

                dbContext.SaveChanges();
                return Content("Invoice ID #" + invoice.InvoiceId + " was successfully deleted.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
