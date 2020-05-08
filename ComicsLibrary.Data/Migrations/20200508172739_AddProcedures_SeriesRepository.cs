using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AddProcedures_SeriesRepository : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Create_UpdateArchivedStatus);
            migrationBuilder.Sql(StoredProcedures.Create_GetSeriesWithBooks);
            migrationBuilder.Sql(StoredProcedures.Create_GetBooks);
            migrationBuilder.Sql(StoredProcedures.Create_DeleteSeries);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_UpdateArchivedStatus);
            migrationBuilder.Sql(StoredProcedures.Drop_GetSeriesWithBooks);
            migrationBuilder.Sql(StoredProcedures.Drop_GetBooks);
            migrationBuilder.Sql(StoredProcedures.Drop_DeleteSeries);
        }
    }
}
