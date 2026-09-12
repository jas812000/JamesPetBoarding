using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum EmployeeRoleEnum
    {
        [Display(Name = "Administrator")]
        Admin = 1,
            
        [Display(Name = "Manager")]
        Manager = 2,

        [Display(Name = "Supervisor")]
        Supervisor = 3,

        [Display(Name = "Front Desk")]
        FrontDesk = 4,

        [Display(Name = "Kennel Staff")]
        KennelStaff = 5,

        [Display(Name = "Caretaker")]
        Caretaker = 6,

        [Display(Name = "Groomer")]
        Groomer = 7,

        [Display(Name = "Veterinary Technician")]
        VeterinaryTechnician = 8,

        [Display(Name = "Veterinarian")]
        Veterinarian = 9,

        [Display(Name = "Trainer")]
        Trainer = 10

    }
}