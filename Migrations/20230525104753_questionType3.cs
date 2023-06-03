using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxeAssessmentToolWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class questionType3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionType_QuestionTypeId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuestionTypeId",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "short_name",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionType_short_name",
                table: "QuestionType",
                column: "short_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_short_name",
                table: "Questions",
                column: "short_name");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionType_short_name",
                table: "Questions",
                column: "short_name",
                principalTable: "QuestionType",
                principalColumn: "QuestionTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionType_short_name",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionType_short_name",
                table: "QuestionType");

            migrationBuilder.DropIndex(
                name: "IX_Questions_short_name",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "short_name",
                table: "Questions");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionType_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId",
                principalTable: "QuestionType",
                principalColumn: "QuestionTypeId");
        }
    }
}
