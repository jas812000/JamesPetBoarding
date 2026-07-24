using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PetDetailsVM
    {
        public PetDetailsVM() 
        { 
            CustomerPets = new List<CustomerPetSummaryVM>();
            Diets = new List<DietSummaryVM>();
            Medications = new List<MedicationSummaryVM>();
            PetVaccines = new List<PetVaccineSummaryVM>();
            Boardings = new List<BoardingSummaryVM>();
        }

        public Guid PetId { get; set; }

        public Guid? VetId { get; set; }

        public string VeterinarianDisplay { get; set; }

        public string PetNameDisplay { get; set; }

        public string SpeciesDisplay { get; set; }

        public string BreedDisplay { get; set; }

        public string SexDisplay { get; set; }

        public string BirthDateDisplay { get; set; }

        public string AgeDisplay { get; set; }

        public string WeightDisplay { get; set; }

        public bool IsActive { get; set; }

        public string ActiveStatusDisplay { get; set; }

        public string InactivationReasonDisplay { get; set; }

        public string InactivationDateDisplay { get; set; }

        public string InactiveNotesDisplay { get; set; }

        public string ReactivationDateDisplay { get; set; }

        public string ReactivationNotesDisplay { get; set; }

        public string NotesDisplay { get; set; }

        public List<CustomerPetSummaryVM> CustomerPets { get; set; }

        public List<DietSummaryVM> Diets { get; set; }

        public List<MedicationSummaryVM> Medications { get; set; }

        public List<PetVaccineSummaryVM> PetVaccines { get; set; }

        public List<BoardingSummaryVM> Boardings { get; set; }
    
    }
}