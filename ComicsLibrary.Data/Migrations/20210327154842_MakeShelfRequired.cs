using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class MakeShelfRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var updateShelfSql = "UPDATE [ComicsLibrary].[Series] "
                + "SET Shelf = CASE "
                + "WHEN [Abandoned] = 1 AND [ReadBooks] = 0 THEN 6 "
                + "WHEN [UnreadBooks] = 0 THEN 5 "
                + "WHEN [Abandoned] = 1 THEN 4 "
                + "WHEN [ReadBooks] > 0 AND [UnreadBooks] > 0 THEN 1 "
                + "ELSE 3 "
                + "END "
                + "FROM [ComicsLibrary].[Series] S "
                + "LEFT JOIN[ComicsLibrary].[SeriesProgress] P ON S.Id = P.Id";

            migrationBuilder.Sql(updateShelfSql);

            migrationBuilder.AlterColumn<int>(
                name: "Shelf",
                schema: "ComicsLibrary",
                table: "Series",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Shelf",
                schema: "ComicsLibrary",
                table: "Series",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
