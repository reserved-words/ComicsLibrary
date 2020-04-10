using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AddedHomeBookTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeBookTypes",
                schema: "ComicsLibrary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesId = table.Column<int>(nullable: false),
                    BookTypeId = table.Column<int>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeBookTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeBookTypes_BookType_BookTypeId",
                        column: x => x.BookTypeId,
                        principalSchema: "ComicsLibrary",
                        principalTable: "BookType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeBookTypes_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalSchema: "ComicsLibrary",
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeBookTypes_BookTypeId",
                schema: "ComicsLibrary",
                table: "HomeBookTypes",
                column: "BookTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeBookTypes_SeriesId",
                schema: "ComicsLibrary",
                table: "HomeBookTypes",
                column: "SeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeBookTypes",
                schema: "ComicsLibrary");
        }
    }
}
