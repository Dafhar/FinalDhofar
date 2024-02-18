using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DhofarAppWeb.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTypeSubject_SubjectTypes_SubjectTypeId",
                table: "SubjectTypeSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTypeSubject_Subjects_SubjectId",
                table: "SubjectTypeSubject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectTypeSubject",
                table: "SubjectTypeSubject");

            migrationBuilder.RenameTable(
                name: "SubjectTypeSubject",
                newName: "SubjectTypeSubjects");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectTypeSubject_SubjectTypeId",
                table: "SubjectTypeSubjects",
                newName: "IX_SubjectTypeSubjects_SubjectTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectTypeSubjects",
                table: "SubjectTypeSubjects",
                columns: new[] { "SubjectId", "SubjectTypeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTypeSubjects_SubjectTypes_SubjectTypeId",
                table: "SubjectTypeSubjects",
                column: "SubjectTypeId",
                principalTable: "SubjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTypeSubjects_Subjects_SubjectId",
                table: "SubjectTypeSubjects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTypeSubjects_SubjectTypes_SubjectTypeId",
                table: "SubjectTypeSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTypeSubjects_Subjects_SubjectId",
                table: "SubjectTypeSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubjectTypeSubjects",
                table: "SubjectTypeSubjects");

            migrationBuilder.RenameTable(
                name: "SubjectTypeSubjects",
                newName: "SubjectTypeSubject");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectTypeSubjects_SubjectTypeId",
                table: "SubjectTypeSubject",
                newName: "IX_SubjectTypeSubject_SubjectTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubjectTypeSubject",
                table: "SubjectTypeSubject",
                columns: new[] { "SubjectId", "SubjectTypeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTypeSubject_SubjectTypes_SubjectTypeId",
                table: "SubjectTypeSubject",
                column: "SubjectTypeId",
                principalTable: "SubjectTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTypeSubject_Subjects_SubjectId",
                table: "SubjectTypeSubject",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
