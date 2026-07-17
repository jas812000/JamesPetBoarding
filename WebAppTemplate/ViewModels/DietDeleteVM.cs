using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class DietDeleteVM
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