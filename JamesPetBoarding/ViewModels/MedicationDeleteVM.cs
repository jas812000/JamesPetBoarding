using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class MedicationDeleteVM
    {
        public Guid MedicationId { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public string MedicationName { get; set; }

        public string Dosage { get; set; }

        public string RouteDisplay { get; set; }

        public string FrequencyDisplay { get; set; }

        public string StartDateDisplay { get; set; }

        public string EndDateDisplay { get; set; }

        public string NotesDisplay { get; set; }
    }
}