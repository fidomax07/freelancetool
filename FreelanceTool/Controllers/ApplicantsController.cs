using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreelanceTool.Data;
using FreelanceTool.Helpers.Enums;
using FreelanceTool.Models;
using FreelanceTool.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceTool.Controllers
{
	public class ApplicantsController : Controller
	{
		private readonly ApplicationDataContext _context;
		private readonly  IHostingEnvironment _host;


		public ApplicantsController(
			ApplicationDataContext context, IHostingEnvironment host)
		{
			_context = context;
			_host = host;
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
			return View(new ApplicationCreateViewModel(_context));
		}

		// POST: Applicants/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(
			ApplicationCreateViewModel viewModel, string[] spokenLanguages)
		{
			//return new JsonResult(new Dictionary<string, object>
			//{
			//	{"ViewModel", viewModel},
			//	{"SpokenLanguages", spokenLanguages}
			//});

			if (!ModelState.IsValid)
			{
				return View(viewModel
					.SetDataContext(_context)
					.PopulateViewData(spokenLanguages));
			}

			// Spoken lanugages
			if (spokenLanguages != null)
				viewModel.AttachSpokenLanguages(spokenLanguages);
			
			// Certificates
			viewModel.AttachJSTrainingCertificates();

			// Files
			if (viewModel.ProfilePicture.Length > 0)
				await viewModel.AttachFile(_host, ApplicantFileType.ProfilePicture);
			if (viewModel.OfficialFreelanceStatement.Length > 0)
				await viewModel.AttachFile(_host, ApplicantFileType.OfficialFreelanceStatement);

			_context.Add(viewModel.Applicant);

			try
			{
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception e)
			{
				ModelState.AddModelError("", "Unable to save changes. " +
				                             "Try again, and if the problem persists, " +
				                             "see your system administrator.");
			}

			viewModel.SetDataContext(_context).PopulateViewData(spokenLanguages);
			return View(viewModel);
		}
	}
}