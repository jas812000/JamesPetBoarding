using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class EmergencyContactModel
    {

        [Key]
        public Guid EmergencyContactId { get; set; }

        public EmergencyContactModel()
        {
            EmergencyContactId = Guid.NewGuid();
        }

        [Required]
        public Guid CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public CustomerModel Customer { get; set; }
        
        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(300)]
        public string Address { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required]
        public StateEnum State { get; set; }

        [Required, MaxLength(20)]
        public string ZipCode { get; set; }

        [Required, MaxLength(20)]
        public string Phone { get; set; }

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; }

        [Required]
        public EmergencyContactRelationshipEnum RelationshipType { get; set; }

        public bool IsActive { get; set; } = true;

        public InactivationReasonEnum? InactivationReason { get; set; }

        public DateTime? InactivationDate { get; set; }

        [MaxLength(500)]
        public string InactivationNotes { get; set; }

        public DateTime? ReactivationDate { get; set; }

        [MaxLength(500)]
        public string ReactivationNotes { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }
    }
}