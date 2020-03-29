using Microsoft.EntityFrameworkCore.Migrations;

namespace CharacterBackend.Migrations
{
    public partial class RaidDamage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "damage",
                table: "UserRaids",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "damage",
                table: "UserRaids");
        }
    }
}
