namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSpeciesToServiceModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceModels", "Species", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceModels", "Species");
        }
    }
}
