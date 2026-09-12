using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using JamesPetBoarding.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace JamesPetBoarding.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {

        // GET: Services/Search
        public ActionResult Search() 
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewServices(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            ViewBag.CanManageServices = CanManageServices(currentEmployee);

            ServiceSearchVM serviceSearch = new ServiceSearchVM();

            return View(serviceSearch);
        
        }


        // POST: Services/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(ServiceSearchVM serviceSearch) 
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewServices(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            ViewBag.CanManageServices = CanManageServices(currentEmployee);

            List<ServiceModel> services = dbContext.Services.ToList();

            if (serviceSearch.ServiceName.HasValue)
            {
                services = services
                    .Where(x => x.ServiceName == serviceSearch.ServiceName.Value)
                    .ToList();
            }

            if (serviceSearch.Species.HasValue)
            {
                services = services
                    .Where(x => x.Species == serviceSearch.Species.Value)
                    .ToList();
            }

            if (serviceSearch.PricingType.HasValue)
            {
                services = services
                    .Where(x => x.PricingType == serviceSearch.PricingType.Value)
                    .ToList();
            }

            serviceSearch.ServiceSummaryResults.Clear();

            foreach (ServiceModel service in services)
            {
                serviceSearch.ServiceSummaryResults.Add(new ServiceSummaryVM
                {
                    ServiceId = service.ServiceId,
                    ServiceNameDisplay = service.ServiceName.ToString(),
                    SpeciesDisplay = service.Species.ToString(),
                    BasePriceDisplay = service.BasePrice.ToString("C"),
                    PricingTypeDisplay = service.PricingType.ToString()
                });
            }

            return View(serviceSearch);

        }


        // GET: Services/Create
        public ActionResult Create() 
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageServices(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            ServiceFormVM serviceForm = new ServiceFormVM();

            return View(serviceForm);

        }


        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceFormVM serviceForm) 
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageServices(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            if (!ModelState.IsValid)
            {
                return View(serviceForm);
            }

            ServiceModel service = new ServiceModel();

            service.ServiceName = serviceForm.ServiceName;
            service.Species = serviceForm.Species;
            service.BasePrice = serviceForm.BasePrice;
            service.PricingType = serviceForm.PricingType;
            service.Notes = serviceForm.Notes;

            dbContext.Services.Add(service);
            dbContext.SaveChanges();

            return RedirectToAction("Search");

        }


        // GET: Services/Read
        public ActionResult Read(Guid serviceId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanViewServices(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            ServiceModel service = dbContext.Services
                .FirstOrDefault(x => x.ServiceId == serviceId);

            if (service == null) 
            { 
                return Content("Service Id #" + serviceId + " does not exist."); 
            }

            ServiceDetailsVM serviceDetails = new ServiceDetailsVM();

            serviceDetails.ServiceId = service.ServiceId;
            serviceDetails.ServiceNameDisplay = service.ServiceName.ToString();
            serviceDetails.SpeciesDisplay = service.Species.ToString();
            serviceDetails.BasePriceDisplay = service.BasePrice.ToString("C");
            serviceDetails.PricingTypeDisplay = service.PricingType.ToString();
            serviceDetails.NotesDisplay = string.IsNullOrWhiteSpace(service.Notes) 
                ? "No notes" 
                : service.Notes;

            return View(serviceDetails);
        }


        // GET: Services/Update
        public ActionResult Update(Guid serviceId)
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageServices(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            ServiceModel service = dbContext.Services
                .FirstOrDefault(x => x.ServiceId == serviceId);

            if (service == null)
            { 
                return Content("Service ID #" + serviceId + " does not exist."); 
            }

            ServiceFormVM serviceForm = new ServiceFormVM();

            serviceForm.ServiceId = service.ServiceId;
            serviceForm.ServiceName = service.ServiceName;
            serviceForm.Species = service.Species;
            serviceForm.BasePrice = service.BasePrice;
            serviceForm.PricingType = service.PricingType;
            serviceForm.Notes = service.Notes;

            return View(serviceForm);
         
        }


        // POST: Services/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ServiceFormVM serviceForm)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageServices(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            ServiceModel service = dbContext.Services
                .FirstOrDefault(x => x.ServiceId == serviceForm.ServiceId);

            if (service == null)
            {
                return Content("Service ID #" + serviceForm.ServiceId + " does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return View(serviceForm);
            }

            service.ServiceName = serviceForm.ServiceName;
            service.Species = serviceForm.Species;
            service.BasePrice = serviceForm.BasePrice;
            service.PricingType = serviceForm.PricingType;
            service.Notes = serviceForm.Notes;

            dbContext.SaveChanges();

            return RedirectToAction("Read", new { serviceId = service.ServiceId });

        }


        // GET: Service/Delete
        public ActionResult Delete(Guid serviceId) 
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageServices(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            ServiceModel service = dbContext.Services
                .FirstOrDefault(x => x.ServiceId == serviceId);

            if (service == null)
            {
                return Content("Service ID #" + serviceId + " does not exist.");
            }

            ServiceDeleteVM serviceDelete = new ServiceDeleteVM();

            serviceDelete.ServiceId = service.ServiceId;
            serviceDelete.ServiceNameDisplay = service.ServiceName.ToString();
            serviceDelete.SpeciesDisplay = service.Species.ToString(); 
            serviceDelete.BasePriceDisplay = service.BasePrice.ToString("C");
            serviceDelete.PricingTypeDisplay = service.PricingType.ToString();
            serviceDelete.NotesDisplay = string.IsNullOrWhiteSpace(service.Notes)
                ? "No notes"
                : service.Notes;

            return View(serviceDelete);
        }


        // POST: Service/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ServiceDeleteVM serviceDelete) 
        {

            ApplicationDbContext dbContext = new ApplicationDbContext();

            EmployeeModel currentEmployee = GetCurrentEmployee(dbContext);

            if (!CanManageServices(currentEmployee))
            {
                return RedirectToAction("Index", "Staff");
            }

            ServiceModel service = dbContext.Services
                .FirstOrDefault(x => x.ServiceId == serviceDelete.ServiceId);

            if (service == null)
            {
                return Content("Service ID #" + serviceDelete.ServiceId + " does not exist.");
            }

            dbContext.Services.Remove(service);

            dbContext.SaveChanges();


            return RedirectToAction("Search"); 
        
        }


        private EmployeeModel GetCurrentEmployee(ApplicationDbContext dbContext)
        {
            string loggedInEmail = User.Identity.Name;

            return dbContext.Employees
                .FirstOrDefault(x =>
                    x.Email == loggedInEmail &&
                    x.IsActive);

        }


        private bool CanViewServices(EmployeeModel employee)
        {
            return employee != null &&
                (employee.Role == EmployeeRoleEnum.Admin ||
                 employee.Role == EmployeeRoleEnum.Manager ||
                 employee.Role == EmployeeRoleEnum.Supervisor);

        }


        private bool CanManageServices(EmployeeModel employee)
        {
            return employee != null &&
                (employee.Role == EmployeeRoleEnum.Admin ||
                 employee.Role == EmployeeRoleEnum.Manager);

        }
    }
}
