using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class ServiceFormVM
    {
        public Guid ServiceId { get; set; }

        [Required]
        public ServiceNameEnum ServiceName { get; set; }

        [Required]
        public SpeciesEnum Species { get; set; }

        [Required]
        [Range(0, 999999999.99)]
        public decimal BasePrice { get; set; }

        [Required]
        public PricingTypeEnum PricingType { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }
    }
}