namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNoShowFieldsToBoarding : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoardingModels", "NoShowDateTime", c => c.DateTime());
            AddColumn("dbo.BoardingModels", "NoShowByEmployeeId", c => c.Guid());
            CreateIndex("dbo.BoardingModels", "NoShowByEmployeeId");
            AddForeignKey("dbo.BoardingModels", "NoShowByEmployeeId", "dbo.EmployeeModels", "EmployeeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BoardingModels", "NoShowByEmployeeId", "dbo.EmployeeModels");
            DropIndex("dbo.BoardingModels", new[] { "NoShowByEmployeeId" });
            DropColumn("dbo.BoardingModels", "NoShowByEmployeeId");
            DropColumn("dbo.BoardingModels", "NoShowDateTime");
        }
    }
}
