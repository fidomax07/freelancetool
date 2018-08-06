using System.ComponentModel.DataAnnotations;

namespace FreelanceTool.Models.Enums
{
	public enum Country
	{
		[Display(Name = "Switzerland")]
		CH = 1,
		[Display(Name = "Germany")]
		DE,
		[Display(Name = "France")]
		FR,
		[Display(Name = "Austria")]
		AT,
		[Display(Name = "Italy")]
		IT,
		[Display(Name = "Liechtenstein")]
		LI
	}
}