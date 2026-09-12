namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerModels",
                c => new
                    {
                        CustomerId = c.Guid(nullable: false),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 300),
                        City = c.String(nullable: false, maxLength: 100),
                        State = c.String(nullable: false, maxLength: 50),
                        ZipCode = c.String(nullable: false, maxLength: 20),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            AddColumn("dbo.EmergencyContactModels", "IsActive", c => c.Boolean(nullable: false));
            CreateIndex("dbo.BoardingModels", "CustomerId");
            CreateIndex("dbo.CustomerPetModels", "CustomerId");
            CreateIndex("dbo.EmergencyContactModels", "CustomerId");
            CreateIndex("dbo.InvoiceModels", "CustomerId");
            AddForeignKey("dbo.BoardingModels", "CustomerId", "dbo.CustomerModels", "CustomerId", cascadeDelete: false);
            AddForeignKey("dbo.CustomerPetModels", "CustomerId", "dbo.CustomerModels", "CustomerId", cascadeDelete: false);
            AddForeignKey("dbo.EmergencyContactModels", "CustomerId", "dbo.CustomerModels", "CustomerId", cascadeDelete: false);
            AddForeignKey("dbo.InvoiceModels", "CustomerId", "dbo.CustomerModels", "CustomerId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceModels", "CustomerId", "dbo.CustomerModels");
            DropForeignKey("dbo.EmergencyContactModels", "CustomerId", "dbo.CustomerModels");
            DropForeignKey("dbo.CustomerPetModels", "CustomerId", "dbo.CustomerModels");
            DropForeignKey("dbo.BoardingModels", "CustomerId", "dbo.CustomerModels");
            DropIndex("dbo.InvoiceModels", new[] { "CustomerId" });
            DropIndex("dbo.EmergencyContactModels", new[] { "CustomerId" });
            DropIndex("dbo.CustomerPetModels", new[] { "CustomerId" });
            DropIndex("dbo.BoardingModels", new[] { "CustomerId" });
            DropColumn("dbo.EmergencyContactModels", "IsActive");
            DropTable("dbo.CustomerModels");
        }
    }
}
