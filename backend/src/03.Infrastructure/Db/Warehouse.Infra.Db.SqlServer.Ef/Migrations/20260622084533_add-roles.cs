using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Migrations
{
    /// <inheritdoc />
    public partial class addroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "c7b013f0-5201-4317-abd8-c211f91b7330", "Admin", "ADMIN" },
                    { 2, "033fa693-010e-436f-a602-e0f0dfc836e4", "WarehouseManager", "WAREHOUSEMANAGER" },
                    { 3, "5c52c6f6-49a3-4876-8805-5da845cce51c", "DepartmentHead", "DEPARTMENTHEAD" },
                    { 4, "81ba5de7-f4e7-4bd9-9fc4-8bdc87ccbf86", "WorkshopSupervisor", "WORKSHOPSUPERVISOR" },
                    { 5, "d11b22e1-4545-4b7d-b3b3-8551da7ebf54", "ProcurementOfficer", "PROCUREMENTOFFICER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
