using JamesPetBoarding.Enums;
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
        // /Services/Create?serviceName=MedicationAdministration&basePrice=10.00&pricingType=PerService&notes=
        public ActionResult Create(
            ServiceNameEnum serviceName,
            decimal basePrice,
            PricingTypeEnum pricingType,
            string notes
        )
        {
           
            ApplicationDbContext dbContext = new ApplicationDbContext();

            if (basePrice <= 0) 
            { 
                return Content("Base price must be greater than zero."); 
            }

            ServiceModel existingService = dbContext.Services.FirstOrDefault(x => x.ServiceName == serviceName);

            if (existingService != null)
            {
                return Content(serviceName + " already exists.");
            }

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

            string serviceNameDisplay = service.ServiceName.ToString();

            switch (service.ServiceName) 
            {
                case ServiceNameEnum.FullGrooming:
                    serviceNameDisplay = "Full Grooming";
                    break;

                case ServiceNameEnum.NailTrim:
                    serviceNameDisplay = "Nail Trim";
                    break;

                case ServiceNameEnum.EarCleaning:
                    serviceNameDisplay = "Ear Cleaning";
                    break;

                case ServiceNameEnum.TeethBrushing:
                    serviceNameDisplay = "Teeth Brushing";
                    break;

                case ServiceNameEnum.HandFeeding:
                    serviceNameDisplay = "Hand Feeding";
                    break;

                case ServiceNameEnum.FoodPreparation:
                    serviceNameDisplay = "Food Preparation";
                    break;

                case ServiceNameEnum.MedicationAdministration:
                    serviceNameDisplay = "Medication Administration";
                    break;

                case ServiceNameEnum.ExtraPlayTime:
                    serviceNameDisplay = "Extra Play Time";
                    break;

                case ServiceNameEnum.ExtraWalk:
                    serviceNameDisplay = "Extra Walk";
                    break;

                case ServiceNameEnum.OneOnOnePlay:
                    serviceNameDisplay = "One on One Play";
                    break;

                case ServiceNameEnum.LatePickUp:
                    serviceNameDisplay = "Late Pick Up";
                    break;

                case ServiceNameEnum.AfterHoursPickup:
                    serviceNameDisplay = "After Hours Pick Up";
                    break;

                case ServiceNameEnum.EarlyDropOff:
                    serviceNameDisplay = "Early Drop Off";
                    break;

                case ServiceNameEnum.BoardingUpgrade:
                    serviceNameDisplay = "Boarding Upgrade";
                    break;

                case ServiceNameEnum.LuxurySuiteUpgrade:
                    serviceNameDisplay = "Luxury Suite Upgrade";
                    break;

                case ServiceNameEnum.TrainingSession:
                    serviceNameDisplay = "Training Session";
                    break;

                case ServiceNameEnum.BehavioralAssessment:
                    serviceNameDisplay = "Behavioral Assessment";
                    break;

            }

            string pricingTypeDisplay = service.PricingType.ToString();

            switch (service.PricingType) 
            {
                case PricingTypeEnum.PerPet:
                    pricingTypeDisplay = "Per Pet";
                    break;

                case PricingTypeEnum.PerNight:
                    pricingTypeDisplay = "Per Night";
                    break;

                case PricingTypeEnum.PerDay:
                    pricingTypeDisplay = "Per Day";
                    break;

                case PricingTypeEnum.PerHour:
                    pricingTypeDisplay = "Per Hour";
                    break;

                case PricingTypeEnum.PerStay:
                    pricingTypeDisplay = "Per Stay";
                    break;

                case PricingTypeEnum.PerService:
                    pricingTypeDisplay = "Per Service";
                    break;

            }

            return Content(
                "Service ID #" + service.ServiceId +
                "<br />Service Name: " + serviceNameDisplay +
                "<br />Base Price: $" + service.BasePrice.ToString("F2") +
                "<br />Pricing Type: " + pricingTypeDisplay +
                "<br />Notes: " + notesDisplay
            );
        }

        // GET: Services/Update
        // /Services/Update?serviceId=USE_EXISTING_SERVICE_ID&serviceName=MedicationAdministration&basePrice=9.99&pricingType=PerService&notes=
        public ActionResult Update(
            Guid serviceId,
            ServiceNameEnum serviceName,
            decimal basePrice,
            PricingTypeEnum pricingType,
            string notes
        )
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            ServiceModel service = dbContext.Services.FirstOrDefault(x => x.ServiceId == serviceId);

            if (service == null) { return Content("Service ID #" + serviceId + " does not exist."); }

            if (basePrice <= 0) { return Content("Base price must be greater than zero."); }

            ServiceModel existingService = dbContext.Services.FirstOrDefault(x => x.ServiceId != serviceId && x.ServiceName == serviceName);

            if (existingService != null)
            {
                return Content(serviceName + " already exists.");
            }

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
