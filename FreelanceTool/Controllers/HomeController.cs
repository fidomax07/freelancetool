using System;
using System.Diagnostics;
using FreelanceTool.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Localization.CookieRequestCultureProvider;

namespace FreelanceTool.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return RedirectToAction("Create", "Applicants");

			//return View();
		}

		public IActionResult About()
		{
			return RedirectToAction("Create", "Applicants");

			//ViewData["Message"] = "Your application description page.";

			//return View();
		}

		public IActionResult Contact()
		{
			return RedirectToAction("Create", "Applicants");

			//ViewData["Message"] = "Your contact page.";

			//return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
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
