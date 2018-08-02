using System.Threading.Tasks;
using FreelanceTool.Data;
using FreelanceTool.Helpers;
using FreelanceTool.Models;
using FreelanceTool.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FreelanceTool.Controllers
{
	public class TestController : Controller
	{
		private readonly IConfiguration _config;
		private readonly ApplicationDataContext _dataContext;
		private readonly IEmailSender _emailService;
		private readonly AppLocalizer _localizer;
		private readonly PathHandler _pathService;


		public TestController(
			IConfiguration config,
			ApplicationDataContext dataContext, 
			IEmailSender emailService,
			AppLocalizer localizer,
			PathHandler pathService)
		{
			_config = config;
			_dataContext = dataContext;
			_emailService = emailService;
			_localizer = localizer;
			_pathService = pathService;
		}

		public async Task<IActionResult> Index()
		{

			var applicant = await _dataContext.Applicants
				.Include(a => a.MainLanguage)
				.Include(a => a.Nationality)
				.Include(a => a.SpokenLanguages)
				.Include(a => a.JsTrainingCertificates)
				.Include(a => a.ApplicantFiles)
				.SingleOrDefaultAsync(a => a.Id == 1);
			

			var csvModel = new CsvModel(applicant);
			csvModel.MapComplexProperty(
				applicant,
				nameof(applicant.MainLanguage),
				nameof(applicant.MainLanguage.NameEnglish));

			return new JsonResult(csvModel);
		}
	}
}