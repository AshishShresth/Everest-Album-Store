namespace EverestAlbumStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FourthCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoanViewModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        Name = c.String(),
                        CName = c.String(),
                        NumberOfLoans = c.Int(nullable: false),
                        LoanStatus = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LoanViewModels");
        }
    }
}
