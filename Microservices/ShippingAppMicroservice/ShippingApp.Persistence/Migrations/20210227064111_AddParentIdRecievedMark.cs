using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingApp.Persistence.Migrations
{
    public partial class AddParentIdRecievedMark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PrintCount",
                table: "ShippingMarks",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPrePrint",
                table: "ShippingMarks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastPrePrintBy",
                table: "ShippingMarks",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PrintCount",
                table: "ReceivedMarks",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPrePrint",
                table: "ReceivedMarks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastPrePrintBy",
                table: "ReceivedMarks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "ReceivedMarks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "ParentId",
                table: "ReceivedMarks");

            migrationBuilder.AlterColumn<int>(
                name: "PrintCount",
                table: "ShippingMarks",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PrintCount",
                table: "ReceivedMarks",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldDefaultValue: 0);
        }
    }
}
