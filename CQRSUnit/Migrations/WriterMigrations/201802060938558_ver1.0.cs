namespace CQRSUnit.Migrations.WriterMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataItemEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(maxLength: 100),
                        Description = c.String(maxLength: 32),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DataItemEntities");
        }
    }
}
