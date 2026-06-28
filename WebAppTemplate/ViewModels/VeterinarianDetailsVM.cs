using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VeterinarianDetailsVM
    {
        public Guid VetId { get; set; }

        public string ClinicName { get; set; }

        public string FullNameCredentials { get; set; }

        public string Address { get; set; }

        public string CityStateZip { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Notes { get; set; }
    }
}