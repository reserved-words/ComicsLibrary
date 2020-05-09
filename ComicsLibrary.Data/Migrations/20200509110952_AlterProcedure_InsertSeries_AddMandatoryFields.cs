using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AlterProcedure_InsertSeries_AddMandatoryFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_InsertSeries);
            migrationBuilder.Sql(StoredProcedures.Create_InsertSeries_v2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_InsertSeries);
            migrationBuilder.Sql(StoredProcedures.Create_InsertSeries);
        }
    }
}
