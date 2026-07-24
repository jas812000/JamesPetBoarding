using JamesPetBoarding.Enums;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VaccineSearchVM
    {
        public VaccineSearchVM() 
        {
            VaccineSummaryResults = new List<VaccineSummaryVM>(); 
        }

        public string VaccineName { get; set; }

        public SpeciesEnum? Species { get; set; }

        public bool? RequiredFlag { get; set; }

        public List<VaccineSummaryVM> VaccineSummaryResults { get; set; }

    }
}