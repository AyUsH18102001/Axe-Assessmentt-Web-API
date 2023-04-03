using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxeAssessmentToolWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class userToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserToken",
                table: "Users",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserToken",
                table: "Users");
        }
    }
}
