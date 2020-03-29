using Microsoft.EntityFrameworkCore.Migrations;

namespace CharacterBackend.Migrations
{
    public partial class UserSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionKey",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionKey",
                table: "Users");
        }
    }
}
