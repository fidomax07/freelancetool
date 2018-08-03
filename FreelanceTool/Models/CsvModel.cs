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
		public Language MainLanguage { get; set; }

		[Display(Name = "PhoneNumber")]
		public string PhoneNumber { get; set; }

		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required, StringLength(80)]
		[Display(Name = "Address")]
		public string Address { get; set; }

		[StringLength(80)]
		[Display(Name = "Address information")]
		public string AddressInformation { get; set; }

		[Display(Name = "Zip")]
		public string Zip { get; set; }

		[Display(Name = "City")]
		public string City { get; set; }

		[Display(Name = "Country")]
		public Country Country { get; set; }

		[Display(Name = "Swiss social security number")]
		public string SwissSocialSecurityNumber { get; set; }

		[Display(Name = "Civil status")]
		public CivilStatus CivilStatus { get; set; }

		[Display(Name = "Number of kids")]
		public int ChildrenCount { get; set; }

		[Display(Name = "Nationality")]
		public Nationality Nationality { get; set; }

		[Display(Name = "Residence permit")]
		public ResidencePermit? ResidencePermit { get; set; }

		[Display(Name = "Occupation")]
		public Occupation Occupation { get; set; }



		// Lifecycle
		public CsvModel()
		{
		}

		public CsvModel(Applicant applicant)
		{
			MapObject(applicant);
		}



		// Interface
		public CsvModel MapObject(object objectInstance)
		{
			var csvPropertiesNames = GetPropertiesNames(this);
			foreach (var propInfo in GetProperties(objectInstance))
			{
				// Filter out phone-prefix and phone, since they need to
				// be concatenated together and then displayed as one.
				if (propInfo.Name == nameof(Applicant.PhonePrefix) ||
					propInfo.Name == nameof(Applicant.PhoneNumber))
				{
					HandlePhoneConcatenation(objectInstance, propInfo);
					continue;
				}

				// Filter out properties that are not part of CSV.
				if (!csvPropertiesNames.Contains(propInfo.Name))
					continue;

				var objectPropType = propInfo.PropertyType;
				var csvProperty = GetType().GetProperty(propInfo.Name);
				var csvPropType = csvProperty.PropertyType;

				// Since the type of DateTime is struct, if falls through with
				// other structural types, but it cannot be simply assigned
				// as other properties, without first locally formmating.
				if (objectPropType == typeof(DateTime))
				{
					var propValue = propInfo.GetValue(objectInstance) as DateTime?;
					GetProperty(this, propInfo.Name)
						.SetValue(this, propValue?.ToStringLocale());
					continue;
				}

				// Filter out complex types (like naviagtional properties).
				//if (objectPropType.IsClass && objectPropType != typeof(string))
				//	continue;

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

			if (!GetPropertiesNames(this).Contains(propertyName))
				return this;

			var csvProperty = GetProperty(this, propertyName);
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
		private static PropertyInfo GetProperty(object objectInstance, string propertyName)
		{
			return objectInstance
				.GetType()
				.GetProperty(propertyName);
		}

		private static IEnumerable<PropertyInfo> GetProperties(object objectInstance)
		{
			return objectInstance
				.GetType()
				.GetProperties();
		}

		private static HashSet<string> GetPropertiesNames(object objectInstance)
		{
			return GetProperties(objectInstance)
				.Select(p => p.Name)
				.ToHashSet();
		}

		private void HandlePhoneConcatenation(object objectInstance, PropertyInfo propInfo)
		{
			if (propInfo.Name == nameof(Applicant.PhonePrefix))
			{
				var objectPhonePrefix = propInfo
					.GetValue(objectInstance)
					.ToString();
				PhoneNumber = string.IsNullOrWhiteSpace(PhoneNumber) ?
					objectPhonePrefix : $"{objectPhonePrefix}{PhoneNumber}";
			}
			else if (propInfo.Name == nameof(Applicant.PhoneNumber))
			{
				var objectPhoneNumber = propInfo
					.GetValue(objectInstance)
					.ToString();
				PhoneNumber = string.IsNullOrWhiteSpace(PhoneNumber) ?
					objectPhoneNumber : $"{PhoneNumber}{objectPhoneNumber}";
			}
		}
	}
}