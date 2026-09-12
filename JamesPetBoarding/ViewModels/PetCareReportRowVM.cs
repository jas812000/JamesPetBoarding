using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PetCareReportRowVM
    {
        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public string CustomerNameDisplay { get; set; }

        public string SpeciesDisplay { get; set; }

        public string DietNameDisplay { get; set; }

        public string FeedingAmountDisplay { get; set; }

        public string FeedingFrequencyDisplay { get; set; }

        public string DietNotesDisplay { get; set; }

        public string MedicationNameDisplay { get; set; }

        public string DosageDisplay { get; set; }

        public string MedicationRouteDisplay { get; set; }

        public string MedicationFrequencyDisplay { get; set; }

        public string MedicationStartDateDisplay { get; set; }

        public string MedicationEndDateDisplay { get; set; }

        public string MedicationNotesDisplay { get; set; }

    }
}