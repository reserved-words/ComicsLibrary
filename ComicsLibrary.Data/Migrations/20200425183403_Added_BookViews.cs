using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class Added_BookViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comics",
                schema: "ComicsLibrary");

            migrationBuilder.DropColumn(
                name: "MarvelId",
                schema: "ComicsLibrary",
                table: "Series");

            migrationBuilder.Sql(Views.Create_SeriesAllBooks);
            migrationBuilder.Sql(Views.Create_SeriesUnreadBooks);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Views.Drop_SeriesUnreadBooks);
            migrationBuilder.Sql(Views.Drop_SeriesAllBooks);

            migrationBuilder.AddColumn<int>(
                name: "MarvelId",
                schema: "ComicsLibrary",
                table: "Series",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Comics",
                schema: "ComicsLibrary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creators = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateRead = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiamondCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IssueNumber = table.Column<double>(type: "float", nullable: true),
                    MarvelId = table.Column<int>(type: "int", nullable: true),
                    OnSaleDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReadUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadUrlAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Upc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comics_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalSchema: "ComicsLibrary",
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comics_SeriesId",
                schema: "ComicsLibrary",
                table: "Comics",
                column: "SeriesId");
        }
    }
}
