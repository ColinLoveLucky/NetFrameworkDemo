namespace LinqToEntity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver70 : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure(
                "ModelOne.InsertBlogs",
                p => new
                    {
                        Name = p.String(maxLength: 250),
                    },
                body:
                    @"INSERT [ModelOne].[blogs]([Name])
                      VALUES (@Name)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [ModelOne].[blogs]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id] AS BlogId
                      FROM [ModelOne].[blogs] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "ModelOne.updateBlogs",
                p => new
                    {
                        blogId = p.Int(),
                        Name = p.String(maxLength: 250),
                    },
                body:
                    @"UPDATE [ModelOne].[blogs]
                      SET [Name] = @Name
                      WHERE ([Id] = @blogId)"
            );
            
            AlterStoredProcedure(
                "ModelOne.DeleteBlogs",
                p => new
                    {
                        blogId = p.Int(),
                    },
                body:
                    @"DELETE [ModelOne].[blogs]
                      WHERE ([Id] = @blogId)"
            );
            
        }
        
        public override void Down()
        {
            throw new NotSupportedException("在 down 方法中不支持基架创建或更改过程操作。");
        }
    }
}
