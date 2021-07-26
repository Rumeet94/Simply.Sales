using Microsoft.EntityFrameworkCore.Migrations;

namespace Simply.Sales.DLL.Migrations
{
    public partial class UpdateDelivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientToDeliveryZone",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeliveryZoneId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientToDeliveryZone", x => new { x.ClientId, x.DeliveryZoneId });
                    table.ForeignKey(
                        name: "FK_ClientToDeliveryZone_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientToDeliveryZone_DeliveryZones_DeliveryZoneId",
                        column: x => x.DeliveryZoneId,
                        principalTable: "DeliveryZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientToDeliveryZone_DeliveryZoneId",
                table: "ClientToDeliveryZone",
                column: "DeliveryZoneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientToDeliveryZone");
        }
    }
}
