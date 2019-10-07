namespace Sentinel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTwitchID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TwitchUsers", "TwitchID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TwitchUsers", "TwitchID");
        }
    }
}
