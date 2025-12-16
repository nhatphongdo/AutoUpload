using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoUpload.WebService.Migrations
{
    public partial class AddUsertoMockupTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "MockupTemplates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MockupTemplates_UserId",
                table: "MockupTemplates",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MockupTemplates_Users_UserId",
                table: "MockupTemplates",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MockupTemplates_Users_UserId",
                table: "MockupTemplates");

            migrationBuilder.DropIndex(
                name: "IX_MockupTemplates_UserId",
                table: "MockupTemplates");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MockupTemplates");
        }
    }
}
