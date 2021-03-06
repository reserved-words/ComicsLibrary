﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AddedSourceItemIDToSeriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SourceItemID",
                schema: "ComicsLibrary",
                table: "Series",
                nullable: true);

            migrationBuilder.Sql("UPDATE ComicsLibrary.Series SET SourceItemID = MarvelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceItemID",
                schema: "ComicsLibrary",
                table: "Series");
        }
    }
}
