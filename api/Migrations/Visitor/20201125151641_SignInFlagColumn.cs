using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations.Visitor
{
    public partial class SignInFlagColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CheckOut",
                table: "Visitor",
                newName: "SignOut");

            migrationBuilder.RenameColumn(
                name: "CheckIn",
                table: "Visitor",
                newName: "SignIn");

            migrationBuilder.AddColumn<bool>(
                name: "SignInFlag",
                table: "Visitor",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignInFlag",
                table: "Visitor");

            migrationBuilder.RenameColumn(
                name: "SignOut",
                table: "Visitor",
                newName: "CheckOut");

            migrationBuilder.RenameColumn(
                name: "SignIn",
                table: "Visitor",
                newName: "CheckIn");
        }
    }
}
