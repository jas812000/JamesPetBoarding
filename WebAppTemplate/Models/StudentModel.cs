using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppTemplate.Models
{
    public class StudentModel
    {
        [Key]
        public Guid StudentID { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Range(0.0,4.0)]
        public decimal GPA { get; set; }
    }
}