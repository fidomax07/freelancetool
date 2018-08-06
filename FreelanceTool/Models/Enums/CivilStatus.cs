using System.ComponentModel.DataAnnotations;

namespace FreelanceTool.Models.Enums
{
	public enum CivilStatus
	{
		[Display(Name = "Single")]
		SN = 1,
		[Display(Name = "Married")]
		MR,
		[Display(Name = "Separated")]
		SP,
		[Display(Name = "Divorced")]
		DV,
		[Display(Name = "Widow")]
		WD
	}
}