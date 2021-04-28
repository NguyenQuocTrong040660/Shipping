using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Descriptions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryCode = table.Column<string>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    CountryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryCode);
                });

            migrationBuilder.CreateTable(
                name: "MovementRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Prefix = table.Column<string>(nullable: true, defaultValue: "MMRQ"),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovementRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Prefix = table.Column<string>(nullable: true, defaultValue: "PROD"),
                    ProductNumber = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    QtyPerPackage = table.Column<int>(nullable: false),
                    PartRevisionRaw = table.Column<string>(nullable: true),
                    PartRevisionClean = table.Column<string>(nullable: true),
                    ProcessRevision = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReceivedMarks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Prefix = table.Column<string>(nullable: true, defaultValue: "REMARK"),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivedMarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingMarkHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefId = table.Column<string>(nullable: true),
                    ActionType = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingMarkHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingMarks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Prefix = table.Column<string>(nullable: true, defaultValue: "SHIPMARK"),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingMarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingPlans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Prefix = table.Column<string>(nullable: true, defaultValue: "SHIPPL"),
                    PurchaseOrder = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    ShippingDate = table.Column<DateTime>(nullable: false),
                    SalesOrder = table.Column<string>(nullable: true),
                    SalelineNumber = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    BillTo = table.Column<string>(nullable: true),
                    BillToAddress = table.Column<string>(nullable: true),
                    ShipTo = table.Column<string>(nullable: true),
                    ShipToAddress = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<int>(nullable: false),
                    ProductLine = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Prefix = table.Column<string>(nullable: true, defaultValue: "SHIPRQ"),
                    Notes = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true, defaultValue: "New"),
                    AccountNumber = table.Column<int>(nullable: false),
                    ShippingDate = table.Column<DateTime>(nullable: false),
                    CustomerName = table.Column<string>(nullable: true),
                    BillTo = table.Column<string>(nullable: true),
                    BillToAddress = table.Column<string>(nullable: true),
                    ShipTo = table.Column<string>(nullable: true),
                    ShipToAddress = table.Column<string>(nullable: true),
                    PickupDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Prefix = table.Column<string>(nullable: true, defaultValue: "WO"),
                    RefId = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    PartRevision = table.Column<string>(nullable: true),
                    ProcessRevision = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.Id);
                });

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
                    Prefix = table.Column<string>(nullable: true, defaultValue: "RECEIVEDMARK"),
                    Sequence = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true, defaultValue: "New"),
                    PrintCount = table.Column<int>(nullable: false, defaultValue: 0),
                    ParentId = table.Column<int>(nullable: false),
                    Revision = table.Column<string>(nullable: true),
                    RePrintingBy = table.Column<string>(nullable: true),
                    RePrintingDate = table.Column<DateTime>(nullable: true),
                    PrintingBy = table.Column<string>(nullable: true),
                    PrintingDate = table.Column<DateTime>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ReceivedMarkId = table.Column<int>(nullable: false),
                    ShippingMarkId = table.Column<int>(nullable: true),
                    MovementRequestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivedMarkPrintings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceivedMarkPrintings_MovementRequests_MovementRequestId",
                        column: x => x.MovementRequestId,
                        principalTable: "MovementRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.ForeignKey(
                        name: "FK_ReceivedMarkPrintings_ShippingMarks_ShippingMarkId",
                        column: x => x.ShippingMarkId,
                        principalTable: "ShippingMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShippingMarkPrintings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Prefix = table.Column<string>(nullable: true, defaultValue: "SHIPPINGMARK"),
                    Sequence = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true, defaultValue: "New"),
                    PrintCount = table.Column<int>(nullable: false, defaultValue: 0),
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
                name: "ShippingPlanDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    ShippingMode = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: false),
                    ShippingPlanId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
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
                name: "ShippingRequestDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    ShippingMode = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: false),
                    PurchaseOrder = table.Column<string>(nullable: true),
                    SalesOrder = table.Column<string>(nullable: true),
                    SalelineNumber = table.Column<string>(nullable: true),
                    ProductLine = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ShippingRequestId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ShippingRequestLogistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    GrossWeight = table.Column<float>(nullable: false),
                    CustomDeclarationNumber = table.Column<string>(nullable: true),
                    TrackingNumber = table.Column<string>(nullable: true),
                    TotalPackages = table.Column<int>(nullable: false),
                    Forwarder = table.Column<string>(nullable: true),
                    NetWeight = table.Column<float>(nullable: false),
                    Dimension = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ShippingRequestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingRequestLogistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingRequestLogistics_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShippingRequestLogistics_ShippingRequests_ShippingRequestId",
                        column: x => x.ShippingRequestId,
                        principalTable: "ShippingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovementRequestDetails",
                columns: table => new
                {
                    WorkOrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    MovementRequestId = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    IsDirect = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovementRequestDetails", x => new { x.WorkOrderId, x.MovementRequestId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_MovementRequestDetails_MovementRequests_MovementRequestId",
                        column: x => x.MovementRequestId,
                        principalTable: "MovementRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovementRequestDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovementRequestDetails_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderDetails",
                columns: table => new
                {
                    WorkOrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Quantity = table.Column<int>(nullable: false)
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
                name: "IX_MovementRequestDetails_MovementRequestId",
                table: "MovementRequestDetails",
                column: "MovementRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MovementRequestDetails_ProductId",
                table: "MovementRequestDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarkMovements_MovementRequestId",
                table: "ReceivedMarkMovements",
                column: "MovementRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarkMovements_ProductId",
                table: "ReceivedMarkMovements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarkPrintings_MovementRequestId",
                table: "ReceivedMarkPrintings",
                column: "MovementRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarkPrintings_ProductId",
                table: "ReceivedMarkPrintings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarkPrintings_ReceivedMarkId",
                table: "ReceivedMarkPrintings",
                column: "ReceivedMarkId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMarkPrintings_ShippingMarkId",
                table: "ReceivedMarkPrintings",
                column: "ShippingMarkId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRequestLogistics_ProductId",
                table: "ShippingRequestLogistics",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingRequestLogistics_ShippingRequestId",
                table: "ShippingRequestLogistics",
                column: "ShippingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderDetails_ProductId",
                table: "WorkOrderDetails",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "MovementRequestDetails");

            migrationBuilder.DropTable(
                name: "ReceivedMarkMovements");

            migrationBuilder.DropTable(
                name: "ReceivedMarkPrintings");

            migrationBuilder.DropTable(
                name: "ShippingMarkHistory");

            migrationBuilder.DropTable(
                name: "ShippingMarkPrintings");

            migrationBuilder.DropTable(
                name: "ShippingMarkShippings");

            migrationBuilder.DropTable(
                name: "ShippingPlanDetails");

            migrationBuilder.DropTable(
                name: "ShippingRequestDetails");

            migrationBuilder.DropTable(
                name: "ShippingRequestLogistics");

            migrationBuilder.DropTable(
                name: "WorkOrderDetails");

            migrationBuilder.DropTable(
                name: "MovementRequests");

            migrationBuilder.DropTable(
                name: "ReceivedMarks");

            migrationBuilder.DropTable(
                name: "ShippingMarks");

            migrationBuilder.DropTable(
                name: "ShippingPlans");

            migrationBuilder.DropTable(
                name: "ShippingRequests");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "WorkOrders");
        }
    }
}
