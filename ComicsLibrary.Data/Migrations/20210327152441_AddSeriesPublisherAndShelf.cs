using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AddSeriesPublisherAndShelf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                schema: "ComicsLibrary",
                table: "Series",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Shelf",
                schema: "ComicsLibrary",
                table: "Series",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Publisher",
                schema: "ComicsLibrary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(maxLength: 20, nullable: false),
                    ShortName = table.Column<string>(maxLength: 2, nullable: false),
                    Colour = table.Column<string>(maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publisher", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Series_PublisherId",
                schema: "ComicsLibrary",
                table: "Series",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Publisher_PublisherId",
                schema: "ComicsLibrary",
                table: "Series",
                column: "PublisherId",
                principalSchema: "ComicsLibrary",
                principalTable: "Publisher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Series_Publisher_PublisherId",
                schema: "ComicsLibrary",
                table: "Series");

            migrationBuilder.DropTable(
                name: "Publisher",
                schema: "ComicsLibrary");

            migrationBuilder.DropIndex(
                name: "IX_Series_PublisherId",
                schema: "ComicsLibrary",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                schema: "ComicsLibrary",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Shelf",
                schema: "ComicsLibrary",
                table: "Series");
        }
    }
}
