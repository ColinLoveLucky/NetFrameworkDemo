namespace SecKill.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver15 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SecKillTokens", "Token", c => c.String(maxLength: 100));
            AlterColumn("dbo.SecKillTokens", "ProductName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SecKillTokens", "ProductName", c => c.String());
            AlterColumn("dbo.SecKillTokens", "Token", c => c.String());
        }
    }
}
