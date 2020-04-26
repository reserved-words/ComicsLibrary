using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class FixedAndRenamedProcedure_GetAllSeries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_GetAllSeries);
            migrationBuilder.Sql(StoredProcedures.Create_GetSeries);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_GetSeries);
            migrationBuilder.Sql(StoredProcedures.Create_GetAllSeries_v2);
        }
    }
}
