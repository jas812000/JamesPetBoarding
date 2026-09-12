using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CurrentBoardersReportRowVM
    {
        public Guid BoardingId { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public Guid? BoardingUnitId { get; set; }

        public string BoardingUnitDisplay { get; set; }

        public string SpeciesDisplay { get; set; }

        public DateTime? CheckInDateTime { get; set; }

        public string CheckInDateTimeDisplay { get; set; }

        public string ScheduledCheckOutDateTimeDisplay { get; set; }

        public string LengthOfStayDisplay { get; set; }

        public string DietDisplay { get; set; }

        public string MedicationStatusDisplay { get; set; }

        public string VaccineComplianceDisplay { get; set; }

        public string NotesDisplay { get; set; }

    }
}