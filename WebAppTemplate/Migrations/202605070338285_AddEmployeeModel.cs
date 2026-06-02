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

            CreateIndex("dbo.BoardingModels", "CheckedInByEmployeeId");
            CreateIndex("dbo.BoardingModels", "CheckedOutByEmployeeId");
            CreateIndex("dbo.BoardingModels", "CancelledByEmployeeId");
            AddForeignKey("dbo.BoardingModels", "CheckedInByEmployeeId", "dbo.EmployeeModels", "EmployeeId", cascadeDelete: false);
            AddForeignKey("dbo.BoardingModels", "CheckedOutByEmployeeId", "dbo.EmployeeModels", "EmployeeId", cascadeDelete: false);
            AddForeignKey("dbo.BoardingModels", "CancelledByEmployeeId", "dbo.EmployeeModels", "EmployeeId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BoardingModels", "CancelledByEmployeeId", "dbo.EmployeeModels");
            DropForeignKey("dbo.BoardingModels", "CheckedOutByEmployeeId", "dbo.EmployeeModels");
            DropForeignKey("dbo.BoardingModels", "CheckedInByEmployeeId", "dbo.EmployeeModels");
            DropIndex("dbo.BoardingModels", new[] { "CancelledByEmployeeId" });
            DropIndex("dbo.BoardingModels", new[] { "CheckedOutByEmployeeId" });
            DropIndex("dbo.BoardingModels", new[] { "CheckedInByEmployeeId" });
            DropTable("dbo.EmployeeModels");
        }
    }
}
