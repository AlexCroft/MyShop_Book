namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SimplifyBasketItem : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BasketItems", "ProductName");
            DropColumn("dbo.BasketItems", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BasketItems", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BasketItems", "ProductName", c => c.String());
        }
    }
}
