namespace MVCDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateInstructor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Instructors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstMidName = c.String(),
                        HireDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.InstructorCourses",
                c => new
                    {
                        Instructor_ID = c.Int(nullable: false),
                        Course_CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Instructor_ID, t.Course_CourseId })
                .ForeignKey("dbo.Instructors", t => t.Instructor_ID, cascadeDelete: true)
                .ForeignKey("dbo.App_Course", t => t.Course_CourseId, cascadeDelete: true)
                .Index(t => t.Instructor_ID)
                .Index(t => t.Course_CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InstructorCourses", "Course_CourseId", "dbo.App_Course");
            DropForeignKey("dbo.InstructorCourses", "Instructor_ID", "dbo.Instructors");
            DropIndex("dbo.InstructorCourses", new[] { "Course_CourseId" });
            DropIndex("dbo.InstructorCourses", new[] { "Instructor_ID" });
            DropTable("dbo.InstructorCourses");
            DropTable("dbo.Instructors");
        }
    }
}
