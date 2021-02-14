using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class UpdateMoveRequestColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MovementRequestId",
                table: "WorkOrders",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MovementRequestId",
                table: "WorkOrders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
