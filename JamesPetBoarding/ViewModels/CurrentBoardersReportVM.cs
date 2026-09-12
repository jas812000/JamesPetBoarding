using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class CurrentBoardersReportVM
    {
        public CurrentBoardersReportFilterVM CurrentBoardersReportFilter { get; set; }

        public List<CurrentBoardersReportRowVM> CurrentBoardersReportRows { get; set; }

        public SelectList CustomerSelectList { get; set; }

        public SelectList PetSelectList { get; set; }

        public SelectList BoardingUnitSelectList { get; set; }

        public bool HasSearched { get; set; }

        public int TotalCurrentBoarderCount { get; set; }

    }
}