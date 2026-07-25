using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class PetSearchVM
    {

        [MaxLength(50)]
        public string PetName { get; set; }

        public SpeciesEnum? Species { get; set; }

        [MaxLength(50)]
        public string Breed { get; set; }

        public SexEnum? Sex { get; set; }

        public bool? IsActive { get; set; }

        public List<PetSummaryVM> PetSummaryResults { get; set; } = new List<PetSummaryVM>();

    }
}