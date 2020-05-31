namespace MUE.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Streets",
                c => new
                    {
                        StreetId = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.StreetId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Streets");
        }
    }
}
