namespace MVCDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class metadataType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Instructors", "LastName", c => c.String(maxLength: 20));
            AlterColumn("dbo.Instructors", "FirstMidName", c => c.String(maxLength: 20));
            AlterStoredProcedure(
                "dbo.Instructor_Insert",
                p => new
                    {
                        LastName = p.String(maxLength: 20),
                        FirstMidName = p.String(maxLength: 20),
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
                        LastName = p.String(maxLength: 20),
                        FirstMidName = p.String(maxLength: 20),
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
            
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Instructors", "FirstMidName", c => c.String());
            AlterColumn("dbo.Instructors", "LastName", c => c.String());
            throw new NotSupportedException("在 down 方法中不支持基架创建或更改过程操作。");
        }
    }
}
