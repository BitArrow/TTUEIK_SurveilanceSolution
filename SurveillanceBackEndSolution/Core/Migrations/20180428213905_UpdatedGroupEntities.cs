using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Core.Migrations
{
    public partial class UpdatedGroupEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_UserId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Groups");

            migrationBuilder.AddColumn<long>(
                name: "GroupId",
                table: "GroupUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "GroupRoleId",
                table: "GroupUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Notify",
                table: "GroupUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "RoleId",
                table: "GroupUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "Track",
                table: "GroupUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "GroupUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OwnerId",
                table: "Groups",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_GroupId",
                table: "GroupUsers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_GroupRoleId",
                table: "GroupUsers",
                column: "GroupRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_UserId",
                table: "GroupUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_OwnerId",
                table: "Groups",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_Groups_GroupId",
                table: "GroupUsers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_GroupRoles_GroupRoleId",
                table: "GroupUsers",
                column: "GroupRoleId",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_Users_UserId",
                table: "GroupUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_OwnerId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_Groups_GroupId",
                table: "GroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_GroupRoles_GroupRoleId",
                table: "GroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_Users_UserId",
                table: "GroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_GroupUsers_GroupId",
                table: "GroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_GroupUsers_GroupRoleId",
                table: "GroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_GroupUsers_UserId",
                table: "GroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "GroupRoleId",
                table: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "Notify",
                table: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "Track",
                table: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Groups");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GroupUsers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Groups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
