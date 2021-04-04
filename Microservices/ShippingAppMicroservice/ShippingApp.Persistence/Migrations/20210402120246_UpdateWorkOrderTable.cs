using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateWorkOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesID",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "SemlineNumber",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "SalesID",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "SemlineNumber",
                table: "ShippingPlans");

            migrationBuilder.AddColumn<string>(
                name: "SalelineNumber",
                table: "ShippingRequestDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesOrder",
                table: "ShippingRequestDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalelineNumber",
                table: "ShippingPlans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesOrder",
                table: "ShippingPlans",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalelineNumber",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "SalesOrder",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "SalelineNumber",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "SalesOrder",
                table: "ShippingPlans");

            migrationBuilder.AddColumn<string>(
                name: "SalesID",
                table: "ShippingRequestDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SemlineNumber",
                table: "ShippingRequestDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesID",
                table: "ShippingPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SemlineNumber",
                table: "ShippingPlans",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
