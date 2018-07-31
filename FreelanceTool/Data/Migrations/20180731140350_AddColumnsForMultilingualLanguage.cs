using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FreelanceTool.Data.Migrations
{
    public partial class AddColumnsForMultilingualLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Language");

            migrationBuilder.AddColumn<string>(
                name: "Alpha2",
                table: "Language",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEnglish",
                table: "Language",
                maxLength: 45,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameFrench",
                table: "Language",
                maxLength: 45,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameGerman",
                table: "Language",
                maxLength: 45,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UniqueName",
                table: "ApplicantFile",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalName",
                table: "ApplicantFile",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "ApplicantFile",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alpha2",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "NameEnglish",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "NameFrench",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "NameGerman",
                table: "Language");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Language",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UniqueName",
                table: "ApplicantFile",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalName",
                table: "ApplicantFile",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "ApplicantFile",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 5);
        }
    }
}
