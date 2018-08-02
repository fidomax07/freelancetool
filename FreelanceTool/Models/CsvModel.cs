using System;
using System.ComponentModel.DataAnnotations;
using FreelanceTool.Models.Enums;

namespace FreelanceTool.Models
{
	public class CsvModel
	{
		[Display(Name = "Sex")]
		public string Sex { get; set; }

		[Display(Name = "Last name")]
		public string LastName { get; set; }

		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Display(Name = "Date of birth")]
		public DateTime DateOfBirth { get; set; }

		[Display(Name = "Main language")]
		public Language MainLanguage { get; set; }

		[Display(Name = "Country")]
		public Country Country { get; set; }

		public CsvModel()
		{
		}

		public CsvModel(Applicant applicant)
		{
			MapObject(applicant);
		}

		public CsvModel MapObject(object objectInstance)
		{
			var csvPropertiesNames = this.GetPropertiesNames();
			foreach (var propInfo in objectInstance.GetProperties())
			{
				// Filter out properties that are not part of CSV.
				if (!csvPropertiesNames.Contains(propInfo.Name))
					continue;

				var csvProperty = this.GetType().GetProperty(propInfo.Name);
				var csvPropType = csvProperty.PropertyType;

				// Filter out properties that don't match with the
				// corresponding property type of the CSV model.
				if (!csvPropType.IsAssignableFrom(propInfo.PropertyType))
					continue;

				// Filter out complex types (like naviagtional properties).
				if (csvPropType.IsClass && csvPropType != typeof(string))
					continue;

				csvProperty.SetValue(this, propInfo.GetValue(objectInstance));
			}

			return this;
		}
	}
}