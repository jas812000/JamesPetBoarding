using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class SpeciesReportFilterVM
    {
        public SpeciesEnum? Species { get; set; }

        public ActiveStatusEnum? ActiveStatus { get; set; }

    }
}