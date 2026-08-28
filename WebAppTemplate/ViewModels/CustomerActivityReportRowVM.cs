using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CustomerActivityReportRowVM
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

        public int EmergencyContactCount { get; set; }

        public int PetCount { get; set; }

        public int BoardingCount { get; set; }

        public string LastBoardingDateDisplay { get; set; }

        public int InvoiceCount { get; set; }

        public string TotalInvoiceAmountDisplay { get; set; }

        public string TotalPaymentAmountDisplay { get; set; }

        public string OutstandingBalanceDisplay { get; set; }

        public string LastActivityDateDisplay { get; set; }

        public string IsFrequentCustomerDisplay { get; set; }

    }
}