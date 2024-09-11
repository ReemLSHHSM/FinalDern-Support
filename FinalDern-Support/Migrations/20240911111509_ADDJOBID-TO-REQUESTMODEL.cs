using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalDern_Support.Migrations
{
    /// <inheritdoc />
    public partial class ADDJOBIDTOREQUESTMODEL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "f3c50b2e-4fe9-47be-a30b-31837ee3dc68");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "customer",
                column: "ConcurrencyStamp",
                value: "018816e7-0d38-4e80-afb0-cc88148d5c55");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "270c9acf-4ab1-443b-bccb-6283c55da490");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "6ca3c5d2-925b-4057-925c-47382e0f09aa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "customer",
                column: "ConcurrencyStamp",
                value: "38b06f36-4836-4107-a23b-41617f8f9a04");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "cd5ff052-1e86-466d-8354-ff7a7f1df90b");
        }
    }
}
