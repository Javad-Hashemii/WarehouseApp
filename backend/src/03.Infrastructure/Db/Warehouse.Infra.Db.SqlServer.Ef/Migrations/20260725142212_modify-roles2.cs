using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Migrations
{
    /// <inheritdoc />
    public partial class modifyroles2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 6, "e1f3c8d2-5b6a-4f9e-9c3b-1f2e3d4c5b6a", "WorkshopEmployee", "WORKSHOPEMPLOYEE" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
