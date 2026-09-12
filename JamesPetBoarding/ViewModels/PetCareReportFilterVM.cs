using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PetCareReportFilterVM
    {
        public Guid? PetId { get; set; }

        public Guid? CustomerId { get; set; }

        public string PetName { get; set; }

        public SpeciesEnum? Species { get; set; }

        public bool? HasDiet { get; set; }

        public bool? HasMedication { get; set; }
    }
}