using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DhofarAppWeb.Migrations
{
    /// <inheritdoc />
    public partial class almost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SubjectTypes",
                newName: "Name_En");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SubCategories",
                newName: "Name_En");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DepartmentTypes",
                newName: "Name_En");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Complaints",
                newName: "Title_En");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Complaints",
                newName: "Title_Ar");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Complaints",
                newName: "Status_En");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "Name_En");

            migrationBuilder.AddColumn<string>(
                name: "Name_Ar",
                table: "SubjectTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name_Ar",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name_Ar",
                table: "DepartmentTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State_Ar",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State_En",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status_Ar",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name_Ar",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LogInDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SelectedLanguage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Sound",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name_Ar",
                table: "SubjectTypes");

            migrationBuilder.DropColumn(
                name: "Name_Ar",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "Name_Ar",
                table: "DepartmentTypes");

            migrationBuilder.DropColumn(
                name: "State_Ar",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "State_En",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Status_Ar",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Name_Ar",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LogInDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SelectedLanguage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Sound",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Name_En",
                table: "SubjectTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Name_En",
                table: "SubCategories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Name_En",
                table: "DepartmentTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title_En",
                table: "Complaints",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Title_Ar",
                table: "Complaints",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Status_En",
                table: "Complaints",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "Name_En",
                table: "Categories",
                newName: "Name");
        }
    }
}
