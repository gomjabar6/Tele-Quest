using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CharacterBackend.Migrations
{
    public partial class AddRaids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Raid",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    XpLevel = table.Column<long>(nullable: false),
                    XpReward = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Success = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raid", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRaids",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    RaidId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRaids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRaids_Raid_RaidId",
                        column: x => x.RaidId,
                        principalTable: "Raid",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRaids_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRaids_RaidId",
                table: "UserRaids",
                column: "RaidId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRaids_UserId",
                table: "UserRaids",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRaids");

            migrationBuilder.DropTable(
                name: "Raid");
        }
    }
}
