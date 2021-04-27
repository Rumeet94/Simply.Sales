using Microsoft.EntityFrameworkCore.Migrations;

namespace Simply.Sales.DLL.Migrations
{
    public partial class Initinal4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DatePaided",
                table: "Orders",
                newName: "DateReceiving");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateReceiving",
                table: "Orders",
                newName: "DatePaided");
        }
    }
}
