using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FreelanceTool.Models.Enums;

namespace FreelanceTool.Models
{
	public class JSTrainingCertificate
	{
		public int Id { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public DateTime CreatedAt { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime UpdatedAt { get; set; }

		[StringLength(80)]
		public string Name { get; set; }
		public JSCertificateType? Type { get; set; }


		// Relationships and Navigation Properties
		public int ApplicantId { get; set; }
		public Applicant Applicant { get; set; }
	}
}