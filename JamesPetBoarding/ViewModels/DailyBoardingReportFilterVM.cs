using JamesPetBoarding.Enums;
using JamesPetBoarding.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class DailyBoardingReportFilterVM
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? PetId { get; set; }

        public BoardingStatusEnum? BoardingStatus { get; set; }

    }
}
