using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Core.Migrations
{
    public partial class GroupUserFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_GroupRoles_GroupRoleId",
                table: "GroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_GroupUsers_GroupRoleId",
                table: "GroupUsers");

            migrationBuilder.DropColumn(
                name: "GroupRoleId",
                table: "GroupUsers");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_RoleId",
                table: "GroupUsers",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_GroupRoles_RoleId",
                table: "GroupUsers",
                column: "RoleId",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_GroupRoles_RoleId",
                table: "GroupUsers");

            migrationBuilder.DropIndex(
                name: "IX_GroupUsers_RoleId",
                table: "GroupUsers");

            migrationBuilder.AddColumn<long>(
                name: "GroupRoleId",
                table: "GroupUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_GroupRoleId",
                table: "GroupUsers",
                column: "GroupRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_GroupRoles_GroupRoleId",
                table: "GroupUsers",
                column: "GroupRoleId",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
