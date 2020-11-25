using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations.Visitor
{
    public partial class SignOutFlagColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SignOutFlag",
                table: "Visitor",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignOutFlag",
                table: "Visitor");
        }
    }
}
