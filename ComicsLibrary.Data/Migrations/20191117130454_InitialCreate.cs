using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ComicsLibrary");

            migrationBuilder.CreateTable(
                name: "Series",
                schema: "ComicsLibrary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    MarvelId = table.Column<int>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: true),
                    StartYear = table.Column<int>(nullable: true),
                    EndYear = table.Column<int>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Characters = table.Column<string>(nullable: true),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    IsFinished = table.Column<bool>(nullable: false),
                    Abandoned = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComicSeries",
                schema: "ComicsLibrary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    MarvelId = table.Column<int>(nullable: true),
                    ReadUrl = table.Column<string>(nullable: true),
                    DiamondCode = table.Column<string>(nullable: true),
                    Isbn = table.Column<string>(nullable: true),
                    IssueNumber = table.Column<double>(nullable: true),
                    Upc = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    SeriesId = table.Column<int>(nullable: false),
                    Creators = table.Column<string>(nullable: true),
                    OnSaleDate = table.Column<DateTimeOffset>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    ToReadNext = table.Column<bool>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    ReadUrlAdded = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComicSeries_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalSchema: "ComicsLibrary",
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComicSeries_SeriesId",
                schema: "ComicsLibrary",
                table: "ComicSeries",
                column: "SeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComicSeries",
                schema: "ComicsLibrary");

            migrationBuilder.DropTable(
                name: "Series",
                schema: "ComicsLibrary");
        }
    }
}
