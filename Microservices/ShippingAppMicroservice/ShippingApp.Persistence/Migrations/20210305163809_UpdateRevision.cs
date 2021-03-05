using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateRevision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Revision",
                table: "ShippingMarkPrintings");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "ShippingMarkPrintings",
                nullable: true,
                defaultValue: "SHIPPINGMARK");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "ReceivedMarkPrintings",
                nullable: true,
                defaultValue: "RECEIVEDMARK");

            migrationBuilder.AddColumn<string>(
                name: "Revision",
                table: "ReceivedMarkPrintings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "ShippingMarkPrintings");

            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "ReceivedMarkPrintings");

            migrationBuilder.DropColumn(
                name: "Revision",
                table: "ReceivedMarkPrintings");

            migrationBuilder.AddColumn<string>(
                name: "Revision",
                table: "ShippingMarkPrintings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
