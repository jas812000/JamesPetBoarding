using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VaccineComplianceReportRowVM
    {
        public Guid PetVaccineId { get; set; }

        public Guid VaccineId { get; set; }

        public string VaccineNameDisplay { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public string SpeciesDisplay { get; set; }

        public string RequiredFlagDisplay { get; set; }

        public string DateGivenDisplay { get; set; }

        public string ExpirationDateDisplay { get; set; }

        public string VaccineComplianceStatusDisplay { get; set; }

        public int DaysUntilExpiration { get; set; }

        public string DocumentFilePathDisplay { get; set; }

        public string NotesDisplay { get; set; }

    }
}