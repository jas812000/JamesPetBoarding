using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum UnitTypeEnum
    {
        [Display(Name = "Kennel")]
        Kennel = 1,

        [Display(Name = "Suite")]
        Suite = 2,

        [Display(Name = "Bird Cage")]
        BirdCage = 3,

        [Display(Name = "Cat Condo")]
        CatCondo = 4,

        [Display(Name = "Run")]
        Run = 5,

        [Display(Name = "Isolation")]
        Isolation = 6,

        [Display(Name = "Aquarium")]
        Aquarium = 7,

        [Display(Name = "Small Animal Enclosure")]
        SmallAnimalEnclosure = 8,

        [Display(Name = "Large Animal Enclosure")]
        LargeAnimalEnclosure = 9,

        [Display(Name = "Other")]
        Other = 10

    }
}