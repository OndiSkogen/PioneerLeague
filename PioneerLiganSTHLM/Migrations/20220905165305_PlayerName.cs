using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PioneerLiganSTHLM.Migrations
{
    public partial class PlayerName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlayerName",
                table: "EventResult",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerName",
                table: "EventResult");
        }
    }
}
