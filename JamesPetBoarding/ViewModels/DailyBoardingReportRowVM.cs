using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class DailyBoardingReportRowVM
    {
        public Guid BoardingId { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public Guid? BoardingUnitId { get; set; }

        public string BoardingUnitDisplay { get; set; }

        public string StartDateDisplay { get; set; }

        public string EndDateDisplay { get; set; }

        public string CheckInDateTimeDisplay { get; set; }

        public string CheckOutDateTimeDisplay { get; set; }

        public string BoardingStatusDisplay { get; set; }

        public string VaccineComplianceDisplay { get; set; }

        public string BalanceDisplay { get; set; }

        public string NotesDisplay { get; set; }

    }
}