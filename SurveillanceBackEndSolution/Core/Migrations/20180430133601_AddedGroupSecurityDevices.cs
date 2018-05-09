using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Core.Migrations
{
    public partial class AddedGroupSecurityDevices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSecurityDevice_Groups_GroupId",
                table: "GroupSecurityDevice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupSecurityDevice",
                table: "GroupSecurityDevice");

            migrationBuilder.RenameTable(
                name: "GroupSecurityDevice",
                newName: "GroupSecurityDevices");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSecurityDevice_GroupId",
                table: "GroupSecurityDevices",
                newName: "IX_GroupSecurityDevices_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupSecurityDevices",
                table: "GroupSecurityDevices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSecurityDevices_Groups_GroupId",
                table: "GroupSecurityDevices",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSecurityDevices_Groups_GroupId",
                table: "GroupSecurityDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupSecurityDevices",
                table: "GroupSecurityDevices");

            migrationBuilder.RenameTable(
                name: "GroupSecurityDevices",
                newName: "GroupSecurityDevice");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSecurityDevices_GroupId",
                table: "GroupSecurityDevice",
                newName: "IX_GroupSecurityDevice_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupSecurityDevice",
                table: "GroupSecurityDevice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSecurityDevice_Groups_GroupId",
                table: "GroupSecurityDevice",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
