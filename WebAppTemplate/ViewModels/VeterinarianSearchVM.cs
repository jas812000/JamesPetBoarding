using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class VeterinarianSearchVM
    {
        public VeterinarianSearchVM() 
        {
            VeterinarianSummaryResults = new List<VeterinarianSummaryVM>();
        }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ClinicName { get; set; }

        public string City { get; set; }

        public StateEnum? State { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public List<VeterinarianSummaryVM> VeterinarianSummaryResults { get; set; }
    }
}