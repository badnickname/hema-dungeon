using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HemaDungeon.Migrations
{
    /// <inheritdoc />
    public partial class RegionFill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO \"Regions\" (\"Id\", \"Name\") VALUES ('NOVOSIBIRSK', 'Новосибирск')");
            migrationBuilder.Sql("UPDATE \"AspNetUsers\" SET \"RegionId\" = 'NOVOSIBIRSK'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
