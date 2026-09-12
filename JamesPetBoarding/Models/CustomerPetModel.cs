using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class CustomerPetModel
    {

        [Key]
        public Guid CustomerPetId { get; set; }

        public CustomerPetModel()
        {
            CustomerPetId = Guid.NewGuid();
        }

        [Required]
        public Guid PetId { get; set; }

        [ForeignKey("PetId")]
        public PetModel Pet { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public CustomerModel Customer { get; set; }

        [Required]
        public RelationshipTypeEnum RelationshipType { get; set; }

    }
}