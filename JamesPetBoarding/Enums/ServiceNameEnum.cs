using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum ServiceNameEnum
    {
        [Display(Name = "Full Grooming")]
        FullGrooming = 1,

        [Display(Name = "Nail Trim")]
        NailTrim = 2,

        [Display(Name = "Bath")]
        Bath = 3,

        [Display(Name = "Ear Cleaning")]
        EarCleaning = 4,

        [Display(Name = "Teeth Brushing")]
        TeethBrushing = 5,

        [Display(Name = "Hand Feeding")]
        HandFeeding = 6,

        [Display(Name = "Food Preparation")]
        FoodPreparation = 7,

        [Display(Name = "Medication Administration")]
        MedicationAdministration = 8,

        [Display(Name = "Extra Play Time")]
        ExtraPlayTime = 9,

        [Display(Name = "Extra Walk")]
        ExtraWalk = 10,

        [Display(Name = "One on One Play")]
        OneOnOnePlay = 11,

        [Display(Name = "Late Pick Up")]
        LatePickUp = 12,

        [Display(Name = "After Hours Pick Up")]
        AfterHoursPickup = 13,

        [Display(Name = "Early Drop Off")]
        EarlyDropOff = 14,

        [Display(Name = "Boarding Upgrade")]
        BoardingUpgrade = 15,

        [Display(Name = "Luxury Suite Upgrade")]
        LuxurySuiteUpgrade = 16,

        [Display(Name = "Training Session")]
        TrainingSession = 17,

        [Display(Name = "Behavioral Assessment")]
        BehavioralAssessment = 18,

        [Display(Name = "Detangling")]
        Detangling = 18


    }
}