namespace MUE.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatemig_1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Buildings",
                c => new
                    {
                        BuildingId = c.Guid(nullable: false),
                        Number = c.String(),
                        Description = c.String(),
                        StreetId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.BuildingId)
                .ForeignKey("dbo.Streets", t => t.StreetId, cascadeDelete: true)
                .Index(t => t.StreetId);
            
            CreateTable(
                "dbo.Flats",
                c => new
                    {
                        FlatId = c.Guid(nullable: false),
                        Number = c.Int(nullable: false),
                        Rooms = c.Int(nullable: false),
                        CountResidents = c.Int(nullable: false),
                        Square = c.Double(nullable: false),
                        BuildingId = c.Guid(nullable: false),
                        OwnersId = c.Guid(),
                        Owner_OwnerId = c.Guid(),
                    })
                .PrimaryKey(t => t.FlatId)
                .ForeignKey("dbo.Buildings", t => t.BuildingId, cascadeDelete: true)
                .ForeignKey("dbo.Owners", t => t.Owner_OwnerId)
                .Index(t => t.BuildingId)
                .Index(t => t.Owner_OwnerId);
            
            CreateTable(
                "dbo.Owners",
                c => new
                    {
                        OwnerId = c.Guid(nullable: false),
                        LastName = c.String(),
                        FirstName = c.String(),
                        MiddlleName = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.OwnerId);
            
            CreateTable(
                "dbo.Tariffs",
                c => new
                    {
                        TariffId = c.Guid(nullable: false),
                        TypeOfServiceId = c.Guid(nullable: false),
                        Value = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TariffId)
                .ForeignKey("dbo.TypeOfServices", t => t.TypeOfServiceId, cascadeDelete: true)
                .Index(t => t.TypeOfServiceId);
            
            CreateTable(
                "dbo.TypeOfServices",
                c => new
                    {
                        TypeOfServiceId = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        UnitOfMeasurment = c.String(),
                        IsMeter = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TypeOfServiceId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        Login = c.String(),
                        HashPassword = c.String(),
                        RoleId = c.Guid(nullable: false),
                        OwnerId = c.Guid(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Owners", t => t.OwnerId)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.TariffFlats",
                c => new
                    {
                        Tariff_TariffId = c.Guid(nullable: false),
                        Flat_FlatId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tariff_TariffId, t.Flat_FlatId })
                .ForeignKey("dbo.Tariffs", t => t.Tariff_TariffId, cascadeDelete: true)
                .ForeignKey("dbo.Flats", t => t.Flat_FlatId, cascadeDelete: true)
                .Index(t => t.Tariff_TariffId)
                .Index(t => t.Flat_FlatId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Users", "OwnerId", "dbo.Owners");
            DropForeignKey("dbo.Tariffs", "TypeOfServiceId", "dbo.TypeOfServices");
            DropForeignKey("dbo.TariffFlats", "Flat_FlatId", "dbo.Flats");
            DropForeignKey("dbo.TariffFlats", "Tariff_TariffId", "dbo.Tariffs");
            DropForeignKey("dbo.Flats", "Owner_OwnerId", "dbo.Owners");
            DropForeignKey("dbo.Flats", "BuildingId", "dbo.Buildings");
            DropForeignKey("dbo.Buildings", "StreetId", "dbo.Streets");
            DropIndex("dbo.TariffFlats", new[] { "Flat_FlatId" });
            DropIndex("dbo.TariffFlats", new[] { "Tariff_TariffId" });
            DropIndex("dbo.Users", new[] { "OwnerId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Tariffs", new[] { "TypeOfServiceId" });
            DropIndex("dbo.Flats", new[] { "Owner_OwnerId" });
            DropIndex("dbo.Flats", new[] { "BuildingId" });
            DropIndex("dbo.Buildings", new[] { "StreetId" });
            DropTable("dbo.TariffFlats");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.TypeOfServices");
            DropTable("dbo.Tariffs");
            DropTable("dbo.Owners");
            DropTable("dbo.Flats");
            DropTable("dbo.Buildings");
        }
    }
}
