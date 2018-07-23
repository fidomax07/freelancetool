using System.ComponentModel.DataAnnotations;
using FreelanceTool.Helpers.Enums;

namespace FreelanceTool.Models
{
	public class JSTrainingCertificate
	{
		public int Id { get; set; }
		[StringLength(80)]
		public string Name { get; set; }
		public JSCertificateType Type { get; set; }


		// Relationships and Navigation Properties
		public int ApplicantId { get; set; }
		public Applicant Applicant { get; set; }
	}
}