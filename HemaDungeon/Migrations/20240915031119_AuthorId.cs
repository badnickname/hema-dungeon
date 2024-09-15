using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HemaDungeon.Migrations
{
    /// <inheritdoc />
    public partial class AuthorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "FightStates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "FightCharacters",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "FightStates");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "FightCharacters");
        }
    }
}
