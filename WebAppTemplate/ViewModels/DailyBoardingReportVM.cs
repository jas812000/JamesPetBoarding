using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class DailyBoardingReportVM
    {
        public DailyBoardingReportFilterVM DailyBoardingReportFilter { get; set; }

        public List<DailyBoardingReportRowVM> DailyBoardingReportRows { get; set; }

        public SelectList PetSelectList { get; set; }

        public SelectList CustomerSelectList { get; set; }

        public bool HasSearched { get; set; }

        public int ScheduledArrivalCount { get; set; }

        public int ActualArrivalCount { get; set; }

        public int ScheduledDepartureCount { get; set; }

        public int ActualDepartureCount { get; set; }

    }
}