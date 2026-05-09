namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBoardingUnitModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoardingUnitModels",
                c => new
                    {
                        BoardingUnitId = c.Guid(nullable: false),
                        UnitName = c.String(nullable: false, maxLength: 50),
                        UnitType = c.String(nullable: false, maxLength: 50),
                        SpeciesAllowed = c.String(nullable: false, maxLength: 20),
                        SizeCategory = c.String(nullable: false, maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.BoardingUnitId);
            
            CreateIndex("dbo.BoardingModels", "BoardingUnitId");
            AddForeignKey("dbo.BoardingModels", "BoardingUnitId", "dbo.BoardingUnitModels", "BoardingUnitId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BoardingModels", "BoardingUnitId", "dbo.BoardingUnitModels");
            DropIndex("dbo.BoardingModels", new[] { "BoardingUnitId" });
            DropTable("dbo.BoardingUnitModels");
        }
    }
}
