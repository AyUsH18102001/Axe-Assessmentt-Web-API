using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxeAssessmentToolWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class questionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "D_Date",
                table: "Questions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "D_Id",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "I_Date",
                table: "Questions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "I_Id",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "U_Date",
                table: "Questions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "U_Id",
                table: "Questions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "D_Date",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "D_Id",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "I_Date",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "I_Id",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "U_Date",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "U_Id",
                table: "Questions");
        }
    }
}
