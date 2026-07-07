using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class CustomerPetFormVM
    {

        public Guid CustomerPetId { get; set; }

        [Required]
        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        [Required]
        public RelationshipTypeEnum RelationshipType { get; set; }

        public SelectList PetSelectList { get; set; }

    }
}