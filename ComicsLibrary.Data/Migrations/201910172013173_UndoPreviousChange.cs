namespace ComicsLibrary.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using SqlResources = ComicsLibrary.Data.Resources;

    public partial class UndoPreviousChange : DbMigration
    {
        public override void Up()
        {
            Sql(SqlResources.CreateProcedure_CreateUsers);
        }

        public override void Down()
        {
            Sql(SqlResources.DropProcedure_CreateUsers);
        }
    }
}
