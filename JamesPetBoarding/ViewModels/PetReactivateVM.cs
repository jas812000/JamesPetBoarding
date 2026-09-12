using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PetReactivateVM
    {
        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public string SpeciesDisplay { get; set; }

        public string BreedDisplay { get; set; }

        public string SexDisplay { get; set; }

        public string BirthDateDisplay { get; set; }

        public string AgeDisplay { get; set; }

        public string ActiveStatusDisplay { get; set; }

        public string NotesDisplay { get; set; }

        public string InactivationReasonDisplay { get; set; }

        public string InactivationDateDisplay { get; set; }

        public string InactivationNotesDisplay { get; set; }

        public DateTime? ReactivationDate { get; set; }

        [MaxLength(500)]
        public string ReactivationNotes { get; set; }

    }
}
