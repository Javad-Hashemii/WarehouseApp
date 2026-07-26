using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Migrations
{
    /// <inheritdoc />
    public partial class modifyroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "WorkshopManager", "WORKSHOPMANAGER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "CentralManager", "CENTRALMANAGER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "DepartmentHead", "DEPARTMENTHEAD" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "WorkshopSupervisor", "WORKSHOPSUPERVISOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 6, "a1c2d3e4-f5g6-h7i8-j9k0-l1m2n3o4p5q6", "DepartmentManager", "DEPARTMENTMANAGER" });
        }
    }
}
