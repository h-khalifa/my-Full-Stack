namespace AskFm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixNotificationTabl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnswerDTOes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Q = c.String(),
                        A = c.String(),
                        Likes = c.Int(nullable: false),
                        IsAnon = c.Boolean(nullable: false),
                        ContainsPhoto = c.Boolean(nullable: false),
                        DoClientLike = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                        AnsweredBy_Id = c.String(maxLength: 128),
                        AskedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MiniUsers", t => t.AnsweredBy_Id)
                .ForeignKey("dbo.MiniUsers", t => t.AskedBy_Id)
                .Index(t => t.AnsweredBy_Id)
                .Index(t => t.AskedBy_Id);
            
            CreateTable(
                "dbo.MiniUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        UserName = c.String(),
                        AnswerDTO_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AnswerDTOes", t => t.AnswerDTO_Id)
                .Index(t => t.AnswerDTO_Id);
            
            

            AlterColumn("dbo.Notifications", "message", c => c.String());

            //fix auto scafolding shiiiit

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MiniUsers", "AnswerDTO_Id", "dbo.AnswerDTOes");
            DropForeignKey("dbo.AnswerDTOes", "AskedBy_Id", "dbo.MiniUsers");
            DropForeignKey("dbo.AnswerDTOes", "AnsweredBy_Id", "dbo.MiniUsers");
            DropIndex("dbo.MiniUsers", new[] { "AnswerDTO_Id" });
            DropIndex("dbo.AnswerDTOes", new[] { "AskedBy_Id" });
            DropIndex("dbo.AnswerDTOes", new[] { "AnsweredBy_Id" });
            AlterColumn("dbo.Notifications", "message", c => c.Int(nullable: false));
            DropTable("dbo.MiniUsers");
            DropTable("dbo.AnswerDTOes");
        }
    }
}
