using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalDern_Support.Migrations
{
    /// <inheritdoc />
    public partial class changetotaltimedatatype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
               name: "TotalTime",
               table: "Reports",
               type: "datetime2",
               nullable: false,
               oldClrType: typeof(TimeSpan),
               oldType: "time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                 name: "TotalTime",
                 table: "Reports",
                 type: "time",
                 nullable: false,
                 oldClrType: typeof(DateTime),
                 oldType: "datetime2");
        }
    }
}
