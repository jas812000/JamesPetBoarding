using JamesPetBoarding.Enums;
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
            InvoiceItems = new List<InvoiceItemModel>();
            Payments = new List<PaymentModel>();

        }

        [Required]
        public Guid CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public CustomerModel Customer { get; set; }

        [Required]
        public Guid PetId { get; set; }

        [ForeignKey("PetId")]
        public PetModel Pet { get; set; }

        public Guid? BoardingId { get; set; }

        [ForeignKey("BoardingId")]
        public BoardingModel Boarding { get; set; }

        [Required]
        public DateTime InvoiceDateTime { get; set; }

        [Required]
        public InvoiceStatusEnum InvoiceStatus { get; set; }

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

        public InvoiceVoidReasonEnum? VoidReason { get; set; }

        [MaxLength(1000)]
        public string VoidNotes { get; set; }

        public DateTime? VoidDateTime { get; set; }

        public Guid? VoidedByEmployeeId { get; set; }

        [ForeignKey("VoidedByEmployeeId")]
        public EmployeeModel VoidedByEmployee { get; set; }

        public List<InvoiceItemModel> InvoiceItems { get; set; }

        public List<PaymentModel> Payments { get; set; }

    }
}