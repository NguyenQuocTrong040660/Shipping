using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class AddColumnShippingPlanRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "ShippingRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillTo",
                table: "ShippingRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillToAddress",
                table: "ShippingRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipTo",
                table: "ShippingRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipToAddress",
                table: "ShippingRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "ShippingPlans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillTo",
                table: "ShippingPlans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillToAddress",
                table: "ShippingPlans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipTo",
                table: "ShippingPlans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipToAddress",
                table: "ShippingPlans",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "BillTo",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "BillToAddress",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "ShipTo",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "ShipToAddress",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "BillTo",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "BillToAddress",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "ShipTo",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "ShipToAddress",
                table: "ShippingPlans");
        }
    }
}
