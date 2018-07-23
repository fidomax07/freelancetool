using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FreelanceTool.Helpers.Enums;

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

		[Required, StringLength(30)]
		public string Sex { get; set; }

		[Required, StringLength(80)]
		public string LastName { get; set; }

		[Required, StringLength(80)]
		public string FirstName { get; set; }

		[Required, DataType(DataType.Text)]
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
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
		public Country Country { get; set; }

		[Required, RegularExpression(@"^756\.?[0-9]{4}\.?[0-9]{4}\.?[0-9]{2}$")]
		public string SwissSocialSecurityNumber { get; set; }

		[Required, StringLength(20)]
		public CivilStatus CivilStatus { get; set; }

		[Required, Range(0, 10)]
		public int ChildrenCount { get; set; }

		public ResidencePermit ResidencePermit { get; set; }

		[Required]
		public Occupation Occupation { get; set; }

		[StringLength(40)]
		public string Employer { get; set; }

		[StringLength(80)]
		public string CompanyName { get; set; }

		[StringLength(80)]
		public string CompanyWebsite { get; set; }

		[Required]
		public TShirtSize TShirtSize { get; set; }

		public bool DriverLicenseB { get; set; }

		[StringLength(80)]
		public string TrainingNumber { get; set; }

		[Required, StringLength(34)]
		public string IbanNumber { get; set; }


		// Relationships and Navigation Properties
		public int LanguageId { get; set; }
		[Required, Display(Name = "Main language")]
		public Language MainLanguage { get; set; }

		public int NationalityId { get; set; }
		[Required]
		public Nationality Nationality { get; set; }

		public ICollection<ApplicantLanguage> SpokenLanguages { get; set; }

		public ICollection<JSTrainingCertificate> JsTrainingCertificates { get; set; }

		public ICollection<ApplicantFile> ApplicantFiles { get; set; }
		public ApplicantFile ProfilePicture => ApplicantFiles.FirstOrDefault(
			f => f.Type == ApplicantFileType.ProfilePicture);
		public ApplicantFile OfficialFrelanceStatement => ApplicantFiles.FirstOrDefault(
			f => f.Type == ApplicantFileType.OfficialFreelanceStatement);



		// Lifecycle
		public Applicant()
		{
			// Set default values for database columns
			var now = DateTime.Now;
			DateOfBirth = new DateTime(now.Year - 20, now.Month, now.Day);
			Country = Country.Switzerland;
		}
	}
}
