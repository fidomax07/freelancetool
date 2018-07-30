using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreelanceTool.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreelanceTool.Controllers
{
	public class TestController : Controller
    {
	    private readonly IHostingEnvironment _host;
	    private readonly ApplicationDataContext _context;

	    public TestController(IHostingEnvironment host, ApplicationDataContext context)
	    {
		    _host = host;
		    _context = context;
	    }

		public IActionResult Index()
		{
			var applicant = _context.Applicants
				.Include(a => a.SpokenLanguages)
					.ThenInclude(al => al.Language)
				.AsNoTracking()
				.SingleOrDefault(a => a.Id == 1);

			var spokenLanguages = new StringBuilder();

			foreach (var spokenLang in applicant.SpokenLanguages)
			{
				spokenLanguages.Append($"{spokenLang?.Language.Name} \n");
			}

			return Content(spokenLanguages.ToString());
		}

	    public IActionResult Create()
	    {
		    return View();
	    }

		[HttpPost]
		[ValidateAntiForgeryToken]
	    public async Task<IActionResult> Create(IFormFile TestFile)
		{
			if (TestFile?.Length <= 0)
				return BadRequest(new {error = "File not found in request"});

			var uniqueFileName = TestFile.GetUniqueFileName();
			var filePath = Path.Combine(
				_host.ContentRootPath, 
				"App_Data",
				uniqueFileName);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await TestFile.CopyToAsync(stream);
			}

			if (!System.IO.File.Exists(filePath))
				return BadRequest(new { error = "File not uploaded" });

			//System.IO.File.Delete(filePath);

			return Ok(new Dictionary<string,object>
		    {
			    {"Original name:", TestFile.FileName},
			    {"Unique name:", uniqueFileName},
			    {"Extension:", TestFile.GetExtension()},
			    {"Path:", filePath}
		    });
	    }

	    

    }
}