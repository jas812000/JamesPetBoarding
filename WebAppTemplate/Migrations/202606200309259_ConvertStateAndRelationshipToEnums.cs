namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConvertStateAndRelationshipToEnums : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoardingUnitModels", "UnitNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.BoardingModels", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.BoardingUnitModels", "UnitName", c => c.Int(nullable: false));
            AlterColumn("dbo.BoardingUnitModels", "UnitType", c => c.Int(nullable: false));
            AlterColumn("dbo.BoardingUnitModels", "SpeciesAllowed", c => c.Int(nullable: false));
            AlterColumn("dbo.BoardingUnitModels", "SizeCategory", c => c.Int(nullable: false));
            AlterColumn("dbo.EmployeeModels", "Role", c => c.Int(nullable: false));
            AlterColumn("dbo.EmployeeModels", "Email", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.CustomerModels", "State", c => c.Int(nullable: false));
            AlterColumn("dbo.CustomerModels", "Email", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.CustomerPetModels", "RelationshipType", c => c.Int(nullable: false));
            AlterColumn("dbo.PetModels", "Species", c => c.Int(nullable: false));
            AlterColumn("dbo.DietModels", "Frequency", c => c.Int(nullable: false));
            AlterColumn("dbo.MedicationModels", "Route", c => c.Int(nullable: false));
            AlterColumn("dbo.MedicationModels", "Frequency", c => c.Int(nullable: false));
            AlterColumn("dbo.VaccineModels", "Species", c => c.Int(nullable: false));
            AlterColumn("dbo.VeterinarianModels", "State", c => c.Int(nullable: false));
            AlterColumn("dbo.VeterinarianModels", "Email", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.EmergencyContactModels", "State", c => c.Int(nullable: false));
            AlterColumn("dbo.EmergencyContactModels", "Email", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.EmergencyContactModels", "RelationshipType", c => c.Int(nullable: false));
            AlterColumn("dbo.InvoiceModels", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.InvoiceItemModels", "ItemType", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceModels", "ServiceName", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceModels", "PricingType", c => c.Int(nullable: false));
            AlterColumn("dbo.PaymentModels", "PaymentMethod", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PaymentModels", "PaymentMethod", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.ServiceModels", "PricingType", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.ServiceModels", "ServiceName", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.InvoiceItemModels", "ItemType", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.InvoiceModels", "Status", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.EmergencyContactModels", "RelationshipType", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.EmergencyContactModels", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.EmergencyContactModels", "State", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.VeterinarianModels", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.VeterinarianModels", "State", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.VaccineModels", "Species", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.MedicationModels", "Frequency", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.MedicationModels", "Route", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.DietModels", "Frequency", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.PetModels", "Species", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.CustomerPetModels", "RelationshipType", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.CustomerModels", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.CustomerModels", "State", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.EmployeeModels", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.EmployeeModels", "Role", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.BoardingUnitModels", "SizeCategory", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.BoardingUnitModels", "SpeciesAllowed", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.BoardingUnitModels", "UnitType", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.BoardingUnitModels", "UnitName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.BoardingModels", "Status", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.BoardingUnitModels", "UnitNumber");
        }
    }
}
