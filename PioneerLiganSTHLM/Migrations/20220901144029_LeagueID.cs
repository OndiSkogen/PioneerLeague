using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PioneerLiganSTHLM.Migrations
{
    public partial class LeagueID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeagueID",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeagueID",
                table: "Event");
        }
    }
}
