using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HemaDungeon.Migrations
{
    /// <inheritdoc />
    public partial class Reborn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDead",
                table: "AspNetUsers",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeadCharacters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    Story = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false),
                    PushUp = table.Column<int>(type: "integer", nullable: false),
                    PullUp = table.Column<int>(type: "integer", nullable: false),
                    Abdominal = table.Column<int>(type: "integer", nullable: false),
                    RunTwenty = table.Column<float>(type: "real", nullable: false),
                    RunFifteen = table.Column<float>(type: "real", nullable: false),
                    Rang = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    Rope = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadCharacters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeadTournament",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DeadCharacterId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadTournament", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadTournament_DeadCharacters_DeadCharacterId",
                        column: x => x.DeadCharacterId,
                        principalTable: "DeadCharacters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeadVisit",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DeadCharacterId = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CanSkip = table.Column<bool>(type: "boolean", nullable: false),
                    WasHere = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadVisit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadVisit_DeadCharacters_DeadCharacterId",
                        column: x => x.DeadCharacterId,
                        principalTable: "DeadCharacters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeadTournament_DeadCharacterId",
                table: "DeadTournament",
                column: "DeadCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_DeadVisit_DeadCharacterId",
                table: "DeadVisit",
                column: "DeadCharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeadTournament");

            migrationBuilder.DropTable(
                name: "DeadVisit");

            migrationBuilder.DropTable(
                name: "DeadCharacters");

            migrationBuilder.DropColumn(
                name: "IsDead",
                table: "AspNetUsers");
        }
    }
}
