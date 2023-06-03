using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxeAssessmentToolWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class questions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionType_QuestionTypeId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_TestType_TestTypeId",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "TestTypeId",
                table: "Questions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionTypeId",
                table: "Questions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsSQL",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionType_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId",
                principalTable: "QuestionType",
                principalColumn: "QuestionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_TestType_TestTypeId",
                table: "Questions",
                column: "TestTypeId",
                principalTable: "TestType",
                principalColumn: "TestTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionType_QuestionTypeId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_TestType_TestTypeId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsSQL",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "TestTypeId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionTypeId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionType_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId",
                principalTable: "QuestionType",
                principalColumn: "QuestionTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_TestType_TestTypeId",
                table: "Questions",
                column: "TestTypeId",
                principalTable: "TestType",
                principalColumn: "TestTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
