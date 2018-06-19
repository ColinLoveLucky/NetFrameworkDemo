namespace LinqToEntity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver80 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ModelOne.department",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        Budget = c.Decimal(precision: 18, scale: 2),
                        Name = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId);
        }
        
        public override void Down()
        {
            DropTable("ModelOne.department");
        }
    }
}
