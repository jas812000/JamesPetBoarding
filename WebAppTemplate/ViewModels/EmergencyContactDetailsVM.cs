using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class EmergencyContactDetailsVM
    {
        public Guid EmergencyContactId { get; set; }

        public string FullName { get; set; }

        public string RelationshipDisplay { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string CityStateZip { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public string StatusDisplay { get; set; }
    }
}