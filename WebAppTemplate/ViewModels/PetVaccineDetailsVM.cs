using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PetVaccineDetailsVM
    {
        public Guid PetVaccineId { get; set; }

        public string VaccineNameDisplay { get; set; }

        public string DateGivenDisplay { get; set; }

        public string ExpirationDateDisplay { get; set; }

        public string DocumentFilePath { get; set; }

        public string Notes { get; set; }
    }
}