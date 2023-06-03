using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxeAssessmentToolWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class testperformance2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_OptionSelected_Questions_QuestionId",
                table: "User_OptionSelected");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "User_OptionSelected",
                newName: "QuestionAttemptedId");

            migrationBuilder.RenameIndex(
                name: "IX_User_OptionSelected_QuestionId",
                table: "User_OptionSelected",
                newName: "IX_User_OptionSelected_QuestionAttemptedId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_OptionSelected_User_QuestionAttempted_QuestionAttemptedId",
                table: "User_OptionSelected",
                column: "QuestionAttemptedId",
                principalTable: "User_QuestionAttempted",
                principalColumn: "QuestionAttemptedId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_OptionSelected_User_QuestionAttempted_QuestionAttemptedId",
                table: "User_OptionSelected");

            migrationBuilder.RenameColumn(
                name: "QuestionAttemptedId",
                table: "User_OptionSelected",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_User_OptionSelected_QuestionAttemptedId",
                table: "User_OptionSelected",
                newName: "IX_User_OptionSelected_QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_OptionSelected_Questions_QuestionId",
                table: "User_OptionSelected",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
