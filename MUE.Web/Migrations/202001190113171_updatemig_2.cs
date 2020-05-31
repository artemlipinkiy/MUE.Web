namespace MUE.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatemig_2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeterReadings",
                c => new
                    {
                        MeterReadingId = c.Guid(nullable: false),
                        Value = c.Double(nullable: false),
                        PeriodId = c.Guid(nullable: false),
                        TypeofServiceId = c.Guid(nullable: false),
                        FlatId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.MeterReadingId)
                .ForeignKey("dbo.Flats", t => t.FlatId, cascadeDelete: true)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: true)
                .ForeignKey("dbo.TypeOfServices", t => t.TypeofServiceId, cascadeDelete: true)
                .Index(t => t.PeriodId)
                .Index(t => t.TypeofServiceId)
                .Index(t => t.FlatId);
            
            CreateTable(
                "dbo.Periods",
                c => new
                    {
                        PeriodId = c.Guid(nullable: false),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsCurrent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PeriodId);
            
            CreateTable(
                "dbo.ServiceBills",
                c => new
                    {
                        ServiceBillId = c.Guid(nullable: false),
                        PeriodId = c.Guid(nullable: false),
                        TypeOfServiceId = c.Guid(nullable: false),
                        FlatId = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceBillId)
                .ForeignKey("dbo.Flats", t => t.FlatId, cascadeDelete: true)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: true)
                .ForeignKey("dbo.TypeOfServices", t => t.TypeOfServiceId, cascadeDelete: true)
                .Index(t => t.PeriodId)
                .Index(t => t.TypeOfServiceId)
                .Index(t => t.FlatId);
            
            CreateTable(
                "dbo.SettlementSheets",
                c => new
                    {
                        SettlementSheetId = c.Guid(nullable: false),
                        PeriodId = c.Guid(nullable: false),
                        FlatId = c.Guid(nullable: false),
                        AmmountToBePaid = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.SettlementSheetId)
                .ForeignKey("dbo.Flats", t => t.FlatId, cascadeDelete: true)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: true)
                .Index(t => t.PeriodId)
                .Index(t => t.FlatId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SettlementSheets", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.SettlementSheets", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.ServiceBills", "TypeOfServiceId", "dbo.TypeOfServices");
            DropForeignKey("dbo.ServiceBills", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.ServiceBills", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.MeterReadings", "TypeofServiceId", "dbo.TypeOfServices");
            DropForeignKey("dbo.MeterReadings", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.MeterReadings", "FlatId", "dbo.Flats");
            DropIndex("dbo.SettlementSheets", new[] { "FlatId" });
            DropIndex("dbo.SettlementSheets", new[] { "PeriodId" });
            DropIndex("dbo.ServiceBills", new[] { "FlatId" });
            DropIndex("dbo.ServiceBills", new[] { "TypeOfServiceId" });
            DropIndex("dbo.ServiceBills", new[] { "PeriodId" });
            DropIndex("dbo.MeterReadings", new[] { "FlatId" });
            DropIndex("dbo.MeterReadings", new[] { "TypeofServiceId" });
            DropIndex("dbo.MeterReadings", new[] { "PeriodId" });
            DropTable("dbo.SettlementSheets");
            DropTable("dbo.ServiceBills");
            DropTable("dbo.Periods");
            DropTable("dbo.MeterReadings");
        }
    }
}
