using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.Controllers
{
    public class ServicesController : Controller
    {
        // GET: Services
        public ActionResult Index()
        {
            return View();
        }


        // GET: Services/Create
        // /Services/Create?serviceName=Medicatn%20Administran&basePrice=10.00&pricingType=Per%20Doce&notes=
        public ActionResult Create(
            string serviceName,
            decimal basePrice,
            string pricingType,
            string notes
        )
        {
           
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (string.IsNullOrWhiteSpace(serviceName)) { return Content("Service name is required."); }
            if (basePrice <= 0) { return Content("Base price must be greater than zero."); }
            if (string.IsNullOrWhiteSpace(pricingType)) { return Content("Pricing type is required."); }

            ServiceModel service = new ServiceModel();

            service.ServiceName = serviceName;
            service.BasePrice = basePrice;
            service.PricingType = pricingType;
            service.Notes = notes;

            try
            {
                dbContext.Services.Add(service);
                dbContext.SaveChanges();

                return Content(service.ServiceName + " was successfully created.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: Services/Read
        // /Services/Read?serviceId=USE_EXISTING_SERVICE_ID
        public ActionResult Read(Guid serviceId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            ServiceModel service = dbContext.Services.FirstOrDefault(x => x.ServiceId == serviceId);

            if (service == null) { return Content("Service Id #" + serviceId + " does not exist."); }

            string notesDisplay = string.IsNullOrWhiteSpace(service.Notes) 
                ? "No notes" 
                : service.Notes;

            return Content(
                "Service ID #" + service.ServiceId +
                "<br />Service Name: " + service.ServiceName +
                "<br />Base Price: $" + service.BasePrice.ToString("F2") +
                "<br />Pricing Type: " + service.PricingType +
                "<br />Notes: " + notesDisplay 
            );
        }

        // GET: Services/Update
        // /Services/Update?serviceId=USE_EXISTING_SERVICE_ID&serviceName=Medication%20Administration&basePrice=9.99&pricingType=Per%20Dose&notes=
        public ActionResult Update(
            Guid serviceId,
            string serviceName,
            decimal basePrice,
            string pricingType,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            ServiceModel service = dbContext.Services.FirstOrDefault(x => x.ServiceId == serviceId);

            if (service == null) { return Content("Service ID #" + serviceId + " does not exist."); }

            if (string.IsNullOrWhiteSpace(serviceName)) { return Content("Swervice name is required."); }
            if (basePrice <= 0) { return Content("Base price must be greater than zero."); }
            if (string.IsNullOrWhiteSpace(pricingType)) { return Content("Pricing type is required."); }

            service.ServiceName = serviceName;
            service.BasePrice = basePrice;
            service.PricingType = pricingType;
            service.Notes = notes;

            try
            {
                dbContext.SaveChanges();

                return Content("Service ID #" + service.ServiceId + " was successfully updated.");
            }
            catch (Exception ex )
            {
                return Content(ex.Message);
            }
        }

        // GET: Services/Delete
        // /Services/Delete?serviceId=USE_EXISTING_SERVICE_ID
        public ActionResult Delete(Guid serviceId)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            ServiceModel service = dbContext.Services.FirstOrDefault(x => x.ServiceId == serviceId);

            if (service == null) { return Content("Service ID #" + serviceId + " does not exist."); }

            try
            {
                dbContext.Services.Remove(service);
                dbContext.SaveChanges();

                return Content("Service ID #" + service.ServiceId + " was successfully deleted.");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
