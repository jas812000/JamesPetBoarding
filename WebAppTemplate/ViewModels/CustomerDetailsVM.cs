using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CustomerDetailsVM
    {
        public CustomerDetailsVM() 
        { 
            EmergencyContacts = new List<EmergencyContactSummaryVM>(); 

            CustomerPets = new List<CustomerPetSummaryVM>(); 
        }

        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public string PhoneDisplay {  get; set; }

        public string EmailDisplay { get; set; }

        public string AddressDisplay { get; set; }

        public string CityStateZipDisplay { get; set; }

        public string NotesDisplay { get; set; }

        public bool IsActive { get; set; }

        public string ActiveStatusDisplay { get; set; }

        public string InactivationReasonDisplay { get; set; }

        public string InactivationDateDisplay { get; set; }

        public string InactiveNotesDisplay { get; set; }

        public string ReactivationDateDisplay { get; set; }

        public string ReactivationNotesDisplay { get; set; }

        public List<EmergencyContactSummaryVM> EmergencyContacts { get; set; }

        public List<CustomerPetSummaryVM> CustomerPets { get; set; }
    }
}