using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AddedProcedures_ForSearchService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Create_InsertSeries);
            migrationBuilder.Sql(StoredProcedures.Create_GetSeriesIds);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_InsertSeries);
            migrationBuilder.Sql(StoredProcedures.Drop_GetSeriesIds);
        }
    }
}
