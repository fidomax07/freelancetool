using System.Diagnostics;
using FreelanceTool.Models;
using Microsoft.AspNetCore.Mvc;

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
	}
}
