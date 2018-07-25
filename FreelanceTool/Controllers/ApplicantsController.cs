using System.Collections.Generic;
using FreelanceTool.Data;
using FreelanceTool.Models;
using FreelanceTool.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceTool.Controllers
{
	public class ApplicantsController : Controller
    {
	    private readonly ApplicationDataContext _context;



	    public ApplicantsController(ApplicationDataContext context)
	    {
		    _context = context;
	    }



        // GET: Applicants
        public ActionResult Index()
        {
            return View(new List<Applicant>());
        }

        // GET: Applicants/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Applicants/Create
        public ActionResult Create()
        {
	        var viewModel = new ApplicationCreateViewModel(_context);

            return View(viewModel);
        }

        // POST: Applicants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
	        ApplicationCreateViewModel viewModel, string[] spokenLanguages)
        {
			if (!ModelState.IsValid) {
				return View(viewModel
					.SetDataContext(_context)
					.InitializeStaticProperties());
	        }

			try
            {
				return Json(new Dictionary<string, object>
				{
					{"ViewModel", viewModel},
					{"SpokenLanguages", spokenLanguages}
				});

				//return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
	}
}