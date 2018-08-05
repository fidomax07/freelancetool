using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FreelanceTool.Data;
using FreelanceTool.Helpers;
using FreelanceTool.Models;
using FreelanceTool.Models.Enums;
using FreelanceTool.Services;
using FreelanceTool.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreelanceTool.Controllers
{
	public class ApplicantsController : Controller
	{
		// Fields
		private readonly ApplicationDataContext _dataContext;
		private readonly IHostingEnvironment _env;
		private readonly AppLocalizer _appLocalizer;
		private readonly IEmailSender _emailService;



		// Lifecycle
		public ApplicantsController(
			ApplicationDataContext dataContext,
			IHostingEnvironment env,
			AppLocalizer localizer,
			IEmailSender emailService)
		{
			_dataContext = dataContext;
			_env = env;
			_appLocalizer = localizer;
			_emailService = emailService;
		}



		// GET: Applicants
		public IActionResult Index()
		{
			return View(new List<Applicant>());
		}

		// GET: Applicants/Details/5
		public IActionResult Details(int id)
		{
			return View();
		}

		// GET: Applicants/Create
		public IActionResult Create()
		{
			return View(new ApplicationCreateViewModel(_dataContext, HttpContext));
		}

		// POST: Applicants/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(
			ApplicationCreateViewModel viewModel, string[] spokenLanguages)
		{
			if (ModelState.IsValid)
			{
				var applicant = viewModel.Applicant;
				await AttachDependencies(viewModel, spokenLanguages);

				try
				{
					await TryAttachProfilePicture(viewModel);
					_dataContext.Add(applicant);
					await _dataContext.SaveChangesAsync();

					await TrySaveCsvFile(applicant);
					await _emailService.SendNewApplicationEmailAsync(applicant);

					return RedirectToAction(nameof(CreateSuccess));
				}
				catch (Exception e)
				{
					ModelState.AddModelError("", e.Message);
				}
			}

			return View(viewModel
				.SetDataContext(_dataContext)
				.SetHttpContext(HttpContext)
				.PopulateViewData(spokenLanguages));
		}

		public IActionResult CreateSuccess()
		{
			return View();
		}


		// Private methods
		private async Task AttachDependencies(
			ApplicationCreateViewModel viewModel, string[] spokenLanguages)
		{
			if (spokenLanguages != null)
			{
				viewModel.AttachSpokenLanguages(spokenLanguages);
			}

			viewModel.AttachJSTrainingCertificates();

			if (viewModel.HasOfficialFreelanceStatement)
			{
				await viewModel.TryAttachFile(
					_env,
					ApplicantFileType.OfficialFreelanceStatement);
			}
		}

		private async Task TryAttachProfilePicture(ApplicationCreateViewModel viewModel)
		{
			var isAttached = await viewModel.TryAttachFile(
				_env, ApplicantFileType.ProfilePicture);
			if (!isAttached)
			{
				throw new FileNotFoundException("Profile picture cannot be uploaded!");
			}
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

		private async Task TrySaveCsvFile(Applicant applicant)
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

			applicant.ApplicantFiles.Add(new ApplicantFile(ApplicantFileType.Csv)
			{
				ApplicantId = applicant.Id,
				OriginalName = csvName,
				UniqueName = csvName,
				Extension = ".csv"
			});
			await _dataContext.SaveChangesAsync();
		}

	}
}