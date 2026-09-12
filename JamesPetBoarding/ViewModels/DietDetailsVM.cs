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
    public class DietDetailsVM
    {
        public Guid DietId { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public string FoodName { get; set; }

        public string Amount { get; set; }

        public string FrequencyDisplay { get; set; }

        public string NotesDisplay { get; set; }

    }
}