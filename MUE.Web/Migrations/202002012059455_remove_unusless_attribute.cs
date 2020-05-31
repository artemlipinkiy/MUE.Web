namespace MUE.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_unusless_attribute : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SettlementSheets", "Summ");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SettlementSheets", "Summ", c => c.Double(nullable: false));
        }
    }
}
