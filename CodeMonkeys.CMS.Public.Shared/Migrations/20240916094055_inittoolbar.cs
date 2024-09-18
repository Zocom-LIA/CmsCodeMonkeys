using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeMonkeys.CMS.Public.Shared.Migrations
{
    /// <inheritdoc />
    public partial class inittoolbar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FontFamily",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FontSize",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FontFamily",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "FontSize",
                table: "Contents");
        }
    }
}
