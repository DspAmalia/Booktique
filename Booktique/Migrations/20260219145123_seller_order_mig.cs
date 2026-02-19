using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booktique.Migrations
{
    /// <inheritdoc />
    public partial class seller_order_mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "Order",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Order");
        }
    }
}
