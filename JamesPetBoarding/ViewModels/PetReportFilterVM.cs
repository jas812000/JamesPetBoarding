using JamesPetBoarding.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PetReportFilterVM
    {
      
        public Guid? PetId { get; set; }

        public ActiveStatusEnum? ActiveStatus { get; set; }

        public Guid? VetId { get; set; }

        public Guid? CustomerId { get; set; }

        public string PetName { get; set; }

        public SpeciesEnum? Species { get; set; }

        public string Breed { get; set; }

        public SexEnum? Sex { get; set; }

        public DateTime? BirthDate { get; set; }

        [Range(
            typeof(decimal),
            "0.01",
            "9999.99",
            ErrorMessage = "Weight must be greater than zero.")]
        public decimal? Weight { get; set; }

        public WeightUnitEnum WeightUnit { get; set; }

    }
}