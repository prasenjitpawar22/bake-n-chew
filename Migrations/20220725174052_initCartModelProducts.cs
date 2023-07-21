using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakeNChew.Migrations
{
    /// <inheritdoc />
    public partial class initCartModelProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBase64",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageBase64",
                table: "Products",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "Carts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
