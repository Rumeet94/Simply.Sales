using Microsoft.EntityFrameworkCore.Migrations;

namespace Simply.Sales.DLL.Migrations
{
    public partial class AddDeliveryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "OrderState",
                table: "Orders",
                newName: "DeliveryZoneId");

            migrationBuilder.CreateTable(
                name: "DeliveryCities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryCities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryZones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Discription = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<int>(type: "INTEGER", nullable: true),
                    MinPriceForFreeDelivery = table.Column<int>(type: "INTEGER", nullable: true),
                    CityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryZones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryZones_DeliveryCities_CityId",
                        column: x => x.CityId,
                        principalTable: "DeliveryCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryZoneId",
                table: "Orders",
                column: "DeliveryZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryZones_CityId",
                table: "DeliveryZones",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryZones_DeliveryZoneId",
                table: "Orders",
                column: "DeliveryZoneId",
                principalTable: "DeliveryZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryZones_DeliveryZoneId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "DeliveryZones");

            migrationBuilder.DropTable(
                name: "DeliveryCities");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryZoneId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DeliveryZoneId",
                table: "Orders",
                newName: "OrderState");

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
