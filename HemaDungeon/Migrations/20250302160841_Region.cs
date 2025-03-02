using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HemaDungeon.Migrations
{
    /// <inheritdoc />
    public partial class Region : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cataclysm_AspNetUsers_CharacterId",
                table: "Cataclysm");

            migrationBuilder.DropForeignKey(
                name: "FK_Page_AspNetUsers_CharacterId",
                table: "Page");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournament_AspNetUsers_CharacterId",
                table: "Tournament");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_AspNetUsers_CharacterId",
                table: "Visits");

            migrationBuilder.AlterColumn<string>(
                name: "CharacterId",
                table: "Visits",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CharacterId",
                table: "Tournament",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CharacterId",
                table: "Page",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CharacterId",
                table: "Cataclysm",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "RegionId",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RegionId",
                table: "AspNetUsers",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Regions_RegionId",
                table: "AspNetUsers",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cataclysm_AspNetUsers_CharacterId",
                table: "Cataclysm",
                column: "CharacterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Page_AspNetUsers_CharacterId",
                table: "Page",
                column: "CharacterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournament_AspNetUsers_CharacterId",
                table: "Tournament",
                column: "CharacterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_AspNetUsers_CharacterId",
                table: "Visits",
                column: "CharacterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Regions_RegionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Cataclysm_AspNetUsers_CharacterId",
                table: "Cataclysm");

            migrationBuilder.DropForeignKey(
                name: "FK_Page_AspNetUsers_CharacterId",
                table: "Page");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournament_AspNetUsers_CharacterId",
                table: "Tournament");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_AspNetUsers_CharacterId",
                table: "Visits");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RegionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "CharacterId",
                table: "Visits",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CharacterId",
                table: "Tournament",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CharacterId",
                table: "Page",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CharacterId",
                table: "Cataclysm",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cataclysm_AspNetUsers_CharacterId",
                table: "Cataclysm",
                column: "CharacterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Page_AspNetUsers_CharacterId",
                table: "Page",
                column: "CharacterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournament_AspNetUsers_CharacterId",
                table: "Tournament",
                column: "CharacterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_AspNetUsers_CharacterId",
                table: "Visits",
                column: "CharacterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
