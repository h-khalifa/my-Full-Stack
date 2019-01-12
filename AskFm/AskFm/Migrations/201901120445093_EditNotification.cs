namespace AskFm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "TriggerUserName", c => c.String());
            DropColumn("dbo.Notifications", "TriggerUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "TriggerUserId", c => c.String());
            DropColumn("dbo.Notifications", "TriggerUserName");
        }
    }
}
