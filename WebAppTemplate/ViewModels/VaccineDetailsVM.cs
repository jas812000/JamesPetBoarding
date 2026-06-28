using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VaccineDetailsVM
    {

        public Guid VaccineId { get; set; }

        public string VaccineName { get; set; }

        public string SpeciesDisplay { get; set; }

        public bool RequiredFlag { get; set; }

        public string RequiredDisplay { get; set; }

        public string Notes { get; set; }
    }
}