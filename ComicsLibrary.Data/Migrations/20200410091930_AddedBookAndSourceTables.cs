using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AddedBookAndSourceTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                schema: "ComicsLibrary",
                table: "Series",
                nullable: true);

            migrationBuilder.Sql("UPDATE ComicsLibrary.Series SET SourceId = 1");

            migrationBuilder.CreateTable(
                name: "Sources",
                schema: "ComicsLibrary",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: true),
                    SeriesUrlFormat = table.Column<string>(maxLength: 255, nullable: true),
                    BookUrlFormat = table.Column<string>(maxLength: 255, nullable: true),
                    ReadUrlFormat = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Series_SourceId",
                schema: "ComicsLibrary",
                table: "Series",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Sources_SourceId",
                schema: "ComicsLibrary",
                table: "Series",
                column: "SourceId",
                principalSchema: "ComicsLibrary",
                principalTable: "Sources",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Series_Sources_SourceId",
                schema: "ComicsLibrary",
                table: "Series");

            migrationBuilder.DropTable(
                name: "Sources",
                schema: "ComicsLibrary");

            migrationBuilder.DropIndex(
                name: "IX_Series_SourceId",
                schema: "ComicsLibrary",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "SourceId",
                schema: "ComicsLibrary",
                table: "Series");
        }
    }
}
