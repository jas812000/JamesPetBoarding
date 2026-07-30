using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class BoardingDetailsVM
    {
        public Guid BoardingId { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public Guid BoardingUnitId { get; set; }

        public string BoardingUnitDisplay { get; set; }

        public BoardingStatusEnum BoardingStatus { get; set; }

        public string StartDateTimeDisplay { get; set; }

        public string EndDateTimeDisplay { get; set; }

        public string StatusDisplay { get; set; }

        public string ActualCheckInDateTimeDisplay { get; set; }

        public string CheckedInByEmployeeNameDisplay { get; set; }

        public string ActualCheckOutDateTimeDisplay { get; set; }

        public string CheckedOutByEmployeeNameDisplay { get; set; }

        public string CancelledDateTimeDisplay { get; set; }

        public string CancelledByEmployeeNameDisplay { get; set; }

        public string CancelledReasonDisplay { get; set; }

        public string NoShowDateTimeDisplay { get; set; }

        public string NoShowByEmployeeNameDisplay { get; set; }

        public string NotesDisplay { get; set; }

    }
}