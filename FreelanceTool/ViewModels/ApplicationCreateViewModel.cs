using FreelanceTool.Data;
using FreelanceTool.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FreelanceTool.ViewModels
{
	public class ApplicationCreateViewModel
	{
		public SelectList LanguagesList { get; }

		public Applicant Applicant { get; set; }

		public ApplicationCreateViewModel(ApplicationDataContext context)
	    {
		    var languages = context.Languages.AsNoTracking();

		    LanguagesList = new SelectList(languages, "Id", "Name");
			// TODO: After loading all languages, choose one based on the borwser-language.
			Applicant = new Applicant();
	    }
	}
}
