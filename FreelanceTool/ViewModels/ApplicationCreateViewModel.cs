using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FreelanceTool.Data;
using FreelanceTool.Models;
using FreelanceTool.Models.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FreelanceTool.ViewModels
{
	public class ApplicationCreateViewModel
	{
		// Fields
		private ApplicationDbContext _dataContext;
		private HttpContext _httpContext;


		// View Data
		public SelectList MainLanguageList { get; private set; }
		public List<ApplicantLanguageViewModel> SpokenLanguages { get; }
		public List<SelectListItem> PhonePrefixesList { get; }
		public SelectList NationalitiesList { get; private set; }
		public Nationality NativeNationality { get; private set; }


		// Binding properties
		public Applicant Applicant { get; set; }
		private string _applicantDateOfBirth;

		[Required(ErrorMessage = "The {0} field is required.")]
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
		[Display(Name = "Date of Birth")]
		public string ApplicantDateOfBirth
		{
			get => _applicantDateOfBirth;
			set
			{
				_applicantDateOfBirth = value;

				DateTime.TryParseExact(
					value,
					"dd.MM.yyyy",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out var dateParsed);
				Applicant.DateOfBirth = dateParsed;
			}
		}

		[DataType(DataType.Upload)]
		[Display(Name = "Official Freelance Statement")]
		public IFormFile OfficialFreelanceStatement { get; set; }

		[Required(ErrorMessage = "The {0} field is required.")]
		[DataType(DataType.Upload)]
		[Display(Name = "Profile picture")]
		public IFormFile ProfilePicture { get; set; }

		/*public JSTrainingCertificate JsTrainingCertificate_1 { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_2 { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_3 { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_4 { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_5 { get; set; }*/
		public List<JSTrainingCertificate> JsTrainingCertificates;

		// Helper properties
		public bool HasProfilePicture => ProfilePicture?.Length > 0;
		public bool HasOfficialFreelanceStatement => OfficialFreelanceStatement?.Length > 0;



		// Lifecycle
		public ApplicationCreateViewModel()
		{
			// Initialize collections
			SpokenLanguages = new List<ApplicantLanguageViewModel>();
			PhonePrefixesList = new List<SelectListItem>();
			JsTrainingCertificates = new List<JSTrainingCertificate>
			{
				new JSTrainingCertificate(),
				new JSTrainingCertificate(),
				new JSTrainingCertificate(),
				new JSTrainingCertificate(),
				new JSTrainingCertificate()
			};

			Applicant = new Applicant();
			//ApplicantDateOfBirth = Applicant.DateOfBirth.ToStringLocale();
		}

		public ApplicationCreateViewModel(ApplicationDbContext dataContext, HttpContext httpContext)
			: this()
		{
			_dataContext = dataContext;
			_httpContext = httpContext;
			PopulateViewData();
		}



		// Public methods
		public ApplicationCreateViewModel SetDataContext(ApplicationDbContext dataContext)
		{
			_dataContext = dataContext;

			return this;
		}

		public ApplicationCreateViewModel SetHttpContext(HttpContext httpContext)
		{
			_httpContext = httpContext;

			return this;
		}

		public ApplicationCreateViewModel PopulateViewData(string[] spokenLanguages = null)
		{
			if (_dataContext == null || _httpContext == null) return this;

			var uiCulture = _httpContext.Features
				.Get<IRequestCultureFeature>()
				.RequestCulture
				.UICulture;

			// Language related data
			var languages = _dataContext.Languages.AsNoTracking();
			MainLanguageList = new SelectList(
				languages.Where(l => l.NameEnglish != "English"), 
				"Id", 
				Language.GetLocalizedColumnName(uiCulture));
			var currentCultureLanguage = languages.SingleOrDefault(
				l => l.NameEnglish == uiCulture.EnglishName);
			Applicant.SetMainLanguage(currentCultureLanguage);
			PopulateSpokenLanguages(languages.ToList(), uiCulture, spokenLanguages);

			// Phone prefixes data
			foreach (var prefix in Applicant.PhonePrefixes)
			{
				PhonePrefixesList.Add(new SelectListItem
				{ Value = prefix, Text = prefix });
			}

			// Nationality related data
			var nationalities = OrderLocalizedNationalities(
				_dataContext.Nationalities.AsNoTracking(), uiCulture);
			NationalitiesList = new SelectList(
				nationalities, "Id", Nationality.GetLocalizedColumnName(uiCulture));
			NativeNationality = nationalities.SingleOrDefault(n => n.Alpha2 == "CH");
			//Applicant.SetNationality(NativeNationality);

			return this;
		}

		public ApplicationCreateViewModel AttachSpokenLanguages(string[] spokenLanguages)
		{
			foreach (var idString in spokenLanguages)
			{
				if (!int.TryParse(idString, out var langauageId))
					continue;

				Applicant.SpokenLanguages.Add(new ApplicantLanguage
				{
					ApplicantId = Applicant.Id,
					LanguageId = langauageId
				});
			}

			return this;
		}

		public ApplicationCreateViewModel AttachJSTrainingCertificates()
		{
			foreach (var cert in JsTrainingCertificates)
			{
				if (string.IsNullOrWhiteSpace(cert.Name) || cert.Type == null)
					continue;

				Applicant.JsTrainingCertificates.Add(new JSTrainingCertificate
				{
					Name = cert.Name,
					Type = cert.Type
				});
			}

			return this;
		}

		public async Task<bool> TryAttachFile(
			IHostingEnvironment env, ApplicantFileType type)
		{
			// Select
			var file = type == ApplicantFileType.ProfilePicture ?
				ProfilePicture : OfficialFreelanceStatement;

			// Validate
			if (file.Length <= 0) return false;

			// Try upload
			var uniqueFileName = await file.TrySaveFile(env);
			if (uniqueFileName == null) return false;
			
			// Attach
			Applicant.ApplicantFiles.Add(new ApplicantFile(type)
			{
				ApplicantId = Applicant.Id,
				OriginalName = file.FileName,
				UniqueName = uniqueFileName,
				Extension = file.GetExtension(),
				Length = file.Length
			});

			return true;
		}


		// Private methods
		private void PopulateSpokenLanguages(
			IEnumerable<Language> languages,
			CultureInfo culture,
			string[] spokenLanguages = null)
		{
			foreach (var language in languages)
			{
				var langViewModel = new ApplicantLanguageViewModel
				{
					Id = language.Id,
					Name = language.GetLocalizedName(culture)
				};

				if (spokenLanguages != null)
				{
					langViewModel.IsChecked = spokenLanguages
						.Contains(language.Id.ToString());
				}

				SpokenLanguages?.Add(langViewModel);
			}
		}

		private IQueryable<Nationality> OrderLocalizedNationalities(
			IQueryable<Nationality> nationalities, CultureInfo culture)
		{
			IQueryable<Nationality> nationalitiesOrdered;
			switch (culture.EnglishName)
			{
				case "English":
					nationalitiesOrdered = nationalities.OrderBy(n => n.NameEnglish);
					break;
				case "German":
					nationalitiesOrdered = nationalities.OrderBy(n => n.NameGerman);
					break;
				case "French":
					nationalitiesOrdered = nationalities.OrderBy(n => n.NameFrench);
					break;
				default:
					nationalitiesOrdered = nationalities.OrderBy(n => n.NameEnglish);
					break;
			}

			return nationalitiesOrdered;
		}
	}
}
