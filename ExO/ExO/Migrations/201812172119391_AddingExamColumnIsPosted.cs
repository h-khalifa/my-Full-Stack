namespace ExO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingExamColumnIsPosted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exams", "Isposted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exams", "Isposted");
        }
    }
}
