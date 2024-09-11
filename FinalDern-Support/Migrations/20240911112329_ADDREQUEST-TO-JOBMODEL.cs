using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalDern_Support.Migrations
{
    /// <inheritdoc />
    public partial class ADDREQUESTTOJOBMODEL : Migration
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

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin",
                column: "ConcurrencyStamp",
                value: "387b3433-41d9-40e7-b4bb-a9bfd623829c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "customer",
                column: "ConcurrencyStamp",
                value: "aea6f71d-8e83-4c5a-90b7-315b973a1ad5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "technician",
                column: "ConcurrencyStamp",
                value: "edf55a46-6c26-4fc2-af1b-d10276482b76");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Jobs_JobID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_JobID",
                table: "Requests");

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

            migrationBuilder.CreateIndex(
                name: "IX_Requests_JobID",
                table: "Requests",
                column: "JobID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Jobs_JobID",
                table: "Requests",
                column: "JobID",
                principalTable: "Jobs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
