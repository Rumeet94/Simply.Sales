using Microsoft.EntityFrameworkCore.Migrations;

namespace Simply.Sales.DLL.Migrations
{
    public partial class UpdateDelivery3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryZones_DeliveryZoneId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryZoneId",
                table: "Orders",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryZones_DeliveryZoneId",
                table: "Orders",
                column: "DeliveryZoneId",
                principalTable: "DeliveryZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryZones_DeliveryZoneId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryZoneId",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryZones_DeliveryZoneId",
                table: "Orders",
                column: "DeliveryZoneId",
                principalTable: "DeliveryZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
