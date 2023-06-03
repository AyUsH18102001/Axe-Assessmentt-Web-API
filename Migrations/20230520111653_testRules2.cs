using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxeAssessmentToolWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class testRules2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rule",
                table: "TestRules",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "RuleId",
                table: "TestRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TestCreatedDate",
                table: "Test",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TestRules_RuleId",
                table: "TestRules",
                column: "RuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestRules_Rules_RuleId",
                table: "TestRules",
                column: "RuleId",
                principalTable: "Rules",
                principalColumn: "RuleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestRules_Rules_RuleId",
                table: "TestRules");

            migrationBuilder.DropIndex(
                name: "IX_TestRules_RuleId",
                table: "TestRules");

            migrationBuilder.DropColumn(
                name: "RuleId",
                table: "TestRules");

            migrationBuilder.DropColumn(
                name: "TestCreatedDate",
                table: "Test");

            migrationBuilder.AlterColumn<string>(
                name: "Rule",
                table: "TestRules",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
