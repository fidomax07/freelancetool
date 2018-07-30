using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FreelanceTool.Data;
using FreelanceTool.Helpers.Enums;
using FreelanceTool.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FreelanceTool.ViewModels
{
	public class ApplicationCreateViewModel
	{
		// Fields
		private ApplicationDataContext _context;

		// View Data
		public SelectList MainLanguageList { get; private set; }
		public List<ApplicantLanguageViewModel> SpokenLanguages { get; }
		public List<SelectListItem> PhonePrefixesList { get; }
		public SelectList NationalitiesList { get; private set; }
		public Nationality NativeNationality { get; private set; }

		// Binding properties
		public Applicant Applicant { get; set; }
		private string _applicantDateOfBirth;
		[Required]
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
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
		public IFormFile OfficialFreelanceStatement { get; set; }
		[Required, DataType(DataType.Upload)]
		public IFormFile ProfilePicture { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_1 { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_2 { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_3 { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_4 { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_5 { get; set; }

		// Helper properties
		public List<JSTrainingCertificate> JsTrainingCertificates
		{
			get
			{
				return new List<JSTrainingCertificate>
				{
					JsTrainingCertificate_1,
					JsTrainingCertificate_2,
					JsTrainingCertificate_3,
					JsTrainingCertificate_4,
					JsTrainingCertificate_5
				};
			}
		}
		public bool HasProfilePicture => ProfilePicture?.Length > 0;
		public bool HasOfficialFreelanceStatement => OfficialFreelanceStatement?.Length > 0;



		// Lifecycle
		public ApplicationCreateViewModel()
		{
			// Initialize collections
			SpokenLanguages = new List<ApplicantLanguageViewModel>();
			PhonePrefixesList = new List<SelectListItem>();

			// TODO: After loading all languages, choose one based on the borwser-language.
			Applicant = new Applicant();
			var before20Years = new DateTime(DateTime.Now.Year - 20, 1, 31);
			ApplicantDateOfBirth = before20Years.ToString("dd.MM.yyyy");
		}

		public ApplicationCreateViewModel(ApplicationDataContext context)
			: this()
		{
			_context = context;
			PopulateViewData();
		}



		// Public methods
		public ApplicationCreateViewModel SetDataContext(ApplicationDataContext context)
		{
			_context = context;

			return this;
		}

		public ApplicationCreateViewModel PopulateViewData(string[] spokenLanguages = null)
		{
			if (_context == null) return this;

			var languages = _context.Languages.AsNoTracking();
			MainLanguageList = new SelectList(languages.Where(
				l => l.Name != "English"), "Id", "Name");
			PopulateSpokenLanguages(languages.ToList(), spokenLanguages);

			foreach (var prefix in Applicant.PhonePrefixes)
			{
				PhonePrefixesList.Add(new SelectListItem
				{ Value = prefix, Text = prefix });
			}

			var nationalities = _context.Nationalities
				.AsNoTracking()
				.OrderBy(n => n.NameEnglish);
			NationalitiesList = new SelectList(nationalities, "Id", "NameEnglish");
			NativeNationality = nationalities.SingleOrDefault(n => n.Alpha2 == "CH");
			Applicant.NationalityId = NativeNationality?.Id ?? 1;

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
			IHostingEnvironment host, ApplicantFileType type)
		{
			// Select
			var file = type == ApplicantFileType.ProfilePicture ?
				ProfilePicture : OfficialFreelanceStatement;

			// Validate
			if (file.Length <= 0) return false;

			// Try upload
			var uniqueFileName = await file.TryUploadFile(host);
			if (uniqueFileName == null) return false;

			// Attach
			Applicant.ApplicantFiles.Add(new ApplicantFile(type)
			{
				ApplicantId = Applicant.Id,
				OriginalName = file.FileName,
				UniqueName = uniqueFileName,
				Extension = file.GetExtension()
			});

			return true;
		}


		// Private methods
		private void PopulateSpokenLanguages(
			IEnumerable<Language> languages,
			string[] spokenLanguages = null)
		{
			foreach (var language in languages)
			{
				var langViewModel = new ApplicantLanguageViewModel
					{ Id = language.Id, Name = language.Name };

				if (spokenLanguages != null)
				{
					langViewModel.IsChecked = spokenLanguages
						.Contains(language.Id.ToString());
				}

				SpokenLanguages?.Add(langViewModel);
			}
		}
	}
}
