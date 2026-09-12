using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CurrentBoardersReportFilterVM
    {
        public Guid? CustomerId { get; set; }

        public Guid? PetId { get; set; }

        public Guid? BoardingUnitId { get; set; }

        public SpeciesEnum? Species { get; set; }

    }
}