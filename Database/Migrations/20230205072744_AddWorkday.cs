using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkday : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Works",
                newName: "Workload");

            migrationBuilder.AddColumn<int>(
                name: "WorkdayId",
                table: "Works",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Workdays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workdays", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Works_WorkdayId",
                table: "Works",
                column: "WorkdayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Workdays_WorkdayId",
                table: "Works",
                column: "WorkdayId",
                principalTable: "Workdays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_Workdays_WorkdayId",
                table: "Works");

            migrationBuilder.DropTable(
                name: "Workdays");

            migrationBuilder.DropIndex(
                name: "IX_Works_WorkdayId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "WorkdayId",
                table: "Works");

            migrationBuilder.RenameColumn(
                name: "Workload",
                table: "Works",
                newName: "Time");
        }
    }
}
