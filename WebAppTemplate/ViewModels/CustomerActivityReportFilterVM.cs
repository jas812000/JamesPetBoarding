using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CustomerActivityReportFilterVM
    {

        public Guid? CustomerId { get; set; }

        public ActiveStatusEnum? ActiveStatus { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        public StateEnum? State { get; set; }

        [MaxLength(20)]
        public string ZipCode { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [EmailAddress, MaxLength(256)]
        public string Email { get; set; }
    }
}