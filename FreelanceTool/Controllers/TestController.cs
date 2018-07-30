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
using Microsoft.Extensions.Localization;

namespace FreelanceTool.Controllers
{
	public class TestController : Controller
    {
	    private readonly IHostingEnvironment _host;
	    private readonly ApplicationDataContext _context;
	    private readonly IStringLocalizer<TestController> _localizer;

		public TestController(
			IHostingEnvironment host, 
			ApplicationDataContext context,
			IStringLocalizer<TestController> localizer)
	    {	
		    _host = host;
		    _context = context;
		    _localizer = localizer;
	    }

		public string Index()
		{
			return _localizer["Create an application"];
		}

    }
}