using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeMonkeys.CMS.Public.Shared.Migrations
{
    /// <inheritdoc />
    public partial class accessmenuitems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItem_Menus_MenuId",
                table: "MenuItem");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItem_Pages_WebPageId",
                table: "MenuItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItem",
                table: "MenuItem");

            migrationBuilder.RenameTable(
                name: "MenuItem",
                newName: "MenuItems");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItem_WebPageId",
                table: "MenuItems",
                newName: "IX_MenuItems_WebPageId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItem_MenuId",
                table: "MenuItems",
                newName: "IX_MenuItems_MenuId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItems",
                table: "MenuItems",
                column: "MenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Menus_MenuId",
                table: "MenuItems",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "MenuId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Pages_WebPageId",
                table: "MenuItems",
                column: "WebPageId",
                principalTable: "Pages",
                principalColumn: "WebPageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Menus_MenuId",
                table: "MenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Pages_WebPageId",
                table: "MenuItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItems",
                table: "MenuItems");

            migrationBuilder.RenameTable(
                name: "MenuItems",
                newName: "MenuItem");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItems_WebPageId",
                table: "MenuItem",
                newName: "IX_MenuItem_WebPageId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItems_MenuId",
                table: "MenuItem",
                newName: "IX_MenuItem_MenuId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItem",
                table: "MenuItem",
                column: "MenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItem_Menus_MenuId",
                table: "MenuItem",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "MenuId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItem_Pages_WebPageId",
                table: "MenuItem",
                column: "WebPageId",
                principalTable: "Pages",
                principalColumn: "WebPageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
