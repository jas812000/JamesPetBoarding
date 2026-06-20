namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVoidFieldsToPaymentModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentModels", "IsVoided", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentModels", "VoidedDateTime", c => c.DateTime());
            AddColumn("dbo.PaymentModels", "VoidedReason", c => c.String(maxLength: 500));
            AddColumn("dbo.PaymentModels", "VoidedByEmployeeId", c => c.Guid());
            CreateIndex("dbo.PaymentModels", "VoidedByEmployeeId");
            AddForeignKey("dbo.PaymentModels", "VoidedByEmployeeId", "dbo.EmployeeModels", "EmployeeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentModels", "VoidedByEmployeeId", "dbo.EmployeeModels");
            DropIndex("dbo.PaymentModels", new[] { "VoidedByEmployeeId" });
            DropColumn("dbo.PaymentModels", "VoidedByEmployeeId");
            DropColumn("dbo.PaymentModels", "VoidedReason");
            DropColumn("dbo.PaymentModels", "VoidedDateTime");
            DropColumn("dbo.PaymentModels", "IsVoided");
        }
    }
}
