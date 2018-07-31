using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FreelanceTool.Models.Enums;

namespace FreelanceTool.Models
{
	public class ApplicantFile
	{
		public int Id { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public DateTime CreatedAt { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime UpdatedAt { get; set; }

		public ApplicantFileType Type { get; set; }
		[Required, StringLength(100)]
		public string OriginalName { get; set; }
		[Required, StringLength(100)]
		public string UniqueName { get; set; }
		[Required, StringLength(5)]
		public string Extension { get; set; }


		// Relationships and Navigation Properties
		public int ApplicantId { get; set; }
		public Applicant Applicant { get; set; }


		public ApplicantFile()
		{
		}

		public ApplicantFile(ApplicantFileType fileType)
		{
			Type = fileType;
		}
	}
}