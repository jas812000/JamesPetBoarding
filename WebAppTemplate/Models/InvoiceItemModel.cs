using JamesPetBoarding.Enums;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class InvoiceItemModel
    {

        [Key]
        public Guid InvoiceItemId { get; set; }

        public InvoiceItemModel()
        {
            InvoiceItemId = Guid.NewGuid();
        }

        public Guid? BoardingId { get; set; }

        [ForeignKey("BoardingId")]
        public BoardingModel Boarding {  get; set; }

        [Required]
        public Guid InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public InvoiceModel Invoice { get; set; }

        [Required]
        public Guid ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public ServiceModel Service { get; set; }

        [Required]
        public InvoiceItemTypeEnum ItemType { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Range(0, 999999999.99)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0, 999999999.99)]
        public decimal LineTotal { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

    }
}