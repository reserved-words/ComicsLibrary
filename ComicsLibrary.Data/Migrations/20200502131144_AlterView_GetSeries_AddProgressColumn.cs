using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AlterView_GetSeries_AddProgressColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_GetSeries);
            migrationBuilder.Sql(StoredProcedures.Create_GetSeries_v2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_GetSeries);
            migrationBuilder.Sql(StoredProcedures.Create_GetSeries);
        }
    }
}
