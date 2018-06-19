namespace LinqToEntity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver30 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "ModelOne.on_line_course", newName: "OnLineCourse");
            MoveTable(name: "ModelOne.OnLineCourse", newSchema: "ModelThree");
        }
        
        public override void Down()
        {
            MoveTable(name: "ModelThree.OnLineCourse", newSchema: "ModelOne");
            RenameTable(name: "ModelOne.OnLineCourse", newName: "on_line_course");
        }
    }
}
