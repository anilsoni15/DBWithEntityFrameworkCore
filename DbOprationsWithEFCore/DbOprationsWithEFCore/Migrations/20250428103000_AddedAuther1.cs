﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbOprationsWithEFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddedAuther1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_BookAuthorId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "BookAuthorId",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_BookAuthorId",
                table: "Books",
                column: "BookAuthorId",
                principalTable: "Authors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_BookAuthorId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "BookAuthorId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_BookAuthorId",
                table: "Books",
                column: "BookAuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
