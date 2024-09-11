using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalDern_Support.Migrations
{
    /// <inheritdoc />
    public partial class REMOVEDFEEDBACKANDREPORTID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Feedbacks_FeedbackID",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Requests_RequestID",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_FeedbackID",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_RequestID",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "FeedbackID",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ReportID",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "RequestID",
                table: "Jobs");

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

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_JobID",
                table: "Feedbacks",
                column: "JobID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Jobs_JobID",
                table: "Feedbacks",
                column: "JobID",
                principalTable: "Jobs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Jobs_JobID",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_JobID",
                table: "Feedbacks");

            migrationBuilder.AddColumn<int>(
                name: "FeedbackID",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReportID",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_Jobs_FeedbackID",
                table: "Jobs",
                column: "FeedbackID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_RequestID",
                table: "Jobs",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Feedbacks_FeedbackID",
                table: "Jobs",
                column: "FeedbackID",
                principalTable: "Feedbacks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Requests_RequestID",
                table: "Jobs",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
