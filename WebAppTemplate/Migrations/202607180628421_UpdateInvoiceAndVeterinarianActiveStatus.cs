namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInvoiceAndVeterinarianActiveStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VeterinarianModels", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.InvoiceModels", "InvoiceStatus", c => c.Int(nullable: false));
            AddColumn("dbo.InvoiceModels", "VoidReason", c => c.Int());
            AddColumn("dbo.InvoiceModels", "VoidNotes", c => c.String(maxLength: 1000));
            AddColumn("dbo.InvoiceModels", "VoidDateTime", c => c.DateTime());
            AddColumn("dbo.InvoiceModels", "VoidedByEmployeeId", c => c.Guid());
            CreateIndex("dbo.InvoiceModels", "VoidedByEmployeeId");
            AddForeignKey("dbo.InvoiceModels", "VoidedByEmployeeId", "dbo.EmployeeModels", "EmployeeId");
            DropColumn("dbo.InvoiceModels", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InvoiceModels", "Status", c => c.Int(nullable: false));
            DropForeignKey("dbo.InvoiceModels", "VoidedByEmployeeId", "dbo.EmployeeModels");
            DropIndex("dbo.InvoiceModels", new[] { "VoidedByEmployeeId" });
            DropColumn("dbo.InvoiceModels", "VoidedByEmployeeId");
            DropColumn("dbo.InvoiceModels", "VoidDateTime");
            DropColumn("dbo.InvoiceModels", "VoidNotes");
            DropColumn("dbo.InvoiceModels", "VoidReason");
            DropColumn("dbo.InvoiceModels", "InvoiceStatus");
            DropColumn("dbo.VeterinarianModels", "IsActive");
        }
    }
}
