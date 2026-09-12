using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VeterinarianStatusVM
    {

        public Guid VetId { get; set; }

        public string FullNameCredentialsDisplay { get; set; }

        public string ClinicNameDisplay { get; set; }

        public string ActiveStatusDisplay { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

    }
}