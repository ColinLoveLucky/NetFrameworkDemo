namespace EntityFrameWorkInfrans.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initaldatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.App_Action",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 20),
                    Name = c.String(maxLength: 50),
                    Description = c.String(maxLength: 300),
                    SocialSecurityNumber = c.Int(nullable: false),
                    DeleteFlag = c.Int(nullable: false),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    CreateTime = c.DateTime(),
                    UpdateTime = c.DateTime(),
                    SortId = c.Int(nullable: false),
                    Menu_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.App_Menu", t => t.Menu_Id)
                .Index(t => t.Menu_Id);

            CreateTable(
                "dbo.App_Comoany",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CompanyName = c.String(nullable: false, maxLength: 200),
                    JobInTime = c.DateTime(nullable: false),
                    DeleteFlag = c.Int(nullable: false),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    CreateTime = c.DateTime(),
                    UpdateTime = c.DateTime(),
                    SortId = c.Int(nullable: false),
                    User_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.App_User", t => t.User_Id)
                .Index(t => t.User_Id);

            CreateTable(
                "dbo.App_Dept",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(nullable: false, maxLength: 20),
                    Name = c.String(nullable: false, maxLength: 20),
                    DeleteFlag = c.Int(nullable: false),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    CreateTime = c.DateTime(),
                    UpdateTime = c.DateTime(),
                    SortId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.App_User",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserName = c.String(nullable: false, maxLength: 16),
                    PassWord = c.String(nullable: false, maxLength: 12),
                    DeleteFlag = c.Int(nullable: false),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    CreateTime = c.DateTime(),
                    UpdateTime = c.DateTime(),
                    SortId = c.Int(nullable: false),
                    UserGroup_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.App_User_Group", t => t.UserGroup_Id)
                .Index(t => t.UserGroup_Id);

            CreateTable(
                "dbo.App_Dictionary_Child",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Field = c.String(maxLength: 100),
                    Value = c.String(maxLength: 100),
                    DeleteFlag = c.Int(nullable: false),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    CreateTime = c.DateTime(),
                    UpdateTime = c.DateTime(),
                    SortId = c.Int(nullable: false),
                    Dictionary_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.App_Dictionary", t => t.Dictionary_Id)
                .Index(t => t.Dictionary_Id);

            CreateTable(
                "dbo.App_Dictionary",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(maxLength: 20),
                    Name = c.String(maxLength: 50),
                    DeleteFlag = c.Int(nullable: false),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    CreateTime = c.DateTime(),
                    UpdateTime = c.DateTime(),
                    SortId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.App_Menu",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    MenuIcon = c.String(maxLength: 100),
                    MenuUrl = c.String(maxLength: 100),
                    ParentId = c.Int(nullable: false),
                    DeleteFlag = c.Int(nullable: false),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    CreateTime = c.DateTime(),
                    UpdateTime = c.DateTime(),
                    SortId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.App_Role",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(maxLength: 20),
                    Name = c.String(maxLength: 20),
                    DeleteFlag = c.Int(nullable: false),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    CreateTime = c.DateTime(),
                    UpdateTime = c.DateTime(),
                    SortId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.App_User_Group",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(maxLength: 20),
                    Name = c.String(maxLength: 20),
                    DeleteFlag = c.Int(nullable: false),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    CreateTime = c.DateTime(),
                    UpdateTime = c.DateTime(),
                    SortId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.App_User_Depts",
                c => new
                {
                    User_Id = c.Int(nullable: false),
                    Dept_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.User_Id, t.Dept_Id })
                .ForeignKey("dbo.App_User", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.App_Dept", t => t.Dept_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Dept_Id);

            CreateTable(
                "dbo.App_Role_Menus",
                c => new
                {
                    Role_Id = c.Int(nullable: false),
                    Menu_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Role_Id, t.Menu_Id })
                .ForeignKey("dbo.App_Role", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.App_Menu", t => t.Menu_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Menu_Id);

            CreateTable(
                "dbo.App_UserGroup_Roles",
                c => new
                {
                    UserGroup_Id = c.Int(nullable: false),
                    Role_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.UserGroup_Id, t.Role_Id })
                .ForeignKey("dbo.App_User_Group", t => t.UserGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.App_Role", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.UserGroup_Id)
                .Index(t => t.Role_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.App_User", "UserGroup_Id", "dbo.App_User_Group");
            DropForeignKey("dbo.App_UserGroup_Roles", "Role_Id", "dbo.App_Role");
            DropForeignKey("dbo.App_UserGroup_Roles", "UserGroup_Id", "dbo.App_User_Group");
            DropForeignKey("dbo.App_Role_Menus", "Menu_Id", "dbo.App_Menu");
            DropForeignKey("dbo.App_Role_Menus", "Role_Id", "dbo.App_Role");
            DropForeignKey("dbo.App_Action", "Menu_Id", "dbo.App_Menu");
            DropForeignKey("dbo.App_Dictionary_Child", "Dictionary_Id", "dbo.App_Dictionary");
            DropForeignKey("dbo.App_User_Depts", "Dept_Id", "dbo.App_Dept");
            DropForeignKey("dbo.App_User_Depts", "User_Id", "dbo.App_User");
            DropForeignKey("dbo.App_Comoany", "User_Id", "dbo.App_User");
            DropIndex("dbo.App_UserGroup_Roles", new[] { "Role_Id" });
            DropIndex("dbo.App_UserGroup_Roles", new[] { "UserGroup_Id" });
            DropIndex("dbo.App_Role_Menus", new[] { "Menu_Id" });
            DropIndex("dbo.App_Role_Menus", new[] { "Role_Id" });
            DropIndex("dbo.App_User_Depts", new[] { "Dept_Id" });
            DropIndex("dbo.App_User_Depts", new[] { "User_Id" });
            DropIndex("dbo.App_Dictionary_Child", new[] { "Dictionary_Id" });
            DropIndex("dbo.App_User", new[] { "UserGroup_Id" });
            DropIndex("dbo.App_Comoany", new[] { "User_Id" });
            DropIndex("dbo.App_Action", new[] { "Menu_Id" });
            DropTable("dbo.App_UserGroup_Roles");
            DropTable("dbo.App_Role_Menus");
            DropTable("dbo.App_User_Depts");
            DropTable("dbo.App_User_Group");
            DropTable("dbo.App_Role");
            DropTable("dbo.App_Menu");
            DropTable("dbo.App_Dictionary");
            DropTable("dbo.App_Dictionary_Child");
            DropTable("dbo.App_User");
            DropTable("dbo.App_Dept");
            DropTable("dbo.App_Comoany");
            DropTable("dbo.App_Action");
        }
    }
}
