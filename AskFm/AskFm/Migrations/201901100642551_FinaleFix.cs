namespace AskFm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinaleFix : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.AnswerDTOes", "AnsweredBy_Id", "dbo.MiniUsers");
            //DropForeignKey("dbo.AnswerDTOes", "AskedBy_Id", "dbo.MiniUsers");
            //DropForeignKey("dbo.MiniUsers", "AnswerDTO_Id", "dbo.AnswerDTOes");
            //DropIndex("dbo.AnswerDTOes", new[] { "AnsweredBy_Id" });
            //DropIndex("dbo.AnswerDTOes", new[] { "AskedBy_Id" });
            //DropIndex("dbo.MiniUsers", new[] { "AnswerDTO_Id" });
            DropIndex("dbo.Notifications", new[] { "User_Id" });
            DropColumn("dbo.Notifications", "UserId");
            RenameColumn(table: "dbo.Notifications", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Notifications", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.notifications", "userid");
            //droptable("dbo.answerdtoes");
            //droptable("dbo.miniusers");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.MiniUsers",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            FullName = c.String(),
            //            UserName = c.String(),
            //            AnswerDTO_Id = c.Guid(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.AnswerDTOes",
            //    c => new
            //        {
            //            Id = c.Guid(nullable: false),
            //            Q = c.String(),
            //            A = c.String(),
            //            Likes = c.Int(nullable: false),
            //            IsAnon = c.Boolean(nullable: false),
            //            ContainsPhoto = c.Boolean(nullable: false),
            //            DoClientLike = c.Boolean(nullable: false),
            //            Date = c.DateTime(nullable: false),
            //            AnsweredBy_Id = c.String(maxLength: 128),
            //            AskedBy_Id = c.String(maxLength: 128),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.Notifications", new[] { "UserId" });
            AlterColumn("dbo.Notifications", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Notifications", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Notifications", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Notifications", "User_Id");
            //CreateIndex("dbo.MiniUsers", "AnswerDTO_Id");
            //CreateIndex("dbo.AnswerDTOes", "AskedBy_Id");
            //CreateIndex("dbo.AnswerDTOes", "AnsweredBy_Id");
            //AddForeignKey("dbo.MiniUsers", "AnswerDTO_Id", "dbo.AnswerDTOes", "Id");
            //AddForeignKey("dbo.AnswerDTOes", "AskedBy_Id", "dbo.MiniUsers", "Id");
            //AddForeignKey("dbo.AnswerDTOes", "AnsweredBy_Id", "dbo.MiniUsers", "Id");
        }
    }
}
