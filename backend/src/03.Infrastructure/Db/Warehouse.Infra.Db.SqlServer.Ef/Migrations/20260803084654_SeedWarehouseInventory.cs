using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Migrations
{
    /// <inheritdoc />
    public partial class SeedWarehouseInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "InventoryBalances",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "DeletedAt", "DeletedByUserId", "IsDeleted", "LastUpdatedAt", "ProductId", "Quantity", "StockLocationId", "UpdatedAt", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 67, null, null, false, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 100m, 1, null, null },
                    { 2, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 67, null, null, false, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 500m, 1, null, null },
                    { 3, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 67, null, null, false, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 75m, 2, null, null },
                    { 4, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 67, null, null, false, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 300m, 2, null, null },
                    { 5, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 67, null, null, false, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 50m, 3, null, null },
                    { 6, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 67, null, null, false, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 200m, 3, null, null },
                    { 7, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 67, null, null, false, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 40m, 4, null, null },
                    { 8, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 67, null, null, false, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 150m, 4, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InventoryBalances",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "InventoryBalances",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "InventoryBalances",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "InventoryBalances",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "InventoryBalances",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "InventoryBalances",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "InventoryBalances",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "InventoryBalances",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
