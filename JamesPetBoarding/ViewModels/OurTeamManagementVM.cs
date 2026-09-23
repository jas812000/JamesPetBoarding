using System.Collections.Generic;

namespace JamesPetBoarding.ViewModels
{
    public class OurTeamManagementVM
    {
        public OurTeamManagementVM()
        {
            OurTeamMembers = new List<OurTeamMemberSummaryVM>();
        }

        public List<OurTeamMemberSummaryVM> OurTeamMembers { get; set; }
    }
}
