using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class EmployeeModel
    {
        [Key]
        public Guid EmployeeId { get; set; }

        public EmployeeModel()
        {
            EmployeeId = Guid.NewGuid();
        }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string Role { get; set; }

        [Required, MaxLength(20)]
        public string Phone { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public List<BoardingModel> CheckedInBoardings { get; set; }

        public List<BoardingModel> CheckedOutBoardings { get; set; }

        public List<BoardingModel> CancelledBoardings { get; set; }

    }
}