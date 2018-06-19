namespace SecKill.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SecKillTokens", "IsBuy", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SecKillTokens", "IsBuy");
        }
    }
}
