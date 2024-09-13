using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalDern_Support.Migrations
{
    /// <inheritdoc />
    public partial class addIDtoJunctiontableforreel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop and recreate the table with new schema
            migrationBuilder.DropTable(name: "JobSpareParts");

            migrationBuilder.CreateTable(
                name: "JobSpareParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobID = table.Column<int>(type: "int", nullable: false),
                    SparePartID = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSpareParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobSpareParts_Jobs_JobID",
                        column: x => x.JobID,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobSpareParts_SpareParts_SparePartID",
                        column: x => x.SparePartID,
                        principalTable: "SpareParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobSpareParts_JobID",
                table: "JobSpareParts",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_JobSpareParts_SparePartID",
                table: "JobSpareParts",
                column: "SparePartID");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "JobSpareParts");
        }
    }
}
