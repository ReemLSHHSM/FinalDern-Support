using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalDern_Support.Migrations
{
    /// <inheritdoc />
    public partial class RemoveJOBIDFROMREQUEST : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Jobs_JobID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_JobID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "JobID",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "RequestID",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "1946dbda-29cf-4e30-8218-61f564b988b2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "customer",
                column: "ConcurrencyStamp",
                value: "82a1bfca-22e0-4f4a-a8fe-14725424706f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "6a7fe620-10fb-46e8-a542-0013666ecc58");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_RequestID",
                table: "Jobs",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Requests_RequestID",
                table: "Jobs",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Requests_RequestID",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_RequestID",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "RequestID",
                table: "Jobs");

            migrationBuilder.AddColumn<int>(
                name: "JobID",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "98d3a3da-99dd-4483-b94b-d079253a171e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "customer",
                column: "ConcurrencyStamp",
                value: "fdfc8095-a443-4597-a3be-f6afef7d9fde");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "53908842-b5ae-4194-bc62-43adde2ab1c2");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_JobID",
                table: "Requests",
                column: "JobID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Jobs_JobID",
                table: "Requests",
                column: "JobID",
                principalTable: "Jobs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
