using System.Collections.Generic;
using FreelanceTool.Data;
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

		public Applicant Applicant { get; set; }

		public ApplicationCreateViewModel(ApplicationDataContext context)
		{
			var languages = context.Languages.AsNoTracking();
			LanguagesList = new SelectList(languages, "Id", "Name");

			PhonePrefixesList = new List<SelectListItem>();
			foreach (var prefix in Applicant.PhonePrefixes)
				PhonePrefixesList.Add(new SelectListItem { Value = prefix, Text = prefix });

			var nationalities = context.Nationalities.AsNoTracking();
			NationalitiesList = new SelectList(nationalities, "Id", "NameEnglish");

			// TODO: After loading all languages, choose one based on the borwser-language.
			Applicant = new Applicant();
		}
	}
}
