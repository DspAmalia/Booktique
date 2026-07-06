using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booktique.Migrations
{
    /// <inheritdoc />
    public partial class bookid_notif_mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Notification",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_BookId",
                table: "Notification",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_book_BookId",
                table: "Notification",
                column: "BookId",
                principalTable: "book",
                principalColumn: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_book_BookId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_BookId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Notification");
        }
    }
}
