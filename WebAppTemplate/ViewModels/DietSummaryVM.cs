using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class DietSummaryVM
    {
        public Guid DietId { get; set; }

        public string FoodName { get; set; }

        public string Amount { get; set; }

    }
}