using System.ComponentModel.DataAnnotations;

namespace FreelanceTool.Models.Enums
{
	public enum Sex
	{
		[Display(Name = "Male")]
		Male = 1,
		[Display(Name = "Female")]
		Female
	}
}