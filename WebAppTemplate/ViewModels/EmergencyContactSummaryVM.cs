using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class EmergencyContactSummaryVM
    {
        public Guid EmergencyContactId { get; set; }

        public string FullName { get; set; }

        public string RelationshipDisplay {  get; set; }

        public bool IsActive { get; set; }
    }
}