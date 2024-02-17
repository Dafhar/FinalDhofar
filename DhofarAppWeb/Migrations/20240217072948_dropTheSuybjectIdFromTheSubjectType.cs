using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DhofarAppWeb.Migrations
{
    /// <inheritdoc />
    public partial class dropTheSuybjectIdFromTheSubjectType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "SubjectTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "SubjectTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
