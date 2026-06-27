using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Migrations
{
    /// <inheritdoc />
    public partial class addappuserseedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, "6f4b8a3d-9f21-4b12-889c-123456abcdef", null, "omid@gmail.com", false, "Omid Hashemi", false, null, "OMID.GMAIL.COM", "OMID@GMAIL.COM", "AQAAAAIAAYagAAAAENwIvr4Pn6t28/KI5Kh0/n+zAPGZGOKjq5iWt0sfzvdBRz/UaaiDbwNH2e5JbmDF0A==", null, false, "f3a9e8b2-1234-4bcd-9f2a-abcdef123456", false, "omid@gmail.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
