using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalDern_Support.Migrations
{
    /// <inheritdoc />
    public partial class EDITJObrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "c955b15d-7d9e-42b3-8acf-aeff39944a2b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "customer",
                column: "ConcurrencyStamp",
                value: "a8f975e4-3b3b-4bad-8bdd-2f24a59cd535");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "14d7f06f-187a-4c5e-8ebc-dc0b3f3549b7");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "95bb8bc3-09d5-488e-aa10-5786f8a7406d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "customer",
                column: "ConcurrencyStamp",
                value: "e3fd8947-f1d7-4166-bbbf-c426a2977bba");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "a13c7896-2191-44b3-a843-06c82dee7737");
        }
    }
}
