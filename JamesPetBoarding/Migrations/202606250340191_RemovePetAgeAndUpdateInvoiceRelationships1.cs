namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePetAgeAndUpdateInvoiceRelationships1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InvoiceItemModels", "BoardingId", "dbo.BoardingModels");
            DropIndex("dbo.InvoiceItemModels", new[] { "BoardingId" });
            AlterColumn("dbo.InvoiceItemModels", "BoardingId", c => c.Guid());
            CreateIndex("dbo.InvoiceItemModels", "BoardingId");
            AddForeignKey("dbo.InvoiceItemModels", "BoardingId", "dbo.BoardingModels", "BoardingId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceItemModels", "BoardingId", "dbo.BoardingModels");
            DropIndex("dbo.InvoiceItemModels", new[] { "BoardingId" });
            AlterColumn("dbo.InvoiceItemModels", "BoardingId", c => c.Guid(nullable: false));
            CreateIndex("dbo.InvoiceItemModels", "BoardingId");
            AddForeignKey("dbo.InvoiceItemModels", "BoardingId", "dbo.BoardingModels", "BoardingId", cascadeDelete: true);
        }
    }
}
