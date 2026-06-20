using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace JamesPetBoarding.Controllers
{
    public class PaymentsController : Controller
    {
        // GET: Payments
        public ActionResult Index()
        {
            return View();
        }


        // GET: Payments/Create
        // /Payments/Create?invoiceId=  &paymentMethod=  ^&amount=  &notes=
        public ActionResult Create(
            Guid invoiceId,
            string paymentMethod,
            decimal amount,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            InvoiceModel invoice = dbContext.Invoices.FirstOrDefault(x => x.InvoiceId == invoiceId);
            if (invoice == null) { return Content("Invoice ID #" + invoiceId + " does not exist."); }
            if (string.IsNullOrWhiteSpace(paymentMethod)) { return Content("A payment method is required."); }
            if (amount <= 0) { return Content("The amount must be greater than zero."); }
            if (amount > invoice.Balance) { return Content("Payment amount cannot be greater than the invoice balance of $" + invoice.Balance.ToString("F2") + "."); }

            PaymentModel payment = new PaymentModel();

            DateTime payDateTime = DateTime.Now;

            payment.InvoiceId = invoiceId;
            payment.PaymentDateTime = payDateTime;
            payment.PaymentMethod = paymentMethod;
            payment.Amount = amount;
            
            string generatedTimeStamp = payDateTime.ToString("yyyyMMddHHmmss");
            string uniqueValue = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
            payment.TransactionReference = "payment-" + generatedTimeStamp + "-" + uniqueValue;
            payment.Notes = notes;

            try
            {
                dbContext.Payments.Add(payment);
                dbContext.SaveChanges();

                decimal amountPaid = dbContext.Payments
                    .Where(x => x.InvoiceId == invoiceId && x.IsVoided == false)
                    .Select(x => x.Amount)
                    .DefaultIfEmpty(0)
                    .Sum();

                invoice.AmountPaid = amountPaid;
                invoice.Balance = invoice.TotalAmount - invoice.AmountPaid;

                dbContext.SaveChanges();

                return Content("Payment ID #" + payment.PaymentId + " was successfully created.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        // GET: Payments/Read
        // Payments/Read?paymentId=USE_EXISTING_PAYMENT_ID
        public ActionResult Read(Guid paymentId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            PaymentModel payment = dbContext.Payments.FirstOrDefault(x => x.PaymentId == paymentId);
            if (payment == null) { return Content("Payment ID #" + paymentId + " does not exist."); }

            string isVoidedDisplay = payment.IsVoided
                ? "Yes"
                : "No";

            string voidDateTimeDisplay = payment.IsVoided
                ? payment.VoidedDateTime.Value.ToString("MM/dd/yyyy hh:mm tt")
                : "N/A";

            string voidEmployeeDisplay = payment.IsVoided && payment.VoidedByEmployee != null
                ? payment.VoidedByEmployee.LastName + ", " + payment.VoidedByEmployee.FirstName
                : "N/A";

            string voidedReasonDisplay = string.IsNullOrWhiteSpace(payment.VoidedReason)
                ? "N/A"
                : payment.VoidedReason;

            string notesDisplay = string.IsNullOrWhiteSpace(payment.Notes)
                ? "No notes"
                : payment.Notes;

            return Content(
                "Payment ID #" + payment.PaymentId +
                "<br />Invoice ID #" + payment.InvoiceId +
                "<br />Payment Date-Time: " + payment.PaymentDateTime +
                "<br />Payment Method: " + payment.PaymentMethod +
                "<br />Amount: " + payment.Amount.ToString("F2") +
                "<br />Transaction Reference: " + payment.TransactionReference +
                "<br />Voided: " + isVoidedDisplay +
                "<br />Voided Date-Time: " + voidDateTimeDisplay +
                "<br />Voided By: " + voidEmployeeDisplay +
                "<br />Voided Reason: " + voidedReasonDisplay +
                "<br />Notes: " + notesDisplay 
                );
        }


        // GET: Payments/Update
        // Payments/Update?paymentId=USE_EXISTING_PAYMENT_ID&notes=receipt%20emailed%20to%20customer
        public ActionResult Update(
            Guid paymentId,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PaymentModel payment = dbContext.Payments.FirstOrDefault(x => x.PaymentId == paymentId);
            if (payment == null) { return Content("Payment ID #" + paymentId + " does not exist."); }

            payment.Notes = notes;

            try
            {
                dbContext.SaveChanges();

                return Content("Payment ID #" + payment.PaymentId + " was successfully updated.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        // GET: Payments/Delete
        // /Payments/Delete?paymentId=USE_EXISTING_PAYMENT_ID&voidReason=Returned%20by%20bank&voidedByEmployeeID=USE_EXISTING_EMPLOYEE_ID
        public ActionResult Delete(Guid paymentId, string voidReason, Guid voidedByEmployeeId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            PaymentModel payment = dbContext.Payments.FirstOrDefault(x => x.PaymentId == paymentId);
            if (payment == null) { return Content("Payment ID #" + paymentId + " does not exist."); }

            if (payment.IsVoided) { return Content("Payment ID #" + paymentId + " has already been voided."); }

            Guid invoiceId = payment.InvoiceId;
            InvoiceModel invoice = dbContext.Invoices.FirstOrDefault(x => x.InvoiceId == invoiceId);
            if (invoice == null) { return Content("Invoice ID #" + invoiceId + " does not exist."); }

            if (string.IsNullOrWhiteSpace(voidReason)) { return Content("A reason for the void is required."); }

            EmployeeModel employee = dbContext.Employees.FirstOrDefault(x => x.EmployeeId == voidedByEmployeeId);
            if (employee == null) { return Content("Employee ID #" + voidedByEmployeeId  + " does not exist."); }

            try
            {
                payment.IsVoided = true;
                payment.VoidedDateTime = DateTime.Now;
                payment.VoidedReason = voidReason;
                payment.VoidedByEmployeeId = voidedByEmployeeId;

                dbContext.SaveChanges();

                decimal amountPaid = dbContext.Payments
                    .Where(x => x.InvoiceId == invoiceId && x.IsVoided == false)
                    .Select(x => x.Amount)
                    .DefaultIfEmpty(0)
                    .Sum();

                invoice.AmountPaid = amountPaid;
                invoice.Balance = invoice.TotalAmount - invoice.AmountPaid;

                dbContext.SaveChanges();

                return Content("Payment ID #" + payment.PaymentId + " was successfully voided.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
