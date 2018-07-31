using FreelanceTool.Data;
using FreelanceTool.Helpers;
using FreelanceTool.Models;
using FreelanceTool.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FreelanceTool.Controllers
{
	public class TestController : Controller
	{
		private readonly IHostingEnvironment _host;
		private readonly ApplicationDataContext _context;
		private readonly IStringLocalizer<Applicant> _localizer;
		private readonly EnumsLocalizer _enumsLocalizer;
		private readonly ClassesLocalizer _classesLocalizer;


		public TestController(
			IHostingEnvironment host,
			ApplicationDataContext context,
			IStringLocalizer<Applicant> localizer,
			EnumsLocalizer enumsLocalizer,
			ClassesLocalizer classesLocalizer)
		{
			_host = host;
			_context = context;
			_localizer = localizer;
			_enumsLocalizer = enumsLocalizer;
			_classesLocalizer = classesLocalizer;
		}

		public string Index()
		{
			return _classesLocalizer.Localize<ApplicationCreateViewModel>(
				nameof(ApplicationCreateViewModel.ApplicantDateOfBirth));
		}
	}
}