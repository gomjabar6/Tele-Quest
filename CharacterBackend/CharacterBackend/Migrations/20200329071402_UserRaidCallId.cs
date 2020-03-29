using Microsoft.EntityFrameworkCore.Migrations;

namespace CharacterBackend.Migrations
{
    public partial class UserRaidCallId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CallId",
                table: "UserRaids",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CallId",
                table: "UserRaids");
        }
    }
}
