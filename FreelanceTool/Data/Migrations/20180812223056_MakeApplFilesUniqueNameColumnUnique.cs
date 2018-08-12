using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FreelanceTool.Data.Migrations
{
    public partial class MakeApplFilesUniqueNameColumnUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ApplicantFile_UniqueName",
                table: "ApplicantFile",
                column: "UniqueName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicantFile_UniqueName",
                table: "ApplicantFile");
        }
    }
}
