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

namespace FreelanceTool.Controllers
{
	public class TestController : Controller
	{
		private readonly ApplicationDataContext _dataContext;
		private readonly IHostingEnvironment _env;
		private readonly AppLocalizer _appLocalizer;
		private readonly IEmailSender _emailService;


		public TestController(
			ApplicationDataContext dataContext,
			IHostingEnvironment env,
			AppLocalizer appLocalizer,
			IEmailSender emailService)
		{
			_dataContext = dataContext;
			_env = env;
			_appLocalizer = appLocalizer;
			_emailService = emailService;
		}

		public async Task<string> Index()
		{
			var applicant = await _dataContext.Applicants
				.SingleOrDefaultAsync(a => a.Id == 1);

			return await TrySaveCsvFile(applicant);
		}

		private async Task<CsvModel> BuildCsvModel(int applicantId)
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
				.SingleOrDefaultAsync(a => a.Id == applicantId);


			return new CsvModel(_appLocalizer, applicant)
				.MapSpokenLanguages(applicant.SpokenLanguages)
				.MapJsTrainingCertificates(applicant.JsTrainingCertificates)
				.BuildContent(currentCulture);
		}

		private async Task<string> TrySaveCsvFile(Applicant applicant)
		{
			var csvModel = await BuildCsvModel(applicant.Id);
			var csvName = $"{csvModel.DbId}.csv";
			var csvPath = Path.Combine(
				PathHandler.GetCsvPath(_env), csvName);
			var isSaved = true;
			try
			{
				using (var csvFile = new StreamWriter(csvPath, false, Encoding.UTF8))
					await csvFile.WriteAsync(csvModel.GetContent());

				// Verify if file was saved successful
				if (!System.IO.File.Exists(csvPath))
					isSaved = false;
			}
			catch (Exception)
			{
				isSaved = false;
			}

			if (!isSaved)
				throw new FileNotFoundException("Csv file cannot be saved!");

			return csvModel.GetContent();

			//applicant.ApplicantFiles.Add(new ApplicantFile(ApplicantFileType.Csv)
			//{
			//	ApplicantId = applicant.Id,
			//	OriginalName = csvName,
			//	UniqueName = csvName,
			//	Extension = ".csv"
			//});
			//await _dataContext.SaveChangesAsync();
		}
	}
}