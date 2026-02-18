using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booktique.Migrations
{
    /// <inheritdoc />
    public partial class antique_book_mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePosted",
                table: "book",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "book",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_book_SellerId",
                table: "book",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_book_user_SellerId",
                table: "book",
                column: "SellerId",
                principalTable: "user",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_book_user_SellerId",
                table: "book");

            migrationBuilder.DropIndex(
                name: "IX_book_SellerId",
                table: "book");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "book");

            migrationBuilder.DropColumn(
                name: "DatePosted",
                table: "book");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "book");
        }
    }
}
