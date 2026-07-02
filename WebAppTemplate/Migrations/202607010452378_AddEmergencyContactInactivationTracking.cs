namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmergencyContactInactivationTracking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmergencyContactModels", "InactivatedReason", c => c.Int());
            AddColumn("dbo.EmergencyContactModels", "InactivatedDate", c => c.DateTime());
            AddColumn("dbo.EmergencyContactModels", "InactivatedNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.EmergencyContactModels", "ReactivatedDate", c => c.DateTime());
            AddColumn("dbo.EmergencyContactModels", "ReactivatedNotes", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmergencyContactModels", "ReactivatedNotes");
            DropColumn("dbo.EmergencyContactModels", "ReactivatedDate");
            DropColumn("dbo.EmergencyContactModels", "InactivatedNotes");
            DropColumn("dbo.EmergencyContactModels", "InactivatedDate");
            DropColumn("dbo.EmergencyContactModels", "InactivatedReason");
        }
    }
}
