using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateShippingProcess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "ProductLine",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "PurchaseOrder",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "SalesID",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "SemlineNumber",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "ShippingDate",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "BillToCustomer",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "ReceiverAddress",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "ReceiverCustomer",
                table: "ShippingRequestLogistics");

            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                table: "ShippingRequestDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "ShippingRequestDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductLine",
                table: "ShippingRequestDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseOrder",
                table: "ShippingRequestDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesID",
                table: "ShippingRequestDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SemlineNumber",
                table: "ShippingRequestDetails",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingDate",
                table: "ShippingRequestDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "ProductLine",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "PurchaseOrder",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "SalesID",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "SemlineNumber",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "ShippingDate",
                table: "ShippingRequestDetails");

            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                table: "ShippingRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "ShippingRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductLine",
                table: "ShippingRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseOrder",
                table: "ShippingRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesID",
                table: "ShippingRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SemlineNumber",
                table: "ShippingRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingDate",
                table: "ShippingRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "BillToCustomer",
                table: "ShippingRequestLogistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverAddress",
                table: "ShippingRequestLogistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverCustomer",
                table: "ShippingRequestLogistics",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
