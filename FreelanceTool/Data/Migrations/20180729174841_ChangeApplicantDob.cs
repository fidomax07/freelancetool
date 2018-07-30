using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FreelanceTool.Data.Migrations
{
    public partial class ChangeApplicantDob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "ApplicantFile");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Applicant",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "ApplicantFile",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Applicant",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
