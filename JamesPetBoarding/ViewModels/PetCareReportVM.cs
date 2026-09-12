using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class PetCareReportVM
    {

        public PetCareReportFilterVM PetCareReportFilter { get; set; }

        public List<PetCareReportRowVM> PetCareReportRows { get; set; }

        public SelectList PetSelectList { get; set; }

        public SelectList CustomerSelectList { get; set; }

        public int TotalCount { get; set; }

        public int DietCount { get; set; }

        public int MedicationCount { get; set; }

    }
}