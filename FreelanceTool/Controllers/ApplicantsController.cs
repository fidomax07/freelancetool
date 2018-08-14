using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreelanceTool.Data;
using FreelanceTool.Helpers;
using FreelanceTool.Models;
using FreelanceTool.Models.Enums;
using FreelanceTool.Services;
using FreelanceTool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.IO.Path;
using static FreelanceTool.Helpers.Constants;
using SystemFile = System.IO.File;

namespace FreelanceTool.Controllers
{
	public class ApplicantsController : Controller
	{
		// Fields
		private readonly ApplicationDbContext _dataContext;
		private readonly IHostingEnvironment _env;
		private readonly AppLocalizer _appLocalizer;
		private readonly IEmailSender _emailService;



		// Lifecycle
		public ApplicantsController(
			ApplicationDbContext dataContext,
			IHostingEnvironment env,
			AppLocalizer appLocalizer,
			IEmailSender emailService)
		{
			_dataContext = dataContext;
			_env = env;
			_appLocalizer = appLocalizer;
			_emailService = emailService;
		}



		// GET: Applicants
		public async Task<IActionResult> Index(string applicantId, string message)
		{
			ViewData["CurrentApplicantId"] = applicantId;
			if (!string.IsNullOrEmpty(message))
			{
				ViewBag.Message = message;
			}

			var applicantsQuery = _dataContext.Applicants
				.OrderByDescending(a => a.CreatedAt)
				.AsNoTracking();

			// Handle searching with applicant id if
			// a value was provided from the user.
			if (!string.IsNullOrEmpty(applicantId))
			{
				var isIdParsed = int.TryParse(applicantId, out var idParsed);
				if (!isIdParsed) return NotFound();

				var isApplicantFound = await applicantsQuery
					.AnyAsync(a => a.Id == idParsed);
				if (!isApplicantFound) return NotFound();

				return RedirectToAction(nameof(Details), new {id = idParsed});
			}

			var applicants = await applicantsQuery.ToListAsync();
			return View(applicants);
		}

		// GET: Applicants/Details/5
		public async Task<IActionResult> Details(int id)
		{
			var applicant = await _dataContext.Applicants
				.Include(a => a.ApplicantFiles)
				.AsNoTracking()
				.SingleOrDefaultAsync(a => a.Id == id);
			if (applicant == null) return NotFound();

			return View(applicant);
		}

		// GET: Applicants/Create
		[AllowAnonymous]
		public IActionResult Create()
		{
			return View(new ApplicationCreateViewModel(_dataContext, HttpContext));
		}

		// POST: Applicants/Create
		[HttpPost]
		[AllowAnonymous]
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

		[AllowAnonymous]
		public IActionResult CreateSuccess()
		{
			return View();
		}

		public async Task<IActionResult> DownloadFile(int fileId)
		{
			var file = await _dataContext
				.ApplicantFiles
				.AsNoTracking()
				.SingleOrDefaultAsync(af => af.Id == fileId);
			if (file == null) return NotFound();

			var path = UPLOAD_PATH;
			if (file.Type == ApplicantFileType.Csv)
				path = Combine(path, CSV_PATH);
			path = Combine(path, file.UniqueName);

			var fileStream = _env
				.ContentRootFileProvider
				.GetFileInfo(path)
				.CreateReadStream();

			return File(fileStream, "text/csv", file.OriginalName);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteFiles(int id)
		{
			var applicant = await _dataContext
				.Applicants
				.Include(a => a.ApplicantFiles)
				.AsNoTracking()
				.SingleOrDefaultAsync(a => a.Id == id);
			if (applicant == null) return NotFound();

			var filesToDelete = applicant
				.ApplicantFiles
				.Where(af => af.Type != ApplicantFileType.Csv)
				.ToList();

			foreach (var file in filesToDelete)
			{
				var path = Combine(_env.ContentRootPath, UPLOAD_PATH, file.UniqueName);
				if (!SystemFile.Exists(path)) continue;

				try
				{
					SystemFile.Delete(path);
					_dataContext.Remove(file);
				}
				catch (Exception) { /* File failed to delete */ }
			}

			try
			{
				await _dataContext.SaveChangesAsync();
			}
			catch (Exception) { /* File deatach failed. */ }

			return RedirectToAction(
				nameof(Index), 
				new { message = "Files have been deleted successfully." });
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
			var csvPath = Combine(
				PathHandler.GetCsvPath(_env), csvName);
			var isSaved = true;
			try
			{
				using (var csvFile = new StreamWriter(csvPath, false, Encoding.UTF8))
					await csvFile.WriteAsync(csvModel.GetContent());

				// Verify if file was saved successful
				if (!SystemFile.Exists(csvPath))
					isSaved = false;
			}
			catch (Exception)
			{
				isSaved = false;
			}
			
			if (!isSaved)
				throw new FileNotFoundException("Csv file cannot be saved!");
			
			var csvFileInfo = _env.ContentRootFileProvider.GetFileInfo(csvPath);
			applicant.ApplicantFiles.Add(new ApplicantFile(ApplicantFileType.Csv)
			{
				ApplicantId = applicant.Id,
				OriginalName = csvName,
				UniqueName = csvName,
				Extension = ".csv",
				Length = csvFileInfo.Length
			});
			await _dataContext.SaveChangesAsync();
		}
	}
}