namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVeterinarianModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VeterinarianModels",
                c => new
                    {
                        VetId = c.Guid(nullable: false),
                        ClinicName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        Credentials = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 300),
                        City = c.String(nullable: false, maxLength: 100),
                        State = c.String(nullable: false, maxLength: 50),
                        ZipCode = c.String(nullable: false, maxLength: 20),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.VetId);
            
            AlterColumn("dbo.PetModels", "Species", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.PetModels", "VetId");
            AddForeignKey("dbo.PetModels", "VetId", "dbo.VeterinarianModels", "VetId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PetModels", "VetId", "dbo.VeterinarianModels");
            DropIndex("dbo.PetModels", new[] { "VetId" });
            AlterColumn("dbo.PetModels", "Species", c => c.String(nullable: false, maxLength: 10));
            DropTable("dbo.VeterinarianModels");
        }
    }
}
