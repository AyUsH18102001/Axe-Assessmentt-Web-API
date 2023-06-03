using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxeAssessmentToolWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class testperformance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User_OptionSelected",
                columns: table => new
                {
                    OptionSelctedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    OptionIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_OptionSelected", x => x.OptionSelctedId);
                    table.ForeignKey(
                        name: "FK_User_OptionSelected_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_QuestionAttempted",
                columns: table => new
                {
                    QuestionAttemptedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_QuestionAttempted", x => x.QuestionAttemptedId);
                    table.ForeignKey(
                        name: "FK_User_QuestionAttempted_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_OptionSelected_QuestionId",
                table: "User_OptionSelected",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_QuestionAttempted_UserId",
                table: "User_QuestionAttempted",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_OptionSelected");

            migrationBuilder.DropTable(
                name: "User_QuestionAttempted");
        }
    }
}
