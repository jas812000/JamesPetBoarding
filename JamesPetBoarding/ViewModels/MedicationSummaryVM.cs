using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class MedicationSummaryVM
    {
        public Guid MedicationId { get; set; }

        public string MedicationName { get; set; }

        public string Dosage { get; set; }

    }
}