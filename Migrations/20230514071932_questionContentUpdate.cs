using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxeAssessmentToolWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class questionContentUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QuestionImage",
                table: "Questions",
                type: "NVARCHAR(700)",
                maxLength: 700,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QuestionImage",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(700)",
                oldMaxLength: 700,
                oldNullable: true);
        }
    }
}
