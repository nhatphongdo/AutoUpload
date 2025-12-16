using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoUpload.WebService.Migrations
{
    public partial class Add_Index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licenses_Users_UserId",
                table: "Licenses");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_LicenseKey",
                table: "Licenses",
                column: "LicenseKey",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Licenses_Users_UserId",
                table: "Licenses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licenses_Users_UserId",
                table: "Licenses");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Licenses_LicenseKey",
                table: "Licenses");

            migrationBuilder.AddForeignKey(
                name: "FK_Licenses_Users_UserId",
                table: "Licenses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
