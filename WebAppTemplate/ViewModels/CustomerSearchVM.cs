using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CustomerSearchVM
    {

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [EmailAddress, MaxLength(256)]
        public string Email { get; set; }

        public bool? IsActive { get; set; }

        public List<CustomerSearchResultVM> Customers { get; set; } = new List<CustomerSearchResultVM>();


    }
}