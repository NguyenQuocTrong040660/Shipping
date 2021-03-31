using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateAccountNumberColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "ShippingRequestDetails");

            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                table: "ShippingRequests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "ShippingRequests");

            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                table: "ShippingRequestDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
