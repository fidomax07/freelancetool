using FreelanceTool.Data;
using FreelanceTool.Helpers;
using FreelanceTool.Models.Enums;
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
		private readonly AppLocalizer _localizer;
		private readonly IStringLocalizer<Country> _countryLocalizer;


		public TestController(
			IHostingEnvironment host,
			ApplicationDataContext context,
			AppLocalizer localizer,
			IStringLocalizer<Country> countryLocalizer)
		{
			_host = host;
			_context = context;
			_localizer = localizer;
			_countryLocalizer = countryLocalizer;
		}

		public string Index()
		{
			return _localizer.LocalizeClassMember<ApplicationCreateViewModel>(
				nameof(ApplicationCreateViewModel.ApplicantDateOfBirth));

			//return _localizer.LocalizeEnum(Country.Germany);
		}
	}
}