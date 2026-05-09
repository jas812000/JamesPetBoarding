namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentAndInvoiceItemModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceItemModels",
                c => new
                    {
                        InvoiceItemId = c.Guid(nullable: false),
                        BoardingId = c.Guid(nullable: false),
                        InvoiceId = c.Guid(nullable: false),
                        ServiceId = c.Guid(nullable: false),
                        ItemType = c.String(nullable: false, maxLength: 200),
                        Description = c.String(maxLength: 2000),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LineTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.InvoiceItemId);
            
            CreateTable(
                "dbo.PaymentModels",
                c => new
                    {
                        PaymentId = c.Guid(nullable: false),
                        InvoiceId = c.Guid(nullable: false),
                        PaymentDateTime = c.DateTime(nullable: false),
                        PaymentMethod = c.String(nullable: false, maxLength: 50),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionReference = c.String(maxLength: 200),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.PaymentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PaymentModels");
            DropTable("dbo.InvoiceItemModels");
        }
    }
}
