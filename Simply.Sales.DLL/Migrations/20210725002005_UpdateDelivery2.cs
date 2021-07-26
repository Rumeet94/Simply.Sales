using Microsoft.EntityFrameworkCore.Migrations;

namespace Simply.Sales.DLL.Migrations
{
    public partial class UpdateDelivery2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientToDeliveryZone_Clients_ClientId",
                table: "ClientToDeliveryZone");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientToDeliveryZone_DeliveryZones_DeliveryZoneId",
                table: "ClientToDeliveryZone");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientToDeliveryZone",
                table: "ClientToDeliveryZone");

            migrationBuilder.RenameTable(
                name: "ClientToDeliveryZone",
                newName: "ClientsToDeliveryZones");

            migrationBuilder.RenameIndex(
                name: "IX_ClientToDeliveryZone_DeliveryZoneId",
                table: "ClientsToDeliveryZones",
                newName: "IX_ClientsToDeliveryZones_DeliveryZoneId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientsToDeliveryZones",
                table: "ClientsToDeliveryZones",
                columns: new[] { "ClientId", "DeliveryZoneId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsToDeliveryZones_Clients_ClientId",
                table: "ClientsToDeliveryZones",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsToDeliveryZones_DeliveryZones_DeliveryZoneId",
                table: "ClientsToDeliveryZones",
                column: "DeliveryZoneId",
                principalTable: "DeliveryZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientsToDeliveryZones_Clients_ClientId",
                table: "ClientsToDeliveryZones");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientsToDeliveryZones_DeliveryZones_DeliveryZoneId",
                table: "ClientsToDeliveryZones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientsToDeliveryZones",
                table: "ClientsToDeliveryZones");

            migrationBuilder.RenameTable(
                name: "ClientsToDeliveryZones",
                newName: "ClientToDeliveryZone");

            migrationBuilder.RenameIndex(
                name: "IX_ClientsToDeliveryZones_DeliveryZoneId",
                table: "ClientToDeliveryZone",
                newName: "IX_ClientToDeliveryZone_DeliveryZoneId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientToDeliveryZone",
                table: "ClientToDeliveryZone",
                columns: new[] { "ClientId", "DeliveryZoneId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClientToDeliveryZone_Clients_ClientId",
                table: "ClientToDeliveryZone",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientToDeliveryZone_DeliveryZones_DeliveryZoneId",
                table: "ClientToDeliveryZone",
                column: "DeliveryZoneId",
                principalTable: "DeliveryZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
