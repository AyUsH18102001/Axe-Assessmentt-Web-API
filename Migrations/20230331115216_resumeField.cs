using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxeAssessmentToolWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class resumeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserResumeFileName",
                table: "Users",
                type: "nvarchar(150)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserResumeFileName",
                table: "Users");
        }
    }
}
