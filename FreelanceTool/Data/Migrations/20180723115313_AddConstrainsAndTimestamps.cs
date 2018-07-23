using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FreelanceTool.Data.Migrations
{
    public partial class AddConstrainsAndTimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Language",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Language",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "JSTrainingCertificate",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "JSTrainingCertificate",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ApplicantFile",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ApplicantFile",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Applicant",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Applicant",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "JSTrainingCertificate");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "JSTrainingCertificate");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ApplicantFile");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ApplicantFile");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Applicant");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Applicant");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
