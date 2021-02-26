using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class AddPurchaseOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PurchaseOrder",
                table: "ShippingRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseOrder",
                table: "ShippingPlans",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseOrder",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "PurchaseOrder",
                table: "ShippingPlans");
        }
    }
}
