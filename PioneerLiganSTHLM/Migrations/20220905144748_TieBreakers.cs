using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PioneerLiganSTHLM.Migrations
{
    public partial class TieBreakers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "GW",
                table: "EventResult",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "OGW",
                table: "EventResult",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "OMW",
                table: "EventResult",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GW",
                table: "EventResult");

            migrationBuilder.DropColumn(
                name: "OGW",
                table: "EventResult");

            migrationBuilder.DropColumn(
                name: "OMW",
                table: "EventResult");
        }
    }
}
