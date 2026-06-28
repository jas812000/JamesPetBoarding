using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class BoardingModel
    {

        [Key]
        public Guid BoardingId { get; set; }

        public BoardingModel()
        {
            BoardingId = Guid.NewGuid();
        }

        [Required]
        public Guid CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public CustomerModel Customer { get; set; }

        [Required]
        public Guid PetId { get; set; }

        [ForeignKey("PetId")]
        public PetModel Pet { get; set; }

        [Required]
        public Guid BoardingUnitId { get; set; }

        [ForeignKey("BoardingUnitId")]
        public BoardingUnitModel BoardingUnit { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        public DateTime? ActualCheckInDateTime { get; set; }

        public Guid? CheckedInByEmployeeId { get; set; }

        [ForeignKey("CheckedInByEmployeeId")]
        public EmployeeModel CheckedInByEmployee { get; set; }

        public DateTime? ActualCheckOutDateTime { get; set; }

        public Guid? CheckedOutByEmployeeId { get; set; }

        [ForeignKey("CheckedOutByEmployeeId")]
        public EmployeeModel CheckedOutByEmployee { get; set; }

        public DateTime? CancelledDateTime { get; set; }

        public Guid? CancelledByEmployeeId { get; set; }

        [ForeignKey("CancelledByEmployeeId")]
        public EmployeeModel CancelledByEmployee { get; set; }

        [MaxLength(1000)]
        public string CancelledReason { get; set; }

        [Required]
        public BoardingStatusEnum Status { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

        public List<InvoiceItemModel> InvoiceItems { get; set; }

    }
}