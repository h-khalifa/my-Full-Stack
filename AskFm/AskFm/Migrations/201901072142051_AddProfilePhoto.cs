namespace AskFm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfilePhoto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfilePhotoes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Photo = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfilePhotoes", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.ProfilePhotoes", new[] { "Id" });
            DropTable("dbo.ProfilePhotoes");
        }
    }
}
