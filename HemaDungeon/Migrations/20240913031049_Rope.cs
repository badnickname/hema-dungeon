using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HemaDungeon.Migrations
{
    /// <inheritdoc />
    public partial class Rope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Visit",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "RunTwenty",
                table: "AspNetUsers",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "Rope",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "Rope",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "RunTwenty",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
