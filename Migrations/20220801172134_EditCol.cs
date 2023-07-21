using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakeNChew.Migrations
{
    /// <inheritdoc />
    public partial class EditCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Carts",
                newName: "CustomerEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerEmail",
                table: "Carts",
                newName: "CustomerId");
        }
    }
}
