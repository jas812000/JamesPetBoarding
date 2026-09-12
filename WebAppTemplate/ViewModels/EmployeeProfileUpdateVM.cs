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

        [Required, MaxLength(20)]
        public string Phone { get; set; }
    }
}