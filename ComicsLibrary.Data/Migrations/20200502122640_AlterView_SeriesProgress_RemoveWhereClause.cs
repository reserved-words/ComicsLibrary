using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AlterView_SeriesProgress_RemoveWhereClause : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Views.Drop_SeriesProgress);
            migrationBuilder.Sql(Views.Create_SeriesProgress_v2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Views.Drop_SeriesProgress);
            migrationBuilder.Sql(Views.Create_SeriesProgress);
        }
    }
}
