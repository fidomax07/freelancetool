using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FreelanceTool.CustomValidators;
using FreelanceTool.Models.Enums;

namespace FreelanceTool.Models
{
	public class Applicant
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
		[StringLength(30)]
		[Display(Name = "Sex")]
		public string Sex { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(80, ErrorMessage = "The <b>{0}</b> field must be at most {1} characters long.")]
		[Display(Name = "Last name")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[StringLength(80)]
		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Column(TypeName = "date")]
		[Required, DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }

		[Required, StringLength(5)]
		public string PhonePrefix { get; set; }

		[Required, StringLength(30), RegularExpression(@"^[0-9]*$")]
		public string PhoneNumber { get; set; }

		[Required, StringLength(80), DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required, StringLength(80)]
		public string Address { get; set; }

		[StringLength(80)]
		[Display(Name = "Address information")]
		public string AddressInformation { get; set; }

		[Required, StringLength(4), RegularExpression(@"^([1-9][0-9]{3})$")]
		public string Zip { get; set; }

		[Required, StringLength(40)]
		public string City { get; set; }

		[Required]
		[Display(Name = "Country")]
		public Country Country { get; set; }

		[Required, StringLength(20)]
		[RegularExpression(@"^[0-9]{3}\.?[0-9]{4}\.?[0-9]{4}\.?[0-9]{2}$")]
		public string SwissSocialSecurityNumber { get; set; }

		[Required]
		public CivilStatus CivilStatus { get; set; }

		[Required, Range(0, 10)]
		[Display(Name = "Number of Kids")]
		public int ChildrenCount { get; set; }

		[ResidencePermitRequired]
		public ResidencePermit? ResidencePermit { get; set; }

		[Required]
		public Occupation Occupation { get; set; }

		// Make this property required, if Occupation is
		// Occupation.PartTime or Occupation.FullTime.
		[StringLength(40)]
		[EmployerRequired]
		public string Employer { get; set; }

		// Make this property required, only if the
		// Occupation is Occupation.SelfEmployed.
		[StringLength(80)]
		[CompanyNameRequired]
		public string CompanyName { get; set; }

		// Populate this property, only if property
		// Occupation is Occupation.SelfEmployed.
		[StringLength(80)]
		public string CompanyWebsite { get; set; }

		[Required]
		public TShirtSize TShirtSize { get; set; }

		public bool DriverLicenseB { get; set; }

		[StringLength(80)]
		public string TrainingNumber { get; set; }

		[Required, StringLength(34), RegularExpression(@"^[0-9]*$")]
		public string IbanNumber { get; set; }

		public string CsvPath { get; set; }


		// Relationships and Navigation Properties
		[Required, Display(Name = "Main language")]
		public int LanguageId { get; set; }
		public Language MainLanguage { get; set; }

		[Required, Display(Name = "Nationality")]
		public int NationalityId { get; set; }
		public Nationality Nationality { get; set; }

		public ICollection<ApplicantLanguage> SpokenLanguages { get; set; }

		public ICollection<JSTrainingCertificate> JsTrainingCertificates { get; set; }

		public ICollection<ApplicantFile> ApplicantFiles { get; set; }
		public ApplicantFile ProfilePicture
		{
			get
			{
				return ApplicantFiles.FirstOrDefault(
					f => f.Type == ApplicantFileType.ProfilePicture);
			}
		}
		public ApplicantFile OfficialFrelanceStatement
		{
			get
			{
				return ApplicantFiles.FirstOrDefault(
					f => f.Type == ApplicantFileType.OfficialFreelanceStatement);
			}
		}



		// Lifecycle
		public Applicant()
		{
			// Initialize default values
			Country = Country.Switzerland;
			DateOfBirth = DateTime.UtcNow.AddYears(-20);

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
