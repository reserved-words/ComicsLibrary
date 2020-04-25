using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AlterProcedure_GetAllSeries_AddSeriesParameter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_GetAllSeries);
            migrationBuilder.Sql(StoredProcedures.Create_GetAllSeries_v2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_GetAllSeries);
            migrationBuilder.Sql(StoredProcedures.Create_GetAllSeries);
        }
    }
}
