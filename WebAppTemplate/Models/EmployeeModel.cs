using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
            CheckedInBoardings = new List<BoardingModel>();
            CheckedOutBoardings = new List<BoardingModel>();
            CancelledBoardings = new List<BoardingModel>();
        }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        public EmployeeRoleEnum Role { get; set; }

        [Required, MaxLength(20)]
        public string Phone { get; set; }

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public InactivationReasonEnum? InactivationReason { get; set; }

        public DateTime? InactivationDate { get; set; }

        [MaxLength(500)]
        public string InactivationNotes { get; set; }

        public DateTime? ReactivationDate { get; set; }

        [MaxLength(500)]
        public string ReactivationNotes { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        [InverseProperty("CheckedInByEmployee")]
        public List<BoardingModel> CheckedInBoardings { get; set; }

        [InverseProperty("CheckedOutByEmployee")]
        public List<BoardingModel> CheckedOutBoardings { get; set; }

        [InverseProperty("CancelledByEmployee")]
        public List<BoardingModel> CancelledBoardings { get; set; }

    }
}