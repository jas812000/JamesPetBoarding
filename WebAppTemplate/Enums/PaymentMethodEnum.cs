using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum PaymentMethodEnum
    {
        [Display(Name = "Cash")]
        Cash = 1,

        [Display(Name = "Credit Card")]
        CreditCard = 2,

        [Display(Name = "Debit Card")]
        DebitCard = 3,

        [Display(Name = "Check")]
        Check = 4,

        [Display(Name = "ACH / Bank Transfer")]
        ACH = 5,

        [Display(Name = "Apple Pay")]
        ApplePay = 6,

        [Display(Name = "Google Pay")]
        GooglePay = 7,

        [Display(Name = "PayPal")]
        PayPal = 8,

        [Display(Name = "Other")]
        Other = 9
    }
}