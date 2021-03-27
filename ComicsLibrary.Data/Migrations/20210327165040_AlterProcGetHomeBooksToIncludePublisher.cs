using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AlterProcGetHomeBooksToIncludePublisher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_GetHomeBooks);
            migrationBuilder.Sql(StoredProcedures.Create_GetHomeBooks_v6);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_GetHomeBooks);
            migrationBuilder.Sql(StoredProcedures.Create_GetHomeBooks_v5);
        }
    }
}
