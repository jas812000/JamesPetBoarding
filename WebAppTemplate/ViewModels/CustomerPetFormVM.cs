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
    public class CustomerPetFormVM
    {

        public Guid CustomerPetId { get; set; }

        [Required]
        public Guid PetId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public RelationshipTypeEnum RelationshipType { get; set; }

    }
}