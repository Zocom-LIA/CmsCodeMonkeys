using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeMonkeys.CMS.Public.Shared.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_AspNetUsers_UserId",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_Sites_Pages_WebPageId",
                table: "Sites");

            migrationBuilder.DropIndex(
                name: "IX_Pages_UserId",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Pages");

            migrationBuilder.RenameColumn(
                name: "WebPageId",
                table: "Sites",
                newName: "LandingPageId");

            migrationBuilder.RenameIndex(
                name: "IX_Sites_WebPageId",
                table: "Sites",
                newName: "IX_Sites_LandingPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_CreatorId",
                table: "Sites",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_AuthorId",
                table: "Pages",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_AuthorId",
                table: "Contents",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_AspNetUsers_AuthorId",
                table: "Contents",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_AspNetUsers_AuthorId",
                table: "Pages",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sites_AspNetUsers_CreatorId",
                table: "Sites",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sites_Pages_LandingPageId",
                table: "Sites",
                column: "LandingPageId",
                principalTable: "Pages",
                principalColumn: "WebPageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_AspNetUsers_AuthorId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Pages_AspNetUsers_AuthorId",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_Sites_AspNetUsers_CreatorId",
                table: "Sites");

            migrationBuilder.DropForeignKey(
                name: "FK_Sites_Pages_LandingPageId",
                table: "Sites");

            migrationBuilder.DropIndex(
                name: "IX_Sites_CreatorId",
                table: "Sites");

            migrationBuilder.DropIndex(
                name: "IX_Pages_AuthorId",
                table: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_Contents_AuthorId",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "LandingPageId",
                table: "Sites",
                newName: "WebPageId");

            migrationBuilder.RenameIndex(
                name: "IX_Sites_LandingPageId",
                table: "Sites",
                newName: "IX_Sites_WebPageId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Pages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_UserId",
                table: "Pages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_AspNetUsers_UserId",
                table: "Pages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sites_Pages_WebPageId",
                table: "Sites",
                column: "WebPageId",
                principalTable: "Pages",
                principalColumn: "WebPageId");
        }
    }
}
