using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PioneerLiganSTHLM.Migrations
{
    public partial class Placement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Placement",
                table: "EventResult",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Placement",
                table: "EventResult");
        }
    }
}
