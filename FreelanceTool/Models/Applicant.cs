using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FreelanceTool.CustomValidators;
using FreelanceTool.Models.Enums;

namespace FreelanceTool.Models
{
	public class Applicant : ApplicationModel
	{
		// Properties
		[NotMapped]
		public static string[] PhonePrefixes =
			{ "079", "078", "077", "076", "075", "0774" };

		// Database columns
		public int Id { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public DateTime CreatedAt { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime UpdatedAt { get; set; }


		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Sex")]
		public Sex Sex { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(80, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[Display(Name = "Last name")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(80, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Column(TypeName = "date")]
		[Required(ErrorMessage = "The {0} field is required.")]
		[DataType(DataType.Date)]
		[Display(Name = "Date of Birth")]
		public DateTime DateOfBirth { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(5, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[Display(Name = "Phone prefix")]
		public string PhonePrefix { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(30, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[RegularExpression(@"^[0-9]*$", ErrorMessage = "The <b>{0}</b> field must be a valid {0}.")]
		[Display(Name = "Phone number")]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(80, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[RegularExpression(
			@"^(([^<>()\[\]\\.,;:\s@""]+(\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$",
			ErrorMessage = "Please enter a valid email address.")]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(80, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[Display(Name = "Address")]
		public string Address { get; set; }

		[StringLength(80, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[Display(Name = "Address information")]
		public string AddressInformation { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(4, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[RegularExpression(@"^([1-9][0-9]{3})$", ErrorMessage = "The <b>{0}</b> field must be a valid {0}.")]
		[Display(Name = "Zip")]
		public string Zip { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(40, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[Display(Name = "City")]
		public string City { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Country")]
		public Country Country { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(20, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[RegularExpression(
			@"^[0-9]{3}\.?[0-9]{4}\.?[0-9]{4}\.?[0-9]{2}$",
			ErrorMessage = "The <b>{0}</b> field must be a valid {0}.")]
		[Display(Name = "Swiss social security number")]
		public string SwissSocialSecurityNumber { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Civil status")]
		public CivilStatus CivilStatus { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Range(0, 10, ErrorMessage = "The {0} must be between {1} and {2}.")]
		[Display(Name = "Number of kids")]
		public int ChildrenCount { get; set; }

		[ResidencePermitRequired(ErrorMessage = "Heyy!! Residence permit is required when selected country is not Switzerland.")]
		[Display(Name = "Residence permit")]
		public ResidencePermit? ResidencePermit { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "Occupation")]
		public Occupation Occupation { get; set; }

		// Make this property required, if Occupation is
		// Occupation.PartTime or Occupation.FullTime.
		[EmployerRequired(ErrorMessage = "Employer is required when occupation is part or full-time.")]
		[StringLength(40, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[Display(Name = "Employer")]
		public string Employer { get; set; }

		// Make this property required, only if the
		// Occupation is Occupation.SelfEmployed.
		[CompanyNameRequired(ErrorMessage = "Company name is required when occupation is self-employed.")]
		[StringLength(80, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[Display(Name = "Company name")]
		public string CompanyName { get; set; }

		// Populate this property, only if property
		// Occupation is Occupation.SelfEmployed.
		[StringLength(80, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[Display(Name = "Company website")]
		public string CompanyWebsite { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[Display(Name = "T shirt size")]
		public TShirtSize TShirtSize { get; set; }

		[Display(Name = "Driver license B")]
		public bool DriverLicenseB { get; set; }

		[StringLength(80, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[Display(Name = "Training number")]
		public string TrainingNumber { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(34, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[RegularExpression(
			@"^AL\d{10}[0-9A-Z]{16}$|^AD\d{10}[0-9A-Z]{12}$|^AT\d{18}$|^BH\d{2}[A-Z]{4}[0-9A-Z]{14}$|^BE\d{14}$|^BA\d{18}$|^BG\d{2}[A-Z]{4}\d{6}[0-9A-Z]{8}$|^HR\d{19}$|^CY\d{10}[0-9A-Z]{16}$|^CZ\d{22}$|^DK\d{16}$|^FO\d{16}$|^GL\d{16}$|^DO\d{2}[0-9A-Z]{4}\d{20}$|^EE\d{18}$|^FI\d{16}$|^FR\d{12}[0-9A-Z]{11}\d{2}$|^GE\d{2}[A-Z]{2}\d{16}$|^DE\d{20}$|^GI\d{2}[A-Z]{4}[0-9A-Z]{15}$|^GR\d{9}[0-9A-Z]{16}$|^HU\d{26}$|^IS\d{24}$|^IE\d{2}[A-Z]{4}\d{14}$|^IL\d{21}$|^IT\d{2}[A-Z]\d{10}[0-9A-Z]{12}$|^[A-Z]{2}\d{5}[0-9A-Z]{13}$|^KW\d{2}[A-Z]{4}22!$|^LV\d{2}[A-Z]{4}[0-9A-Z]{13}$|^LB\d{6}[0-9A-Z]{20}$|^LI\d{7}[0-9A-Z]{12}$|^LT\d{18}$|^LU\d{5}[0-9A-Z]{13}$|^MK\d{5}[0-9A-Z]{10}\d{2}$|^MT\d{2}[A-Z]{4}\d{5}[0-9A-Z]{18}$|^MR13\d{23}$|^MU\d{2}[A-Z]{4}\d{19}[A-Z]{3}$|^MC\d{12}[0-9A-Z]{11}\d{2}$|^ME\d{20}$|^NL\d{2}[A-Z]{4}\d{10}$|^NO\d{13}$|^PL\d{10}[0-9A-Z]{0,16}n$|^PT\d{23}$|^RO\d{2}[A-Z]{4}[0-9A-Z]{16}$|^SM\d{2}[A-Z]\d{10}[0-9A-Z]{12}$|^SA\d{4}[0-9A-Z]{18}$|^RS\d{20}$|^SK\d{22}$|^SI\d{17}$|^ES\d{22}$|^SE\d{22}$|^CH\d{7}[0-9A-Z]{12}$|^TN59\d{20}$|^TR\d{7}[0-9A-Z]{17}$|^AE\d{21}$|^GB\d{2}[A-Z]{4}\d{14}$", 
			ErrorMessage = "The <b>{0}</b> field must be a valid {0}.")]
		[Display(Name = "IBAN number")]
		public string IbanNumber { get; set; }


		// Relationships and Navigation Properties
		[Required(ErrorMessage = "The {0} field is required.")]
		public int LanguageId { get; set; }
		[Display(Name = "Main language")]
		public Language MainLanguage { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		public int NationalityId { get; set; }
		[Display(Name = "Nationality")]
		public Nationality Nationality { get; set; }

		public ICollection<ApplicantLanguage> SpokenLanguages { get; set; }

		public ICollection<JSTrainingCertificate> JsTrainingCertificates { get; set; }

		public ICollection<ApplicantFile> ApplicantFiles { get; set; }

		[Display(Name = "Profile picture")]
		public ApplicantFile ProfilePicture
		{
			get
			{
				return ApplicantFiles.FirstOrDefault(
					f => f.Type == ApplicantFileType.ProfilePicture);
			}
		}

		[Display(Name = "Official Freelance Statement")]
		public ApplicantFile OfficialFrelanceStatement
		{
			get
			{
				return ApplicantFiles.FirstOrDefault(
					f => f.Type == ApplicantFileType.OfficialFreelanceStatement);
			}
		}

		[Display(Name = "Csv")]
		public ApplicantFile Csv
		{
			get
			{
				return ApplicantFiles.FirstOrDefault(
					f => f.Type == ApplicantFileType.Csv);
			}
		}



		// Lifecycle
		public Applicant()
		{
			// Initialize default values
			Country = Country.CH;
			//DateOfBirth = DateTime.UtcNow.AddYears(-20);

			// Initialize collections
			SpokenLanguages = new List<ApplicantLanguage>();
			JsTrainingCertificates = new List<JSTrainingCertificate>();
			ApplicantFiles = new List<ApplicantFile>();
		}



		// Interface
		public Applicant SetMainLanguage(Language language)
		{
			if (language != null)
			{
				LanguageId = language.Id;
				MainLanguage = language;
			}

			return this;
		}

		public Applicant SetNationality(Nationality nationality)
		{
			if (nationality != null)
			{
				NationalityId = nationality.Id;
				Nationality = nationality;
			}


			return this;
		}
	}
}
