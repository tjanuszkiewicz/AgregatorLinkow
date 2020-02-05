﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace AgregatorLinkow.Data.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoteType",
                table: "Votes");

            migrationBuilder.AddColumn<bool>(
                name: "Voted",
                table: "Votes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "Links",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Links",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Voted",
                table: "Votes");

            migrationBuilder.AddColumn<bool>(
                name: "VoteType",
                table: "Votes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "Links",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Links",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
