﻿// <auto-generated />
using FreelanceTool.Data;
using FreelanceTool.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace FreelanceTool.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180812223056_MakeApplFilesUniqueNameColumnUnique")]
    partial class MakeApplFilesUniqueNameColumnUnique
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FreelanceTool.Models.Applicant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<string>("AddressInformation")
                        .HasMaxLength(80);

                    b.Property<int>("ChildrenCount");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<int>("CivilStatus");

                    b.Property<string>("CompanyName")
                        .HasMaxLength(80);

                    b.Property<string>("CompanyWebsite")
                        .HasMaxLength(80);

                    b.Property<int>("Country");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<bool>("DriverLicenseB");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<string>("Employer")
                        .HasMaxLength(40);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<string>("IbanNumber")
                        .IsRequired()
                        .HasMaxLength(34);

                    b.Property<int>("LanguageId");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<int>("NationalityId");

                    b.Property<int>("Occupation");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("PhonePrefix")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<int?>("ResidencePermit");

                    b.Property<int>("Sex");

                    b.Property<string>("SwissSocialSecurityNumber")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int>("TShirtSize");

                    b.Property<string>("TrainingNumber")
                        .HasMaxLength(80);

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasMaxLength(4);

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.HasIndex("NationalityId");

                    b.ToTable("Applicant");
                });

            modelBuilder.Entity("FreelanceTool.Models.ApplicantFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ApplicantId");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<long>("Length");

                    b.Property<string>("OriginalName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("Type");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.HasIndex("ApplicantId");

                    b.HasIndex("UniqueName")
                        .IsUnique();

                    b.ToTable("ApplicantFile");
                });

            modelBuilder.Entity("FreelanceTool.Models.ApplicantLanguage", b =>
                {
                    b.Property<int>("ApplicantId");

                    b.Property<int>("LanguageId");

                    b.HasKey("ApplicantId", "LanguageId");

                    b.HasIndex("LanguageId");

                    b.ToTable("ApplicantLanguage");
                });

            modelBuilder.Entity("FreelanceTool.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("FreelanceTool.Models.JSTrainingCertificate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ApplicantId");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Name")
                        .HasMaxLength(80);

                    b.Property<int?>("Type");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.HasIndex("ApplicantId");

                    b.ToTable("JSTrainingCertificate");
                });

            modelBuilder.Entity("FreelanceTool.Models.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alpha1")
                        .IsRequired()
                        .HasMaxLength(1);

                    b.Property<string>("Alpha2")
                        .IsRequired()
                        .HasMaxLength(2);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("NameEnglish")
                        .IsRequired()
                        .HasMaxLength(45);

                    b.Property<string>("NameFrench")
                        .IsRequired()
                        .HasMaxLength(45);

                    b.Property<string>("NameGerman")
                        .IsRequired()
                        .HasMaxLength(45);

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.ToTable("Language");
                });

            modelBuilder.Entity("FreelanceTool.Models.Nationality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alpha2")
                        .IsRequired()
                        .HasMaxLength(2);

                    b.Property<string>("Alpha3")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<int>("Code")
                        .HasMaxLength(3);

                    b.Property<string>("NameEnglish")
                        .IsRequired()
                        .HasMaxLength(45);

                    b.Property<string>("NameFrench")
                        .IsRequired()
                        .HasMaxLength(45);

                    b.Property<string>("NameGerman")
                        .HasMaxLength(45);

                    b.HasKey("Id");

                    b.ToTable("Nationality");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("FreelanceTool.Models.Applicant", b =>
                {
                    b.HasOne("FreelanceTool.Models.Language", "MainLanguage")
                        .WithMany("Applicants")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FreelanceTool.Models.Nationality", "Nationality")
                        .WithMany("Applicants")
                        .HasForeignKey("NationalityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FreelanceTool.Models.ApplicantFile", b =>
                {
                    b.HasOne("FreelanceTool.Models.Applicant", "Applicant")
                        .WithMany("ApplicantFiles")
                        .HasForeignKey("ApplicantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FreelanceTool.Models.ApplicantLanguage", b =>
                {
                    b.HasOne("FreelanceTool.Models.Applicant", "Applicant")
                        .WithMany("SpokenLanguages")
                        .HasForeignKey("ApplicantId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FreelanceTool.Models.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FreelanceTool.Models.JSTrainingCertificate", b =>
                {
                    b.HasOne("FreelanceTool.Models.Applicant", "Applicant")
                        .WithMany("JsTrainingCertificates")
                        .HasForeignKey("ApplicantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FreelanceTool.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FreelanceTool.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FreelanceTool.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FreelanceTool.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
