using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace JamesPetBoarding.ViewModels
{
    public class VeterinarianSummaryVM
    {
        public Guid VetId { get; set; }

        public string ClinicNameDisplay { get; set; }

        public string FullNameCredentialsDisplay { get; set; }

        public string CityStateDisplay { get; set; }

        public string PhoneDisplay { get; set; }

        public string EmailDisplay { get; set; }

        public string ActiveStatusDisplay { get; set; }

        public bool IsActive { get; set; }

        public string NotesDisplay { get; set; }
    }
}
