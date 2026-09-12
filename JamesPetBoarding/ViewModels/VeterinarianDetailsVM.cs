using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VeterinarianDetailsVM
    {
        public VeterinarianDetailsVM()
        { 
            PetSummaries = new List<PetSummaryVM>();  
        }

        public Guid VetId { get; set; }

        public string ClinicNameDisplay { get; set; }

        public string FullNameCredentialsDisplay { get; set; }

        public string AddressDisplay { get; set; }

        public string CityStateZipDisplay { get; set; }

        public string PhoneDisplay { get; set; }

        public string EmailDisplay { get; set; }

        public string ActiveStatusDisplay { get; set; }

        public bool IsActive { get; set; }

        public string NotesDisplay { get; set; }

        public List<PetSummaryVM> PetSummaries { get; set; }
    }
}