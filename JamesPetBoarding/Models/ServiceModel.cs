using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class ServiceModel
    {

        [Key]
        public Guid ServiceId { get; set; }

        public ServiceModel()
        {
            ServiceId = Guid.NewGuid();
            InvoiceItems = new List<InvoiceItemModel>();
        }

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

        public List<InvoiceItemModel> InvoiceItems { get; set; }

    }
}