namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvoiceTypeToInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceModels", "InvoiceType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceModels", "InvoiceType");
        }
    }
}
