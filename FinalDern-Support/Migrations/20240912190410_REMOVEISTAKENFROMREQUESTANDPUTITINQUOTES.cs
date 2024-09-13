using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalDern_Support.Migrations
{
    /// <inheritdoc />
    public partial class REMOVEISTAKENFROMREQUESTANDPUTITINQUOTES : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTaken",
                table: "Requests");

            // Add IsTaken column to Quotes table
            migrationBuilder.AddColumn<bool>(
                name: "IsTaken",
                table: "Quotes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add IsTaken column back to Requests table
            migrationBuilder.AddColumn<bool>(
                name: "IsTaken",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Remove IsTaken column from Quotes table
            migrationBuilder.DropColumn(
                name: "IsTaken",
                table: "Quotes");
        }
    }
}