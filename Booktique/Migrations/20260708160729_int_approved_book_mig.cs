using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booktique.Migrations
{
    /// <inheritdoc />
    public partial class int_approved_book_mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsApproved",
                table: "book",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "book",
                type: "bit",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
