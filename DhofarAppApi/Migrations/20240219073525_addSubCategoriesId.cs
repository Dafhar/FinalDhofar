using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DhofarAppApi.Migrations
{
    /// <inheritdoc />
    public partial class addSubCategoriesId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "subccategory",
                table: "Complaints",
                type: "int",
                nullable: true,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "subccategory",
                table: "Complaints");
        }
    }
}
