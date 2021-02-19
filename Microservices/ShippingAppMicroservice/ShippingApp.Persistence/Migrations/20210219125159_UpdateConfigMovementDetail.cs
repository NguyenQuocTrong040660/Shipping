using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateConfigMovementDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "WorkOrderDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WorkOrderDetails");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ShippingRequestDetails");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ShippingPlanDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ShippingPlanDetails");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "MovementRequests");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MovementRequests");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "MovementRequestDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MovementRequestDetails");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Configs");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "MovementRequestDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Descriptions",
                table: "Configs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "MovementRequestDetails");

            migrationBuilder.DropColumn(
                name: "Descriptions",
                table: "Configs");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "WorkOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WorkOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "WorkOrderDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WorkOrderDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ShippingRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ShippingRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ShippingRequestLogistics",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ShippingRequestLogistics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ShippingRequestDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ShippingRequestDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ShippingPlans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ShippingPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ShippingPlanDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ShippingPlanDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ShippingMarks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ShippingMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ReceivedMarks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ReceivedMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "MovementRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MovementRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "MovementRequestDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MovementRequestDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Countries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Configs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Configs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
