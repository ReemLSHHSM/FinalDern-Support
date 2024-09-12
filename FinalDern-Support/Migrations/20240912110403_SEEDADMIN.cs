using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalDern_Support.Migrations
{
    /// <inheritdoc />
    public partial class SEEDADMIN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "d05eb0f7-c1c0-4f6f-af3d-30ecfeee4e8f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "customer",
                column: "ConcurrencyStamp",
                value: "8aa4d161-4766-41cc-8315-aa3be7d31b82");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "7d7e596d-42c4-49c1-b507-07158db397aa");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "type" },
                values: new object[] { "ad07ccdc-3359-4b35-bf53-91c81568b275", 0, "d9d2914a-de03-4c5f-8437-ec0a722d0f49", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEHM7Nk1XcG9xHmKw+tYOiajIVWry2RUcVsaHg10eEn+LXt751J8Sg0dCt9XWTDlTtw==", "1234567890", true, "4793d967-8370-46f2-8723-297fef74fc63", false, "admin", "Admin" });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "ID", "UserID" },
                values: new object[] { 1, "ad07ccdc-3359-4b35-bf53-91c81568b275" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "Admin", "ad07ccdc-3359-4b35-bf53-91c81568b275" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "Admin", "ad07ccdc-3359-4b35-bf53-91c81568b275" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ad07ccdc-3359-4b35-bf53-91c81568b275");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "468d433f-61dd-41ca-ac83-828280c9ebe1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "customer",
                column: "ConcurrencyStamp",
                value: "9b061d3d-ed30-4a8b-8843-ff3111dcb68c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "fb9df298-e812-425c-94ec-f7bf1c4fadca");
        }
    }
}
