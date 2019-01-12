namespace AskFm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDateToAnswer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "biography", c => c.String());
            AddColumn("dbo.AspNetUsers", "Gender", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Gender");
            DropColumn("dbo.AspNetUsers", "biography");
            DropColumn("dbo.Answers", "Date");
        }
    }
}
