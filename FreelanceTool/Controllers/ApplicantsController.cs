using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FreelanceTool.Data;
using FreelanceTool.Models;
using FreelanceTool.Models.Enums;
using FreelanceTool.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceTool.Controllers
{
	public class ApplicantsController : Controller
	{
		private readonly ApplicationDataContext _dataContext;
		private readonly IHostingEnvironment _env;


		public ApplicantsController(
			ApplicationDataContext dataContext, IHostingEnvironment env)
		{
			_dataContext = dataContext;
			_env = env;
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
				if (spokenLanguages != null)
					viewModel.AttachSpokenLanguages(spokenLanguages);

				viewModel.AttachJSTrainingCertificates();

				if (viewModel.HasOfficialFreelanceStatement)
				{
					await viewModel.TryAttachFile(_env,
						ApplicantFileType.OfficialFreelanceStatement);
				}

				try
				{
					var isAttached = await viewModel.TryAttachFile(
						_env, ApplicantFileType.ProfilePicture);
					if (!isAttached)
					{
						throw new FileNotFoundException("Profile picture cannot be uploaded!");
					}

					_dataContext.Add(viewModel.Applicant);
					await _dataContext.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (FileNotFoundException e)
				{
					ModelState.AddModelError("", e.Message);
				}
				catch (Exception)
				{
					ModelState.AddModelError("", "Unable to save changes. " +
												 "Try again, and if the problem persists, " +
												 "see your system administrator.");
				}
			}

			return View(viewModel
				.SetDataContext(_dataContext)
				.SetHttpContext(HttpContext)
				.PopulateViewData(spokenLanguages));
		}
	}
}