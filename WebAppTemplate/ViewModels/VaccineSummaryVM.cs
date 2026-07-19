using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VaccineSummaryVM
    {
        public Guid VaccineId { get; set; }

        public string VaccineName { get; set; }

        public string SpeciesDisplay { get; set; }

        public string RequiredDisplay { get; set; }
    }
}