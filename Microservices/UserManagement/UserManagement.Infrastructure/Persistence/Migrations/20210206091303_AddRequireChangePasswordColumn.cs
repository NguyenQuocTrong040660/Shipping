using Microsoft.EntityFrameworkCore.Migrations;

namespace UserManagement.Infrastructure.Migrations
{
    public partial class AddRequireChangePasswordColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequireChangePassword",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequireChangePassword",
                table: "AspNetUsers");
        }
    }
}
