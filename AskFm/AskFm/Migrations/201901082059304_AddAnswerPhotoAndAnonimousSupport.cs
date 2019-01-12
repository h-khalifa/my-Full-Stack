namespace AskFm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnswerPhotoAndAnonimousSupport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answer_Photo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Photo = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.Answers", "IsAnon", c => c.Boolean(nullable: false));
            AddColumn("dbo.Answers", "ContainsPhoto", c => c.Boolean(nullable: false));
            AddColumn("dbo.Questions", "IsAnon", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answer_Photo", "Id", "dbo.Answers");
            DropIndex("dbo.Answer_Photo", new[] { "Id" });
            DropColumn("dbo.Questions", "IsAnon");
            DropColumn("dbo.Answers", "ContainsPhoto");
            DropColumn("dbo.Answers", "IsAnon");
            DropTable("dbo.Answer_Photo");
        }
    }
}
