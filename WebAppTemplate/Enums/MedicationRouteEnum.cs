using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum MedicationRouteEnum
    {

        [Display(Name = "Oral")]
        Oral = 1,

        [Display(Name = "Topical")]
        Topical = 2,

        [Display(Name = "Intramuscular")]
        Intramuscular = 3,

        [Display(Name = "Subcutaneous")]
        Subcutaneous = 4,

        [Display(Name = "Inhalation")]
        Inhalation = 5,

        [Display(Name = "Rectal")]
        Rectal = 6,

        [Display(Name = "Otic (Ear)")]
        Otic = 7,

        [Display(Name = "Ophthalmic (Eye)")]
        Ophthalmic = 8,

        [Display(Name = "Nasal")]
        Nasal = 9,

        [Display(Name = "Other")]
        Other = 10

    }
}