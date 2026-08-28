namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UseTransactionVoidReasonEnum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentModels", "VoidReason", c => c.Int());
            DropColumn("dbo.PaymentModels", "VoidedReason");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentModels", "VoidedReason", c => c.String(maxLength: 500));
            DropColumn("dbo.PaymentModels", "VoidReason");
        }
    }
}
