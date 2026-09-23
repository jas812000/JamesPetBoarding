namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployeeProfileImagePath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeModels", "ProfileImagePath", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeModels", "ProfileImagePath");
        }
    }
}
