using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateColumnShipping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartRevision",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessRevision",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountNumber",
                table: "ShippingPlans",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductLine",
                table: "ShippingPlans",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "PartRevision",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ProcessRevision",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ProductLine",
                table: "ShippingPlans");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "ShippingRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillTo",
                table: "ShippingRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillToAddress",
                table: "ShippingRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipTo",
                table: "ShippingRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipToAddress",
                table: "ShippingRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "ShippingPlans",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
