using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateShippingRequestDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillTo",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "BillToAddress",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "ShipTo",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "ShipToAddress",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "ShippingDate",
                table: "ShippingRequestDetails");

            migrationBuilder.AddColumn<string>(
                name: "BillTo",
                table: "ShippingRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillToAddress",
                table: "ShippingRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
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

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingDate",
                table: "ShippingRequests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillTo",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "BillToAddress",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "ShipTo",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "ShipToAddress",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "ShippingDate",
                table: "ShippingRequests");

            migrationBuilder.AddColumn<string>(
                name: "BillTo",
                table: "ShippingRequestLogistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillToAddress",
                table: "ShippingRequestLogistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipTo",
                table: "ShippingRequestLogistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipToAddress",
                table: "ShippingRequestLogistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "ShippingRequestDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingDate",
                table: "ShippingRequestDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
