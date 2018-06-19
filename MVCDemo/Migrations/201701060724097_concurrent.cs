namespace MVCDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class concurrent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Instructors", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterStoredProcedure(
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
                      
                      SELECT t0.[ID], t0.[RowVersion]
                      FROM [dbo].[Instructors] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
            AlterStoredProcedure(
                "dbo.Instructor_Update",
                p => new
                    {
                        ID = p.Int(),
                        LastName = p.String(),
                        FirstMidName = p.String(),
                        HireDate = p.DateTime(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"UPDATE [dbo].[Instructors]
                      SET [LastName] = @LastName, [FirstMidName] = @FirstMidName, [HireDate] = @HireDate
                      WHERE (([ID] = @ID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))
                      
                      SELECT t0.[RowVersion]
                      FROM [dbo].[Instructors] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ID] = @ID"
            );
            
            AlterStoredProcedure(
                "dbo.Instructor_Delete",
                p => new
                    {
                        ID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"DELETE [dbo].[Instructors]
                      WHERE (([ID] = @ID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Instructors", "RowVersion");
            throw new NotSupportedException("在 down 方法中不支持基架创建或更改过程操作。");
        }
    }
}
