using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum SpeciesEnum
    {
        [Display(Name = "Dog")]
        Dog = 1,

        [Display(Name = "Cat")]
        Cat = 2,

        [Display(Name = "Bird")]
        Bird = 3,

        [Display(Name = "Rabbit")]
        Rabbit = 4,

        [Display(Name = "Horse")]
        Horse = 5,

        [Display(Name = "Reptile")]
        Reptile = 6,

        [Display(Name = "Other")]
        Other = 7
    }
}