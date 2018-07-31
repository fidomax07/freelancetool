using System.ComponentModel.DataAnnotations;

namespace FreelanceTool.Models.Enums
{
	public enum CivilStatus
	{
		[Display(Name = "Single")]
		Single = 1,
		[Display(Name = "Married")]
		Married,
		[Display(Name = "Separated")]
		Separated,
		[Display(Name = "Divorced")]
		Divorced,
		[Display(Name = "Widow")]
		Widow
	}
}