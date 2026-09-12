namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMedicationModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MedicationModels",
                c => new
                    {
                        MedicationId = c.Guid(nullable: false),
                        PetId = c.Guid(nullable: false),
                        MedicationName = c.String(nullable: false, maxLength: 50),
                        Dosage = c.String(nullable: false, maxLength: 20),
                        Route = c.String(nullable: false, maxLength: 20),
                        Frequency = c.String(nullable: false, maxLength: 50),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.MedicationId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MedicationModels");
        }
    }
}
