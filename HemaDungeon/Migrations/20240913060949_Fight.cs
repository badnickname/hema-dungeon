using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HemaDungeon.Migrations
{
    /// <inheritdoc />
    public partial class Fight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FightCharacters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CharacterId = table.Column<string>(type: "text", nullable: true),
                    Health = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FightCharacters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FightCharacters_AspNetUsers_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FightStates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CharacterId = table.Column<string>(type: "text", nullable: false),
                    ScoreHealth = table.Column<int>(type: "integer", nullable: false),
                    Damage = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FightStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FightStates_FightCharacters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "FightCharacters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FightCharacters_CharacterId",
                table: "FightCharacters",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_FightStates_CharacterId",
                table: "FightStates",
                column: "CharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FightStates");

            migrationBuilder.DropTable(
                name: "FightCharacters");
        }
    }
}
