namespace EfResposity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ef_User", "UserName", c => c.String(maxLength: 20));
            AlterColumn("dbo.Ef_User", "Password", c => c.String(maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ef_User", "Password", c => c.String());
            AlterColumn("dbo.Ef_User", "UserName", c => c.String());
        }
    }
}
