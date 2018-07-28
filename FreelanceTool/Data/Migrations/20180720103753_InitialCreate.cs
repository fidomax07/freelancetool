using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FreelanceTool.Data.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Language",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Name = table.Column<string>(maxLength: 20, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Language", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Nationality",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Code = table.Column<int>(maxLength: 3, nullable: false),
					Alpha2 = table.Column<string>(maxLength: 2, nullable: false),
					Alpha3 = table.Column<string>(maxLength: 3, nullable: false),
					NameEnglish = table.Column<string>(maxLength: 45, nullable: false),
					NameGerman = table.Column<string>(maxLength: 45, nullable: true),
					NameFrench = table.Column<string>(maxLength: 45, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Nationality", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Applicant",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Sex = table.Column<string>(maxLength: 30, nullable: false),
					LastName = table.Column<string>(maxLength: 80, nullable: false),
					FirstName = table.Column<string>(maxLength: 80, nullable: false),
					DateOfBirth = table.Column<DateTime>(nullable: false),
					PhonePrefix = table.Column<string>(maxLength: 5, nullable: false),
					PhoneNumber = table.Column<string>(maxLength: 30, nullable: false),
					Email = table.Column<string>(maxLength: 80, nullable: false),
					Address = table.Column<string>(maxLength: 80, nullable: false),
					AddressInformation = table.Column<string>(maxLength: 80, nullable: true),
					Zip = table.Column<string>(maxLength: 4, nullable: false),
					City = table.Column<string>(maxLength: 40, nullable: false),
					Country = table.Column<int>(nullable: false),
					SwissSocialSecurityNumber = table.Column<string>(nullable: false),
					CivilStatus = table.Column<int>(maxLength: 20, nullable: false),
					ChildrenCount = table.Column<int>(nullable: false),
					ResidencePermit = table.Column<int>(nullable: false),
					Occupation = table.Column<int>(nullable: false),
					Employer = table.Column<string>(maxLength: 40, nullable: true),
					CompanyName = table.Column<string>(maxLength: 80, nullable: true),
					CompanyWebsite = table.Column<string>(maxLength: 80, nullable: true),
					TShirtSize = table.Column<int>(nullable: false),
					DriverLicenseB = table.Column<bool>(nullable: false),
					TrainingNumber = table.Column<string>(maxLength: 80, nullable: true),
					IbanNumber = table.Column<string>(maxLength: 34, nullable: false),
					CsvPath = table.Column<string>(nullable: true),
					LanguageId = table.Column<int>(nullable: false),
					NationalityId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Applicant", x => x.Id);
					table.ForeignKey(
						name: "FK_Applicant_Language_LanguageId",
						column: x => x.LanguageId,
						principalTable: "Language",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Applicant_Nationality_NationalityId",
						column: x => x.NationalityId,
						principalTable: "Nationality",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "ApplicantFile",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Type = table.Column<int>(nullable: false),
					Path = table.Column<string>(nullable: true),
					OriginalName = table.Column<string>(nullable: false),
					UniqueName = table.Column<string>(nullable: true),
					Extension = table.Column<string>(nullable: true),
					ApplicantId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApplicantFile", x => x.Id);
					table.ForeignKey(
						name: "FK_ApplicantFile_Applicant_ApplicantId",
						column: x => x.ApplicantId,
						principalTable: "Applicant",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ApplicantLanguage",
				columns: table => new
				{
					ApplicantId = table.Column<int>(nullable: false),
					LanguageId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApplicantLanguage", x => new { x.ApplicantId, x.LanguageId });
					table.ForeignKey(
						name: "FK_ApplicantLanguage_Applicant_ApplicantId",
						column: x => x.ApplicantId,
						principalTable: "Applicant",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_ApplicantLanguage_Language_LanguageId",
						column: x => x.LanguageId,
						principalTable: "Language",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "JSTrainingCertificate",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Name = table.Column<string>(maxLength: 80, nullable: true),
					Type = table.Column<int>(nullable: false),
					ApplicantId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_JSTrainingCertificate", x => x.Id);
					table.ForeignKey(
						name: "FK_JSTrainingCertificate_Applicant_ApplicantId",
						column: x => x.ApplicantId,
						principalTable: "Applicant",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Applicant_LanguageId",
				table: "Applicant",
				column: "LanguageId");

			migrationBuilder.CreateIndex(
				name: "IX_Applicant_NationalityId",
				table: "Applicant",
				column: "NationalityId");

			migrationBuilder.CreateIndex(
				name: "IX_ApplicantFile_ApplicantId",
				table: "ApplicantFile",
				column: "ApplicantId");

			migrationBuilder.CreateIndex(
				name: "IX_ApplicantLanguage_LanguageId",
				table: "ApplicantLanguage",
				column: "LanguageId");

			migrationBuilder.CreateIndex(
				name: "IX_JSTrainingCertificate_ApplicantId",
				table: "JSTrainingCertificate",
				column: "ApplicantId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ApplicantFile");

			migrationBuilder.DropTable(
				name: "ApplicantLanguage");

			migrationBuilder.DropTable(
				name: "JSTrainingCertificate");

			migrationBuilder.DropTable(
				name: "Applicant");

			migrationBuilder.DropTable(
				name: "Language");

			migrationBuilder.DropTable(
				name: "Nationality");
		}
	}
}
