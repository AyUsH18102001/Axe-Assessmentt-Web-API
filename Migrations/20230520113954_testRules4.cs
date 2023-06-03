using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxeAssessmentToolWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class testRules4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RuleId",
                table: "TestRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
