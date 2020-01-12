using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class RenamedComicsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComicSeries_Series_SeriesId",
                schema: "ComicsLibrary",
                table: "ComicSeries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComicSeries",
                schema: "ComicsLibrary",
                table: "ComicSeries");

            migrationBuilder.RenameTable(
                name: "ComicSeries",
                schema: "ComicsLibrary",
                newName: "Comics",
                newSchema: "ComicsLibrary");

            migrationBuilder.RenameIndex(
                name: "IX_ComicSeries_SeriesId",
                schema: "ComicsLibrary",
                table: "Comics",
                newName: "IX_Comics_SeriesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comics",
                schema: "ComicsLibrary",
                table: "Comics",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comics_Series_SeriesId",
                schema: "ComicsLibrary",
                table: "Comics",
                column: "SeriesId",
                principalSchema: "ComicsLibrary",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comics_Series_SeriesId",
                schema: "ComicsLibrary",
                table: "Comics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comics",
                schema: "ComicsLibrary",
                table: "Comics");

            migrationBuilder.RenameTable(
                name: "Comics",
                schema: "ComicsLibrary",
                newName: "ComicSeries",
                newSchema: "ComicsLibrary");

            migrationBuilder.RenameIndex(
                name: "IX_Comics_SeriesId",
                schema: "ComicsLibrary",
                table: "ComicSeries",
                newName: "IX_ComicSeries_SeriesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComicSeries",
                schema: "ComicsLibrary",
                table: "ComicSeries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComicSeries_Series_SeriesId",
                schema: "ComicsLibrary",
                table: "ComicSeries",
                column: "SeriesId",
                principalSchema: "ComicsLibrary",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
