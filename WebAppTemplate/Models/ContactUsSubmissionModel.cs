using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class ContactUsSubmissionModel
    {
        [Key]
        public Guid SubmissionId { get; set; }

        public ContactUsSubmissionModel()
        {
            SubmissionId = Guid.NewGuid();
        }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(20)]
        public string Phone { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(2000)]
        public string Message { get; set; }

        public DateTime SubmissionDateTime { get; set; }

    }
}