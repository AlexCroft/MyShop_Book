namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItems", "ProductName", c => c.String());
            DropColumn("dbo.OrderItems", "Name");
            DropColumn("dbo.OrderItems", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderItems", "Category", c => c.String());
            AddColumn("dbo.OrderItems", "Name", c => c.String());
            DropColumn("dbo.OrderItems", "ProductName");
        }
    }
}
