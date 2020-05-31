namespace MUE.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_summ : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SettlementSheets", "Summ", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SettlementSheets", "Summ");
        }
    }
}
