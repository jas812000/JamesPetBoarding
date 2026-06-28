namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInactiveTrackingAndPaymentEmployeeTracking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerModels", "InactiveReason", c => c.Int());
            AddColumn("dbo.CustomerModels", "InactivatedDate", c => c.DateTime());
            AddColumn("dbo.CustomerModels", "InactiveNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.PetModels", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.PetModels", "InactiveReason", c => c.Int());
            AddColumn("dbo.PetModels", "InactivatedDate", c => c.DateTime());
            AddColumn("dbo.PetModels", "InactiveNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.PaymentModels", "ProcessedByEmployeeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.PaymentModels", "ProcessedByEmployeeId");
            AddForeignKey("dbo.PaymentModels", "ProcessedByEmployeeId", "dbo.EmployeeModels", "EmployeeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentModels", "ProcessedByEmployeeId", "dbo.EmployeeModels");
            DropIndex("dbo.PaymentModels", new[] { "ProcessedByEmployeeId" });
            DropColumn("dbo.PaymentModels", "ProcessedByEmployeeId");
            DropColumn("dbo.PetModels", "InactiveNotes");
            DropColumn("dbo.PetModels", "InactivatedDate");
            DropColumn("dbo.PetModels", "InactiveReason");
            DropColumn("dbo.PetModels", "IsActive");
            DropColumn("dbo.CustomerModels", "InactiveNotes");
            DropColumn("dbo.CustomerModels", "InactivatedDate");
            DropColumn("dbo.CustomerModels", "InactiveReason");
        }
    }
}
