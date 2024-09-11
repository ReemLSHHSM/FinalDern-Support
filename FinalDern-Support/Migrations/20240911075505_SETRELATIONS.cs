using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalDern_Support.Migrations
{
    /// <inheritdoc />
    public partial class SETRELATIONS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Technicians",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "JobID",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Admins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "JobSpareParts",
                columns: table => new
                {
                    JobID = table.Column<int>(type: "int", nullable: false),
                    SparePartID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSpareParts", x => new { x.JobID, x.SparePartID });
                    table.ForeignKey(
                        name: "FK_JobSpareParts_Jobs_JobID",
                        column: x => x.JobID,
                        principalTable: "Jobs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobSpareParts_SpareParts_SparePartID",
                        column: x => x.SparePartID,
                        principalTable: "SpareParts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Technicians_UserID",
                table: "Technicians",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CustomerID",
                table: "Requests",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_JobID",
                table: "Requests",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_JobID",
                table: "Reports",
                column: "JobID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TechnicianID",
                table: "Reports",
                column: "TechnicianID");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_RequestID",
                table: "Quotes",
                column: "RequestID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeBases_AdminID",
                table: "KnowledgeBases",
                column: "AdminID");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_FeedbackID",
                table: "Jobs",
                column: "FeedbackID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_QuoteID",
                table: "Jobs",
                column: "QuoteID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_TechID",
                table: "Jobs",
                column: "TechID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_CustomerID",
                table: "Feedbacks",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserID",
                table: "Customers",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserID",
                table: "Admins",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobSpareParts_SparePartID",
                table: "JobSpareParts",
                column: "SparePartID");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_AspNetUsers_UserID",
                table: "Admins",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_AspNetUsers_UserID",
                table: "Customers",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Customers_CustomerID",
                table: "Feedbacks",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Feedbacks_FeedbackID",
                table: "Jobs",
                column: "FeedbackID",
                principalTable: "Feedbacks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Quotes_QuoteID",
                table: "Jobs",
                column: "QuoteID",
                principalTable: "Quotes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Technicians_TechID",
                table: "Jobs",
                column: "TechID",
                principalTable: "Technicians",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KnowledgeBases_Admins_AdminID",
                table: "KnowledgeBases",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_Requests_RequestID",
                table: "Quotes",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Jobs_JobID",
                table: "Reports",
                column: "JobID",
                principalTable: "Jobs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Technicians_TechnicianID",
                table: "Reports",
                column: "TechnicianID",
                principalTable: "Technicians",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Customers_CustomerID",
                table: "Requests",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Jobs_JobID",
                table: "Requests",
                column: "JobID",
                principalTable: "Jobs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_AspNetUsers_UserID",
                table: "Technicians",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_AspNetUsers_UserID",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AspNetUsers_UserID",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Customers_CustomerID",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Feedbacks_FeedbackID",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Quotes_QuoteID",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Technicians_TechID",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_KnowledgeBases_Admins_AdminID",
                table: "KnowledgeBases");

            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_Requests_RequestID",
                table: "Quotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Jobs_JobID",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Technicians_TechnicianID",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Customers_CustomerID",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Jobs_JobID",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Technicians_AspNetUsers_UserID",
                table: "Technicians");

            migrationBuilder.DropTable(
                name: "JobSpareParts");

            migrationBuilder.DropIndex(
                name: "IX_Technicians_UserID",
                table: "Technicians");

            migrationBuilder.DropIndex(
                name: "IX_Requests_CustomerID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_JobID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Reports_JobID",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_TechnicianID",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Quotes_RequestID",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_KnowledgeBases_AdminID",
                table: "KnowledgeBases");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_FeedbackID",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_QuoteID",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_TechID",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_CustomerID",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Customers_UserID",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Admins_UserID",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "JobID",
                table: "Requests");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Technicians",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
