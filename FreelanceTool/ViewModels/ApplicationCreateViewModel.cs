using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
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
		[DataType(DataType.Upload)]
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

		public async Task<ApplicationCreateViewModel> AttachFile(
			IHostingEnvironment host, ApplicantFileType type)
		{
			var file = type == ApplicantFileType.ProfilePicture ?
				ProfilePicture : OfficialFreelanceStatement;
			if (file.Length <= 0) return this;

			var fileUniqueName = Path.GetRandomFileName() + ".jpg";
			var filePath = Path.Combine(host.ContentRootPath, "App_Data", fileUniqueName);
			using (var stream = new FileStream(filePath, FileMode.Create))
				await file.CopyToAsync(stream);

			Applicant.ApplicantFiles.Add(new ApplicantFile(type)
			{
				Path = filePath,
				OriginalName = file.FileName,
				UniqueName = fileUniqueName,
				//Extension = file.Name,
				ApplicantId = Applicant.Id,
			});

			return this;
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
