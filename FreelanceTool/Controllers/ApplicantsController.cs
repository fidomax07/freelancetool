using System.Collections.Generic;
using System.Linq;
using FreelanceTool.Data;
using FreelanceTool.Helpers.Enums;
using FreelanceTool.Models;
using FreelanceTool.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public ActionResult Create(ApplicationCreateViewModel viewModel, 
	        string[] spokenLanguages)
        {
	        return Json(new List<object>{viewModel, spokenLanguages});

            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
	}
}