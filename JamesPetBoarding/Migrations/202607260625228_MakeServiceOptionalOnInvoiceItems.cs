namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeServiceOptionalOnInvoiceItems : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InvoiceItemModels", "ServiceId", "dbo.ServiceModels");
            DropIndex("dbo.InvoiceItemModels", new[] { "ServiceId" });
            AlterColumn("dbo.InvoiceItemModels", "ServiceId", c => c.Guid());
            CreateIndex("dbo.InvoiceItemModels", "ServiceId");
            AddForeignKey("dbo.InvoiceItemModels", "ServiceId", "dbo.ServiceModels", "ServiceId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceItemModels", "ServiceId", "dbo.ServiceModels");
            DropIndex("dbo.InvoiceItemModels", new[] { "ServiceId" });
            AlterColumn("dbo.InvoiceItemModels", "ServiceId", c => c.Guid(nullable: false));
            CreateIndex("dbo.InvoiceItemModels", "ServiceId");
            AddForeignKey("dbo.InvoiceItemModels", "ServiceId", "dbo.ServiceModels", "ServiceId", cascadeDelete: true);
        }
    }
}
