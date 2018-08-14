using System;
using System.Diagnostics;
using FreelanceTool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Localization.CookieRequestCultureProvider;

namespace FreelanceTool.Controllers
{
	public class HomeController : Controller
	{
		[AllowAnonymous]
		public IActionResult Index()
		{
			return RedirectToAction(nameof(ApplicantsController.Create), "Applicants");

			//return View();
		}

		[AllowAnonymous]
		public IActionResult About()
		{
			return RedirectToAction("Create", "Applicants");

			//ViewData["Message"] = "Your application description page.";

			//return View();
		}

		[AllowAnonymous]
		public IActionResult Contact()
		{
			return RedirectToAction("Create", "Applicants");

			//ViewData["Message"] = "Your contact page.";

			//return View();
		}

		[AllowAnonymous]
		public IActionResult Error()
		{
			return View(new ErrorViewModel
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
			});
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult SetLanguage(string culture, string returnUrl)
		{
			Response.Cookies.Append(
				DefaultCookieName,
				MakeCookieValue(new RequestCulture(culture)),
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
			);

			return LocalRedirect(returnUrl);
		}
	}
}
