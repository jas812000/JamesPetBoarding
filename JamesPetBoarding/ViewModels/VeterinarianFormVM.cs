using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VeterinarianFormVM
    {
        public Guid VetId { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string Credentials { get; set; }

        [Required, MaxLength(100)]
        public string ClinicName { get; set; }

        [Required, MaxLength(300)]
        public string Address { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required]
        public StateEnum? State { get; set; }

        [Required, MaxLength(20)]
        public string ZipCode { get; set; }

        [Required]
        [RegularExpression(
            @"^\d{3}-\d{3}-\d{4}$",
            ErrorMessage = "Phone number must be in the format 972-555-1212.")]
        [MaxLength(12)]
        public string Phone { get; set; }

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }
    }
}