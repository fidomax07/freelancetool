using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using FreelanceTool.Models.Enums;

namespace FreelanceTool.Models
{
	public class CsvModel
	{
		[Display(Name = "Sex")]
		public Sex Sex { get; set; }

		[Display(Name = "Last name")]
		public string LastName { get; set; }

		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Display(Name = "Date of birth")]
		public string DateOfBirth { get; set; }

		[Display(Name = "Main language")]
		public string MainLanguage { get; set; }

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
			var csvPropertiesNames = GetPropertiesNames();
			foreach (var propInfo in objectInstance.GetProperties())
			{
				// Filter out properties that are not part of CSV.
				if (!csvPropertiesNames.Contains(propInfo.Name))
					continue;

				var objectPropType = propInfo.PropertyType;
				var csvProperty = GetType().GetProperty(propInfo.Name);
				var csvPropType = csvProperty.PropertyType;

				// Filter out complex types (like naviagtional properties).
				if (objectPropType.IsClass && objectPropType != typeof(string))
					continue;

				// Since the type of DateTime is struct, if falls through with
				// other structural types, but it cannot be simply assigned
				// as other properties, without first locally formmating.
				if (objectPropType == typeof(DateTime))
				{
					var propValue = propInfo.GetValue(objectInstance) as DateTime?;
					GetType()
						.GetProperty(propInfo.Name)
						.SetValue(this, propValue?.ToStringLocale());
					continue;
				}

				// Filter out properties that don't match with the
				// corresponding property type of the CSV model.
				if (!csvPropType.IsAssignableFrom(objectPropType))
					continue;

				csvProperty.SetValue(this, propInfo.GetValue(objectInstance));
			}

			return this;
		}

		public CsvModel MapComplexProperty(
			object objectInstance,
			string propertyName, 
			string propertyOfPropertyName)
		{

			if (!GetPropertiesNames().Contains(propertyName))
				return this;

			var csvProperty = GetProperty(propertyName);
			var objectProperty = objectInstance
				.GetType()
				.GetProperty(propertyName)
				.GetValue(objectInstance);
			var objectPropertyOfProperty = objectProperty
				.GetType()
				.GetProperty(propertyOfPropertyName);

			var isAssignable = csvProperty
				.PropertyType
				.IsAssignableFrom(objectPropertyOfProperty.PropertyType);
			if (isAssignable)
			{
				var propertyOfPropertyValue = objectPropertyOfProperty
					.GetValue(objectProperty);
				csvProperty.SetValue(this, propertyOfPropertyValue);
			}

			return this;
		}


		// Private methods
		private PropertyInfo GetProperty(string propertyName)
		{
			return GetType().GetProperty(propertyName);
		}

		private IEnumerable<PropertyInfo> GetProperties()
		{
			return GetType().GetProperties();
		}

		private HashSet<string> GetPropertiesNames()
		{
			return GetProperties()
				.Select(p => p.Name)
				.ToHashSet();
		}
	}
}