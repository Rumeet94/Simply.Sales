using Microsoft.EntityFrameworkCore.Migrations;

namespace Simply.Sales.DLL.Migrations
{
    public partial class Initial_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_ProductParameters_ProductParameterId",
                table: "Baskets");

            migrationBuilder.AlterColumn<int>(
                name: "ProductParameterId",
                table: "Baskets",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_ProductParameters_ProductParameterId",
                table: "Baskets",
                column: "ProductParameterId",
                principalTable: "ProductParameters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_ProductParameters_ProductParameterId",
                table: "Baskets");

            migrationBuilder.AlterColumn<int>(
                name: "ProductParameterId",
                table: "Baskets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_ProductParameters_ProductParameterId",
                table: "Baskets",
                column: "ProductParameterId",
                principalTable: "ProductParameters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
