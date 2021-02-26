using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class AddPrefixTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "WorkOrders",
                nullable: true,
                defaultValue: "WO");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "ShippingRequests",
                nullable: true,
                defaultValue: "SHIPRQ");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ShippingRequestDetails",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "ShippingPlans",
                nullable: true,
                defaultValue: "SHIPPL");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "ShippingMarks",
                nullable: true,
                defaultValue: "SHIPMARK");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "ReceivedMarks",
                nullable: true,
                defaultValue: "REMARK");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "Products",
                nullable: true,
                defaultValue: "PROD");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "MovementRequests",
                nullable: true,
                defaultValue: "MMRQ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "ShippingRequests");

            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "ShippingPlans");

            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "ShippingMarks");

            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "ReceivedMarks");

            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "MovementRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Quantity",
                table: "ShippingRequestDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
