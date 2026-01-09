using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booktique.Migrations
{
    /// <inheritdoc />
    public partial class AddIsSubscribedToUser_mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSubscribed",
                table: "user",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubscribed",
                table: "user");
        }
    }
}
