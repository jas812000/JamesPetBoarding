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
    public class PetSummaryVM
    {
        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public string SpeciesDisplay { get; set; }

        public string BreedDisplay { get; set; }

        public string SexDisplay { get; set; }

        public string BirthDateDisplay { get; set; }

        public string AgeDisplay { get; set; }

        public string WeightDisplay { get; set; }

        public bool IsActive { get; set; }

        public string ActiveStatusDisplay { get; set; }

        public Guid? VetId { get; set; }

        public string VeterinarianDisplay { get; set; }

    }
}