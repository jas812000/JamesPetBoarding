using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JamesPetBoarding.Models
{
    public class OurTeamMemberModel
    {
        public OurTeamMemberModel()
        {
            OurTeamMemberId = Guid.NewGuid();
        }

        [Key]
        public Guid OurTeamMemberId { get; set; }

        [Index(
            "IX_OurTeamMember_EmployeeId",
            IsUnique = true)]
        public Guid EmployeeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PublicJobTitle { get; set; }

        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Display order must be greater than zero.")]
        [Index(
            "IX_OurTeamMember_DisplayOrder",
            IsUnique = true)]
        public int DisplayOrder { get; set; }

        [ForeignKey("EmployeeId")]
        public EmployeeModel Employee { get; set; }
    }
}
