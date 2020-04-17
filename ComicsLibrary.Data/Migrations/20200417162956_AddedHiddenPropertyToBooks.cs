using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AddedHiddenPropertyToBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                schema: "ComicsLibrary",
                table: "Books",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("update ComicsLibrary.Books set Hidden = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hidden",
                schema: "ComicsLibrary",
                table: "Books");
        }
    }
}
