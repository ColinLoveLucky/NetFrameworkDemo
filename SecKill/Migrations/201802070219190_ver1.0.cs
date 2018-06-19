namespace SecKill.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ver10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Phone = c.String(maxLength: 11),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        ProductEntity_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductEntity_Id)
                .Index(t => t.ProductEntity_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductType = c.String(maxLength: 10),
                        ProdcutName = c.String(maxLength: 100),
                        Price = c.Double(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductName = c.String(maxLength: 100),
                        SurplusNum = c.Long(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ProductEntity_Id", "dbo.Products");
            DropIndex("dbo.Orders", new[] { "ProductEntity_Id" });
            DropTable("dbo.Stocks");
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
        }
    }
}
