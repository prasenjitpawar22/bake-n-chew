using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakeNChew.Migrations
{
    /// <inheritdoc />
    public partial class initCartModelProductsEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Carts_CartModelCartId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CartModelCartId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CartModelCartId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductsProductId",
                table: "Carts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductsProductId",
                table: "Carts",
                column: "ProductsProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Products_ProductsProductId",
                table: "Carts",
                column: "ProductsProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Products_ProductsProductId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_ProductsProductId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "ProductsProductId",
                table: "Carts");

            migrationBuilder.AddColumn<int>(
                name: "CartModelCartId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CartModelCartId",
                table: "Products",
                column: "CartModelCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Carts_CartModelCartId",
                table: "Products",
                column: "CartModelCartId",
                principalTable: "Carts",
                principalColumn: "CartId");
        }
    }
}
