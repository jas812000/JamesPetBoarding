using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CustomerSearchResultVM
    {
        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public string PhoneDisplay { get; set; }

        public string EmailDisplay { get; set; }

        public string ActiveStatusDisplay { get; set; }

        public bool IsActive { get; set; }
    }
}