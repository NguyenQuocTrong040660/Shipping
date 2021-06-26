using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateWorkOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkOrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "WorkOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkOrderId",
                table: "ReceivedMarkPrintings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ProductId",
                table: "WorkOrders",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_Products_ProductId",
                table: "WorkOrders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_Products_ProductId",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_ProductId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "WorkOrderId",
                table: "ReceivedMarkPrintings");

            migrationBuilder.CreateTable(
                name: "WorkOrderDetails",
                columns: table => new
                {
                    WorkOrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderDetails", x => new { x.WorkOrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_WorkOrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkOrderDetails_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderDetails_ProductId",
                table: "WorkOrderDetails",
                column: "ProductId");
        }
    }
}
