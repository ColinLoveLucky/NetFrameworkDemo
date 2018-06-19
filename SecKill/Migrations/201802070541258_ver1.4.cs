namespace SecKill.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver14 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SecKillTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Token = c.String(),
                        ProductName = c.String(),
                        ExpireStartTime = c.DateTime(nullable: false),
                        ExpireEndTime = c.DateTime(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SecKillTokens");
        }
    }
}
