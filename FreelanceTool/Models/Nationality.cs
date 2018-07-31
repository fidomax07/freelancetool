using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace FreelanceTool.Models
{
	public class Nationality
	{
		// Properties
		public int Id { get; set; }

		[MaxLength(3)]
		public int Code { get; set; }

		[Required, StringLength(2)]
		public string Alpha2 { get; set; }

		[Required, StringLength(3)]
		public string Alpha3 { get; set; }

		[Required, StringLength(45)]
		public string NameEnglish { get; set; }

		[StringLength(45)]
		public string NameGerman { get; set; }

		[Required, StringLength(45)]
		public string NameFrench { get; set; }


		// Relationships and Navigation Properties
		public ICollection<Applicant> Applicants { get; set; }



		// Interface
		public string GetLocalizedName(CultureInfo culture)
		{
			switch (culture.EnglishName)
			{
				case "English":
					return NameEnglish;
				case "German":
					return NameGerman;
				case "French":
					return NameFrench;
				default:
					return NameEnglish;
			}
		}

		public static string GetLocalizedColumnName(CultureInfo culture)
		{
			return $"Name{culture.EnglishName}";
		}
	}
}