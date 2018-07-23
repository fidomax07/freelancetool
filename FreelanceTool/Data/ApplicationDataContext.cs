using System;
using FreelanceTool.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreelanceTool.Data
{
	public class ApplicationDataContext : IdentityDbContext<ApplicationUser>
	{
		// Static tables first
		public DbSet<Language> Languages { get; set; }
		public DbSet<Nationality> Nationalities { get; set; }

		// Add tables based on dependecies with each-other
		public DbSet<Applicant> Applicants { get; set; }
		public DbSet<ApplicantLanguage> ApplicantLanguages { get; set; }
		public DbSet<JSTrainingCertificate> JsTrainingCertificates { get; set; }
		public DbSet<ApplicantFile> ApplicantFiles { get; set; }



		// Lifecycle
		public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options)
			: base(options)
		{
		}



		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);


			builder.Entity<Language>().ToTable(nameof(Language));
			builder.Entity<Nationality>().ToTable(nameof(Nationality));
			builder.Entity<Applicant>().ToTable(nameof(Applicant));
			builder.Entity<ApplicantLanguage>().ToTable(nameof(ApplicantLanguage));
			builder.Entity<JSTrainingCertificate>().ToTable(nameof(JSTrainingCertificate));
			builder.Entity<ApplicantFile>().ToTable(nameof(ApplicantFile));


			// Keys and indexes
			builder.Entity<ApplicantLanguage>()
				.HasKey(al => new { al.ApplicantId, al.LanguageId });


			// Configuration of auto-generated columns
			builder.Entity<Language>()
				.Property(b => b.CreatedAt)
				.HasDefaultValueSql("getdate()");
			builder.Entity<Language>()
				.Property(b => b.UpdatedAt)
				.HasDefaultValueSql("getdate()");

			builder.Entity<Applicant>()
				.Property(b => b.CreatedAt)
				.HasDefaultValueSql("getdate()");
			builder.Entity<Applicant>()
				.Property(b => b.UpdatedAt)
				.HasDefaultValueSql("getdate()");

			builder.Entity<JSTrainingCertificate>()
				.Property(b => b.CreatedAt)
				.HasDefaultValueSql("getdate()");
			builder.Entity<JSTrainingCertificate>()
				.Property(b => b.UpdatedAt)
				.HasDefaultValueSql("getdate()");

			builder.Entity<ApplicantFile>()
				.Property(b => b.CreatedAt)
				.HasDefaultValueSql("getdate()");
			builder.Entity<ApplicantFile>()
				.Property(b => b.UpdatedAt)
				.HasDefaultValueSql("getdate()");
		}
	}
}
