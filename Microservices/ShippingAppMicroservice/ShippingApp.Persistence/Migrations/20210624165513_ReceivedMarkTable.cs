using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class ReceivedMarkTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReceivedMarkMovements",
                table: "ReceivedMarkMovements");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReceivedMarkMovements",
                table: "ReceivedMarkMovements",
                columns: new[] { "ReceivedMarkId", "MovementRequestId", "ProductId", "WorkOrderId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReceivedMarkMovements",
                table: "ReceivedMarkMovements");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReceivedMarkMovements",
                table: "ReceivedMarkMovements",
                columns: new[] { "ReceivedMarkId", "MovementRequestId", "ProductId" });
        }
    }
}
