using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetMe.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatePetEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "IsNeutered",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "MicrochipId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "VaccinationStatus",
                table: "Pets");

            migrationBuilder.AlterColumn<int>(
                name: "Species",
                table: "Pets",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Pets",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Species",
                table: "Pets",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Pets",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Pets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Pets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsNeutered",
                table: "Pets",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MicrochipId",
                table: "Pets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VaccinationStatus",
                table: "Pets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
