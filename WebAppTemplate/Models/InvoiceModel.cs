using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class InvoiceModel
    {

        [Key]
        public Guid InvoiceId { get; set; }

        public InvoiceModel()
        {
            InvoiceId = Guid.NewGuid();
        }

        [Required]
        public Guid CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public CustomerModel Customer { get; set; }

        [Required]
        public DateTime InvoiceDateTime { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; }

        [Range(0, 999999999.99)]
        public decimal Subtotal { get; set; }

        [Range(0, 999999999.99)]
        public decimal TaxAmount { get; set; }

        [Range(0, 999999999.99)]
        public decimal DiscountAmount { get; set; }

        [Range(0, 999999999.99)]
        public decimal TotalAmount { get; set; }

        [Range(0, 999999999.99)]
        public decimal AmountPaid { get; set; }

        [Range(0, 999999999.99)]
        public decimal Balance {  get; set; }
        
        [MaxLength(2000)]
        public string Notes { get; set; }

        public List<InvoiceItemModel> InvoiceItems { get; set; }

        public List<PaymentModel> Payments { get; set; }

    }
}