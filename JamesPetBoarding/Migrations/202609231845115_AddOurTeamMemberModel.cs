namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddOurTeamMemberModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OurTeamMemberModels",
                c => new
                    {
                        OurTeamMemberId = c.Guid(nullable: false),
                        EmployeeId = c.Guid(nullable: false),
                        PublicJobTitle = c.String(nullable: false, maxLength: 100),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OurTeamMemberId)
                .ForeignKey("dbo.EmployeeModels", t => t.EmployeeId, cascadeDelete: false)
                .Index(t => t.EmployeeId, unique: true, name: "IX_OurTeamMember_EmployeeId")

        }

        public override void Down()
        {
            DropForeignKey("dbo.OurTeamMemberModels", "EmployeeId", "dbo.EmployeeModels");
            DropIndex("dbo.OurTeamMemberModels", "IX_OurTeamMember_DisplayOrder");
            DropIndex("dbo.OurTeamMemberModels", "IX_OurTeamMember_EmployeeId");
            DropTable("dbo.OurTeamMemberModels");
        }
    }
}
