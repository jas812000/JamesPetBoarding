namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPetModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PetModels",
                c => new
                    {
                        PetId = c.Guid(nullable: false),
                        VetId = c.Guid(),
                        Name = c.String(nullable: false, maxLength: 50),
                        Species = c.String(nullable: false, maxLength: 10),
                        Breed = c.String(nullable: false, maxLength: 50),
                        Sex = c.String(nullable: false, maxLength: 10),
                        BirthDate = c.DateTime(nullable: false),
                        Age = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.PetId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PetModels");
        }
    }
}
