using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceTool.Controllers
{
    public class TestController : Controller
    {
        public object Index()
        {
	        DateTime.TryParse("25.07.1990", out var incomingDt);


	        //var parsed = DateTime.TryParseExact("25.07.1990", 
		       // "dd.MM.yyyy", 
		       // CultureInfo.InvariantCulture, 
		       // DateTimeStyles.None,
		       // out var dt);

	        return Json(incomingDt.ToString(CultureInfo.InvariantCulture));
        }
    }
}