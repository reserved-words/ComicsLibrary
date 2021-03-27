using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AddProcGetShelf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Create_GetShelf);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_GetShelf);
        }
    }
}
