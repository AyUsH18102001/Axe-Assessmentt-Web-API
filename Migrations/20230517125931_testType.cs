using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AxeAssessmentToolWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class testType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestTypeId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TestType",
                columns: table => new
                {
                    TestTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Test = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestType", x => x.TestTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TestTypeId",
                table: "Questions",
                column: "TestTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_TestType_TestTypeId",
                table: "Questions",
                column: "TestTypeId",
                principalTable: "TestType",
                principalColumn: "TestTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_TestType_TestTypeId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "TestType");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TestTypeId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TestTypeId",
                table: "Questions");
        }
    }
}
