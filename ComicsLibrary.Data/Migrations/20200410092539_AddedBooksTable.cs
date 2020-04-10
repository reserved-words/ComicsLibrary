using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AddedBooksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookType",
                schema: "ComicsLibrary",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "ComicsLibrary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookTypeID = table.Column<int>(nullable: true),
                    SeriesId = table.Column<int>(nullable: false),
                    SourceItemID = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    Number = table.Column<double>(nullable: true),
                    Creators = table.Column<string>(nullable: true),
                    OnSaleDate = table.Column<DateTimeOffset>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    ReadUrl = table.Column<string>(nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    ReadUrlAdded = table.Column<DateTime>(nullable: true),
                    DateRead = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_BookType_BookTypeID",
                        column: x => x.BookTypeID,
                        principalSchema: "ComicsLibrary",
                        principalTable: "BookType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Books_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalSchema: "ComicsLibrary",
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookTypeID",
                schema: "ComicsLibrary",
                table: "Books",
                column: "BookTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Books_SeriesId",
                schema: "ComicsLibrary",
                table: "Books",
                column: "SeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books",
                schema: "ComicsLibrary");

            migrationBuilder.DropTable(
                name: "BookType",
                schema: "ComicsLibrary");
        }
    }
}
