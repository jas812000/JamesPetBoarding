namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerIdToEmergencyContactModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmergencyContactModels", "CustomerId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmergencyContactModels", "CustomerId");
        }
    }
}
