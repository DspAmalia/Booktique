using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booktique.Migrations
{
    /// <inheritdoc />
    public partial class preview_book_mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookPreviewPath",
                table: "book",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookPreviewPath",
                table: "book");
        }
    }
}
