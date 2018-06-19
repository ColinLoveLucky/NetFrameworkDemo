namespace LinqToEntity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver40 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("ModelThree.OnLineCourse", "Name");
        }
        
        public override void Down()
        {
            DropIndex("ModelThree.OnLineCourse", new[] { "Name" });
        }
    }
}
