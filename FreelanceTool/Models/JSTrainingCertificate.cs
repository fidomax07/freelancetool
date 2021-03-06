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
		[Display(Name = "Certificate name")]
		public string Name { get; set; }

		[Display(Name = "Certificate type")]
		public JsCertificateType? Type { get; set; }


		// Relationships and Navigation Properties
		public int ApplicantId { get; set; }
		public Applicant Applicant { get; set; }
	}
}