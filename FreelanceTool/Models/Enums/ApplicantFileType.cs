using System.ComponentModel.DataAnnotations;

namespace FreelanceTool.Models.Enums
{
	public enum ApplicantFileType
	{
		[Display(Name = "Profile picture")]
		ProfilePicture = 1,
		[Display(Name = "Official Freelance Statement")]
		OfficialFreelanceStatement,
		[Display(Name = "CSV-Import-File")]
		Csv
	}
}