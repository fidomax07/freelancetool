﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FreelanceTool.Helpers.Enums;

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
		public string Path { get; set; }
		[Required]
		public string OriginalName { get; set; }
		public string UniqueName { get; set; }
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