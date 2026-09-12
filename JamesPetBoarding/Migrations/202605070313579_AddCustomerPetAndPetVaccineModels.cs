namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerPetAndPetVaccineModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerPetModels",
                c => new
                    {
                        CustomerPetId = c.Guid(nullable: false),
                        PetId = c.Guid(nullable: false),
                        CustomerId = c.Guid(nullable: false),
                        RelationshipType = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.CustomerPetId);
            
            CreateTable(
                "dbo.PetVaccineModels",
                c => new
                    {
                        PetVaccineId = c.Guid(nullable: false),
                        PetId = c.Guid(nullable: false),
                        VaccineId = c.Guid(nullable: false),
                        DateGiven = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        DocumentFilePath = c.String(nullable: false, maxLength: 200),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.PetVaccineId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PetVaccineModels");
            DropTable("dbo.CustomerPetModels");
        }
    }
}
