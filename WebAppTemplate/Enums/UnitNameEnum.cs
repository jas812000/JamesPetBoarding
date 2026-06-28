using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum UnitNameEnum
    {

        // Kennels - Planets
        [Display(Name = "Mercury")]
        Mercury = 1,

        [Display(Name = "Venus")]
        Venus = 2,
        
        [Display(Name = "Earth")]
        Earth = 3,
        
        [Display(Name = "Mars")]
        Mars = 4,
        
        [Display(Name = "Jupiter")]
        Jupiter = 5,

        [Display(Name = "Saturn")]
        Saturn = 6,

        [Display(Name = "Uranus")]
        Uranus = 7,

        [Display(Name = "Neptune")]
        Neptune = 8,

        [Display(Name = "Pluto")]
        Pluto = 9,


        // Suites - Gemstones
        [Display(Name = "Diamond")]
        Diamond = 10,

        [Display(Name = "Jade")]
        Jade = 11,

        [Display(Name = "Pearl")]
        Pearl = 12,

        [Display(Name = "Ruby")]
        Ruby = 13,


        // Cat Condos - Cities
        [Display(Name = "Paris")]
        Paris = 14,

        [Display(Name = "Rome")]
        Rome = 15,

        [Display(Name = "Tokyo")]
        Tokyo = 16,

        [Display(Name = "Vienna")]
        Vienna = 17,

        [Display(Name = "Prague")]
        Prague = 18,
        

        // Bird Cages - Trees
        [Display(Name = "Oak")]
        Oak = 19, 

        [Display(Name = "Maple")]
        Maple = 20, 

        [Display(Name = "Cedar")]
        Cedar = 21, 

        [Display(Name = "Birch")]
        Birch = 22,


        // Small Animal Enclosures - Flowers
        [Display(Name = "Carnation")]
        Carnation = 23,

        [Display(Name = "Rose")]
        Rose = 24,

        [Display(Name = "Daisy")]
        Daisy = 25,

        [Display(Name = "Orchid")]
        Orchid = 26, 


        // Large Animal Enclosures - Mountains
        [Display(Name = "Everest")]
        Everest = 27,

        [Display(Name = "Fuji")]
        Fuji = 28,

        [Display(Name = "Kilimanjaro")]
        Kilimanjaro = 29,


        // Isolation Units - Colors
        [Display(Name = "Red")]
        Red = 30,

        [Display(Name = "Orange")]
        Orange = 31,

        [Display(Name = "Green")]
        Green = 32,

        [Display(Name = "Blue")]
        Blue = 33,


        // Aquariums - Ocean & Seas
        [Display(Name = "Pacific")]
        Pacific = 34,

        [Display(Name = "Atlantic")]
        Atlantic = 35,

        [Display(Name = "Caribbean")]
        Caribbean = 36,

        [Display(Name = "Mediterranean")]
        Mediterranean = 37,


        // Miscellaneous - Constellations
        [Display(Name = "Orion")]
        Orion = 38,

        [Display(Name = "Cassiopeia")]
        Cassiopeia = 39,

        [Display(Name = "Andromeda")]
        Andromeda = 40,
    }
}