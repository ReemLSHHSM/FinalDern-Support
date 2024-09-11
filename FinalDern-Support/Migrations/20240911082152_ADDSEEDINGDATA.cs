using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinalDern_Support.Migrations
{
    /// <inheritdoc />
    public partial class ADDSEEDINGDATA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "admin", "6ca3c5d2-925b-4057-925c-47382e0f09aa", "Admin", "ADMIN" },
                    { "customer", "38b06f36-4836-4107-a23b-41617f8f9a04", "Customer", "CUSTOMER" },
                    { "technician", "cd5ff052-1e86-466d-8354-ff7a7f1df90b", "Technician", "TECHNICIAN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "customer");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician");
        }
    }
}
