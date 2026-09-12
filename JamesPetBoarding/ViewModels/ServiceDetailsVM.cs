using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class ServiceDetailsVM
    {
        public Guid ServiceId { get; set; }

        public string ServiceNameDisplay { get; set; }

        public string SpeciesDisplay { get; set; }

        public string BasePriceDisplay { get; set; }

        public string PricingTypeDisplay { get; set; }

        public string NotesDisplay { get; set; }
    }
}