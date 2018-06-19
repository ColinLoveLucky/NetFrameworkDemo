namespace LinqToEntity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver60 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "ModelOne.posts", name: "Blogs_BlogId", newName: "Blog_Id");
            RenameIndex(table: "ModelOne.posts", name: "IX_Blogs_BlogId", newName: "IX_Blog_Id");
            CreateStoredProcedure(
                "ModelOne.InsertBlogs",
                p => new
                    {
                        Name = p.String(name: "@Name", maxLength: 250),
                    },
                body:
                    @"INSERT [ModelOne].[blogs]([Name])
                      VALUES (@@Name)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [ModelOne].[blogs]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id] AS BlogId
                      FROM [ModelOne].[blogs] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "ModelOne.updateBlogs",
                p => new
                    {
                        blogId = p.Int(name: "@blogId"),
                        Name = p.String(name: "@Name", maxLength: 250),
                    },
                body:
                    @"UPDATE [ModelOne].[blogs]
                      SET [Name] = @@Name
                      WHERE ([Id] = @@blogId)"
            );
            
            CreateStoredProcedure(
                "ModelOne.DeleteBlogs",
                p => new
                    {
                        blogId = p.Int(name: "@blogId"),
                    },
                body:
                    @"DELETE [ModelOne].[blogs]
                      WHERE ([Id] = @@blogId)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("ModelOne.DeleteBlogs");
            DropStoredProcedure("ModelOne.updateBlogs");
            DropStoredProcedure("ModelOne.InsertBlogs");
            RenameIndex(table: "ModelOne.posts", name: "IX_Blog_Id", newName: "IX_Blogs_BlogId");
            RenameColumn(table: "ModelOne.posts", name: "Blog_Id", newName: "Blogs_BlogId");
        }
    }
}
