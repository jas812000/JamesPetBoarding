using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class SpeciesReportVM
    {

        public SpeciesReportFilterVM SpeciesReportFilter { get; set; }

        public List<SpeciesReportRowVM> SpeciesReportRows { get; set; }

        public int TotalPetCount { get; set; }

        public int ActivePetCount { get; set; }

        public int InactivePetCount { get; set; }

        public int CurrentBoarderCount { get; set; }
    }
}