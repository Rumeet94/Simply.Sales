using Microsoft.EntityFrameworkCore.Migrations;

namespace Simply.Sales.DLL.Migrations
{
    public partial class UpdateDelivery4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discription",
                table: "ClientsToDeliveryZones",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discription",
                table: "ClientsToDeliveryZones");
        }
    }
}
