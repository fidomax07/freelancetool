using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreelanceTool.Data;
using FreelanceTool.Helpers;
using FreelanceTool.Models;
using FreelanceTool.Services;
using FreelanceTool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.IO.Path;
using static FreelanceTool.Helpers.Constants;
using static FreelanceTool.Models.Enums.ApplicantFileType;
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
		public async Task<IActionResult> Index(
			int? applicantId, string message, bool? success, int? page)
		{
			ViewData["CurrentApplicantId"] = applicantId;
			if (!string.IsNullOrEmpty(message))
			{
				ViewBag.Message = message;
				ViewBag.Success = success;
			}

			var applicantsQuery = _dataContext.Applicants
				.OrderByDescending(a => a.CreatedAt)
				.AsNoTracking();

			// Handle searching with applicant id if
			// a value was provided from the user.
			if (applicantId != null)
			{
				var isApplicantFound = await applicantsQuery
					.AnyAsync(a => a.Id == applicantId);
				if (!isApplicantFound)
				{
					return RedirectToAction(
						nameof(Index),
						new { message = "The applicant is not found!", success = false });
				}

				return RedirectToAction(nameof(Details), new { id = applicantId });
			}

			var applicants = await PaginatedList<Applicant>.CreateAsync(
				applicantsQuery, page ?? 1, DEFAULT_PAGE_SIZE);
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
				try
				{
					await TrySaveApplicant(viewModel, spokenLanguages);
					TrySendEmailNotification(viewModel.Applicant);

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

		public async Task<IActionResult> DownloadFile(int id)
		{
            var file = await _dataContext
				.ApplicantFiles
				.AsNoTracking()
				.SingleOrDefaultAsync(af => af.Id == id);

            if (file == null) return NotFound();

			var directory = file.Type == Csv ?
				CSV_DIRECTORY : UPLOAD_DIRECTORY;
			var path = Combine(directory, file.UniqueName);
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
				.Where(af => af.Type != Csv)
				.ToList();

			foreach (var file in filesToDelete)
			{
				var path = Combine(_env.ContentRootPath, UPLOAD_DIRECTORY, file.UniqueName);
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
				new { message = "Files have been deleted successfully.", success = true });
		}


		// Private methods
		private async Task TrySaveApplicant(
			ApplicationCreateViewModel viewModel, string[] spokenLanguages)
		{
			if (spokenLanguages != null)
				viewModel.AttachSpokenLanguages(spokenLanguages);

			viewModel.AttachJSTrainingCertificates();

			if (viewModel.HasOfficialFreelanceStatement)
				await viewModel.TryAttachFile(_env, OfficialFreelanceStatement);

			if (!await viewModel.TryAttachFile(_env, ProfilePicture))
				throw new FileNotFoundException("Profile picture cannot be uploaded!");

			_dataContext.Add(viewModel.Applicant);
			await _dataContext.SaveChangesAsync();

			await TrySaveCsvFile(viewModel.Applicant);
		}

		private async Task<bool> TrySaveCsvFile(Applicant applicant)
		{
			var csvModel = await BuildCsvModel(applicant.Id);
			var csvName = $"{csvModel.DbId}.csv";
			var csvPath = Combine(_env.ContentRootPath, CSV_DIRECTORY, csvName);

			// Try upload the file
			bool isSaved;
			try
			{
				using (var csvFile = new StreamWriter(csvPath, false, Encoding.UTF8))
					await csvFile.WriteAsync(csvModel.GetContent());

				isSaved = SystemFile.Exists(csvPath);
			}
			catch (Exception)
			{
				isSaved = false;
			}

			// Try save the file into the database
			if (isSaved)
			{
				var csvFileInfo = _env
					.ContentRootFileProvider
					.GetFileInfo(Combine(CSV_DIRECTORY, csvName));
				applicant.ApplicantFiles.Add(new ApplicantFile(Csv)
				{
					ApplicantId = applicant.Id,
					OriginalName = csvName,
					UniqueName = csvName,
					Extension = ".csv",
					Length = csvFileInfo.Length
				});

				try
				{
					await _dataContext.SaveChangesAsync();
				}
				catch (Exception)
				{
					isSaved = false;
				}
			}

			return isSaved;
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

		private void TrySendEmailNotification(Applicant applicant)
		{
			Task.Factory.StartNew(async () =>
			{
				await _emailService.SendNewApplicationEmailAsync(applicant);
			});
		}
	}
}