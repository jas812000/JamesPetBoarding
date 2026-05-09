namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployeeModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeModels",
                c => new
                    {
                        EmployeeId = c.Guid(nullable: false),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        Role = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId);

            CreateIndex("dbo.BoardingModels", "CheckedInByEmployeeID");
            CreateIndex("dbo.BoardingModels", "CheckedOutByEmployeeID");
            CreateIndex("dbo.BoardingModels", "CancelledByEmployeeID");
            AddForeignKey("dbo.BoardingModels", "CheckedInByEmployeeID", "dbo.EmployeeModels", "EmployeeId", cascadeDelete: false);
            AddForeignKey("dbo.BoardingModels", "CheckedOutByEmployeeID", "dbo.EmployeeModels", "EmployeeId", cascadeDelete: false);
            AddForeignKey("dbo.BoardingModels", "CancelledByEmployeeID", "dbo.EmployeeModels", "EmployeeId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BoardingModels", "CancelledByEmployeeID", "dbo.EmployeeModels");
            DropForeignKey("dbo.BoardingModels", "CheckedOutByEmployeeID", "dbo.EmployeeModels");
            DropForeignKey("dbo.BoardingModels", "CheckedInByEmployeeID", "dbo.EmployeeModels");
            DropIndex("dbo.BoardingModels", new[] { "CancelledByEmployeeID" });
            DropIndex("dbo.BoardingModels", new[] { "CheckedOutByEmployeeID" });
            DropIndex("dbo.BoardingModels", new[] { "CheckedInByEmployeeID" });
            DropTable("dbo.EmployeeModels");
        }
    }
}
