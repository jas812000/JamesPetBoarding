using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class PetReportVM
    {
        public PetReportFilterVM PetReportFilter { get; set; }

        public List<PetReportRowVM> PetReportRows { get; set; }

        public SelectList PetSelectList { get; set; }

        public SelectList VeterinarianSelectList { get; set; }

        public SelectList CustomerSelectList { get; set; }

        public int TotalCount { get; set; }

        public int ActiveCount { get; set; }

        public int InactiveCount { get; set; }

    }
}