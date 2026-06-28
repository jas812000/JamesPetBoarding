using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class MedicationDetailsVM
    {
        public Guid MedicationId { get; set; }

        public Guid PetId { get; set; }

        public string MedicationName { get; set; }

        public string Dosage { get; set; }

        public string RouteDisplay { get; set; }

        public string FrequencyDisplay { get; set; }

        public string StartDateDisplay { get; set; }

        public string EndDateDisplay { get; set; }

        public string Notes { get; set; }

    }
}