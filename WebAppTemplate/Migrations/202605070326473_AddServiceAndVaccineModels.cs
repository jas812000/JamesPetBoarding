namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddServiceAndVaccineModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceModels",
                c => new
                    {
                        ServiceId = c.Guid(nullable: false),
                        ServiceName = c.String(nullable: false, maxLength: 200),
                        BasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricingType = c.String(nullable: false, maxLength: 50),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.ServiceId);
            
            CreateTable(
                "dbo.VaccineModels",
                c => new
                    {
                        VaccineId = c.Guid(nullable: false),
                        VaccineName = c.String(nullable: false, maxLength: 200),
                        Species = c.String(nullable: false, maxLength: 25),
                        RequiredFlag = c.Boolean(nullable: false),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.VaccineId);
            
            CreateIndex("dbo.CustomerPetModels", "PetId");
            CreateIndex("dbo.DietModels", "PetId");
            CreateIndex("dbo.InvoiceItemModels", "ServiceId");
            CreateIndex("dbo.MedicationModels", "PetId");
            CreateIndex("dbo.PetVaccineModels", "PetId");
            CreateIndex("dbo.PetVaccineModels", "VaccineId");
            AddForeignKey("dbo.CustomerPetModels", "PetId", "dbo.PetModels", "PetId", cascadeDelete: false);
            AddForeignKey("dbo.DietModels", "PetId", "dbo.PetModels", "PetId", cascadeDelete: false);
            AddForeignKey("dbo.MedicationModels", "PetId", "dbo.PetModels", "PetId", cascadeDelete: false);
            AddForeignKey("dbo.PetVaccineModels", "PetId", "dbo.PetModels", "PetId", cascadeDelete: false);
            AddForeignKey("dbo.InvoiceItemModels", "ServiceId", "dbo.ServiceModels", "ServiceId", cascadeDelete: false);
            AddForeignKey("dbo.PetVaccineModels", "VaccineId", "dbo.VaccineModels", "VaccineId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PetVaccineModels", "VaccineId", "dbo.VaccineModels");
            DropForeignKey("dbo.InvoiceItemModels", "ServiceId", "dbo.ServiceModels");
            DropForeignKey("dbo.PetVaccineModels", "PetId", "dbo.PetModels");
            DropForeignKey("dbo.MedicationModels", "PetId", "dbo.PetModels");
            DropForeignKey("dbo.DietModels", "PetId", "dbo.PetModels");
            DropForeignKey("dbo.CustomerPetModels", "PetId", "dbo.PetModels");
            DropIndex("dbo.PetVaccineModels", new[] { "VaccineId" });
            DropIndex("dbo.PetVaccineModels", new[] { "PetId" });
            DropIndex("dbo.MedicationModels", new[] { "PetId" });
            DropIndex("dbo.InvoiceItemModels", new[] { "ServiceId" });
            DropIndex("dbo.DietModels", new[] { "PetId" });
            DropIndex("dbo.CustomerPetModels", new[] { "PetId" });
            DropTable("dbo.VaccineModels");
            DropTable("dbo.ServiceModels");
        }
    }
}
