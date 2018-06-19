namespace SecKill.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "SecKillPrice", c => c.Double(nullable: false));
            AddColumn("dbo.Stocks", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Stocks", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "EndTime");
            DropColumn("dbo.Stocks", "StartTime");
            DropColumn("dbo.Stocks", "SecKillPrice");
        }
    }
}
