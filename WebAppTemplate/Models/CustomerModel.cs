using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class CustomerModel
    {

        [Key]
        public Guid CustomerId { get; set; }

        public CustomerModel()
        {
            CustomerId = Guid.NewGuid();

            CustomerPets = new List<CustomerPetModel>();
            EmergencyContacts = new List<EmergencyContactModel>();
            Boardings = new List<BoardingModel>();
            Invoices = new List<InvoiceModel>();
        }

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

        public bool IsActive { get; set; } = true;

        public InactiveReasonEnum? InactiveReason { get; set; }

        public DateTime? InactivatedDate { get; set; }

        [MaxLength(500)]
        public string InactiveNotes { get; set; }

        public DateTime? ReactivatedDate { get; set; }

        [MaxLength(500)]
        public string ReactivatedNotes { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

        public List<CustomerPetModel> CustomerPets {  get; set; }

        public List<EmergencyContactModel> EmergencyContacts { get; set; }

        public List<BoardingModel> Boardings { get; set; }

        public List<InvoiceModel> Invoices { get; set; }

    }
}