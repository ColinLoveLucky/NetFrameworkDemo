namespace LinqToEntity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ModelOne.blogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ModelOne.posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 250),
                        Content = c.String(maxLength: 250),
                        Blogs_BlogId = c.Int(),
                        UpdatedBy_Id = c.Int(),
                        CreatedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("ModelOne.blogs", t => t.Blogs_BlogId)
                .ForeignKey("ModelOne.person", t => t.UpdatedBy_Id)
                .ForeignKey("ModelOne.person", t => t.CreatedBy_Id)
                .Index(t => t.Blogs_BlogId)
                .Index(t => t.UpdatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "ModelOne.person",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ModelOne.employee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ModelOne.Lodgings",
                c => new
                    {
                        LodgingId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        Owner = c.String(maxLength: 250),
                        Activities = c.String(maxLength: 250),
                        Entertainment = c.String(maxLength: 250),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.LodgingId);
            
            CreateTable(
                "ModelOne.on_line_course",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        CourseDetails_Time = c.DateTime(),
                        CourseDetails_Location = c.String(maxLength: 250),
                        CourseDetails_Days = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ModelOne.product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(precision: 18, scale: 2),
                        productDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ModelOne.product_category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ModelOne.manager",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SectionManaged = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ModelOne.employee", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ModelOne.manager", "Id", "ModelOne.employee");
            DropForeignKey("ModelOne.posts", "CreatedBy_Id", "ModelOne.person");
            DropForeignKey("ModelOne.posts", "UpdatedBy_Id", "ModelOne.person");
            DropForeignKey("ModelOne.posts", "Blogs_BlogId", "ModelOne.blogs");
            DropIndex("ModelOne.manager", new[] { "Id" });
            DropIndex("ModelOne.posts", new[] { "CreatedBy_Id" });
            DropIndex("ModelOne.posts", new[] { "UpdatedBy_Id" });
            DropIndex("ModelOne.posts", new[] { "Blogs_BlogId" });
            DropTable("ModelOne.manager");
            DropTable("ModelOne.product_category");
            DropTable("ModelOne.product");
            DropTable("ModelOne.on_line_course");
            DropTable("ModelOne.Lodgings");
            DropTable("ModelOne.employee");
            DropTable("ModelOne.person");
            DropTable("ModelOne.posts");
            DropTable("ModelOne.blogs");
        }
    }
}
