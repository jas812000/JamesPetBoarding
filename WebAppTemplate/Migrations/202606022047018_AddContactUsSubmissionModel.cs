namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContactUsSubmissionModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactUsSubmissionModels",
                c => new
                    {
                        SubmissionId = c.Guid(nullable: false),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        Message = c.String(nullable: false, maxLength: 2000),
                    })
                .PrimaryKey(t => t.SubmissionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ContactUsSubmissionModels");
        }
    }
}
