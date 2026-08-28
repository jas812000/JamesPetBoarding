using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class SpeciesReportRowVM
    {
        public SpeciesEnum Species { get; set; }

        public string SpeciesDisplay { get; set; }

        public int TotalPetCount { get; set; }

        public int ActivePetCount { get; set; }

        public int InactivePetCount { get; set; }

        public int CurrentBoarderCount { get; set; }

        public int TotalBoardingCount { get; set; }


    }
}