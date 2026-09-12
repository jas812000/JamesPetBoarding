namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequirePaymentTransactionReference : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaymentModels", "TransactionReference", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PaymentModels", "TransactionReference", c => c.String(maxLength: 200));
        }
    }
}
