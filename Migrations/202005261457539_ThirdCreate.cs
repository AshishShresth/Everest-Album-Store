namespace EverestAlbumStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThirdCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "CopyNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Loans", "Fine", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Loans", "Fine");
            DropColumn("dbo.Albums", "CopyNumber");
        }
    }
}
