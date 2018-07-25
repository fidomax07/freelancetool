using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FreelanceTool.Data;
using FreelanceTool.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FreelanceTool.ViewModels
{
	public class ApplicationCreateViewModel
	{
		// Fields
		private readonly ApplicationDataContext _context;

		// Properties
		public SelectList LanguagesList { get; private set; }
		public List<Language> ApplicantLanguages { get; private set; }
		public List<SelectListItem> PhonePrefixesList { get; private set; }
		public SelectList NationalitiesList { get; private set; }
		public Nationality NativeNationality { get; private set; }


		// Binding properties
		public Applicant Applicant { get; set; }
		[DataType(DataType.Upload)]
		public IFormFile OfficialFreelanceStatement { get; set; }
		[DataType(DataType.Upload)]
		public IFormFile ProfilePicture { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_1 { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_2 { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_3 { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_4 { get; set; }
		public JSTrainingCertificate JsTrainingCertificate_5 { get; set; }



		public ApplicationCreateViewModel()
		{
		}

		public ApplicationCreateViewModel(ApplicationDataContext context)
		{
			_context = context;

			InitializeProperties();

			// TODO: After loading all languages, choose one based on the borwser-language.
			Applicant = new Applicant();
		}

		private void InitializeProperties()
		{
			var languages = _context.Languages.AsNoTracking();
			LanguagesList = new SelectList(
				languages.Where(l => l.Name != "English"), "Id", "Name");
			ApplicantLanguages = languages.ToList();

			PhonePrefixesList = new List<SelectListItem>();
			foreach (var prefix in Applicant.PhonePrefixes)
				PhonePrefixesList.Add(new SelectListItem {Value = prefix, Text = prefix});

			var nationalities = _context.Nationalities
				.AsNoTracking()
				.OrderBy(n => n.NameEnglish);
			NationalitiesList = new SelectList(nationalities, "Id", "NameEnglish");
			NativeNationality = nationalities.SingleOrDefault(n => n.Alpha2 == "CH");
		}
	}
}
