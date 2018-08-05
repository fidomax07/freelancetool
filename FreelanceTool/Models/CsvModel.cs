using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using FreelanceTool.Helpers;
using FreelanceTool.Models.Enums;

namespace FreelanceTool.Models
{
	public class CsvModel
	{
		// Fileds
		private readonly AppLocalizer _localizer;
		private readonly StringBuilder _csvBuilder;

		// Binding properties:
		// Single choice properties
		[Display(Name = "Db Id")]
		public int DbId { get; private set; }

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

		[Display(Name = "Address")]
		public string Address { get; set; }

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

		[Display(Name = "Employer")]
		public string Employer { get; set; }

		[Display(Name = "Company name")]
		public string CompanyName { get; set; }

		[Display(Name = "Company website")]
		public string CompanyWebsite { get; set; }

		[Display(Name = "T shirt size")]
		public TShirtSize TShirtSize { get; set; }

		[Display(Name = "Driver license B")]
		public bool DriverLicenseB { get; set; }

		[Display(Name = "Training number")]
		public string TrainingNumber { get; set; }

		[Display(Name = "IBAN number")]
		public string IbanNumber { get; set; }

		// Binding properties
		// Multiple choice properties
		[Display(Name = "Spoken language German")]
		public bool SpokenLangugeGerman { get; set; }

		[Display(Name = "Spoken language French")]
		public bool SpokenLangugeFrench { get; set; }

		[Display(Name = "Spoken language Italian")]
		public bool SpokenLangugeItalian { get; set; }

		[Display(Name = "Js certificate name 1")]
		public string JsCertificateName1 { get; set; }

		[Display(Name = "Js certificate type 1")]
		public JsCertificateType? JsCertificateType1 { get; set; }

		[Display(Name = "Js certificate name 2")]
		public string JsCertificateName2 { get; set; }

		[Display(Name = "Js certificate type 2")]
		public JsCertificateType? JsCertificateType2 { get; set; }

		[Display(Name = "Js certificate name 3")]
		public string JsCertificateName3 { get; set; }

		[Display(Name = "Js certificate type 3")]
		public JsCertificateType? JsCertificateType3 { get; set; }

		[Display(Name = "Js certificate name 4")]
		public string JsCertificateName4 { get; set; }

		[Display(Name = "Js certificate type 4")]
		public JsCertificateType? JsCertificateType4 { get; set; }

		[Display(Name = "Js certificate name 5")]
		public string JsCertificateName5 { get; set; }

		[Display(Name = "Js certificate type 5")]
		public JsCertificateType? JsCertificateType5 { get; set; }



		// Lifecycle
		public CsvModel(AppLocalizer localizer)
		{
			_localizer = localizer;
			_csvBuilder = new StringBuilder();
		}

		public CsvModel(AppLocalizer localizer, Applicant applicant)
			: this(localizer)
		{
			SetDbId(applicant.Id);
			MapObject(applicant);
		}



		// Interface
		public CsvModel SetDbId(int dbId)
		{
			DbId = dbId;

			return this;
		}

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

				// Filter out properties that don't match with the
				// corresponding property type of the CSV model.
				if (!csvPropType.IsAssignableFrom(objectPropType))
					continue;

				csvProperty.SetValue(this, propInfo.GetValue(objectInstance));
			}

			return this;
		}

		public CsvModel MapSpokenLanguages(
			IEnumerable<ApplicantLanguage> applicantSpokenLanguages)
		{
			foreach (var lang in applicantSpokenLanguages)
			{
				switch (lang.Language.Alpha2)
				{
					case "de":
						SpokenLangugeGerman = true;
						break;
					case "fr":
						SpokenLangugeFrench = true;
						break;
					case "it":
						SpokenLangugeItalian = true;
						break;
				}
			}

			return this;
		}

		public CsvModel MapJsTrainingCertificates(
			IEnumerable<JSTrainingCertificate> jsTrainingCertificates)
		{
			var certIndex = 1;
			foreach (var certificate in jsTrainingCertificates)
			{
				GetProperty(this, $"JsCertificateName{certIndex}")
					.SetValue(this, certificate.Name);
				GetProperty(this, $"JsCertificateType{certIndex}")
					.SetValue(this, certificate.Type);

				certIndex++;
			}

			return this;
		}

		public CsvModel BuildContent(CultureInfo culture)
		{
			var csvProperties = GetType().GetProperties();
			BuildHeaders(csvProperties);
			BuildValues(csvProperties, culture);

			return this;
		}

		public string GetContent()
		{
			return _csvBuilder.ToString();
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
				PhoneNumber = string.IsNullOrWhiteSpace(PhoneNumber)
					? objectPhonePrefix
					: $"{objectPhonePrefix}{PhoneNumber}";
			}
			else if (propInfo.Name == nameof(Applicant.PhoneNumber))
			{
				var objectPhoneNumber = propInfo
					.GetValue(objectInstance)
					.ToString();
				PhoneNumber = string.IsNullOrWhiteSpace(PhoneNumber)
					? objectPhoneNumber
					: $"{PhoneNumber}{objectPhoneNumber}";
			}
		}

		private void BuildHeaders(IEnumerable<PropertyInfo> csvProperties)
		{
			foreach (var propInfo in csvProperties)
			{
				if (propInfo.GetValue(this) == null)
					continue;

				var localizedPropName = _localizer
					.LocalizeClassMember<CsvModel>(propInfo.Name);
				_csvBuilder.Append($"{localizedPropName}|");
			}

			// Remove last pipe
			_csvBuilder.Remove(_csvBuilder.Length - 1, 1);
			_csvBuilder.AppendLine();
		}

		private void BuildValues(IEnumerable<PropertyInfo> csvProperties, CultureInfo culture)
		{
			foreach (var propInfo in csvProperties)
			{
				var propType = propInfo.PropertyType;
				var propValue = propInfo.GetValue(this);

				if (propValue == null)
					continue;

				// Handle language value retrieving
				if (propType == typeof(Language))
				{
					var propValueLocalized = MainLanguage
						.GetLocalizedName(culture);
					_csvBuilder.Append($"\"{propValueLocalized}\"|");

					continue;
				}

				// Handle nationality value retrieving
				if (propType == typeof(Nationality))
				{
					var propValueLocalized = Nationality
						.GetLocalizedName(culture);
					_csvBuilder.Append($"\"{propValueLocalized}\"|");

					continue;
				}

				// Handle strings value retrieving
				if (propType == typeof(string))
				{
					_csvBuilder.Append($"\"{propValue}\"|");

					continue;
				}

				// Handle enums value retrieving
				if (propType.IsEnum || IsNullableEnum(propType))
				{
					var propValueCasted = propValue as Enum;
					var propValueLocalized = _localizer
						.LocalizeEnum(propValueCasted);
					_csvBuilder.Append($"{propValueLocalized}|");

					continue;
				}

				// Handle boolean value retrieving
				if (propType == typeof(bool))
				{
					var propValueCasted = (bool)propValue ? 1 : 0;
					_csvBuilder.Append($"{propValueCasted}|");

					continue;
				}

				_csvBuilder.Append($"{propValue}|");
			}

			// Remove last pipe
			_csvBuilder.Remove(_csvBuilder.Length - 1, 1);
		}

		private bool IsNullableEnum(Type enumType)
		{
			var uType = Nullable.GetUnderlyingType(enumType);

			return uType != null && uType.IsEnum;
		}
	}

}