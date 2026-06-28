namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePetAgeAndUpdateInvoiceRelationships : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceModels", "PetId", c => c.Guid(nullable: false));
            AddColumn("dbo.InvoiceModels", "BoardingId", c => c.Guid());
            AlterColumn("dbo.PetModels", "Sex", c => c.Int(nullable: false));
            CreateIndex("dbo.InvoiceModels", "PetId");
            CreateIndex("dbo.InvoiceModels", "BoardingId");
            AddForeignKey("dbo.InvoiceModels", "BoardingId", "dbo.BoardingModels", "BoardingId");
            AddForeignKey("dbo.InvoiceModels", "PetId", "dbo.PetModels", "PetId", cascadeDelete: true);
            DropColumn("dbo.PetModels", "Age");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PetModels", "Age", c => c.Int(nullable: false));
            DropForeignKey("dbo.InvoiceModels", "PetId", "dbo.PetModels");
            DropForeignKey("dbo.InvoiceModels", "BoardingId", "dbo.BoardingModels");
            DropIndex("dbo.InvoiceModels", new[] { "BoardingId" });
            DropIndex("dbo.InvoiceModels", new[] { "PetId" });
            AlterColumn("dbo.PetModels", "Sex", c => c.String(nullable: false, maxLength: 10));
            DropColumn("dbo.InvoiceModels", "BoardingId");
            DropColumn("dbo.InvoiceModels", "PetId");
        }
    }
}
