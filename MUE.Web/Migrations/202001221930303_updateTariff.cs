namespace MUE.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTariff : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TariffFlats", "Tariff_TariffId", "dbo.Tariffs");
            DropForeignKey("dbo.TariffFlats", "Flat_FlatId", "dbo.Flats");
            DropIndex("dbo.TariffFlats", new[] { "Tariff_TariffId" });
            DropIndex("dbo.TariffFlats", new[] { "Flat_FlatId" });
            AddColumn("dbo.Tariffs", "FlatId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Tariffs", "FlatId");
            AddForeignKey("dbo.Tariffs", "FlatId", "dbo.Flats", "FlatId", cascadeDelete: true);
            DropTable("dbo.TariffFlats");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TariffFlats",
                c => new
                    {
                        Tariff_TariffId = c.Guid(nullable: false),
                        Flat_FlatId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tariff_TariffId, t.Flat_FlatId });
            
            DropForeignKey("dbo.Tariffs", "FlatId", "dbo.Flats");
            DropIndex("dbo.Tariffs", new[] { "FlatId" });
            DropColumn("dbo.Tariffs", "FlatId");
            CreateIndex("dbo.TariffFlats", "Flat_FlatId");
            CreateIndex("dbo.TariffFlats", "Tariff_TariffId");
            AddForeignKey("dbo.TariffFlats", "Flat_FlatId", "dbo.Flats", "FlatId", cascadeDelete: true);
            AddForeignKey("dbo.TariffFlats", "Tariff_TariffId", "dbo.Tariffs", "TariffId", cascadeDelete: true);
        }
    }
}
