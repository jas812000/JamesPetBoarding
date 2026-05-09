namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBoardingModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoardingModels",
                c => new
                    {
                        BoardingId = c.Guid(nullable: false),
                        CustomerId = c.Guid(nullable: false),
                        PetId = c.Guid(nullable: false),
                        BoardingUnitId = c.Guid(nullable: false),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        ActualCheckInDateTime = c.DateTime(),
                        CheckedInByEmployeeId = c.Guid(),
                        ActualCheckOutDateTime = c.DateTime(),
                        CheckedOutByEmployeeId = c.Guid(),
                        CancelledDateTime = c.DateTime(),
                        CancelledByEmployeeId = c.Guid(),
                        CancelledReason = c.String(maxLength: 1000),
                        Status = c.String(nullable: false, maxLength: 20),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.BoardingId);
            
            CreateIndex("dbo.InvoiceItemModels", "BoardingId");
            AddForeignKey("dbo.InvoiceItemModels", "BoardingId", "dbo.BoardingModels", "BoardingId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceItemModels", "BoardingId", "dbo.BoardingModels");
            DropIndex("dbo.InvoiceItemModels", new[] { "BoardingId" });
            DropTable("dbo.BoardingModels");
        }
    }
}
