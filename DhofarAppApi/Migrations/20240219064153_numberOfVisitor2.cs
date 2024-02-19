using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DhofarAppApi.Migrations
{
    /// <inheritdoc />
    public partial class numberOfVisitor2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfVisitors",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfVisitors",
                table: "AspNetUsers");
            migrationBuilder.DropColumn(
               name: "AdminNotes",
               table: "Complaints");
        }
    }
}
