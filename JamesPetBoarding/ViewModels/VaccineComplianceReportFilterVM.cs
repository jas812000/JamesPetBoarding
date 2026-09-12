using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VaccineComplianceReportFilterVM
    {
        public Guid? PetId { get; set; }

        public Guid? CustomerId { get; set; }

        public DateTime? ExpirationStartDate { get; set; }

        public DateTime? ExpirationEndDate { get; set; }

        public VaccineComplianceStatusEnum? ComplianceStatus {  get; set; }

        public SpeciesEnum? Species { get; set; }

    }
}