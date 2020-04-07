using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class Add_DateRead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Characters",
                schema: "ComicsLibrary",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Order",
                schema: "ComicsLibrary",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Isbn",
                schema: "ComicsLibrary",
                table: "Comics");

            migrationBuilder.DropColumn(
                name: "ToReadNext",
                schema: "ComicsLibrary",
                table: "Comics");

            migrationBuilder.DropColumn(
                name: "Url",
                schema: "ComicsLibrary",
                table: "Comics");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRead",
                schema: "ComicsLibrary",
                table: "Comics",
                nullable: true);

            migrationBuilder.Sql("UPDATE ComicsLibrary.Comics SET DateRead = GETDATE() WHERE IsRead = 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRead",
                schema: "ComicsLibrary",
                table: "Comics");

            migrationBuilder.AddColumn<string>(
                name: "Characters",
                schema: "ComicsLibrary",
                table: "Series",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                schema: "ComicsLibrary",
                table: "Series",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Isbn",
                schema: "ComicsLibrary",
                table: "Comics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ToReadNext",
                schema: "ComicsLibrary",
                table: "Comics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                schema: "ComicsLibrary",
                table: "Comics",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
