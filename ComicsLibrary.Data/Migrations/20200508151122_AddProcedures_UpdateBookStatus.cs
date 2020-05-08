using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AddProcedures_UpdateBookStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Create_UpdateBookHideStatus);
            migrationBuilder.Sql(StoredProcedures.Create_UpdateBookReadStatus);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_UpdateBookHideStatus);
            migrationBuilder.Sql(StoredProcedures.Drop_UpdateBookReadStatus);
        }
    }
}
