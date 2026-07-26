using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Migrations
{
    /// <inheritdoc />
    public partial class editedentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Workshops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "Workshops",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Workshops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Warehouses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "Warehouses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Warehouses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "SubCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "SubCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SubCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Requests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "RequestItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "RequestItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RequestItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "RequestHistories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "RequestHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RequestHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "InventoryTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "InventoryTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "InventoryTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "InventoryBalances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "InventoryBalances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "InventoryBalances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Cities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "Cities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Cities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "SubCategories",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Warehouses",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Workshops",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Workshops",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Workshops",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });

            migrationBuilder.UpdateData(
                table: "Workshops",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DeletedAt", "DeletedByUserId", "IsDeleted" },
                values: new object[] { null, null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RequestItems");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "RequestHistories");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "RequestHistories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RequestHistories");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "InventoryBalances");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "InventoryBalances");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "InventoryBalances");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categories");
        }
    }
}
