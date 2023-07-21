using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakeNChew.Migrations
{
    /// <inheritdoc />
    public partial class OrderTableCartId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartId1",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CartId1",
                table: "Orders",
                column: "CartId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Carts_CartId1",
                table: "Orders",
                column: "CartId1",
                principalTable: "Carts",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Carts_CartId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CartId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CartId1",
                table: "Orders");
        }
    }
}
