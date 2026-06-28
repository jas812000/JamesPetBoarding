using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CustomerReportRowVM
    {

        public Guid? CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public string ActiveStatusDisplay { get; set; }

        public string AddressDisplay { get; set; }

        public string CityDisplay { get; set; }

        public string StateDisplay { get; set; }

        public string ZipCodeDisplay { get; set; }

        public string PhoneDisplay { get; set; }

        public string EmailDisplay { get; set; }

        public string NotesDisplay { get; set; }
    }
}