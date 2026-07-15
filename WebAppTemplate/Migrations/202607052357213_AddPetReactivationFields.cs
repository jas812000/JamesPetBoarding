namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPetReactivationFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PetModels", "ReactivatedDate", c => c.DateTime());
            AddColumn("dbo.PetModels", "ReactivatedNotes", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PetModels", "ReactivatedNotes");
            DropColumn("dbo.PetModels", "ReactivatedDate");
        }
    }
}
