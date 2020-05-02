using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AlterView_GetHomeBooks_AddColumnProgress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_GetHomeBooks);
            migrationBuilder.Sql(StoredProcedures.Create_GetHomeBooks_v5);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_GetHomeBooks);
            migrationBuilder.Sql(StoredProcedures.Create_GetHomeBooks_v4);
        }
    }
}
