namespace AskFm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingLikeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Likes", "AnsweredBy_Id", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Likes", "AnsweredBy_Id");
        }
    }
}
