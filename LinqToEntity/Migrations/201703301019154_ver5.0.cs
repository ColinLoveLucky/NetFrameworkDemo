namespace LinqToEntity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver50 : DbMigration
    {
        public override void Up()
        {
            DropIndex("ModelThree.OnLineCourse", new[] { "Name" });
            CreateTable(
                "ModelOne.instructor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ModelOne.course",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ModelOne.InstructorCourses",
                c => new
                    {
                        CourseID = c.Int(nullable: false),
                        InstructorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseID, t.InstructorID })
                .ForeignKey("ModelOne.instructor", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("ModelOne.course", t => t.InstructorID, cascadeDelete: true)
                .Index(t => t.CourseID)
                .Index(t => t.InstructorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ModelOne.InstructorCourses", "InstructorID", "ModelOne.course");
            DropForeignKey("ModelOne.InstructorCourses", "CourseID", "ModelOne.instructor");
            DropIndex("ModelOne.InstructorCourses", new[] { "InstructorID" });
            DropIndex("ModelOne.InstructorCourses", new[] { "CourseID" });
            DropTable("ModelOne.InstructorCourses");
            DropTable("ModelOne.course");
            DropTable("ModelOne.instructor");
            CreateIndex("ModelThree.OnLineCourse", "Name");
        }
    }
}
