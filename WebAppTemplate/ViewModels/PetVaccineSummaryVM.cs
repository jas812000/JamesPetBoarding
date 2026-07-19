using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PetVaccineSummaryVM
    {
        
        public Guid PetVaccineId { get; set; }

        public Guid PetId { get; set; }

        public string VaccineNameDisplay { get; set; }

        public string DateGivenDisplay { get; set; }

        public string ExpirationDateDisplay { get; set; }

    }
}