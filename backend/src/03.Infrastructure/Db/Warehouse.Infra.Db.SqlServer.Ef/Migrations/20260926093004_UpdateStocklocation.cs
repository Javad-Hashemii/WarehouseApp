using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStocklocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Warehouses_WorkshopId",
                table: "Warehouses");

            migrationBuilder.AlterColumn<int>(
                name: "WorkshopId",
                table: "Warehouses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_WorkshopId",
                table: "Warehouses",
                column: "WorkshopId",
                unique: true,
                filter: "[WorkshopId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Warehouses_WorkshopId",
                table: "Warehouses");

            migrationBuilder.AlterColumn<int>(
                name: "WorkshopId",
                table: "Warehouses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_WorkshopId",
                table: "Warehouses",
                column: "WorkshopId",
                unique: true);
        }
    }
}
