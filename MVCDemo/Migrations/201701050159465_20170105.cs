namespace MVCDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170105 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.App_Course",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Credits = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseId);
            
            CreateTable(
                "dbo.App_Enrollment",
                c => new
                    {
                        EnrollmentId = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        Grade = c.Int(),
                    })
                .PrimaryKey(t => t.EnrollmentId)
                .ForeignKey("dbo.App_Course", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.App_Student", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.App_Student",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        EnrollmentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.App_Enrollment", "StudentId", "dbo.App_Student");
            DropForeignKey("dbo.App_Enrollment", "CourseId", "dbo.App_Course");
            DropIndex("dbo.App_Enrollment", new[] { "StudentId" });
            DropIndex("dbo.App_Enrollment", new[] { "CourseId" });
            DropTable("dbo.App_Student");
            DropTable("dbo.App_Enrollment");
            DropTable("dbo.App_Course");
        }
    }
}
