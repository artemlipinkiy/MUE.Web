namespace MUE.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_summ_toServiceBill : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceBills", "Summ", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceBills", "Summ");
        }
    }
}
