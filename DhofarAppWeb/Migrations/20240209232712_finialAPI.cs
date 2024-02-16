using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DhofarAppWeb.Migrations
{
    /// <inheritdoc />
    public partial class finialAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleValue",
                table: "SubjectTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "MySpecialist",
                table: "Complaints",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnBoardScreens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WelcomeMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnBoardScreens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserColors",
                columns: table => new
                {
                    ColorsId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserColors", x => new { x.UserId, x.ColorsId });
                    table.ForeignKey(
                        name: "FK_UserColors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserColors_Colors_ColorsId",
                        column: x => x.ColorsId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Red" },
                    { 2, "Orange" },
                    { 3, "Blue" },
                    { 4, "Green" }
                });

            migrationBuilder.InsertData(
                table: "OnBoardScreens",
                columns: new[] { "Id", "Description", "ImageUrl", "Title", "WelcomeMessage" },
                values: new object[,]
                {
                    { 1, "Description goes here  .... ", "image url", "Your title", "Your Welcome message goes here!" },
                    { 2, "Second Description goes here  .... ", "image url", "Your title", "Your second Welcome message goes here!" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserColors_ColorsId",
                table: "UserColors",
                column: "ColorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnBoardScreens");

            migrationBuilder.DropTable(
                name: "UserColors");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropColumn(
                name: "TitleValue",
                table: "SubjectTypes");

            migrationBuilder.DropColumn(
                name: "MySpecialist",
                table: "Complaints");
        }
    }
}
