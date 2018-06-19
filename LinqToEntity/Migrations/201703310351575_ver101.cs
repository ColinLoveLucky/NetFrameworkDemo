namespace LinqToEntity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver101 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ModelOne.university",
                c => new
                    {
                        UniversityID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        Location = c.Geography(),
                    })
                .PrimaryKey(t => t.UniversityID);
            
        }
        
        public override void Down()
        {
            DropTable("ModelOne.university");
        }
    }
}
