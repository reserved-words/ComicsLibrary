namespace ComicsLibrary.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ComicsLibrary.Comics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        ImageUrl = c.String(),
                        MarvelId = c.Int(),
                        ReadUrl = c.String(),
                        DiamondCode = c.String(),
                        Isbn = c.String(),
                        IssueNumber = c.Double(),
                        Upc = c.String(),
                        Url = c.String(),
                        SeriesId = c.Int(nullable: false),
                        Creators = c.String(),
                        OnSaleDate = c.DateTimeOffset(precision: 7),
                        IsRead = c.Boolean(nullable: false),
                        ToReadNext = c.Boolean(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        ReadUrlAdded = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("ComicsLibrary.Series", t => t.SeriesId, cascadeDelete: true)
                .Index(t => t.SeriesId);
            
            CreateTable(
                "ComicsLibrary.Series",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        ImageUrl = c.String(),
                        MarvelId = c.Int(),
                        Url = c.String(),
                        Order = c.Int(),
                        StartYear = c.Int(),
                        EndYear = c.Int(),
                        Type = c.String(),
                        Characters = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                        IsFinished = c.Boolean(nullable: false),
                        Abandoned = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ComicsLibrary.Comics", "SeriesId", "ComicsLibrary.Series");
            DropIndex("ComicsLibrary.Comics", new[] { "SeriesId" });
            DropTable("ComicsLibrary.Series");
            DropTable("ComicsLibrary.Comics");
        }
    }
}
