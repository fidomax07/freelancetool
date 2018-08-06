using System.ComponentModel.DataAnnotations;

namespace FreelanceTool.Models.Enums
{
	public enum Occupation
	{
		[Display(Name = "Student")]
		ST = 1,
		[Display(Name = "Part time")]
		PT,
		[Display(Name = "Full time")]
		FT,
		[Display(Name = "Unemployed")]
		UE,
		[Display(Name = "Self employed")]
		SE,
		[Display(Name = "Housewife")]
		HW
	}
}