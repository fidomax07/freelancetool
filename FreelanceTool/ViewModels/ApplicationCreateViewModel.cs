using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FreelanceTool.Data;
using FreelanceTool.Helpers.Enums;
using FreelanceTool.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FreelanceTool.ViewModels
{
	public class ApplicationCreateViewModel
	{
		public SelectList LanguagesList { get; }
		public List<SelectListItem> PhonePrefixesList { get; }
		public SelectList NationalitiesList { get; }
		public Nationality NativeNationality { get; }

		public Applicant Applicant { get; set; }
		[DataType(DataType.Upload)]
		public ApplicantFile OfficialFreelanceStatement { get; set; }
		public List<Language> ApplicantLanguages { get; set; }
		

		public ApplicationCreateViewModel(ApplicationDataContext context)
		{
			var languages = context.Languages.AsNoTracking();
			LanguagesList = new SelectList(languages, "Id", "Name");

			PhonePrefixesList = new List<SelectListItem>();
			foreach (var prefix in Applicant.PhonePrefixes)
				PhonePrefixesList.Add(new SelectListItem { Value = prefix, Text = prefix });

			var nationalities = context.Nationalities.AsNoTracking();
			NationalitiesList = new SelectList(nationalities, "Id", "NameEnglish");
			NativeNationality = nationalities.SingleOrDefault(n => n.Alpha2 == "CH");

			// TODO: After loading all languages, choose one based on the borwser-language.
			Applicant = new Applicant();
			OfficialFreelanceStatement = new ApplicantFile(ApplicantFileType.OfficialFreelanceStatement);
			ApplicantLanguages = languages.ToList();
		}
	}
}
