namespace FreelanceTool.Models
{
	public class ApplicantLanguage
	{
		public int ApplicantId { get; set; }
		public int LanguageId { get; set; }

		// Relationships and Navigation Properties
		public Applicant Applicant { get; set; }
		public Language Language { get; set; }
	}
}