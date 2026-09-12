using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CustomerPetSummaryVM
    {
        public Guid CustomerPetId { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public string SpeciesDisplay { get; set; }

        public string BreedDisplay { get; set; }

        public string RelationshipTypeDisplay { get; set; }

    }
}