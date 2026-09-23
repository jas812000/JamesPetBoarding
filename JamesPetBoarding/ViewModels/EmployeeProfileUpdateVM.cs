using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace JamesPetBoarding.ViewModels
{
    public class EmployeeProfileUpdateVM
    {
        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(
            @"^\d{3}-\d{3}-\d{4}$",
            ErrorMessage = "Phone number must be in the format 972-555-1212.")]
        [MaxLength(12)]
        public string Phone { get; set; }

        public string ProfileImagePath { get; set; }

        public HttpPostedFileBase ProfileImageFile { get; set; }

        public bool RemoveProfileImage { get; set; }
    }
}
