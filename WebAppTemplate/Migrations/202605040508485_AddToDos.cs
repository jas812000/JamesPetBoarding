namespace WebAppTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToDos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToDoModels",
                c => new
                    {
                        ToDoID = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(maxLength: 1000),
                        IsComplete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ToDoID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ToDoModels");
        }
    }
}
