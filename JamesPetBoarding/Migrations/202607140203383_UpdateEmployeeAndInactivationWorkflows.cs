namespace JamesPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEmployeeAndInactivationWorkflows : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeModels", "InactivationReason", c => c.Int());
            AddColumn("dbo.EmployeeModels", "InactivationDate", c => c.DateTime());
            AddColumn("dbo.EmployeeModels", "InactivationNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.EmployeeModels", "ReactivationDate", c => c.DateTime());
            AddColumn("dbo.EmployeeModels", "ReactivationNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.EmployeeModels", "Notes", c => c.String(maxLength: 500));
            AddColumn("dbo.CustomerModels", "InactivationReason", c => c.Int());
            AddColumn("dbo.CustomerModels", "InactivationDate", c => c.DateTime());
            AddColumn("dbo.CustomerModels", "InactivationNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.CustomerModels", "ReactivationDate", c => c.DateTime());
            AddColumn("dbo.CustomerModels", "ReactivationNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.PetModels", "InactivationReason", c => c.Int());
            AddColumn("dbo.PetModels", "InactivationDate", c => c.DateTime());
            AddColumn("dbo.PetModels", "InactivationNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.PetModels", "ReactivationDate", c => c.DateTime());
            AddColumn("dbo.PetModels", "ReactivationNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.EmergencyContactModels", "InactivationReason", c => c.Int());
            AddColumn("dbo.EmergencyContactModels", "InactivationDate", c => c.DateTime());
            AddColumn("dbo.EmergencyContactModels", "InactivationNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.EmergencyContactModels", "ReactivationDate", c => c.DateTime());
            AddColumn("dbo.EmergencyContactModels", "ReactivationNotes", c => c.String(maxLength: 500));
            DropColumn("dbo.CustomerModels", "InactiveReason");
            DropColumn("dbo.CustomerModels", "InactivatedDate");
            DropColumn("dbo.CustomerModels", "InactiveNotes");
            DropColumn("dbo.CustomerModels", "ReactivatedDate");
            DropColumn("dbo.CustomerModels", "ReactivatedNotes");
            DropColumn("dbo.PetModels", "InactiveReason");
            DropColumn("dbo.PetModels", "InactivatedDate");
            DropColumn("dbo.PetModels", "InactiveNotes");
            DropColumn("dbo.PetModels", "ReactivatedDate");
            DropColumn("dbo.PetModels", "ReactivatedNotes");
            DropColumn("dbo.EmergencyContactModels", "InactivatedReason");
            DropColumn("dbo.EmergencyContactModels", "InactivatedDate");
            DropColumn("dbo.EmergencyContactModels", "InactivatedNotes");
            DropColumn("dbo.EmergencyContactModels", "ReactivatedDate");
            DropColumn("dbo.EmergencyContactModels", "ReactivatedNotes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmergencyContactModels", "ReactivatedNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.EmergencyContactModels", "ReactivatedDate", c => c.DateTime());
            AddColumn("dbo.EmergencyContactModels", "InactivatedNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.EmergencyContactModels", "InactivatedDate", c => c.DateTime());
            AddColumn("dbo.EmergencyContactModels", "InactivatedReason", c => c.Int());
            AddColumn("dbo.PetModels", "ReactivatedNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.PetModels", "ReactivatedDate", c => c.DateTime());
            AddColumn("dbo.PetModels", "InactiveNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.PetModels", "InactivatedDate", c => c.DateTime());
            AddColumn("dbo.PetModels", "InactiveReason", c => c.Int());
            AddColumn("dbo.CustomerModels", "ReactivatedNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.CustomerModels", "ReactivatedDate", c => c.DateTime());
            AddColumn("dbo.CustomerModels", "InactiveNotes", c => c.String(maxLength: 500));
            AddColumn("dbo.CustomerModels", "InactivatedDate", c => c.DateTime());
            AddColumn("dbo.CustomerModels", "InactiveReason", c => c.Int());
            DropColumn("dbo.EmergencyContactModels", "ReactivationNotes");
            DropColumn("dbo.EmergencyContactModels", "ReactivationDate");
            DropColumn("dbo.EmergencyContactModels", "InactivationNotes");
            DropColumn("dbo.EmergencyContactModels", "InactivationDate");
            DropColumn("dbo.EmergencyContactModels", "InactivationReason");
            DropColumn("dbo.PetModels", "ReactivationNotes");
            DropColumn("dbo.PetModels", "ReactivationDate");
            DropColumn("dbo.PetModels", "InactivationNotes");
            DropColumn("dbo.PetModels", "InactivationDate");
            DropColumn("dbo.PetModels", "InactivationReason");
            DropColumn("dbo.CustomerModels", "ReactivationNotes");
            DropColumn("dbo.CustomerModels", "ReactivationDate");
            DropColumn("dbo.CustomerModels", "InactivationNotes");
            DropColumn("dbo.CustomerModels", "InactivationDate");
            DropColumn("dbo.CustomerModels", "InactivationReason");
            DropColumn("dbo.EmployeeModels", "Notes");
            DropColumn("dbo.EmployeeModels", "ReactivationNotes");
            DropColumn("dbo.EmployeeModels", "ReactivationDate");
            DropColumn("dbo.EmployeeModels", "InactivationNotes");
            DropColumn("dbo.EmployeeModels", "InactivationDate");
            DropColumn("dbo.EmployeeModels", "InactivationReason");
        }
    }
}
