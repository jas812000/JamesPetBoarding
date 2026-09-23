using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class OurTeamMemberFormVM
    {
        public OurTeamMemberFormVM()
        {
            EmployeeSelectList = new List<SelectListItem>();
        }

        public Guid? OurTeamMemberId { get; set; }

        [Required(ErrorMessage = "Please select an employee.")]
        public Guid? EmployeeId { get; set; }

        [Required(ErrorMessage = "Public job title is required.")]
        [StringLength(
            100,
            ErrorMessage = "Public job title cannot exceed 100 characters.")]
        public string PublicJobTitle { get; set; }

        [Required(ErrorMessage = "Display order is required.")]
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Display order must be greater than zero.")]
        public int? DisplayOrder { get; set; }

        public List<SelectListItem> EmployeeSelectList { get; set; }
    }
}
