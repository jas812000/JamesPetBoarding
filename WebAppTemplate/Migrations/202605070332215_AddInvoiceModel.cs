namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvoiceModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceModels",
                c => new
                    {
                        InvoiceId = c.Guid(nullable: false),
                        CustomerId = c.Guid(nullable: false),
                        InvoiceDateTime = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, maxLength: 20),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.InvoiceId);
            
            CreateIndex("dbo.BoardingModels", "PetId");
            CreateIndex("dbo.InvoiceItemModels", "InvoiceId");
            CreateIndex("dbo.PaymentModels", "InvoiceId");
            AddForeignKey("dbo.InvoiceItemModels", "InvoiceId", "dbo.InvoiceModels", "InvoiceId", cascadeDelete: false);
            AddForeignKey("dbo.PaymentModels", "InvoiceId", "dbo.InvoiceModels", "InvoiceId", cascadeDelete: false);
            AddForeignKey("dbo.BoardingModels", "PetId", "dbo.PetModels", "PetId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BoardingModels", "PetId", "dbo.PetModels");
            DropForeignKey("dbo.PaymentModels", "InvoiceId", "dbo.InvoiceModels");
            DropForeignKey("dbo.InvoiceItemModels", "InvoiceId", "dbo.InvoiceModels");
            DropIndex("dbo.PaymentModels", new[] { "InvoiceId" });
            DropIndex("dbo.InvoiceItemModels", new[] { "InvoiceId" });
            DropIndex("dbo.BoardingModels", new[] { "PetId" });
            DropTable("dbo.InvoiceModels");
        }
    }
}
