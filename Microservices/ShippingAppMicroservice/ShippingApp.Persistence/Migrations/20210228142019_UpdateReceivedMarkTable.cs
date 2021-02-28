using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateReceivedMarkTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceivedMarks_MovementRequests_MovementRequestId",
                table: "ReceivedMarks");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceivedMarks_Products_ProductId",
                table: "ReceivedMarks");

            migrationBuilder.DropIndex(
                name: "IX_ReceivedMarks_MovementRequestId",
                table: "ReceivedMarks");

            migrationBuilder.DropIndex(
                name: "IX_ReceivedMarks_ProductId",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "LastPrePrint",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "LastPrePrintBy",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "LastPrePrint",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "LastPrePrintBy",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "MovementRequestId",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "PrintCount",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ReceivedMarks");

            migrationBuilder.CreateTable(
                name: "ReceivedMarkMovements",
                columns: table => new
                {
                    ReceivedMarkId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    MovementRequestId = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivedMarkMovements", x => new { x.ReceivedMarkId, x.MovementRequestId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ReceivedMarkMovements_MovementRequests_MovementRequestId",
                        column: x => x.MovementRequestId,
                        principalTable: "MovementRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceivedMarkMovements_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceivedMarkMovements_ReceivedMarks_ReceivedMarkId",
                        column: x => x.ReceivedMarkId,
                        principalTable: "ReceivedMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceivedMarkPrintings",
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
                    ParentId = table.Column<int>(nullable: false),
                    RePrintingBy = table.Column<string>(nullable: true),
                    RePrintingDate = table.Column<DateTime>(nullable: true),
                    PrintingBy = table.Column<string>(nullable: true),
                    PrintingDate = table.Column<DateTime>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ReceivedMarkId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivedMarkPrintings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceivedMarkPrintings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceivedMarkPrintings_ReceivedMarks_ReceivedMarkId",
                        column: x => x.ReceivedMarkId,
                        principalTable: "ReceivedMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceivedMarkSummaries",
                columns: table => new
                {
                    ReceivedMarkId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    TotalQuantity = table.Column<int>(nullable: false),
                    TotalPackage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivedMarkSummaries", x => new { x.ReceivedMarkId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ReceivedMarkSummaries_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceivedMarkSummaries_ReceivedMarks_ReceivedMarkId",
                        column: x => x.ReceivedMarkId,
                        principalTable: "ReceivedMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarkMovements_MovementRequestId",
                table: "ReceivedMarkMovements",
                column: "MovementRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarkMovements_ProductId",
                table: "ReceivedMarkMovements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarkPrintings_ProductId",
                table: "ReceivedMarkPrintings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarkPrintings_ReceivedMarkId",
                table: "ReceivedMarkPrintings",
                column: "ReceivedMarkId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarkSummaries_ProductId",
                table: "ReceivedMarkSummaries",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceivedMarkMovements");

            migrationBuilder.DropTable(
                name: "ReceivedMarkPrintings");

            migrationBuilder.DropTable(
                name: "ReceivedMarkSummaries");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPrePrint",
                table: "ShippingMarks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastPrePrintBy",
                table: "ShippingMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPrePrint",
                table: "ReceivedMarks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastPrePrintBy",
                table: "ReceivedMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovementRequestId",
                table: "ReceivedMarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "ReceivedMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrintCount",
                table: "ReceivedMarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ReceivedMarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ReceivedMarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "ReceivedMarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ReceivedMarks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarks_MovementRequestId",
                table: "ReceivedMarks",
                column: "MovementRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarks_ProductId",
                table: "ReceivedMarks",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceivedMarks_MovementRequests_MovementRequestId",
                table: "ReceivedMarks",
                column: "MovementRequestId",
                principalTable: "MovementRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceivedMarks_Products_ProductId",
                table: "ReceivedMarks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
