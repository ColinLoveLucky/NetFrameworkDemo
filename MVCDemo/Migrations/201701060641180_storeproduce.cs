namespace MVCDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class storeproduce : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.Instructor_Insert",
                p => new
                    {
                        LastName = p.String(),
                        FirstMidName = p.String(),
                        HireDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Instructors]([LastName], [FirstMidName], [HireDate])
                      VALUES (@LastName, @FirstMidName, @HireDate)
                      
                      DECLARE @ID int
                      SELECT @ID = [ID]
                      FROM [dbo].[Instructors]
                      WHERE @@ROWCOUNT > 0 AND [ID] = scope_identity()
                      
                      SELECT t0.[ID]
                      FROM [dbo].[Instructors] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
            CreateStoredProcedure(
                "dbo.Instructor_Update",
                p => new
                    {
                        ID = p.Int(),
                        LastName = p.String(),
                        FirstMidName = p.String(),
                        HireDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Instructors]
                      SET [LastName] = @LastName, [FirstMidName] = @FirstMidName, [HireDate] = @HireDate
                      WHERE ([ID] = @ID)"
            );
            
            CreateStoredProcedure(
                "dbo.Instructor_Delete",
                p => new
                    {
                        ID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Instructors]
                      WHERE ([ID] = @ID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Instructor_Delete");
            DropStoredProcedure("dbo.Instructor_Update");
            DropStoredProcedure("dbo.Instructor_Insert");
        }
    }
}
