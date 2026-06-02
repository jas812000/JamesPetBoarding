namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class MakeMedicationEndDateOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MedicationModels", "EndDate", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.MedicationModels", "EndDate", c => c.DateTime(nullable: false));
        }
    }
}
