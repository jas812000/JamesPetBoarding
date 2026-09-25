using JamesPetBoarding.Enums;
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
using System.Web.Services.Description;

namespace JamesPetBoarding.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {

        // GET: Payments/Search
        public ActionResult Search()
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            PaymentSearchVM paymentSearch = new PaymentSearchVM();

            paymentSearch.InvoiceSelectList =
                BuildInvoiceSelectList(dbContext);

            paymentSearch.CustomerSelectList =
                BuildCustomerSelectList(dbContext);

            paymentSearch.PetSelectList =
                BuildPetSelectList(dbContext);

            paymentSearch.EmployeeSelectList =
                BuildEmployeeSelectList(dbContext);


            return View(paymentSearch);

        }


        // POST: Payments/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(PaymentSearchVM paymentSearch)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            if (paymentSearch.InvoiceId.HasValue)
            {
                InvoiceModel selectedInvoice = dbContext.Invoices
                    .FirstOrDefault(x =>
                        x.InvoiceId == paymentSearch.InvoiceId.Value);

                if (selectedInvoice == null)
                {
                    ModelState.AddModelError(
                        "InvoiceId",
                        "The selected invoice no longer exists.");

                    paymentSearch.InvoiceId = null;
                    paymentSearch.CustomerId = null;
                    paymentSearch.PetId = null;
                }
                else
                {
                    // Disabled Customer/Pet controls are not posted.
                    paymentSearch.CustomerId = selectedInvoice.CustomerId;
                    paymentSearch.PetId = selectedInvoice.PetId;
                }
            }

            paymentSearch.InvoiceSelectList =
                BuildInvoiceSelectList(
                    dbContext,
                    paymentSearch.CustomerId,
                    paymentSearch.PetId);

            if (paymentSearch.InvoiceId.HasValue)
            {
                paymentSearch.InvoiceSelectList =
                    paymentSearch.InvoiceSelectList
                        .Where(x =>
                            x.Value ==
                            paymentSearch.InvoiceId.Value.ToString())
                        .ToList();
            }

            paymentSearch.CustomerSelectList =
                BuildCustomerSelectList(dbContext, paymentSearch.PetId);

            paymentSearch.PetSelectList =
                BuildPetSelectList(dbContext, paymentSearch.CustomerId);

            paymentSearch.EmployeeSelectList =
                BuildEmployeeSelectList(dbContext);

            List<PaymentModel> paymentQuery = dbContext.Payments
                .Include(x => x.Invoice)
                .Include(x => x.Invoice.Customer)
                .Include(x => x.Invoice.Pet)
                .Include(x => x.ProcessedByEmployee).ToList();

            if (paymentSearch.InvoiceId.HasValue)
            {
                paymentQuery = paymentQuery
                    .Where(x => x.InvoiceId == paymentSearch.InvoiceId.Value)
                    .ToList();
            }

            if (paymentSearch.CustomerId.HasValue)
            {
                paymentQuery = paymentQuery
                    .Where(x => x.Invoice.CustomerId == paymentSearch.CustomerId.Value)
                    .ToList();
            }

            if (paymentSearch.PetId.HasValue)
            {
                paymentQuery = paymentQuery
                    .Where(x => x.Invoice.PetId == paymentSearch.PetId.Value)
                    .ToList();
            }

            if (paymentSearch.StartPaymentDateTime.HasValue)
            {
                paymentQuery = paymentQuery
                    .Where(x => x.PaymentDateTime >= paymentSearch.StartPaymentDateTime.Value)
                    .ToList();
            }

            if (paymentSearch.EndPaymentDateTime.HasValue)
            {
                DateTime endDate = paymentSearch.EndPaymentDateTime.Value.Date.AddDays(1);

                paymentQuery = paymentQuery
                    .Where(x => x.PaymentDateTime < endDate)
                    .ToList();
            }

            if (paymentSearch.ProcessedByEmployeeId.HasValue)
            {
                paymentQuery = paymentQuery
                    .Where(x => x.ProcessedByEmployeeId == paymentSearch.ProcessedByEmployeeId.Value)
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(paymentSearch.TransactionReference))
            {
                string transactionReference = paymentSearch.TransactionReference.Trim();

                paymentQuery = paymentQuery
                    .Where(x => x.TransactionReference.IndexOf(
                        transactionReference,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            if (paymentSearch.PaymentMethod.HasValue)
            {
                paymentQuery = paymentQuery
                    .Where(x => x.PaymentMethod == paymentSearch.PaymentMethod.Value)
                    .ToList();
            }

            if (paymentSearch.IsVoided.HasValue)
            {
                paymentQuery = paymentQuery
                    .Where(x => x.IsVoided == paymentSearch.IsVoided.Value)
                    .ToList();
            }

            paymentSearch.PaymentSearchResults = paymentQuery
                .OrderBy(x => x.Invoice.Customer.LastName)
                .ThenBy(x => x.Invoice.Customer.FirstName)
                .ThenBy(x => x.Invoice.Pet.PetName)
                .ThenBy(x => x.PaymentDateTime)
                .ThenBy(x => x.PaymentId)
                .Select(x => new PaymentSummaryVM
                {
                    PaymentId = x.PaymentId,

                    InvoiceId = x.InvoiceId,

                    InvoiceDisplay =
                        $"{x.Invoice.Customer.LastName}, {x.Invoice.Customer.FirstName}, " +
                        $"{x.Invoice.Pet.PetName} - " +
                        $"{x.Invoice.InvoiceDateTime.ToString("MM/dd/yyyy")} - " +
                        $"{x.Invoice.InvoiceStatus} - " +
                        $"(#{x.InvoiceId.ToString().Substring(0, 6)})",

                    CustomerNameDisplay =
                        $"{x.Invoice.Customer.LastName}, " +
                        $"{x.Invoice.Customer.FirstName}",


                    PetNameDisplay = x.Invoice.Pet.PetName,

                    AmountDisplay = x.Amount.ToString("C"),

                    PaymentMethodDisplay = x.PaymentMethod.ToString(),

                    PaymentDateTimeDisplay = x.PaymentDateTime.ToString("MM/dd/yyyy h:mm tt"),

                    ProcessedByEmployeeDisplay =
                        $"{x.ProcessedByEmployee.LastName}, " +
                        $"{x.ProcessedByEmployee.FirstName}",

                    TransactionReferenceDisplay = x.TransactionReference,

                    StatusDisplay = x.IsVoided
                        ? "Voided"
                        : "Processed",

                    IsVoided = x.IsVoided,

                })
                .ToList();

            dbContext.Dispose();

            return View(paymentSearch);

        }


                // GET: Payments/GetPaymentSearchSelections
        public JsonResult GetPaymentSearchSelections(
            Guid? invoiceId,
            Guid? customerId,
            Guid? petId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return Json(
                    new { Error = "Unable to load payment search selections." },
                    JsonRequestBehavior.AllowGet);
            }

            if (invoiceId.HasValue)
            {
                InvoiceModel selectedInvoice = dbContext.Invoices
                    .FirstOrDefault(x => x.InvoiceId == invoiceId.Value);

                if (selectedInvoice == null)
                {
                    return Json(
                        new { Error = "The selected invoice no longer exists." },
                        JsonRequestBehavior.AllowGet);
                }

                customerId = selectedInvoice.CustomerId;
                petId = selectedInvoice.PetId;
            }

            List<SelectListItem> invoices =
                BuildInvoiceSelectList(dbContext, customerId, petId);

            if (invoiceId.HasValue)
            {
                invoices = invoices
                    .Where(x => x.Value == invoiceId.Value.ToString())
                    .ToList();
            }

            return Json(
                new
                {
                    InvoiceId = invoiceId,
                    CustomerId = customerId,
                    PetId = petId,
                    Invoices = invoices,
                    Customers = BuildCustomerSelectList(dbContext, petId),
                    Pets = BuildPetSelectList(dbContext, customerId)
                },
                JsonRequestBehavior.AllowGet);
        }


// GET: Payments/Create
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
                .FirstOrDefault(x => x.InvoiceId == invoiceId);

            if (invoice == null)
            {
                return Content("Invoice ID #" + invoiceId + " does not exist.");
            }

            if (invoice.InvoiceStatus == InvoiceStatusEnum.Void)
            {
                return Content("This invoice is void. A payment cannot be made on voided invoices.");
            }

            if (invoice.Balance <= 0)
            {
                return Content("This invoice has a zero or negative balance. A payment cannot be made.");
            }

            PaymentFormVM paymentForm = new PaymentFormVM();

            paymentForm.InvoiceId = invoice.InvoiceId;

            paymentForm.InvoiceDisplay =
                $"{invoice.Customer.LastName}, {invoice.Customer.FirstName}, " +
                $"{invoice.Pet.PetName} - " +
                $"{invoice.InvoiceDateTime.ToString("MM/dd/yyyy")} - " +
                $"{invoice.InvoiceStatus} - " +
                $"(#{invoice.InvoiceId.ToString().Substring(0, 6)})";

            return View(paymentForm);

        }


        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaymentFormVM paymentForm)
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
                .FirstOrDefault(x => x.InvoiceId == paymentForm.InvoiceId);

            if (invoice == null)
            {
                return Content("Invoice ID #" + paymentForm.InvoiceId + " does not exist.");
            }

            paymentForm.InvoiceDisplay =
                $"{invoice.Customer.LastName}, {invoice.Customer.FirstName}, " +
                $"{invoice.Pet.PetName} - " +
                $"{invoice.InvoiceDateTime.ToString("MM/dd/yyyy")} - " +
                $"{invoice.InvoiceStatus} - " +
                $"(#{invoice.InvoiceId.ToString().Substring(0, 6)})";

            if (!ModelState.IsValid)
            {
                return View(paymentForm);
            }

            if (invoice.InvoiceStatus == InvoiceStatusEnum.Void)
            {
                return Content("This invoice is void and cannot receive new payments.");
            }

            if (invoice.Balance <= 0)
            {
                return Content("This invoice has a zero or negative balance. A payment cannot be made.");
            }

            if (paymentForm.Amount <= 0)
            {
                ModelState.AddModelError(
                    "Amount",
                    "A valid payment amount must be entered."
                );
            }

            if (paymentForm.Amount > invoice.Balance)
            {
                ModelState.AddModelError(
                    "Amount",
                    "Payment amount cannot exceed the invoice balance."
                );
            }

            if (!ModelState.IsValid)
            {
                return View(paymentForm);
            }

            PaymentModel payment = new PaymentModel();

            payment.PaymentId = Guid.NewGuid();

            payment.InvoiceId = paymentForm.InvoiceId;

            payment.PaymentMethod = paymentForm.PaymentMethod;

            payment.Amount = paymentForm.Amount;

            payment.PaymentDateTime = DateTime.Now;

            payment.TransactionReference =
                payment.PaymentDateTime.ToString("yyyyMMdd") +
                "-" +
                Guid.NewGuid()
                    .ToString("N")
                    .Substring(0, 12)
                    .ToUpper();

            payment.ProcessedByEmployeeId = currentEmployee.EmployeeId;

            payment.IsVoided = false;

            payment.Notes = paymentForm.Notes;

            invoice.AmountPaid += payment.Amount;

            invoice.Balance = invoice.TotalAmount - invoice.AmountPaid;

            dbContext.Payments.Add(payment);

            dbContext.SaveChanges();

            return RedirectToAction(
                "Read",
                "Invoices",
                new { invoiceId = invoice.InvoiceId }
            );

        }


        // GET: Payments/Read
        public ActionResult Read(Guid paymentId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            PaymentDetailsVM paymentDetails = BuildPaymentDetails(dbContext, paymentId);

            if (paymentDetails == null)
            {
                return Content("Payment ID #" + paymentId + " does not exist.");
            }

            return View(paymentDetails);

        }


        // GET: Payments/Update
        public ActionResult Update(Guid paymentId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            PaymentModel payment = dbContext.Payments
                .Include(x => x.Invoice)
                .Include(x => x.Invoice.Customer)
                .Include(x => x.Invoice.Pet)
                .FirstOrDefault(x => x.PaymentId == paymentId);

            if (payment == null)
            {
                return Content("Payment ID #" + paymentId + " does not exist.");
            }

            if (payment.IsVoided)
            {
                return Content("This payment has been voided and cannot be updated.");
            }

            PaymentUpdateVM paymentUpdate = new PaymentUpdateVM();

            paymentUpdate.PaymentId = payment.PaymentId;

            paymentUpdate.InvoiceId = payment.InvoiceId;

            paymentUpdate.InvoiceDisplay =
                $"{payment.Invoice.Customer.LastName}, " +
                $"{payment.Invoice.Customer.FirstName}, " +
                $"{payment.Invoice.Pet.PetName} - " +
                $"{payment.Invoice.InvoiceDateTime.ToString("MM/dd/yyyy")} - " +
                $"{payment.Invoice.InvoiceStatus} - " +
                $"(#{payment.InvoiceId.ToString().Substring(0, 6)})";

            paymentUpdate.PaymentDisplay =
                $"{payment.PaymentDateTime.ToString("MM/dd/yyyy hh:mm tt")} - " +
                $"{payment.PaymentMethod} - " +
                $"{payment.Amount.ToString("C")} - " +
                $"{payment.TransactionReference} - " +
                "Processed";

            paymentUpdate.Notes = payment.Notes;

            return View(paymentUpdate);

        }


        // POST: Payments/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(PaymentUpdateVM paymentUpdate)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            PaymentModel payment = dbContext.Payments
                .Include(x => x.Invoice)
                .Include(x => x.Invoice.Customer)
                .Include(x => x.Invoice.Pet)
                .FirstOrDefault(x => x.PaymentId == paymentUpdate.PaymentId);

            if (payment == null)
            {
                return Content("Payment ID #" + paymentUpdate.PaymentId + " does not exist.");
            }

            if (payment.IsVoided)
            {
                return Content("This payment has been voided and cannot be updated.");
            }

            if (!ModelState.IsValid)
            {

                paymentUpdate.InvoiceDisplay =
                    $"{payment.Invoice.Customer.LastName}, " +
                    $"{payment.Invoice.Customer.FirstName}, " +
                    $"{payment.Invoice.Pet.PetName} - " +
                    $"{payment.Invoice.InvoiceDateTime.ToString("MM/dd/yyyy")} - " +
                    $"{payment.Invoice.InvoiceStatus} - " +
                    $"(#{payment.InvoiceId.ToString().Substring(0, 6)})";

                paymentUpdate.PaymentDisplay =
                    $"{payment.PaymentDateTime.ToString("MM/dd/yyyy hh:mm tt")} - " +
                    $"{payment.PaymentMethod} - " +
                    $"{payment.Amount.ToString("C")} - " +
                    $"{payment.TransactionReference} - " +
                    "Processed";

                return View(paymentUpdate);
            }

            payment.Notes = paymentUpdate.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { paymentId = payment.PaymentId });

        }


        // GET: Payments/Void
        public ActionResult Void(Guid paymentId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            PaymentModel payment = dbContext.Payments
                .Include(x => x.Invoice)
                .Include(x => x.Invoice.Customer)
                .Include(x => x.Invoice.Pet)
                .Include(x => x.ProcessedByEmployee)
                .FirstOrDefault(x => x.PaymentId == paymentId);

            if (payment == null)
            {
                return Content("Payment ID #" + paymentId + " does not exist.");
            }

            if (payment.IsVoided)
            {
                return Content("This payment has already been voided.");
            }

            PaymentVoidVM paymentVoid = new PaymentVoidVM();

            paymentVoid.PaymentId = payment.PaymentId;

            paymentVoid.InvoiceId = payment.InvoiceId;

            paymentVoid.InvoiceDisplay =
                $"{payment.Invoice.Customer.LastName}, {payment.Invoice.Customer.FirstName}, " +
                $"{payment.Invoice.Pet.PetName} - " +
                $"{payment.Invoice.InvoiceDateTime.ToString("MM/dd/yyyy")} - " +
                $"{payment.Invoice.InvoiceStatus} - " +
                $"(#{payment.InvoiceId.ToString().Substring(0, 6)})";

            paymentVoid.CustomerNameDisplay =
                $"{payment.Invoice.Customer.LastName}, " +
                $"{payment.Invoice.Customer.FirstName}";

            paymentVoid.PetNameDisplay = payment.Invoice.Pet.PetName;

            paymentVoid.PaymentDateTimeDisplay = payment.PaymentDateTime.ToString("MM/dd/yyyy hh:mm tt");

            paymentVoid.PaymentMethodDisplay = payment.PaymentMethod.ToString();

            paymentVoid.AmountDisplay = payment.Amount.ToString("C");

            paymentVoid.TransactionReferenceDisplay = payment.TransactionReference;

            paymentVoid.ProcessedByEmployeeDisplay =
                $"{payment.ProcessedByEmployee.LastName}, " +
                $"{payment.ProcessedByEmployee.FirstName}";

            paymentVoid.StatusDisplay = "Processed";

            paymentVoid.Notes = payment.Notes;

            return View(paymentVoid);

        }


        // POST: Payments/Void
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Void(PaymentVoidVM paymentVoid)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (currentEmployee == null)
            {
                return RedirectToAction("Index", "Staff");
            }

            PaymentModel payment = dbContext.Payments
                .Include(x => x.Invoice)
                .Include(x => x.Invoice.Customer)
                .Include(x => x.Invoice.Pet)
                .Include(x => x.ProcessedByEmployee)
                .FirstOrDefault(x => x.PaymentId == paymentVoid.PaymentId);

            if (payment == null)
            {
                return Content("Payment ID #" + paymentVoid.PaymentId + " does not exist.");
            }

            if (payment.IsVoided)
            {
                return Content("This payment has already been voided.");
            }

            if (!ModelState.IsValid)
            {

                paymentVoid.PaymentId = payment.PaymentId;

                paymentVoid.InvoiceId = payment.InvoiceId;

                paymentVoid.InvoiceDisplay =
                    $"{payment.Invoice.Customer.LastName}, {payment.Invoice.Customer.FirstName}, " +
                    $"{payment.Invoice.Pet.PetName} - " +
                    $"{payment.Invoice.InvoiceDateTime.ToString("MM/dd/yyyy")} - " +
                    $"{payment.Invoice.InvoiceStatus} - " +
                    $"(#{payment.InvoiceId.ToString().Substring(0, 6)})";

                paymentVoid.CustomerNameDisplay =
                    $"{payment.Invoice.Customer.LastName}, " +
                    $"{payment.Invoice.Customer.FirstName}";

                paymentVoid.PetNameDisplay = payment.Invoice.Pet.PetName;

                paymentVoid.PaymentDateTimeDisplay = payment.PaymentDateTime.ToString("MM/dd/yyyy hh:mm tt");

                paymentVoid.PaymentMethodDisplay = payment.PaymentMethod.ToString();

                paymentVoid.AmountDisplay = payment.Amount.ToString("C");

                paymentVoid.TransactionReferenceDisplay = payment.TransactionReference;

                paymentVoid.ProcessedByEmployeeDisplay =
                    $"{payment.ProcessedByEmployee.LastName}, " +
                    $"{payment.ProcessedByEmployee.FirstName}";

                paymentVoid.StatusDisplay = "Processed";

                paymentVoid.Notes = payment.Notes;

                return View(paymentVoid);
            }

            payment.IsVoided = true;

            payment.VoidedDateTime = DateTime.Now;

            payment.VoidReason = paymentVoid.VoidReason;

            payment.VoidedByEmployeeId = currentEmployee.EmployeeId;

            payment.Invoice.AmountPaid -= payment.Amount;

            payment.Invoice.Balance = payment.Invoice.TotalAmount - payment.Invoice.AmountPaid;

            dbContext.SaveChanges();

            return RedirectToAction(
                "Read",
                "Invoices",
                new { invoiceId = payment.Invoice.InvoiceId }
            );

        }


        private PaymentDetailsVM BuildPaymentDetails(ApplicationDbContext dbContext, Guid paymentId)
        {
            PaymentModel payment = dbContext.Payments
                .Include(x => x.Invoice)
                .Include(x => x.Invoice.Customer)
                .Include(x => x.Invoice.Pet)
                .Include(x => x.ProcessedByEmployee)
                .Include(x => x.VoidedByEmployee)
                .FirstOrDefault(x => x.PaymentId == paymentId);

            if (payment == null)
            {
                return null;
            }

            PaymentDetailsVM paymentDetails = new PaymentDetailsVM
            {
                PaymentId = payment.PaymentId,

                InvoiceId = payment.InvoiceId,

                InvoiceDisplay =
                    $"{payment.Invoice.Customer.LastName}, " +
                    $"{payment.Invoice.Customer.FirstName}, " +
                    $"{payment.Invoice.Pet.PetName} - " +
                    $"{payment.Invoice.InvoiceDateTime.ToString("MM/dd/yyyy")} - " +
                    $"{payment.Invoice.InvoiceStatus} - " +
                    $"(#{payment.InvoiceId.ToString().Substring(0, 6)})",

                CustomerNameDisplay =
                    $"{payment.Invoice.Customer.LastName}, " +
                    $"{payment.Invoice.Customer.FirstName}",

                PetNameDisplay = payment.Invoice.Pet.PetName,

                AmountDisplay = payment.Amount.ToString("C"),

                PaymentMethodDisplay = payment.PaymentMethod.ToString(),

                PaymentDateTimeDisplay = payment.PaymentDateTime.ToString("MM/dd/yyyy hh:mm tt"),

                TransactionReference = payment.TransactionReference,

                ProcessedByEmployeeDisplay =
                    $"{payment.ProcessedByEmployee.LastName}, " +
                    $"{payment.ProcessedByEmployee.FirstName}",

                IsVoided = payment.IsVoided,

                StatusDisplay = payment.IsVoided
                    ? "Voided"
                    : "Processed",

                VoidedReasonDisplay = payment.IsVoided && payment.VoidReason.HasValue
                    ? GetEnumDisplayName(payment.VoidReason)
                    : "Not Voided",

                VoidedDateTimeDisplay = payment.VoidedDateTime.HasValue
                    ? payment.VoidedDateTime.Value.ToString("MM/dd/yyyy hh:mm tt")
                    : "Not Voided",

                VoidedByEmployeeNameDisplay = payment.VoidedByEmployee != null
                    ? $"{payment.VoidedByEmployee.LastName}, " +
                      $"{payment.VoidedByEmployee.FirstName}"
                    : "Not Voided",

                Notes = payment.Notes,

            };

            return paymentDetails;
        }


                private List<SelectListItem> BuildInvoiceSelectList(
            ApplicationDbContext dbContext,
            Guid? customerId = null,
            Guid? petId = null)
        {
            List<InvoiceModel> invoices = dbContext.Invoices
                .Include(x => x.Customer)
                .Include(x => x.Pet)
                .OrderBy(x => x.Customer.LastName)
                .ThenBy(x => x.Customer.FirstName)
                .ThenBy(x => x.Pet.PetName)
                .ThenBy(x => x.InvoiceDateTime)
                .ThenBy(x => x.InvoiceId)
                .ToList();

            if (customerId.HasValue)
            {
                invoices = invoices
                    .Where(x => x.CustomerId == customerId.Value)
                    .ToList();
            }

            if (petId.HasValue)
            {
                invoices = invoices
                    .Where(x => x.PetId == petId.Value)
                    .ToList();
            }

            return invoices
                .Select(x => new SelectListItem
                {
                    Value = x.InvoiceId.ToString(),
                    Text =
                        $"{x.Customer.LastName}, {x.Customer.FirstName}, " +
                        $"{x.Pet.PetName} - " +
                        $"{x.InvoiceDateTime:MM/dd/yyyy} - " +
                        $"{x.InvoiceStatus} - " +
                        $"(#{x.InvoiceId.ToString().Substring(0, 6)})"
                })
                .ToList();
        }


                private List<SelectListItem> BuildCustomerSelectList(
            ApplicationDbContext dbContext,
            Guid? petId = null)
        {
            List<CustomerModel> customers = dbContext.Customers
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToList();

            if (petId.HasValue)
            {
                List<Guid> customerIds = dbContext.CustomerPets
                    .Where(x => x.PetId == petId.Value)
                    .Select(x => x.CustomerId)
                    .ToList();

                // Preserve historical invoice relationships in Search.
                customerIds.AddRange(
                    dbContext.Invoices
                        .Where(x => x.PetId == petId.Value)
                        .Select(x => x.CustomerId)
                        .ToList());

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
            Guid? customerId = null)
        {
            List<PetModel> pets = dbContext.Pets
                .OrderBy(x => x.PetName)
                .ToList();

            if (customerId.HasValue)
            {
                List<Guid> petIds = dbContext.CustomerPets
                    .Where(x => x.CustomerId == customerId.Value)
                    .Select(x => x.PetId)
                    .ToList();

                // Preserve historical invoice relationships in Search.
                petIds.AddRange(
                    dbContext.Invoices
                        .Where(x => x.CustomerId == customerId.Value)
                        .Select(x => x.PetId)
                        .ToList());

                pets = pets
                    .Where(x => petIds.Contains(x.PetId))
                    .ToList();
            }

            return pets
                .Select(x => new SelectListItem
                {
                    Value = x.PetId.ToString(),
                    Text = $"{x.PetName} - {x.Species} - {x.Breed}"
                })
                .ToList();
        }

        private List<SelectListItem> BuildEmployeeSelectList(ApplicationDbContext dbContext)
        {
            return dbContext.Employees
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToList()
                .Select(x => new SelectListItem
                {
                    Value = x.EmployeeId.ToString(),
                    Text = $"{x.LastName}, {x.FirstName}",
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

        private string GetEnumDisplayName(Enum enumValue)
        {
            DisplayAttribute displayAttribute = enumValue
                .GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            return displayAttribute != null
                ? displayAttribute.GetName()
                : enumValue.ToString();
        }

    }
}