namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCustomerReactivationAndPetName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerModels", "ReactivatedDate", c => c.DateTime());
            AddColumn("dbo.CustomerModels", "ReactivatedNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.PetModels", "PetName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.PetModels", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PetModels", "Name", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.PetModels", "PetName");
            DropColumn("dbo.CustomerModels", "ReactivatedNotes");
            DropColumn("dbo.CustomerModels", "ReactivatedDate");
        }
    }
}
