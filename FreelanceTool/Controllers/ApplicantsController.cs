using System;
using System.Collections.Generic;
using System.IO;
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
		private readonly IHostingEnvironment _host;


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
			if (ModelState.IsValid)
			{
				if (spokenLanguages != null)
					viewModel.AttachSpokenLanguages(spokenLanguages);

				viewModel.AttachJSTrainingCertificates();

				if (viewModel.HasOfficialFreelanceStatement)
				{
					await viewModel.TryAttachFile(_host,
						ApplicantFileType.OfficialFreelanceStatement);
				}

				try
				{
					var isAttached = await viewModel.TryAttachFile(
						_host, ApplicantFileType.ProfilePicture);
					if (!isAttached)
					{
						throw new FileNotFoundException("Profile picture cannot be uploaded!");
					}

					_context.Add(viewModel.Applicant);
					await _context.SaveChangesAsync();
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
				.SetDataContext(_context)
				.PopulateViewData(spokenLanguages));
		}
	}
}