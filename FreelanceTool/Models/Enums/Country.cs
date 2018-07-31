using System.ComponentModel.DataAnnotations;

namespace FreelanceTool.Models.Enums
{
	public enum Country
	{
		[Display(Name = "Switzerland")]
		Switzerland = 1,
		[Display(Name = "Germany")]
		Germany,
		[Display(Name = "France")]
		France,
		[Display(Name = "Austria")]
		Austria,
		[Display(Name = "Italy")]
		Italy,
		[Display(Name = "Liechtenstein")]
		Liechtenstein
	}
}