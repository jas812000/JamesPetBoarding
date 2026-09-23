using System;

namespace JamesPetBoarding.ViewModels
{
    public class OurTeamMemberSummaryVM
    {
        public Guid OurTeamMemberId { get; set; }

        public Guid EmployeeId { get; set; }

        public string EmployeeNameDisplay { get; set; }

        public string PublicJobTitle { get; set; }

        public string ProfileImagePath { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsEmployeeActive { get; set; }
    }
}
