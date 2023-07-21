using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakeNChew.Migrations
{
    /// <inheritdoc />
    public partial class OrderTableCartIdInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Carts_CartId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CartId1",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "CartId1",
                table: "Orders",
                newName: "CartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "Orders",
                newName: "CartId1");

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
    }
}
