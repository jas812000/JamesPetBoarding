using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class EmployeeFormVM
    {
        public Guid EmployeeId { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        public EmployeeRoleEnum Role { get; set; }

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; }

        [Required, MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public bool ConfirmPossibleDuplicate { get; set; }

    }
}
