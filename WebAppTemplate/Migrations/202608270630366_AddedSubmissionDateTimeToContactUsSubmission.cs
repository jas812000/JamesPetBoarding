namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSubmissionDateTimeToContactUsSubmission : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactUsSubmissionModels", "SubmissionDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactUsSubmissionModels", "SubmissionDateTime");
        }
    }
}
