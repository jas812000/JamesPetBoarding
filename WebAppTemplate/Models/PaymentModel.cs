using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class PaymentModel
    {

        [Key]
        public Guid PaymentId { get; set; }

        public PaymentModel()
        {
            PaymentId = Guid.NewGuid();
        }

        [Required]
        public Guid InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public InvoiceModel Invoice { get; set; }

        [Required]
        public DateTime PaymentDateTime { get; set; }

        [Required]
        public PaymentMethodEnum PaymentMethod { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "999999999.99")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(200)]
        public string TransactionReference { get; set; }

        [Required]
        public Guid ProcessedByEmployeeId { get; set; }

        [ForeignKey("ProcessedByEmployeeId")]
        public EmployeeModel ProcessedByEmployee { get; set; }

        public bool IsVoided { get; set; }

        public DateTime? VoidedDateTime { get; set; }

        public TransactionVoidReasonEnum? VoidReason { get; set; }

        public Guid? VoidedByEmployeeId { get; set; }

        [ForeignKey("VoidedByEmployeeId")]
        public EmployeeModel VoidedByEmployee { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }   

    }
}