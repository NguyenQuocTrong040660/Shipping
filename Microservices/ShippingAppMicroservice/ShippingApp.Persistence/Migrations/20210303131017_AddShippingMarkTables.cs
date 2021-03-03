using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class AddShippingMarkTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingMarks_Products_ProductId",
                table: "ShippingMarks");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingMarks_ShippingRequests_ShippingRequestId",
                table: "ShippingMarks");

            migrationBuilder.DropIndex(
                name: "IX_ShippingRequestLogistics_ShippingRequestId",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropIndex(
                name: "IX_ShippingMarks_ProductId",
                table: "ShippingMarks");

            migrationBuilder.DropIndex(
                name: "IX_ShippingMarks_ShippingRequestId",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "PrintCount",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "Revision",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "ShippingRequestId",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ShippingMarks");

            migrationBuilder.AddColumn<int>(
                name: "ShippingRequestLogisticId",
                table: "ShippingRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RefId",
                table: "ShippingPlans",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShippingMarkPrintings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Sequence = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true, defaultValue: "New"),
                    PrintCount = table.Column<int>(nullable: false, defaultValue: 0),
                    Revision = table.Column<string>(nullable: true),
                    RePrintingBy = table.Column<string>(nullable: true),
                    RePrintingDate = table.Column<DateTime>(nullable: true),
                    PrintingBy = table.Column<string>(nullable: true),
                    PrintingDate = table.Column<DateTime>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ShippingMarkId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingMarkPrintings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingMarkPrintings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShippingMarkPrintings_ShippingMarks_ShippingMarkId",
                        column: x => x.ShippingMarkId,
                        principalTable: "ShippingMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShippingMarkShippings",
                columns: table => new
                {
                    ShippingMarkId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ShippingRequestId = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingMarkShippings", x => new { x.ShippingMarkId, x.ShippingRequestId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ShippingMarkShippings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShippingMarkShippings_ShippingMarks_ShippingMarkId",
                        column: x => x.ShippingMarkId,
                        principalTable: "ShippingMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShippingMarkShippings_ShippingRequests_ShippingRequestId",
                        column: x => x.ShippingRequestId,
                        principalTable: "ShippingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShippingMarkSummaries",
                columns: table => new
                {
                    ShippingMarkId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    TotalQuantity = table.Column<int>(nullable: false),
                    TotalPackage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingMarkSummaries", x => new { x.ShippingMarkId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ShippingMarkSummaries_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShippingMarkSummaries_ShippingMarks_ShippingMarkId",
                        column: x => x.ShippingMarkId,
                        principalTable: "ShippingMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRequestLogistics_ShippingRequestId",
                table: "ShippingRequestLogistics",
                column: "ShippingRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingMarkPrintings_ProductId",
                table: "ShippingMarkPrintings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingMarkPrintings_ShippingMarkId",
                table: "ShippingMarkPrintings",
                column: "ShippingMarkId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingMarkShippings_ProductId",
                table: "ShippingMarkShippings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingMarkShippings_ShippingRequestId",
                table: "ShippingMarkShippings",
                column: "ShippingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingMarkSummaries_ProductId",
                table: "ShippingMarkSummaries",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippingMarkPrintings");

            migrationBuilder.DropTable(
                name: "ShippingMarkShippings");

            migrationBuilder.DropTable(
                name: "ShippingMarkSummaries");

            migrationBuilder.DropIndex(
                name: "IX_ShippingRequestLogistics_ShippingRequestId",
                table: "ShippingRequestLogistics");

            migrationBuilder.DropColumn(
                name: "ShippingRequestLogisticId",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "RefId",
                table: "ShippingPlans");

            migrationBuilder.AddColumn<int>(
                name: "PrintCount",
                table: "ShippingMarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ShippingMarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ShippingMarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Revision",
                table: "ShippingMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "ShippingMarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShippingRequestId",
                table: "ShippingMarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ShippingMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRequestLogistics_ShippingRequestId",
                table: "ShippingRequestLogistics",
                column: "ShippingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingMarks_ProductId",
                table: "ShippingMarks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingMarks_ShippingRequestId",
                table: "ShippingMarks",
                column: "ShippingRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingMarks_Products_ProductId",
                table: "ShippingMarks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingMarks_ShippingRequests_ShippingRequestId",
                table: "ShippingMarks",
                column: "ShippingRequestId",
                principalTable: "ShippingRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
