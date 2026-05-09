namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmergencyContactAndDietModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DietModels",
                c => new
                    {
                        DietId = c.Guid(nullable: false),
                        PetId = c.Guid(nullable: false),
                        FoodName = c.String(nullable: false, maxLength: 50),
                        Amount = c.String(nullable: false, maxLength: 20),
                        Frequency = c.String(nullable: false, maxLength: 50),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.DietId);
            
            CreateTable(
                "dbo.EmergencyContactModels",
                c => new
                    {
                        EmergencyContactId = c.Guid(nullable: false),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 300),
                        City = c.String(nullable: false, maxLength: 100),
                        State = c.String(nullable: false, maxLength: 50),
                        ZipCode = c.String(nullable: false, maxLength: 20),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        RelationshipType = c.String(nullable: false, maxLength: 50),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.EmergencyContactId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmergencyContactModels");
            DropTable("dbo.DietModels");
        }
    }
}
