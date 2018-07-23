using System.ComponentModel.DataAnnotations;
using FreelanceTool.Helpers.Enums;

namespace FreelanceTool.Models
{
	public class ApplicantFile
	{
		public int Id { get; set; }
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