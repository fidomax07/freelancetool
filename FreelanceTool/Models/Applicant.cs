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
		[Display(Name = "Sex")]
		public Sex Sex { get; set; }

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
		[Display(Name = "Date of birth")]
		public DateTime DateOfBirth { get; set; }

		[Required, StringLength(5)]
		public string PhonePrefix { get; set; }

		[Required, StringLength(30), RegularExpression(@"^[0-9]*$")]
		public string PhoneNumber { get; set; }

		[Required, StringLength(80), DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required, StringLength(80)]
		[Display(Name = "Address")]
		public string Address { get; set; }

		[StringLength(80)]
		[Display(Name = "Address information")]
		public string AddressInformation { get; set; }

		[Required, StringLength(4), RegularExpression(@"^([1-9][0-9]{3})$")]
		[Display(Name = "Zip")]
		public string Zip { get; set; }

		[Required, StringLength(40)]
		[Display(Name = "City")]
		public string City { get; set; }

		[Required]
		[Display(Name = "Country")]
		public Country Country { get; set; }

		[Required, StringLength(20)]
		[RegularExpression(@"^[0-9]{3}\.?[0-9]{4}\.?[0-9]{4}\.?[0-9]{2}$")]
		[Display(Name = "Swiss social security number")]
		public string SwissSocialSecurityNumber { get; set; }

		[Required]
		[Display(Name = "Civil status")]
		public CivilStatus CivilStatus { get; set; }

		[Required, Range(0, 10)]
		[Display(Name = "Number of kids")]
		public int ChildrenCount { get; set; }

		[ResidencePermitRequired]
		[Display(Name = "Residence permit")]
		public ResidencePermit? ResidencePermit { get; set; }

		[Required]
		[Display(Name = "Occupation")]
		public Occupation Occupation { get; set; }

		// Make this property required, if Occupation is
		// Occupation.PartTime or Occupation.FullTime.
		[StringLength(40)]
		[EmployerRequired]
		[Display(Name = "Employer")]
		public string Employer { get; set; }

		// Make this property required, only if the
		// Occupation is Occupation.SelfEmployed.
		[StringLength(80)]
		[CompanyNameRequired]
		[Display(Name = "Company name")]
		public string CompanyName { get; set; }

		// Populate this property, only if property
		// Occupation is Occupation.SelfEmployed.
		[StringLength(80)]
		[Display(Name = "Company website")]
		public string CompanyWebsite { get; set; }

		[Required]
		[Display(Name = "T shirt size")]
		public TShirtSize TShirtSize { get; set; }

		[Display(Name = "Driver license B")]
		public bool DriverLicenseB { get; set; }

		[StringLength(80)]
		[Display(Name = "Training number")]
		public string TrainingNumber { get; set; }

		[Required, StringLength(34), RegularExpression(@"^[0-9]*$")]
		[Display(Name = "IBAN number")]
		public string IbanNumber { get; set; }


		// Relationships and Navigation Properties
		[Required]
		public int LanguageId { get; set; }
		[Display(Name = "Main language")]
		public Language MainLanguage { get; set; }

		[Required]
		public int NationalityId { get; set; }
		[Display(Name = "Nationality")]
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
