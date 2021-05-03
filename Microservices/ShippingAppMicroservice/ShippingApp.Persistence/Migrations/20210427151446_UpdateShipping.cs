using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateShipping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippingPlanDetails");

            migrationBuilder.DropTable(
                name: "ShippingRequestDetails");

            migrationBuilder.AddColumn<float>(
                name: "Amount",
                table: "ShippingPlans",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "ShippingPlans",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ShippingPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ShippingPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ShippingMode",
                table: "ShippingPlans",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShippingRequestId",
                table: "ShippingPlans",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingPlans_ProductId",
                table: "ShippingPlans",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingPlans_ShippingRequestId",
                table: "ShippingPlans",
                column: "ShippingRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingPlans_Products_ProductId",
                table: "ShippingPlans",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingPlans_ShippingRequests_ShippingRequestId",
                table: "ShippingPlans",
                column: "ShippingRequestId",
                principalTable: "ShippingRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingPlans_Products_ProductId",
                table: "ShippingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingPlans_ShippingRequests_ShippingRequestId",
                table: "ShippingPlans");

            migrationBuilder.DropIndex(
                name: "IX_ShippingPlans_ProductId",
                table: "ShippingPlans");

            migrationBuilder.DropIndex(
                name: "IX_ShippingPlans_ShippingRequestId",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "ShippingMode",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "ShippingRequestId",
                table: "ShippingPlans");

            migrationBuilder.CreateTable(
                name: "ShippingPlanDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ShippingMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingPlanDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingPlanDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShippingPlanDetails_ShippingPlans_ShippingPlanId",
                        column: x => x.ShippingPlanId,
                        principalTable: "ShippingPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShippingRequestDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductLine = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SalelineNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingRequestDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingRequestDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShippingRequestDetails_ShippingRequests_ShippingRequestId",
                        column: x => x.ShippingRequestId,
                        principalTable: "ShippingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShippingPlanDetails_ProductId",
                table: "ShippingPlanDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingPlanDetails_ShippingPlanId",
                table: "ShippingPlanDetails",
                column: "ShippingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRequestDetails_ProductId",
                table: "ShippingRequestDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRequestDetails_ShippingRequestId",
                table: "ShippingRequestDetails",
                column: "ShippingRequestId");
        }
    }
}
