using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FreelanceTool.Data;
using FreelanceTool.Helpers;
using FreelanceTool.Models;
using FreelanceTool.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FreelanceTool.Controllers
{
	public class TestController : Controller
	{
		private readonly ApplicationDataContext _dataContext;
		private readonly IHostingEnvironment _env;
		private readonly AppLocalizer _localizer;
		private readonly IEmailSender _emailService;


		public TestController(
			ApplicationDataContext dataContext,
			IHostingEnvironment env,
			AppLocalizer localizer,
			IEmailSender emailService)
		{
			_dataContext = dataContext;
			_env = env;
			_localizer = localizer;
			_emailService = emailService;
		}

		public async Task<string> Index()
		{
			var currentCulture = HttpContext.Features
				.Get<IRequestCultureFeature>()
				.RequestCulture
				.UICulture;

			var applicant = await _dataContext.Applicants
				.Include(a => a.MainLanguage)
				.Include(a => a.Nationality)
				.Include(a => a.SpokenLanguages)
					.ThenInclude(al => al.Language)
				.Include(a => a.JsTrainingCertificates)
				.Include(a => a.ApplicantFiles)
				.SingleOrDefaultAsync(a => a.Id == 1);

			var csvModel = new CsvModel(_localizer, applicant)
				.MapSpokenLanguages(applicant.SpokenLanguages)
				.MapJsTrainingCertificates(applicant.JsTrainingCertificates)
				.BuildContent(currentCulture);

			// Write to Csv file
			var path = Path.Combine(
				PathHandler.GetCsvPath(_env),
				$"{applicant.Id.ToString()}.csv");
			using (var outputFile = new StreamWriter(path, false, Encoding.UTF8))
			{
				outputFile.Write(csvModel.GetContent());
			}

			return csvModel.GetContent();
		}
	}
}